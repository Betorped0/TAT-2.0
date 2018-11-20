using System;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers
{
    public class ModificacionesGlobalesController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

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
            ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel();
            modelView.solicitudPorAprobar = FnCommon.ObtenerSolicitudesPorAprobar(db, num_doci, num_docf, fechai, fechaf, kunnr, usuarioa_id);
            return View(modelView);
        }

    }
}