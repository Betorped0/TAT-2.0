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

namespace TAT001.Controllers
{
    public class ConsoporteController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        // GET: Consoporte
        public ActionResult Index(string tsol)
        {
            int pagina_id = 841; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

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

            var cONSOPORTEs = db.CONSOPORTEs.Include(c => c.TSOL).Include(c => c.TSOPORTE);
            return View(cONSOPORTEs.ToList());
        }

        // GET: Consoporte/Details/5
        public ActionResult Details(string tsol, string tsopo)
        {
            int pagina_id = 843; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }

            if (tsol == null || tsopo == null)
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
            int pagina_id = 843; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

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

            var list = db.CONSOPORTEs.Where(x => x.TSOL_ID == tsol).Select(x => x.TSOPORTE_ID).ToList();
            ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs.Where(x => !list.Contains(x.ID)).ToList(), "ID", "DESCRIPCION");
            ViewBag.TSOL_ID = new SelectList(db.TSOLs.Where(x => !list.Contains(x.ID)).ToList(), "ID", "DESCRIPCION");
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
            int pagina_id = 843; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

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

            if (tsol == null || tsopo == null)
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
                db.Entry(cONSOPORTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { tsol = cONSOPORTE.TSOL_ID });
            }
            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", cONSOPORTE.TSOL_ID);
            ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", cONSOPORTE.TSOPORTE_ID);
            return View(cONSOPORTE.TSOL_ID, cONSOPORTE.TSOPORTE_ID);
        }
        

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
