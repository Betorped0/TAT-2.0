using ClosedXML.Excel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Filters;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    [LoginActive]
    public class ProveedorController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Proveedor
        public ActionResult Index()
        {
            int pagina_id = 771; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            ViewBag.pais = Session["pais"] != null ? Session["pais"].ToString() + ".png" : null;


            ProveedorViewModel viewModel = new ProveedorViewModel();
            viewModel.pageSizes = FnCommon.ObtenerCmbPageSize();
            ObtenerListado(ref viewModel);

            return View(viewModel);
        }

        public ActionResult List(string colOrden, string ordenActual, int? numRegistros = 10, int? pagina = 1, string buscar = "")
        {
            int pagina_id = 771; //ID EN BASE DE DATOS
            ProveedorViewModel viewModel = new ProveedorViewModel();
            ObtenerListado(ref viewModel, colOrden, ordenActual, numRegistros, pagina, buscar);
            FnCommon.ObtenerTextos(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            return View(viewModel);
        }
        public void ObtenerListado(ref ProveedorViewModel viewModel, string colOrden = "", string ordenActual = "", int? numRegistros = 10, int? pagina = 1, string buscar = "")
        {
            int pageIndex = pagina.Value;
            List<PROVEEDOR> clientes = db.PROVEEDORs.ToList();
            viewModel.ordenActual = colOrden;
            viewModel.numRegistros = numRegistros.Value;
            viewModel.buscar = buscar;

            if (!String.IsNullOrEmpty(buscar))
            {
                clientes = clientes.Where(x =>
                String.Concat(x.ID, (x.NOMBRE == null ? "" : x.NOMBRE), (x.SOCIEDAD_ID == null ? "" : x.SOCIEDAD_ID), (x.PAIS_ID == null ? "" : x.PAIS_ID), x.ACTIVO)
                .ToLower().Contains(buscar.ToLower()))
                .ToList();
            }
            switch (colOrden)
            {
                case "ID":
                    if (colOrden.Equals(ordenActual))
                        viewModel.proveedores = clientes.OrderByDescending(m => m.ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.proveedores = clientes.OrderBy(m => m.ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
                case "NOMBRE":
                    if (colOrden.Equals(ordenActual))
                        viewModel.proveedores = clientes.OrderByDescending(m => m.NOMBRE).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.proveedores = clientes.OrderBy(m => m.NOMBRE).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "SOCIEDAD_ID":
                    if (colOrden.Equals(ordenActual))
                        viewModel.proveedores = clientes.OrderByDescending(m => m.SOCIEDAD_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.proveedores = clientes.OrderBy(m => m.SOCIEDAD_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "PAIS_ID":
                    if (colOrden.Equals(ordenActual))
                        viewModel.proveedores = clientes.OrderByDescending(m => m.PAIS_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.proveedores = clientes.OrderBy(m => m.PAIS_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                

                case "ACTIVO":
                    if (colOrden.Equals(ordenActual))
                        viewModel.proveedores = clientes.OrderByDescending(m => m.ACTIVO).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.proveedores = clientes.OrderBy(m => m.ACTIVO).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                default:
                    viewModel.proveedores = clientes.OrderBy(m => m.ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
            }
        }
        // GET: Proveedor/Details/5
        public ActionResult Details(string id)
        {
            int pagina = 772; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                pagina = 502;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(771) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //ViewBag.pais = "mx.svg";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
                ViewBag.lan = user.SPRAS_ID;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROVEEDOR pROVEEDOR = db.PROVEEDORs.Find(id);
            if (pROVEEDOR == null)
            {
                return HttpNotFound();
            }
            return View(pROVEEDOR);
        }

        // GET: Proveedor/Create
        public ActionResult Create()
        {
            int pagina = 774; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(771) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //ViewBag.pais = "mx.svg";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
                ViewBag.lan = user.SPRAS_ID;
            }
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT");
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "LANDX");
            return View();
        }

        // POST: Proveedor/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,SOCIEDAD_ID,PAIS_ID")] PROVEEDOR pROVEEDOR)
        {
            //Valido si ya existe algun registro
            if (db.PROVEEDORs.Where(x => x.ID == pROVEEDOR.ID).ToList().Count == 0)
            {
                //recupero el pais, para que el usuario no lo ingrese
                var pa = db.SOCIEDADs.Where(x => x.BUKRS == pROVEEDOR.SOCIEDAD_ID).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    pROVEEDOR.PAIS_ID = pROVEEDOR.PAIS_ID;
                    pROVEEDOR.SOCIEDAD_ID = pROVEEDOR.SOCIEDAD_ID;
                    pROVEEDOR.ACTIVO = true;
                    db.PROVEEDORs.Add(pROVEEDOR);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            int pagina = 774; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(771) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.error = "Valores Existentes (" + pROVEEDOR.ID + ")";
                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //ViewBag.pais = "mx.svg";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS");
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "LANDX");
            return View(pROVEEDOR);
        }

        // GET: Proveedor/Edit/5
        public ActionResult Edit(string id)
        {
            int pagina = 773; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(771) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //ViewBag.pais = "mx.svg";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
                ViewBag.lan = user.SPRAS_ID;
            }
            var pr = db.PROVEEDORs.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", pr.SOCIEDAD_ID);
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "LANDX", pr.PAIS_ID);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (pr == null)
            {
                return HttpNotFound();
            }
            return View(pr);
        }

        // POST: Proveedor/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOMBRE,SOCIEDAD_ID,PAIS_ID,ACTIVO")] PROVEEDOR pROVEEDOR)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    pROVEEDOR.PAIS_ID = pROVEEDOR.PAIS_ID;
                    pROVEEDOR.SOCIEDAD_ID = pROVEEDOR.SOCIEDAD_ID;
                    db.Entry(pROVEEDOR).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    var ex = e.ToString();
                }
                return RedirectToAction("Index");
            }
            int pagina = 773; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(771) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //ViewBag.pais = "mx.svg";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
                ViewBag.lan = user.SPRAS_ID;
            }
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", pROVEEDOR.SOCIEDAD_ID);
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "LANDX", pROVEEDOR.PAIS_ID);
            return View(pROVEEDOR);
        }

        // GET: Proveedor/Delete/5
        public ActionResult Delete(string id)
        {
            int pagina = 775; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(771) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //ViewBag.pais = "mx.svg";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
                ViewBag.lan = user.SPRAS_ID;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROVEEDOR pROVEEDOR = db.PROVEEDORs.Find(id);
            if (pROVEEDOR == null)
            {
                return HttpNotFound();
            }
            return View(pROVEEDOR);
        }

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                PROVEEDOR pROVEEDOR = db.PROVEEDORs.Find(id);
                pROVEEDOR.ACTIVO = false;
                db.Entry(pROVEEDOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var y = e.ToString();
                return View();
            }
        }
        public ActionResult Desactivar(string id, bool a)
        {
            try
            {
                PROVEEDOR pROVEEDOR = db.PROVEEDORs.Find(id);
                pROVEEDOR.ACTIVO = a;
                db.Entry(pROVEEDOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var y = e.ToString();
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public FileResult Descargar()
        {
            var pr = db.PROVEEDORs.ToList();
            generarExcelHome(pr, Server.MapPath("~/pdfTemp/"));
            return File(Server.MapPath("~/pdfTemp/DocPr" + DateTime.Now.ToShortDateString() + ".xlsx"), "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DocPr" + DateTime.Now.ToShortDateString() + ".xlsx");
        }

        public void generarExcelHome(List<PROVEEDOR> lst, string ruta)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            try
            {
                //Creamos el encabezado
                worksheet.Cell("A1").Value = new[]
             {
                  new {
                      BANNER = "ID"
                      },
                    };
                worksheet.Cell("B1").Value = new[]
            {
                  new {
                      BANNER = "NOMBRE"
                      },
                    };
                worksheet.Cell("C1").Value = new[]
            {
                  new {
                      BANNER = "CO. CODE"
                      },
                    };
                worksheet.Cell("D1").Value = new[]
           {
                  new {
                      BANNER = "PAIS"
                      },
                    };
                worksheet.Cell("E1").Value = new[]
{
                  new {
                      BANNER = "ACTIVO"
                      },
                    };
                for (int i = 2; i <= (lst.Count + 1); i++)
                {
                    string activo = lst[i - 2].ACTIVO ? "SI" : "NO";
                    worksheet.Cell("A" + i).Value = new[]
               {
                  new {
                      BANNER       = lst[i-2].ID
                      },
                    };
                    worksheet.Cell("B" + i).Value = new[]
                {
                  new {
                      BANNER       = lst[i-2].NOMBRE
                      },
                    };
                    worksheet.Cell("C" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].SOCIEDAD_ID
                        },
                      };
                    worksheet.Cell("D" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].PAIS_ID
                        },
                      };
                    worksheet.Cell("E" + i).Value = new[]
{
                    new {
                        BANNER =activo 
                        },
                      };
                }
                var rt = ruta + @"\DocPr" + DateTime.Now.ToShortDateString() + ".xlsx";
                workbook.SaveAs(rt);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
