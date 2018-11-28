using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly SociedadesDao sociedadesDao = new SociedadesDao();
        // GET: ModificacionesGlobales
        public ActionResult Index()
        {
            int pagina_id = 240;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

            ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel
            {
                pageSizes = FnCommon.ObtenerCmbPageSize(),
                sociedades = sociedadesDao
                .ComboSociedades(TATConstantes.ACCION_LISTA_SOCPORUSUARIO, null, User.Identity.Name)
            };
            return View(modelView);
        }
        public ActionResult ListModAutorizador(string sociedad_id,decimal? num_doci, decimal? num_docf, DateTime? fechai, DateTime? fechaf, string kunnr, string usuarioa_id)
        {
            int pagina_id = 240;//ID EN BASE DE DATOS
            FnCommon.ObtenerTextos(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            ViewBag.spras_id = FnCommon.ObtenerSprasId(db, User.Identity.Name);

            ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel
            {
                solicitudPorAprobar = solicitudesDao.ListaSolicitudesPorAprobar(sociedad_id,num_doci, num_docf, fechai, fechaf, kunnr, usuarioa_id)
            };
            return View(modelView);
        }

        [HttpPost]
        public ActionResult ListModAutorizador(List<SolicitudPorAprobar> solicitudes)
        {
            try
            {
                string usuarioa_id = solicitudes.First().USUARIOA_ID_NUEVO;
                foreach (SolicitudPorAprobar sol in solicitudes)
                {
                    //actualizar registro ant.
                    FLUJO f = db.FLUJOes.Where(x => x.NUM_DOC == sol.NUM_DOC).OrderByDescending(a => a.POS).FirstOrDefault();
                    if (f.ESTATUS== "P")
                    {
                        f.POS = f.POS + 1;
                        f.FECHAC = DateTime.Now;
                        f.FECHAM = DateTime.Now;
                        f.USUARIOA_ID = usuarioa_id;
                        f.COMENTARIO = "-";
                        db.FLUJOes.Add(f);
                        db.SaveChanges();
                    }
                }
                return View("Index");
            }
            catch (Exception e)
            {
                Log.ErrorLogApp(e, "ModificacionesGlobales", "ListModAutorizador");

                int pagina_id = 240;//ID EN BASE DE DATOS
                FnCommon.ObtenerTextos(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
                ViewBag.spras_id = FnCommon.ObtenerSprasId(db, User.Identity.Name);

                ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel
                {
                    solicitudPorAprobar = solicitudes
                };
                return View(modelView);
            }
        }

        }
}