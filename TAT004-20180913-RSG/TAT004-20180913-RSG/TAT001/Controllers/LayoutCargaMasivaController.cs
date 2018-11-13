using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers
{
    [Authorize]
    public class LayoutCargaMasivaController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        // GET: LayoutCargaMasiva
        public ActionResult Index()
        {
            int pagina_id = 550;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            LayoutCargaMasivaViewModels viewModel = new LayoutCargaMasivaViewModels();
            viewModel.Layouts = db.LAYOUT_CARGA.ToList();

            return View(viewModel);
        }

        // GET: LayoutCargaMasiva/Create
        public ActionResult Create()
        {
            int pagina_id = 551;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            LayoutCargaMasivaViewModels modelView = new LayoutCargaMasivaViewModels();
            modelView.paises = db.PAIS
                            .Where(x => x.ACTIVO)
                            .Select(x => new SelectListItem
                            {
                                Value = x.LAND,
                                Text = x.LAND + "-"+x.LANDX 
                            }).ToList();
            modelView.sociedades=db.SOCIEDADs
                 .Where(x => x.ACTIVO )
                 .Select(x => new SelectListItem
                 {
                     Value = x.BUKRS,
                     Text = x.BUKRS + "-" + x.BUTXT
                 }).ToList();

            return View(modelView);

        }

        // POST: LayoutCargaMasiva/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase PathArchivo,LayoutCargaMasivaViewModels modelView)
        {
            int pagina_id = 551;//ID EN BASE DE DATOS
            try
            {
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
                LAYOUT_CARGA layou = modelView.Layout;
                layou.FECHAC = DateTime.Now;
                layou.TIPO = "Solicitud";
                //layou.SOCIEDAD_ID = db.PAIS.FirstOrDefault(x => x.LAND == layou.LAND).SOCIEDAD_ID;
                if (PathArchivo != null)
                {
                    var path = Path.Combine(Server.MapPath("~/Archivos/LayoutCargaMasiva"));
                    var ruta = path + "/" + modelView.Layout.LAND + "-" + PathArchivo.FileName;
                    if (!Directory.Exists(path))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(path);
                    }
                    PathArchivo.SaveAs(ruta);
                    layou.RUTA = ruta;


                    db.LAYOUT_CARGA.Add(modelView.Layout);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
                modelView.paises = db.PAIS
                          .Where(x => x.ACTIVO)
                          .Select(x => new SelectListItem
                          {
                              Value = x.LAND,
                              Text = x.LAND + "-" + x.LANDX
                          }).ToList();
                modelView.sociedades = db.SOCIEDADs
                     .Where(x => x.ACTIVO)
                     .Select(x => new SelectListItem
                     {
                         Value = x.BUKRS,
                         Text = x.BUKRS + "-" + x.BUTXT
                     }).ToList();
                ViewBag.mnjError = FnCommon.ObtenerTextoMnj(db, pagina_id, "lbl_mnjErrorGuardar", User.Identity.Name);
                return View(modelView);
            }
        }
        // GET: Alertas/Delete
        public ActionResult Delete(int layout_id)
        {
            LAYOUT_CARGA layout = db.LAYOUT_CARGA.Where(x => x.ID == layout_id).FirstOrDefault();
            if (layout == null) { return RedirectToAction("Index"); }
           
            db.LAYOUT_CARGA.Remove(layout);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}