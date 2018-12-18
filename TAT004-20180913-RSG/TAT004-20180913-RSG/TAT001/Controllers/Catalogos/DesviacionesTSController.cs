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
using TAT001.Models.Dao;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    [LoginActive]
    public class DesviacionesTSController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();

        //------------------DAO------------------------------
        readonly SociedadesDao sociedadesDao = new SociedadesDao();

        // GET: DesviacionesTS
        public ActionResult Index()
        {
            int pagina = 1310; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //ViewBag.pais = "mx.png";
                //return RedirectToAction("Pais", "Home");
            }


            var campos = db.TS_CAMPO.ToList();
            return View(campos);
        }

        // GET: DesviacionesTS/Details/5
        public ActionResult Details(int? id)
        {
            int pagina = 1311; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //ViewBag.pais = "mx.png";
                //return RedirectToAction("Pais", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var des = db.TS_CAMPO.Where(x => x.ID == id).FirstOrDefault();
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(des);
        }

        // GET: DesviacionesTS/Create
        public ActionResult Create()
        {
            int pagina = 1312; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //ViewBag.pais = "mx.png";
                //return RedirectToAction("Pais", "Home");
            }
            ViewBag.SOCIEDADES = sociedadesDao.ComboSociedades(TATConstantes.ACCION_LISTA_SOCIEDADES);
            return View();
        }

        // POST: DesviacionesTS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID")] TS_FORM TSFORM, FormCollection collection)
        {

            int pagina = 1312; //ID EN BASE DE DATOS
            try
            {

                if (ModelState.IsValid)
                {
                    string socSelectedStr = collection["sociedadesSelected"];
                    List<string> socSelected = socSelectedStr.Split(',').ToList();

                    foreach (string soc in socSelected) {
                        TS_CAMPO TS = new TS_CAMPO
                        {
                            ACTIVO = true,
                            ID = TSFORM.ID
                        };
                        db.TS_CAMPO.Add(TS);
                        db.SaveChanges();
                        string land = db.SOCIEDADs.Find(soc).LAND;
                
                        db.TS_FORM.Add(new TS_FORM
                        {
                            POS = TS.ID,
                            CAMPO = TS.ID.ToString(),
                            ID = TS.ID,
                            BUKRS_ID = soc,
                            LAND_ID = land
                        });
                        db.SaveChanges();

                        List<TS_FORMT> listTextos = new List<TS_FORMT>();
                        if (collection.AllKeys.Contains("EN") && !String.IsNullOrEmpty(collection["EN"]))
                        {
                            listTextos.Add(new TS_FORMT { SPRAS_ID = "EN", TSFORM_ID = TS.ID, TXT100 = collection["EN"] });
                        }
                        if (collection.AllKeys.Contains("ES") && !String.IsNullOrEmpty(collection["ES"]))
                        {
                            listTextos.Add(new TS_FORMT { SPRAS_ID = "ES", TSFORM_ID = TS.ID, TXT100 = collection["ES"] });
                        }
                        db.TS_FORMT.AddRange(listTextos);
                        db.SaveChanges();
                    }
                    
                    return RedirectToAction("Index");
                }
                
                FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);

               

                return View(TSFORM);
            }
            catch (Exception e)
            {
                Log.ErrorLogApp(e,"DesviacionesTS","Create");
                FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
                ViewBag.SOCIEDADES = sociedadesDao.ComboSociedades(TATConstantes.ACCION_LISTA_SOCIEDADES);
                return View(TSFORM);
            }
        }

        // GET: DesviacionesTS/Edit/5
        public ActionResult Edit(int id)
        {
            int pagina = 1313; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //ViewBag.pais = "mx.png";
                //return RedirectToAction("Pais", "Home");
            }

            var des = db.TS_CAMPO.Where(x => x.ID == id).FirstOrDefault();
            if (des == null)
            {
                return HttpNotFound();
            }
            TS_CAMPO tSOPORTE = db.TS_CAMPO.Find(id);
            tSOPORTE.TS_FORM = db.TS_FORM.Where(x => x.ID == id).ToList();
            if (tSOPORTE.TS_FORMT.Count > 0)
                foreach (var e in tSOPORTE.TS_FORMT)
                {
                    if (e.SPRAS_ID == "EN")
                        ViewBag.EN = e.TXT100;
                    if (e.SPRAS_ID == "ES")
                        ViewBag.ES = e.TXT100;
                    if (e.SPRAS_ID == "PT")
                        ViewBag.PT = e.TXT100;
                }
            ViewBag.SPRAS = db.SPRAS.ToList();
            ViewBag.des = db.TSOLTs.ToList();
            return View(tSOPORTE);
        }

        // POST: DesviacionesTS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ACTIVO")] TS_CAMPO tc, FormCollection collection)
        {
            if (ModelState.IsValid)
            {

                db.Entry(tc).State = EntityState.Modified;
                
                var materialtextos = db.TS_FORMT.Where(t => t.TSFORM_ID == tc.ID).ToList();
                db.TS_FORMT.RemoveRange(materialtextos);
                List<TS_FORMT> ListmATERIALTs = new List<TS_FORMT>();
                if (collection.AllKeys.Contains("EN") && !String.IsNullOrEmpty(collection["EN"]))
                {
                    TS_FORMT m = new TS_FORMT { SPRAS_ID = "EN", TSFORM_ID = tc.ID, TXT100 = collection["EN"] };
                    ListmATERIALTs.Add(m);

                }
                if (collection.AllKeys.Contains("ES") && !String.IsNullOrEmpty(collection["ES"]))
                {
                    TS_FORMT m = new TS_FORMT { SPRAS_ID = "ES", TSFORM_ID = tc.ID, TXT100 = collection["ES"] };
                    ListmATERIALTs.Add(m);
                }
                if (collection.AllKeys.Contains("PT") && !String.IsNullOrEmpty(collection["PT"]))
                {
                    TS_FORMT m = new TS_FORMT { SPRAS_ID = "PT", TSFORM_ID = tc.ID, TXT100 = collection["PT"] };
                    ListmATERIALTs.Add(m);
                }
                db.TS_FORMT.AddRange(ListmATERIALTs);

                db.SaveChanges();
                return RedirectToAction("Index");
            }


            int pagina = 1313; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //ViewBag.pais = "mx.png";
                //return RedirectToAction("Pais", "Home");
            }

            ViewBag.des = db.TSOLTs.ToList();
            return View(tc);
        }

        // GET: DesviacionesTS/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TS_FORMT tS_FORMT = db.TS_FORMT.Find(id);
            if (tS_FORMT == null)
            {
                return HttpNotFound();
            }
            return View(tS_FORMT);
        }

        // POST: DesviacionesTS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TS_FORMT tS_FORMT = db.TS_FORMT.Find(id);
            db.TS_FORMT.Remove(tS_FORMT);
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
