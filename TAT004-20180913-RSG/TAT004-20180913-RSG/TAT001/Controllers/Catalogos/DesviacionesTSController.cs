using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    public class DesviacionesTSController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: DesviacionesTS
        public ActionResult Index()
        {
            int pagina = 1310; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
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
                    //ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            var tS_FORMT = db.TS_FORMT.Include(t => t.SPRA).Include(t => t.TS_CAMPO);
            return View(tS_FORMT.ToList());
        }

        // GET: DesviacionesTS/Details/5
        public ActionResult Details(int? id)
        {
            int pagina = 1311; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
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
                    //ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TS_FORMT tS_FORMT = db.TS_FORMT.Find(id);
            //if (tS_FORMT == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(tS_FORMT);

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
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
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
                    //ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            //ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION");
            //ViewBag.TSFORM_ID = new SelectList(db.TS_CAMPO, "ID", "ID");
            return View();
        }

        // POST: DesviacionesTS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SPRAS_ID,TSFORM_ID,TXT100")] TS_FORMT tS_FORMT)
        {

            if (ModelState.IsValid)
            {
                TS_CAMPO TS = new TS_CAMPO();
                TS.ACTIVO = true;
                db.TS_CAMPO.Add(TS);
                db.SaveChanges();
                List<SPRA> ss = db.SPRAS.ToList();

                foreach (SPRA s in ss)
                {
                    TS_FORMT pt = new TS_FORMT();
                    pt.TSFORM_ID = TS.ID;
                    pt.SPRAS_ID = s.ID;
                    pt.TXT100 = tS_FORMT.TXT100;
                    db.TS_FORMT.Add(pt);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int pagina = 1312; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
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
                    //ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            //if (ModelState.IsValid)
            //{
            //    db.TS_FORMT.Add(tS_FORMT);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION", tS_FORMT.SPRAS_ID);
            //ViewBag.TSFORM_ID = new SelectList(db.TS_CAMPO, "ID", "ID", tS_FORMT.TSFORM_ID);
            return View(tS_FORMT);
        }

        // GET: DesviacionesTS/Edit/5
        public ActionResult Edit(int id)
        {
            int pagina = 1313; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
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
                    //ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var des = db.TS_CAMPO.Where(x => x.ID == id).FirstOrDefault();
            //TS_FORMT tS_FORMT = db.TS_FORMT.Find(id);
            if (des == null)
            {
                return HttpNotFound();
            }
            TS_CAMPO tSOPORTE = db.TS_CAMPO.Find(id);
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
            else
            { }
            //ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION", tS_FORMT.SPRAS_ID);
            //ViewBag.TSFORM_ID = new SelectList(db.TS_CAMPO, "ID", "ID", tS_FORMT.TSFORM_ID);
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
                TS_CAMPO mATERIAL1 = db.TS_CAMPO.Find(tc.ID);
                var materialtextos = db.TS_FORMT.Where(t => t.TSFORM_ID == tc.ID).ToList();
                db.TS_FORMT.RemoveRange(materialtextos);
                List<TS_FORMT> ListmATERIALTs = new List<TS_FORMT>();
                if (collection.AllKeys.Contains("EN"))
                {
                    if (!String.IsNullOrEmpty(collection["EN"]))
                    {
                        TS_FORMT m = new TS_FORMT { SPRAS_ID = "EN", TSFORM_ID = tc.ID, TXT100 = collection["EN"].ToUpper() };
                        ListmATERIALTs.Add(m);
                    }
                    //if (mATERIAL1.DESCRIPCION != collection["EN"])
                    //{
                    //    mATERIAL1.DESCRIPCION = collection["EN"];
                    //    //mATERIAL1.MAKTG = Convert.ToString(collection["EN"]).ToUpper();
                    //}
                }
                if (collection.AllKeys.Contains("ES") && !String.IsNullOrEmpty(collection["ES"]))
                {
                    TS_FORMT m = new TS_FORMT { SPRAS_ID = "ES", TSFORM_ID = tc.ID, TXT100 = collection["ES"]/*, MAKTG = Convert.ToString(collection["ES"])*/.ToUpper() };
                    ListmATERIALTs.Add(m);
                }
                if (collection.AllKeys.Contains("PT") && !String.IsNullOrEmpty(collection["PT"]))
                {
                    TS_FORMT m = new TS_FORMT { SPRAS_ID = "PT", TSFORM_ID = tc.ID, TXT100 = collection["PT"]/*, MAKTG = Convert.ToString(collection["PT"])*/.ToUpper() };
                    ListmATERIALTs.Add(m);
                }
                db.TS_FORMT.AddRange(ListmATERIALTs);
                //db.Entry(mATERIAL).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        

            int pagina = 1313; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
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
                    //ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            ViewBag.des = db.TSOLTs.ToList();
            //ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION", tS_FORMT.SPRAS_ID);
            //ViewBag.TSFORM_ID = new SelectList(db.TS_CAMPO, "ID", "ID", tS_FORMT.TSFORM_ID);
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
