using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Controllers.Catalogos
{
    public class MaterialesController : Controller
    {
        private TAT004Entities db = new TAT004Entities();

        // GET: Materiales
        public ActionResult Index()
        {
            int pagina = 661; //ID EN BASE DE DATOS
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

            var mATERIALs = db.MATERIALs.Include(m => m.MATERIALGP);
            return View(mATERIALs.Where(x => x.ACTIVO == true).ToList());
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
            foreach(var e in mATERIAL.MATERIALTs) {
                if (e.SPRAS == "EN")
                    ViewBag.EN = e.MAKTX;
                if(e.SPRAS=="ES")
                    ViewBag.ES = e.MAKTX;
                if (e.SPRAS == "PT")
                    ViewBag.PT = e.MAKTX;
            }
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
                    MATERIALT m = new MATERIALT { SPRAS = "EN", MATERIAL_ID = mATERIAL.ID, MAKTX = mATERIAL1.MAKTX, MAKTG = mATERIAL1.MAKTG };
                    ListmATERIALTs.Add(m);
                    if (mATERIAL1.MAKTX != collection["EN"])
                    {
                        mATERIAL1.MAKTX = collection["EN"];
                        mATERIAL1.MAKTG = Convert.ToString(collection["EN"]).ToUpper();
                    }
                }
                if (collection.AllKeys.Contains("ES"))
                    {
                        MATERIALT m = new MATERIALT { SPRAS = "ES", MATERIAL_ID = mATERIAL.ID, MAKTX = collection["ES"], MAKTG = Convert.ToString(collection["ES"]).ToUpper() };
                        ListmATERIALTs.Add(m);
                    }
                if (collection.AllKeys.Contains("PT"))
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
        public ActionResult Delete(string id)
        {
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
