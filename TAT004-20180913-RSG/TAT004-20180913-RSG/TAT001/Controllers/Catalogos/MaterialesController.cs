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
    public class MaterialesController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        public ActionResult Index()
        {
            int pagina_id = 661; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            ViewBag.pais = Session["pais"] != null ? Session["pais"].ToString() + ".png" : null;


           MaterialViewModel viewModel = new MaterialViewModel();
            viewModel.pageSizes = FnCommon.ObtenerCmbPageSize();
            ObtenerListado(ref viewModel);

            return View(viewModel);
        }

        public ActionResult List(string colOrden, string ordenActual, int? numRegistros = 10, int? pagina = 1, string buscar = "")
        {
            int pagina_id = 661; //ID EN BASE DE DATOS
            MaterialViewModel viewModel = new MaterialViewModel();
            ObtenerListado(ref viewModel, colOrden, ordenActual, numRegistros, pagina, buscar);
            FnCommon.ObtenerTextos(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            return View(viewModel);
        }
        public void ObtenerListado(ref MaterialViewModel viewModel, string colOrden = "", string ordenActual = "", int? numRegistros = 10, int? pagina = 1, string buscar = "")
        {
            int pageIndex = pagina.Value;
            List<MVKEMaterial> clientes = db.MATERIALVKEs.Join(db.MATERIALs,
                                                              p => p.MATERIAL_ID,
                                                              e => e.ID,
                                                              (p, e) => new MVKEMaterial
                                                              {
                                                                  MATERIAL_ID = p.MATERIAL_ID,
                                                                  VKORG = p.VKORG,
                                                                  VTWEG = p.VTWEG,
                                                                  MATKL_ID = e.MATKL_ID,
                                                                  MAKTX = e.MAKTX,
                                                                  MATERIAL_GROUP = e.MATERIALGP.DESCRIPCION,
                                                                  ACTIVO=p.ACTIVO
                                                              }).Where(t=>t.ACTIVO).ToList();

            //List<MVKEMaterial> clientes = (from mvke in db.MATERIALVKEs
            //              join m in db.MATERIALs
            //              on mvke.MATERIAL_ID equals m.ID
            //              select new MVKEMaterial
            //              {
            //                  MATERIAL_ID = mvke.MATERIAL_ID,
            //                  VKORG = mvke.VKORG,
            //                  VTWEG = mvke.VTWEG,
            //                  MATKL_ID = m.MATKL_ID,
            //                  MAKTX = m.MAKTX,
            //                  MATERIAL_GROUP = m.MATERIALGP.DESCRIPCION
            //              }).ToList();
            viewModel.ordenActual = colOrden;
            viewModel.numRegistros = numRegistros.Value;
            viewModel.buscar = buscar;

            if (!String.IsNullOrEmpty(buscar))
            {
                clientes = clientes.Where(x =>
                String.Concat(x.MATERIAL_ID, x.VKORG, x.VTWEG, x.MATKL_ID, x.MAKTX, x.MATERIAL_GROUP)
                .ToLower().Contains(buscar.ToLower()))
                .ToList();
            }
            switch (colOrden)
            {
                case "MATERIAL_ID":
                    if (colOrden.Equals(ordenActual))
                        viewModel.materiales = clientes.OrderByDescending(m => m.MATERIAL_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.materiales = clientes.OrderBy(m => m.MATERIAL_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
                case "VKORG":
                    if (colOrden.Equals(ordenActual))
                        viewModel.materiales = clientes.OrderByDescending(m => m.VKORG).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.materiales = clientes.OrderBy(m => m.VKORG).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "VTWEG":
                    if (colOrden.Equals(ordenActual))
                        viewModel.materiales = clientes.OrderByDescending(m => m.VTWEG).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.materiales = clientes.OrderBy(m => m.VTWEG).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "MAKTX":
                    if (colOrden.Equals(ordenActual))
                        viewModel.materiales = clientes.OrderByDescending(m => m.MAKTX).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.materiales = clientes.OrderBy(m => m.MAKTX).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
                case "MATKL_ID":
                    if (colOrden.Equals(ordenActual))
                        viewModel.materiales = clientes.OrderByDescending(m => m.MATKL_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.materiales = clientes.OrderBy(m => m.MATKL_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
                case "MATERIAL_GROUP":
                    if (colOrden.Equals(ordenActual))
                        viewModel.materiales = clientes.OrderByDescending(m => m.MATERIAL_GROUP).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.materiales = clientes.OrderBy(m => m.MATERIAL_GROUP).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
                default:
                    viewModel.materiales = clientes.OrderBy(m => m.MATERIAL_ID).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
            }
        }

        
        // GET: Materiales/Details/5
        public ActionResult Details(string id)
        {
            int pagina = 662; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATERIAL mATERIAL = db.MATERIALs.Find(id);
            if (mATERIAL == null)
            {
                return HttpNotFound();
            }
            return View(mATERIAL);
        }

        // GET: Materiales/Create
        public ActionResult Create()
        {
            int pagina = 663; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
            ViewBag.rol = user.MIEMBROS.FirstOrDefault().ROL.NOMBRE;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            ViewBag.MATKL_ID = new SelectList(db.CATEGORIAs, "ID", "DESCRIPCION");
            ViewBag.CTGR = new SelectList(db.ZCTGRs, "ID_ZC", "DESCRIPCION");
            ViewBag.BRAND = new SelectList(db.ZBRANDs, "ID_ZB", "Descripcion");
            ViewBag.MEINS = new SelectList(db.UMEDIDAs, "MSEHI", "MSEHI");
            return View();
        }

        // POST: Materiales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,MTART,MATKL_ID,MAKTX,MAKTG,MEINS,PUNIT,ACTIVO,CTGR,BRAND")] MATERIAL mATERIAL)
        {
            if (ModelState.IsValid)
            {
                mATERIAL.ACTIVO = true;
                db.MATERIALs.Add(mATERIAL);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MATKL_ID = new SelectList(db.CATEGORIAs, "ID", "DESCRIPCION", mATERIAL.MATKL_ID);
            ViewBag.CTGR = new SelectList(db.ZCTGRs, "ID_ZC", "DESCRIPCION", mATERIAL.CTGR);
            ViewBag.BRAND = new SelectList(db.ZBRANDs, "ID_ZB", "Descripcion", mATERIAL.BRAND);
            ViewBag.MEINS = new SelectList(db.UMEDIDAs, "MSEHI", "MSEHI", mATERIAL.MEINS);
            return View(mATERIAL);
        }

        // GET: Materiales/Edit/5
        public ActionResult Edit(string id)
        {
            int pagina = 664; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATERIAL mATERIAL = db.MATERIALs.Find(id);
            if (mATERIAL == null)
            {
                return HttpNotFound();
            }
            if (mATERIAL.MATERIALTs.Count > 0)
                foreach (var e in mATERIAL.MATERIALTs)
                {
                    if (e.SPRAS == "EN")
                        ViewBag.EN = e.MAKTX;
                    if (e.SPRAS == "ES")
                        ViewBag.ES = e.MAKTX;
                    if (e.SPRAS == "PT")
                        ViewBag.PT = e.MAKTX;
                }
            else
                ViewBag.EN = mATERIAL.MAKTX;
            return View(mATERIAL);
        }

        // POST: Materiales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,MTART,MATKL_ID,MAKTX,MAKTG,MEINS,PUNIT,ACTIVO,CTGR,BRAND,MATERIALGP_ID,EN,ES,PT")] MATERIAL mATERIAL, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                MATERIAL mATERIAL1 = db.MATERIALs.Find(mATERIAL.ID);
                var materialtextos = db.MATERIALTs.Where(t => t.MATERIAL_ID == mATERIAL.ID).ToList();
                db.MATERIALTs.RemoveRange(materialtextos);
                List<MATERIALT> ListmATERIALTs = new List<MATERIALT>();
                if (collection.AllKeys.Contains("EN"))
                {
                    if (!String.IsNullOrEmpty(collection["EN"]))
                    {
                        MATERIALT m = new MATERIALT { SPRAS = "EN", MATERIAL_ID = mATERIAL.ID, MAKTX = collection["EN"], MAKTG = collection["EN"].ToUpper() };
                        ListmATERIALTs.Add(m);
                    }
                        if (mATERIAL1.MAKTX != collection["EN"])
                        {
                            mATERIAL1.MAKTX = collection["EN"];
                            mATERIAL1.MAKTG = Convert.ToString(collection["EN"]).ToUpper();
                        }
                }
                if (collection.AllKeys.Contains("ES")&& !String.IsNullOrEmpty(collection["ES"]))
                    {
                        MATERIALT m = new MATERIALT { SPRAS = "ES", MATERIAL_ID = mATERIAL.ID, MAKTX = collection["ES"], MAKTG = Convert.ToString(collection["ES"]).ToUpper() };
                        ListmATERIALTs.Add(m);
                    }
                if (collection.AllKeys.Contains("PT") && !String.IsNullOrEmpty(collection["PT"]))
                {
                    MATERIALT m = new MATERIALT { SPRAS = "PT", MATERIAL_ID = mATERIAL.ID, MAKTX = collection["PT"], MAKTG = Convert.ToString(collection["PT"]).ToUpper() };
                    ListmATERIALTs.Add(m);
                }
                db.MATERIALTs.AddRange(ListmATERIALTs);
                //db.Entry(mATERIAL).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(mATERIAL);
        }

        // GET: Materiales/Delete/5
        public ActionResult Delete(string id, string vkorg, string vtweg )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATERIALVKE mATERIAL = db.MATERIALVKEs.Find(id,vkorg,vtweg);
            if (mATERIAL == null)
            {
                return HttpNotFound();
            }
            mATERIAL.ACTIVO = false;
            db.SaveChanges();
            var material = db.MATERIALs.Where(t => t.ID == id).SingleOrDefault();
            if (material.MATERIALVKEs.Where(t=>t.ACTIVO==true).ToList().Count == 0)
                material.ACTIVO = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Materiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MATERIAL mATERIAL = db.MATERIALs.Find(id);
            mATERIAL.ACTIVO = false;
            db.SaveChanges();
            return RedirectToAction("Index");
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
