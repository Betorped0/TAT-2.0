using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Controllers
{
    public class ConsoporteController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Consoporte
        public ActionResult Index(string tsol)
        {
            int pagina = 841; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.IdTsol = tsol;

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            var cONSOPORTEs = db.CONSOPORTEs.Include(c => c.TSOL).Include(c => c.TSOPORTE).Where(c => c.TSOL_ID == tsol);
            return View(cONSOPORTEs.ToList());
        }

        // GET: Consoporte/Details/5
        public ActionResult Details(string tsol, string tsopo)
        {
            int pagina = 843; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(842)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID) && b.ID == 842).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            if (tsol == null | tsopo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONSOPORTE cONSOPORTE = db.CONSOPORTEs.Where(x => x.TSOL_ID == tsol && x.TSOPORTE_ID == tsopo).First();
            if (cONSOPORTE == null)
            {
                return HttpNotFound();
            }
            return View(cONSOPORTE);
        }

        // GET: Consoporte/Create
        public ActionResult Create(string tsol)
        {
            int pagina = 843; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.IdTsol = tsol;
            ViewBag.activo = true;

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            var list = db.CONSOPORTEs.Where(x => x.TSOL_ID == tsol).Select(x => x.TSOPORTE_ID).ToList();
            ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs.Where(x => !list.Contains(x.ID)).ToList(), "ID", "DESCRIPCION");
            ViewBag.TSOL_ID = new SelectList(db.TSOLs.Where(x => x.ID == tsol), "ID", "DESCRIPCION");
            //ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION");
            return View();
        }

        // POST: Consoporte/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TSOL_ID,TSOPORTE_ID,OBLIGATORIO,ACTIVO")] CONSOPORTE cONSOPORTE)
        {
            if (ModelState.IsValid)
            {
                cONSOPORTE.ACTIVO = true;
                db.CONSOPORTEs.Add(cONSOPORTE);
                db.SaveChanges();
                return RedirectToAction("Index", new { tsol = cONSOPORTE.TSOL_ID });
            }

            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", cONSOPORTE.TSOL_ID);
            ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", cONSOPORTE.TSOPORTE_ID);
            return View(cONSOPORTE);
        }

        // GET: Consoporte/Edit/5
        public ActionResult Edit(string tsol, string tsopo)
        {
            int pagina = 843; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(844)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID) && b.ID == 844).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.IdTsol = tsol;
            ViewBag.activo = true;

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            if (tsol == null | tsopo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONSOPORTE cONSOPORTE = db.CONSOPORTEs.Where(x => x.TSOL_ID == tsol && x.TSOPORTE_ID == tsopo).First();
            if (cONSOPORTE == null)
            {
                return HttpNotFound();
            }
            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", cONSOPORTE.TSOL_ID);
            ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", cONSOPORTE.TSOPORTE_ID);
            return View(cONSOPORTE);
        }

        // POST: Consoporte/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TSOL_ID,TSOPORTE_ID,OBLIGATORIO,ACTIVO")] CONSOPORTE cONSOPORTE)
        {
            if (ModelState.IsValid)
            {
                cONSOPORTE.ACTIVO = true;
                db.Entry(cONSOPORTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { tsol = cONSOPORTE.TSOL_ID });
            }
            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", cONSOPORTE.TSOL_ID);
            ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", cONSOPORTE.TSOPORTE_ID);
            return View(cONSOPORTE.TSOL_ID, cONSOPORTE.TSOPORTE_ID);
        }

        // GET: Consoporte/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CONSOPORTE cONSOPORTE = db.CONSOPORTEs.Find(id);
        //    if (cONSOPORTE == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cONSOPORTE);
        //}

        // POST: Consoporte/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(string tsol, string tsopo)
        {
            CONSOPORTE cONSOPORTE = db.CONSOPORTEs.Where(x => x.TSOL_ID == tsol && x.TSOPORTE_ID == tsopo).First();
            db.CONSOPORTEs.Remove(cONSOPORTE);
            db.SaveChanges();
            return RedirectToAction("Index", new { tsol = cONSOPORTE.TSOL_ID });
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
