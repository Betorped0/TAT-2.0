using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TAT001.Common;
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
            FnCommon.ObtenerConfPage(db,pagina_id,User.Identity.Name, this.ControllerContext.Controller);

            Calendario445ViewModel modelView = new Calendario445ViewModel();
            modelView.calendarios445 = db.CALENDARIO_AC.ToList();
            modelView.calendariosEx445 = db.CALENDARIO_EX.ToList();

            return View(modelView);
        }

        // GET: Calendario445/Create
        public ActionResult Create()
        {
            int pagina_id = 531;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

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
                DateTime fechaActual = DateTime.Now;

                calendarioAc.EJERCICIO = short.Parse(fechaActual.Year.ToString());
                calendarioAc.ACTIVO = true;
                calendarioAc.USUARIOC_ID = User.Identity.Name;
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
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES, CMB_PERIODOS, CMB_TIPOSSOLICITUD });
                return View(modelView);
            }
        }

        public ActionResult Edit(short ejercicio, int periodo, string sociedad_id, string tsol_id )
        {
            int pagina_id = 532;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

            Calendario445ViewModel modelView = new Calendario445ViewModel();
            modelView.calendario445 = db.CALENDARIO_AC.Where(x=>x.EJERCICIO == ejercicio && x.PERIODO==periodo && x.SOCIEDAD_ID==sociedad_id && x.TSOL_ID==tsol_id).FirstOrDefault();

            CargarSelectList(ref modelView, new string[] {
                CMB_SOCIEDADES +","+ modelView.calendario445.SOCIEDAD_ID,
                CMB_PERIODOS + "," + modelView.calendario445.PERIODO,
                CMB_TIPOSSOLICITUD + "," + modelView.calendario445.TSOL_ID});

            return View(modelView);
        }
        
        [HttpPost]
        public ActionResult Edit(Calendario445ViewModel modelView)
        {
            int pagina_id = 532;//ID EN BASE DE DATOS
            try
            {
                CALENDARIO_AC calendarioAc = modelView.calendario445;
                DateTime fechaActual = DateTime.Now;

                calendarioAc.USUARIOM_ID = User.Identity.Name;
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
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
                CargarSelectList(ref modelView, new string[] {
                    CMB_SOCIEDADES + "," + modelView.calendario445.SOCIEDAD_ID,
                    CMB_PERIODOS + "," + modelView.calendario445.PERIODO,
                    CMB_TIPOSSOLICITUD + "," + modelView.calendario445.TSOL_ID });
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
                ViewBag.mnjError = FnCommon.ObtenerTextoMnj(db,pagina_id, "lbl_mnjExistePeriodo",User.Identity.Name);
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
                ViewBag.mnjError = FnCommon.ObtenerTextoMnj(db,pagina_id, "lbl_mnjErrorRangoFechas",User.Identity.Name);
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
                ViewBag.mnjError = String.Format(FnCommon.ObtenerTextoMnj(db, pagina_id, "lbl_mnjTraslapeEnPeriodo", User.Identity.Name), calendarioAux.PERIODO);
                return false;
            }
            calendarioAux = db.CALENDARIO_AC.Where(x => x.SOCIEDAD_ID == calendarioAc.SOCIEDAD_ID
                && x.EJERCICIO == calendarioAc.EJERCICIO
                && x.TSOL_ID == calendarioAc.TSOL_ID
                && x.PERIODO < calendarioAc.PERIODO
                && x.PRE_FROMF > calendarioAc.PRE_FROMF).FirstOrDefault();
            if (calendarioAux != null)
            {
                ViewBag.mnjError = FnCommon.ObtenerTextoMnj(db, pagina_id, "lbl_mnjMenorAPeriodoAnt", User.Identity.Name);
                return false;
            }
            return true;
        }
        
        void CargarSelectList(ref Calendario445ViewModel modelView,string[] combos)
        {
            string spras_id = FnCommon.ObtenerSprasId(db,User.Identity.Name);

            for (int i = 0; i < combos.Length; i++) {
                string[] combosSplit = combos[i].Split(',');
                string combo = combosSplit[0];
                string id = combosSplit.Length > 1 ? combosSplit[1] : null;

                switch (combo)
                {
                    case CMB_SOCIEDADES:
                        modelView.sociedades = FnCommon.ObtenerCmbSociedades(db,id);
                        break;
                    case CMB_PERIODOS:
                        int? idAux = (id==null?null: (int?)int.Parse(id));
                        modelView.periodos = FnCommon.ObtenerCmbPeriodos(db, spras_id, idAux);
                        break;
                    case CMB_USUARIOS:
                        modelView.usuarios = FnCommon.ObtenerCmbUsuario(db, id);
                        break;
                    case CMB_TIPOSSOLICITUD:
                      //  modelView.tiposSolicitud = FnCommon.ObtenerTreeTiposSolicitud(db, spras_id,id);

                        modelView.tipoSolicitudes = FnCommon.ObtenerCmbTiposSolicitud(db, spras_id, id);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
