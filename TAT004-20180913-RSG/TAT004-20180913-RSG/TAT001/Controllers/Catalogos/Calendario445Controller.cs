using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    public class Calendario445Controller : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        const string CMB_SOCIEDADES= "SOC";
        const string CMB_TIPOSSOLICITUD = "TSOL";
        const string CMB_PERIODOS = "PER";
        const string CMB_USUARIOS = "USU";

        // GET: Calendario445
        public ActionResult Index()
        {
            int pagina_id = 530;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);

            Calendario445ViewModel modelView = new Calendario445ViewModel();
            modelView.calendarios445 = db.CALENDARIO_AC.ToList();
            modelView.calendariosEx445 = db.CALENDARIO_EX.ToList();

            return View(modelView);
        }

        // GET: Calendario445/Create
        public ActionResult Create()
        {
            int pagina_id = 531;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);

            Calendario445ViewModel modelView = new Calendario445ViewModel();
            CargarSelectList(ref modelView,new string[] { CMB_SOCIEDADES, CMB_PERIODOS, CMB_TIPOSSOLICITUD });
            return View(modelView);
        }

        // POST: Calendario445/Create
        [HttpPost]
        public ActionResult Create(Calendario445ViewModel modelView)
        {
            int pagina_id = 531;//ID EN BASE DE DATOS
            try
            {
                CALENDARIO_AC calendarioAc = modelView.calendario445;
                USUARIO user = ObtenerUsuario();
                DateTime fechaActual = DateTime.Now;

                calendarioAc.EJERCICIO = short.Parse(fechaActual.Year.ToString());
                calendarioAc.ACTIVO = true;
                calendarioAc.USUARIOC_ID = user.ID;
                calendarioAc.FECHAC = fechaActual;
               
                if (!ValidarPeriodoExistente(calendarioAc) ||!ValidarFechas(calendarioAc))
                {
                     throw new Exception();
                }
                db.CALENDARIO_AC.Add(calendarioAc);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ObtenerConfPage(pagina_id);
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES, CMB_PERIODOS, CMB_TIPOSSOLICITUD });
                return View(modelView);
            }
        }

        public ActionResult Edit(short ejercicio, int periodo, string sociedad_id, string tsol_id )
        {
            ObtenerConfPage(532);

            Calendario445ViewModel modelView = new Calendario445ViewModel();
            modelView.calendario445 = db.CALENDARIO_AC.Where(x=>x.EJERCICIO == ejercicio && x.PERIODO==periodo && x.SOCIEDAD_ID==sociedad_id && x.TSOL_ID==tsol_id).FirstOrDefault();
            CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES+","+ modelView.calendario445.SOCIEDAD_ID,CMB_PERIODOS + "," + modelView.calendario445.PERIODO, CMB_TIPOSSOLICITUD + "," + modelView.calendario445.TSOL_ID});
            return View(modelView);
        }
        
        [HttpPost]
        public ActionResult Edit(Calendario445ViewModel modelView)
        {
            try
            {
                CALENDARIO_AC calendarioAc = modelView.calendario445;
                USUARIO user = ObtenerUsuario();
                DateTime fechaActual = DateTime.Now;

                calendarioAc.USUARIOM_ID = user.ID;
                calendarioAc.FECHAM = fechaActual;

                if (!ValidarFechas(calendarioAc))
                {
                    throw new Exception();
                }

                db.Entry(calendarioAc).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ObtenerConfPage(532);
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + modelView.calendario445.SOCIEDAD_ID, CMB_PERIODOS + "," + modelView.calendario445.PERIODO, CMB_TIPOSSOLICITUD + "," + modelView.calendario445.TSOL_ID });
                return View(modelView);
            }
        }
        bool ValidarPeriodoExistente(CALENDARIO_AC calendarioAc)
        {
            int pagina_id = 530;
            if (db.CALENDARIO_AC.Any(x => x.SOCIEDAD_ID == calendarioAc.SOCIEDAD_ID 
                && x.PERIODO == calendarioAc.PERIODO
                && x.EJERCICIO == calendarioAc.EJERCICIO
                && x.TSOL_ID == calendarioAc.TSOL_ID))
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjExistePeriodo");
                return false;
            }
            return true;
        }
        bool ValidarFechas(CALENDARIO_AC calendarioAc)
        {
            int pagina_id = 530;

            if (calendarioAc.PRE_FROMF > calendarioAc.PRE_TOF
                || calendarioAc.CIE_FROMF > calendarioAc.CIE_TOF
                || calendarioAc.PRE_TOF > calendarioAc.CIE_FROMF)
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjErrorRangoFechas");
                return false;
            }
            CALENDARIO_AC calendarioAux = db.CALENDARIO_AC.Where(x => x.SOCIEDAD_ID == calendarioAc.SOCIEDAD_ID
                && x.EJERCICIO == calendarioAc.EJERCICIO
                && x.TSOL_ID == calendarioAc.TSOL_ID
                && x.PERIODO != calendarioAc.PERIODO
                && ((calendarioAc.PRE_FROMF>=x.PRE_FROMF && calendarioAc.PRE_FROMF<=x.CIE_TOF)
                || (calendarioAc.CIE_TOF >= x.PRE_FROMF && calendarioAc.CIE_TOF <= x.CIE_TOF))).FirstOrDefault();
            if (calendarioAux != null)
            {
                ViewBag.mnjError = String.Format(ObtenerTextoMnj(pagina_id, "lbl_mnjTraslapeEnPeriodo"), calendarioAux.PERIODO);
                return false;
            }
            calendarioAux = db.CALENDARIO_AC.Where(x => x.SOCIEDAD_ID == calendarioAc.SOCIEDAD_ID
                && x.EJERCICIO == calendarioAc.EJERCICIO
                && x.TSOL_ID == calendarioAc.TSOL_ID
                && x.PERIODO < calendarioAc.PERIODO
                && x.PRE_FROMF > calendarioAc.PRE_FROMF).FirstOrDefault();
            if (calendarioAux != null)
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjMenorAPeriodoAnt");
                return false;
            }
            return true;
        }

        void ObtenerConfPage(int pagina)//ID EN BASE DE DATOS
        {
            var user = ObtenerUsuario();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title =db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.spras_id = user.SPRAS_ID;
        }
        void CargarSelectList(ref Calendario445ViewModel modelView,string[] combos)
        {
            USUARIO user = ObtenerUsuario();
            string spras_id = user.SPRAS_ID;

            for (int i = 0; i < combos.Length; i++) {
                string[] combosSplit = combos[i].Split(',');
                string combo = combosSplit[0];
                string id = combosSplit.Length > 1 ? combosSplit[1] : null;

                switch (combo)
                {
                    case CMB_SOCIEDADES:
                        modelView.sociedades = db.SOCIEDADs
                            .Where(x => x.BUKRS == id || id == null)
                            .Select(x => new SelectListItem
                            {
                                Value = x.BUKRS,
                                Text = x.BUKRS
                            }).ToList();
                        break;
                    case CMB_PERIODOS:
                        int? idAux = (id==null?null: (int?)int.Parse(id));
                        modelView.periodos = db.PERIODOes
                           .Join(db.PERIODOTs, p => p.ID, pt => pt.PERIODO_ID, (p, pt) => pt)
                           .Where(x => x.SPRAS_ID == spras_id && (x.PERIODO_ID == idAux || idAux == null))
                           .Select(x => new SelectListItem
                           {
                               Value = x.PERIODO_ID.ToString(),
                               Text = (x.PERIODO_ID.ToString() + " - " + x.TXT50)
                           }).ToList();
                        break;
                    case CMB_USUARIOS:
                        modelView.usuarios = db.USUARIOs
                            .Where(x => x.ID == id || id == null)
                            .Select(x => new SelectListItem
                            {
                                Value = x.ID,
                                Text = (x.NOMBRE + " " + x.APELLIDO_P + " " + (x.APELLIDO_M == null ? "" : x.APELLIDO_M))
                            }).ToList();
                        break;
                    case CMB_TIPOSSOLICITUD:
                        modelView.tipoSolicitudes = db.TSOLs
                            .Join(db.TSOLTs, s => s.ID, st => st.TSOL_ID, (s, st) => st)
                            .Where(x => x.SPRAS_ID == spras_id && (x.TSOL_ID==id || id == null))
                            .Select(x => new SelectListItem
                            {
                                Value = x.TSOL_ID,
                                Text = (x.TSOL_ID + "-" + x.TXT50)
                            }).ToList();
                        break;
                    default:
                        break;
                }
            }
        }

        USUARIO ObtenerUsuario()
        {
            string u = User.Identity.Name;
            return db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
        }
        string ObtenerTextoMnj(int pagina_id,string campo_id)
        {
            USUARIO usuario = ObtenerUsuario();
            string texto = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina_id) && a.SPRAS_ID.Equals(usuario.SPRAS_ID) && a.CAMPO_ID.Equals(campo_id))).FirstOrDefault().TEXTOS;
            return texto;
        }
    }
}
