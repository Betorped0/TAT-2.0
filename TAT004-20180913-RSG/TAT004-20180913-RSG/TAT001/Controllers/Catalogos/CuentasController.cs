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
    public class CuentasController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Cuentas
        public ActionResult Index()
        {
            var cUENTAs = db.CUENTAs.Include(c => c.PAI).Include(c => c.SOCIEDAD).Include(c => c.TALL);
            return View(cUENTAs.ToList());
        }

        // GET: Cuentas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUENTA cUENTA = db.CUENTAs.Find(id);
            if (cUENTA == null)
            {
                return HttpNotFound();
            }
            return View(cUENTA);
        }

        // GET: Cuentas/Create
        public ActionResult Create()
        {
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS");
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT");
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION");
            return View();
        }

        // POST: Cuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SOCIEDAD_ID,PAIS_ID,TALL_ID,EJERCICIO,ABONO,CARGO,CLEARING,LIMITE")] CUENTA cUENTA)
        {
            if (ModelState.IsValid)
            {
                db.CUENTAs.Add(cUENTA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS", cUENTA.PAIS_ID);
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", cUENTA.SOCIEDAD_ID);
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION", cUENTA.TALL_ID);
            return View(cUENTA);
        }

        // GET: Cuentas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUENTA cUENTA = db.CUENTAs.Find(id);
            if (cUENTA == null)
            {
                return HttpNotFound();
            }
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS", cUENTA.PAIS_ID);
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", cUENTA.SOCIEDAD_ID);
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION", cUENTA.TALL_ID);
            return View(cUENTA);
        }

        // POST: Cuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SOCIEDAD_ID,PAIS_ID,TALL_ID,EJERCICIO,ABONO,CARGO,CLEARING,LIMITE")] CUENTA cUENTA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cUENTA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS", cUENTA.PAIS_ID);
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", cUENTA.SOCIEDAD_ID);
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION", cUENTA.TALL_ID);
            return View(cUENTA);
        }

        // GET: Cuentas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUENTA cUENTA = db.CUENTAs.Find(id);
            if (cUENTA == null)
            {
                return HttpNotFound();
            }
            return View(cUENTA);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CUENTA cUENTA = db.CUENTAs.Find(id);
            db.CUENTAs.Remove(cUENTA);
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
