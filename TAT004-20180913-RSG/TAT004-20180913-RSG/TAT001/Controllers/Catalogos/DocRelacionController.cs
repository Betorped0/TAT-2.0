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

namespace TAT001.Controllers.Catalogos
{
    public class DocRelacionController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        // GET: DocRelacion
        public ActionResult Index()
        {
            int pagina_id = 1210; //ID EN BASE DE DATOS
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
            
            return View(db.FACTURASCONFs.ToList());
        }

        // GET: DocRelacion/Details/5
        public ActionResult Details(string id, string pais, string tsol)
        {
            int pagina_id = 1212; //ID EN BASE DE DATOS
            int pagina2 = 1210;
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller,pagina2);
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
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rel = db.FACTURASCONFs.Where(x => x.SOCIEDAD_ID == id && x.PAIS_ID == pais && x.TSOL == tsol).FirstOrDefault();
             if (id == null)
            {
                return HttpNotFound();
            }
            return View(rel);
        }
        // GET: DocRelacion/Create
        public ActionResult Create()
        {
            int pagina_id = 1211; //ID EN BASE DE DATOS
            int pagina2 = 1210;
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller, pagina2);

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //return RedirectToAction("Pais", "Home");
                }
            return View();
        }

        // POST: DocRelacion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SOCIEDAD_ID,PAIS_ID,TSOL,FACTURA,FECHA,PROVEEDOR,CONTROL,AUTORIZACION,VENCIMIENTO,FACTURAK,EJERCICIOK,BILL_DOC,BELNR,IMPORTE_FAC,PAYER,DESCRIPCION,SOCIEDAD,ACTIVO")] FACTURASCONF fACTURASCONF)
        {
            try
            {
                db.FACTURASCONFs.Add(fACTURASCONF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                int pagina_id = 1211; //ID EN BASE DE DATOS
                int pagina2 = 1210; //ID EN BASE DE DATOS
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller, pagina2);

            }

            
            return View(fACTURASCONF);
        }

        // GET: DocRelacion/Edit/5
        public ActionResult Edit(string id, string pais, string tsol)
        {
            int pagina_id = 1213; //ID EN BASE DE DATOS
            int pagina2 = 1210;
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller, pagina2);

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //return RedirectToAction("Pais", "Home");
                }
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rel = db.FACTURASCONFs.Where(x => x.SOCIEDAD_ID == id && x.PAIS_ID == pais && x.TSOL == tsol).FirstOrDefault();
    
            if (id == null)
            {
                return HttpNotFound();
            }

            return View(rel);
        }

        // POST: DocRelacion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SOCIEDAD_ID,PAIS_ID,TSOL,FACTURA,FECHA,PROVEEDOR,CONTROL,AUTORIZACION,VENCIMIENTO,FACTURAK,EJERCICIOK,BILL_DOC,BELNR,IMPORTE_FAC,PAYER,DESCRIPCION,SOCIEDAD,ACTIVO")] FACTURASCONF fACTURASCONF)
        {
            if (ModelState.IsValid)
            {       
                db.Entry(fACTURASCONF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int pagina_id = 1213; //ID EN BASE DE DATOS
            int pagina2 = 1210;
                FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller, pagina2);

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //return RedirectToAction("Pais", "Home");
                }
            
            return View(fACTURASCONF);
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
