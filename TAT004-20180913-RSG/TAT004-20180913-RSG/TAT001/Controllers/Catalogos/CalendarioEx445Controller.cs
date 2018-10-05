using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    public class CalendarioEx445Controller : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        const string CMB_SOCIEDADES = "SOC";
        const string CMB_TIPOSSOLICITUD = "TSOL";
        const string CMB_PERIODOS = "PER";
        const string CMB_USUARIOS = "USU";

        // GET: CalendarioEx445/5
        public ActionResult Index(short ejercicio, int periodo, string sociedad_id, string tsol_id)
        {
            int pagina_id = 533;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);
            Calendario445ViewModel modelView = new Calendario445ViewModel();
            modelView.calendarioEx445.EJERCICIO = ejercicio;
            modelView.calendarioEx445.PERIODO = periodo;
            modelView.calendarioEx445.SOCIEDAD_ID = sociedad_id;
            modelView.calendarioEx445.TSOL_ID = tsol_id;
            modelView.calendariosEx445 = db.CALENDARIO_EX.Where(x => x.EJERCICIO == ejercicio && x.PERIODO == periodo && x.SOCIEDAD_ID == sociedad_id && x.TSOL_ID == tsol_id).ToList();
            modelView.calendario445 = db.CALENDARIO_AC.Where(x => x.EJERCICIO == ejercicio && x.PERIODO == periodo && x.SOCIEDAD_ID == sociedad_id && x.TSOL_ID == tsol_id).FirstOrDefault();
            CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + sociedad_id, CMB_PERIODOS + "," + periodo, CMB_TIPOSSOLICITUD + "," + tsol_id, CMB_USUARIOS });
            return View(modelView);
        }

        // POST: CalendarioEx445/Index
        [HttpPost]
        public ActionResult Index(Calendario445ViewModel modelView)
        {
            int pagina_id = 533;//ID EN BASE DE DATOS
            CALENDARIO_EX calendarioEx = modelView.calendarioEx445;
            try
            {
                USUARIO user = ObtenerUsuario();
                DateTime fechaActual = DateTime.Now;

                calendarioEx.USUARIOC_ID = user.ID;
                calendarioEx.FEHAC = fechaActual;
                calendarioEx.ACTIVO = true;

                if (!ValidarExcepcionExistente(calendarioEx) || !ValidarFechas(calendarioEx))
                {
                    throw new Exception();
                }
                db.CALENDARIO_EX.Add(calendarioEx);
                db.SaveChanges();

                return RedirectToAction("Index",new { ejercicio = calendarioEx.EJERCICIO, periodo = calendarioEx.PERIODO, sociedad_id = calendarioEx.SOCIEDAD_ID, tsol_id = calendarioEx.TSOL_ID });
            }
            catch (Exception ex)
            {
                ObtenerConfPage(pagina_id);
                modelView.calendariosEx445 = db.CALENDARIO_EX.Where(x => x.EJERCICIO == calendarioEx.EJERCICIO && x.PERIODO == calendarioEx.PERIODO && x.SOCIEDAD_ID == calendarioEx.SOCIEDAD_ID && x.TSOL_ID == calendarioEx.TSOL_ID).ToList();
                modelView.calendario445 = db.CALENDARIO_AC.Where(x => x.EJERCICIO == calendarioEx.EJERCICIO && x.PERIODO == calendarioEx.PERIODO && x.SOCIEDAD_ID == calendarioEx.SOCIEDAD_ID && x.TSOL_ID == calendarioEx.TSOL_ID).FirstOrDefault();
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + calendarioEx.SOCIEDAD_ID, CMB_PERIODOS + "," + calendarioEx.PERIODO, CMB_TIPOSSOLICITUD + "," + calendarioEx.TSOL_ID, CMB_USUARIOS });

                return View(modelView);
            }
        }
        // GET: CalendarioEx445/Edit/5
        public ActionResult Edit(short ejercicio, int periodo, string sociedad_id, string tsol_id, string usuario_id,string paginaIndex)
        {
            int pagina_id = 534;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);
            Calendario445ViewModel modelView = new Calendario445ViewModel();
            modelView.calendarioEx445= db.CALENDARIO_EX.Where(x => x.EJERCICIO == ejercicio && x.PERIODO == periodo && x.SOCIEDAD_ID == sociedad_id && x.TSOL_ID == tsol_id && x.USUARIO_ID == usuario_id).FirstOrDefault();
            modelView.calendario445 = db.CALENDARIO_AC.Where(x => x.EJERCICIO == ejercicio && x.PERIODO == periodo && x.SOCIEDAD_ID == sociedad_id && x.TSOL_ID == tsol_id).FirstOrDefault();
            CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + sociedad_id, CMB_PERIODOS + "," + periodo, CMB_TIPOSSOLICITUD + "," + tsol_id, CMB_USUARIOS+","+ usuario_id });
            ViewBag.paginaIndex = paginaIndex;
            return View(modelView);
        }

        // POST: CalendarioEx445/Edit
        [HttpPost]
        public ActionResult Edit(Calendario445ViewModel modelView, string paginaIndex)
        {
            int pagina_id = 534;//ID EN BASE DE DATOS
            CALENDARIO_EX calendarioEx = modelView.calendarioEx445;
            try
            {
                if ( !ValidarFechas(calendarioEx))
                {
                    throw new Exception();
                }
                db.Entry(calendarioEx).State = EntityState.Modified;
                db.SaveChanges();
                if (paginaIndex== "Calendario445") {
                    return RedirectToAction("Index",paginaIndex);
                   }
                else
                {
                    return RedirectToAction("Index", new { ejercicio = calendarioEx.EJERCICIO, periodo = calendarioEx.PERIODO, sociedad_id = calendarioEx.SOCIEDAD_ID, tsol_id = calendarioEx.TSOL_ID });

                }
            }
            catch (Exception ex)
            {
                ObtenerConfPage(pagina_id);
                modelView.calendario445 = db.CALENDARIO_AC.Where(x => x.EJERCICIO == calendarioEx.EJERCICIO && x.PERIODO == calendarioEx.PERIODO && x.SOCIEDAD_ID == calendarioEx.SOCIEDAD_ID && x.TSOL_ID == calendarioEx.TSOL_ID).FirstOrDefault();
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + calendarioEx.SOCIEDAD_ID, CMB_PERIODOS + "," + calendarioEx.PERIODO, CMB_TIPOSSOLICITUD + "," + calendarioEx.TSOL_ID, CMB_USUARIOS+","+calendarioEx.USUARIO_ID });

                return View(modelView);
            }
        }
        bool ValidarExcepcionExistente(CALENDARIO_EX calendarioEx)
        {
            int pagina_id = 530;
            if (db.CALENDARIO_EX.Any(x => x.SOCIEDAD_ID == calendarioEx.SOCIEDAD_ID 
            && x.PERIODO == calendarioEx.PERIODO && x.EJERCICIO == calendarioEx.EJERCICIO
            && x.TSOL_ID == calendarioEx.TSOL_ID && x.USUARIO_ID == calendarioEx.USUARIO_ID))
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjExisteExcepcion");
                return false;
            }
            return true;
        }
        bool ValidarFechas(CALENDARIO_EX calendarioEx)
        {
            int pagina_id = 530;

            if (calendarioEx.EX_FROMF > calendarioEx.EX_TOF)
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjErrorRangoFechas");
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
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.spras_id = user.SPRAS_ID;
        }
        void CargarSelectList(ref Calendario445ViewModel modelView, string[] combos)
        {
            USUARIO user = ObtenerUsuario();
            string spras_id = user.SPRAS_ID;

            for (int i = 0; i < combos.Length; i++)
            {
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
                        int? idAux = (id == null ? null : (int?)int.Parse(id));
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
                            .Where(x => x.SPRAS_ID == spras_id && (x.TSOL_ID == id || id == null))
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
        string ObtenerTextoMnj(int pagina_id, string campo_id)
        {
            USUARIO usuario = ObtenerUsuario();
            string texto = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina_id) && a.SPRAS_ID.Equals(usuario.SPRAS_ID) && a.CAMPO_ID.Equals(campo_id))).FirstOrDefault().TEXTOS;
            return texto;
        }
    }
}
