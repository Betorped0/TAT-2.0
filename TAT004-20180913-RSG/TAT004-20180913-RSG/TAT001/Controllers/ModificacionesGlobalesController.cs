using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Models.Dao;

namespace TAT001.Controllers
{
    public class ModificacionesGlobalesController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        readonly SolicitudesDao solicitudesDao = new SolicitudesDao();
        // GET: ModificacionesGlobales
        public ActionResult Index()
        {
            int pagina_id = 240;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

            ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel();
            modelView.pageSizes = FnCommon.ObtenerCmbPageSize();
            return View(modelView);
        }
        public ActionResult ListModAutorizador(decimal? num_doci, decimal? num_docf, DateTime? fechai, DateTime? fechaf, string kunnr, string usuarioa_id)
        {
            int pagina_id = 240;//ID EN BASE DE DATOS
            FnCommon.ObtenerTextos(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            ViewBag.spras_id = FnCommon.ObtenerSprasId(db, User.Identity.Name);

            ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel();
            modelView.solicitudPorAprobar = solicitudesDao.ObtenerSolicitudesPorAprobar( num_doci, num_docf, fechai, fechaf, kunnr, usuarioa_id);
            return View(modelView);
        }

        [HttpPost]
        public ActionResult ListModAutorizador(List<SolicitudPorAprobar> solicitudes, decimal? num_doci, decimal? num_docf, DateTime? fechai, DateTime? fechaf, string kunnr, string usuarioa_id)
        {
            try
            {
                return View("Index");
            }
            catch (Exception e)
            {
                Log.ErrorLogApp(e, "ModificacionesGlobales", "ListModAutorizador");

                int pagina_id = 240;//ID EN BASE DE DATOS
                FnCommon.ObtenerTextos(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

                ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel();
                modelView.solicitudPorAprobar = solicitudesDao.ObtenerSolicitudesPorAprobar(num_doci, num_docf, fechai, fechaf, kunnr, usuarioa_id);
                return View(modelView);
            }
        }

        }
}