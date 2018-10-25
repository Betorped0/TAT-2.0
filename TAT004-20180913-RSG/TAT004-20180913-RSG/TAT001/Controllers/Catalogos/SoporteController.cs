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
    public class SoporteController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Consoporte
        public ActionResult Index()
        {
            int pagina = 1314; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            //ViewBag.IdTsol = id;

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

            var conSoporte = (from x in db.TSOLTs

                              join CONSOPOR in db.CONSOPORTEs on x.TSOL_ID equals CONSOPOR.TSOL_ID
                              join TSOPORTE in db.TSOPORTEs on CONSOPOR.TSOPORTE_ID equals TSOPORTE.ID
                              where TSOPORTE.ACTIVO == true && CONSOPOR.ACTIVO == true && TSOPORTE.ID.Equals(TSOPORTE.ID) && x.SPRAS_ID.Equals(user.SPRAS_ID)
                              select x.TXT020).ToList();

            ViewBag.conSoporte = conSoporte.ToList();

            var cONSOPORTEs = db.TSOPORTEs;
            return View(cONSOPORTEs.ToList());
        }

        // GET: Consoporte/Details/5
        public ActionResult Details(string id)
        {
            int pagina = 1315; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID) && b.ID == pagina).FirstOrDefault().TXT50;
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var sopor = db.TSOPORTEs.Where(x => x.ID == id).FirstOrDefault();
            //TSOPORTE tSOPORTE = db.TSOPORTEs.Find(idtsopo);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(sopor);
        }

        // GET: Consoporte/Create
        public ActionResult Create()
        {
            int pagina = 1316; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

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

            //var list = db.CONSOPORTEs.Where(x => x.TSOL_ID == tsol).Select(x => x.TSOPORTE_ID).ToList();
            //ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs.Where(x => !list.Contains(x.ID)).ToList(), "ID", "DESCRIPCION", "ACTIVO");
            //ViewBag.TSOL_ID = new SelectList(db.TSOLs.Where(x => x.ID == tsol), "ID", "DESCRIPCION");
            //ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION");
            return View();
        }

        // POST: Consoporte/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DESCRIPCION,ACTIVO")] TSOPORTE tSOPORTE)
        {
            if (ModelState.IsValid)
            {
                //TSOPORTE TS = new TSOPORTE();
                //tSOPORTE.ACTIVO = true;
                db.TSOPORTEs.Add(tSOPORTE);
                db.SaveChanges();
                List<SPRA> ss = db.SPRAS.ToList();

                foreach (SPRA s in ss)
                {
                    TSOPORTET pt = new TSOPORTET();
                    pt.TSOPORTE_ID = tSOPORTE.ID;
                    pt.SPRAS_ID = s.ID;
                    pt.TXT50 = tSOPORTE.DESCRIPCION;
                    db.TSOPORTETs.Add(pt);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           

            int pagina = 1316; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

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


            //ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", tSOPORTE.ID);
            //ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", tSOPORTET.);
            return View(tSOPORTE);
        }

        // GET: Consoporte/Edit/5
        public ActionResult Edit(string id)
        {
            int pagina = 1317; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID) && b.ID == pagina).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            //ViewBag.IdTsol = tsol;
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

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TSOPORTE tSOPORTE = db.TSOPORTEs.Where(x => x.ID == id).First();
            var des = db.TSOPORTEs.Where(x => x.ID == id).FirstOrDefault();
            if (des == null)
            {
                return HttpNotFound();
            }
            TSOPORTE tSOPORTE = db.TSOPORTEs.Find(id);
            if (tSOPORTE.TSOPORTETs.Count > 0)
                foreach (var e in tSOPORTE.TSOPORTETs)
                {
                    if (e.SPRAS_ID == "EN")
                        ViewBag.EN = e.TXT50;
                    if (e.SPRAS_ID == "ES")
                        ViewBag.ES = e.TXT50;
                    if (e.SPRAS_ID == "PT")
                        ViewBag.PT = e.TXT50;
                }
            else
            {
                ViewBag.EN = tSOPORTE.DESCRIPCION;
            }

            //ViewBag.SPRAS = db.SPRAS.ToList();
            //ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", cONSOPORTE.TSOL_ID);
            //ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", tSOPORTE.ID);
            return View(tSOPORTE);
        }

        // POST: Consoporte/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DESCRIPCION,ACTIVO")] TSOPORTE tSOPORTE, FormCollection collection)
        {

            if (ModelState.IsValid)
            {
                db.Entry(tSOPORTE).State = EntityState.Modified;
                db.SaveChanges();

                TSOPORTE mATERIAL1 = db.TSOPORTEs.Find(tSOPORTE.ID);
                var materialtextos = db.TSOPORTETs.Where(t => t.TSOPORTE_ID == tSOPORTE.ID).ToList();
                db.TSOPORTETs.RemoveRange(materialtextos);
                List<TSOPORTET> ListmATERIALTs = new List<TSOPORTET>();
                if (collection.AllKeys.Contains("EN"))
                {
                    if (!String.IsNullOrEmpty(collection["EN"]))
                    {
                        TSOPORTET m = new TSOPORTET { SPRAS_ID = "EN", TSOPORTE_ID = tSOPORTE.ID, TXT50 = collection["EN"].ToUpper() };
                        ListmATERIALTs.Add(m);
                    }
                    if (mATERIAL1.DESCRIPCION != collection["EN"])
                    {
                        mATERIAL1.DESCRIPCION = collection["EN"];
                        //mATERIAL1.MAKTG = Convert.ToString(collection["EN"]).ToUpper();
                    }
                }
                if (collection.AllKeys.Contains("ES") && !String.IsNullOrEmpty(collection["ES"]))
                {
                    TSOPORTET m = new TSOPORTET { SPRAS_ID = "ES", TSOPORTE_ID = tSOPORTE.ID, TXT50 = collection["ES"]/*, MAKTG = Convert.ToString(collection["ES"])*/.ToUpper() };
                    ListmATERIALTs.Add(m);
                }
                if (collection.AllKeys.Contains("PT") && !String.IsNullOrEmpty(collection["PT"]))
                {
                    TSOPORTET m = new TSOPORTET { SPRAS_ID = "PT", TSOPORTE_ID = tSOPORTE.ID, TXT50 = collection["PT"]/*, MAKTG = Convert.ToString(collection["PT"])*/.ToUpper() };
                    ListmATERIALTs.Add(m);
                }
                db.TSOPORTETs.AddRange(ListmATERIALTs);
                //db.Entry(mATERIAL).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ////TSOPORTE TS = new TSOPORTE();
            //tSOPORTE.ACTIVO = true;
            ////db.TSOPORTEs.Add(tSOPORTE);
            ////db.SaveChanges();
            ////List<SPRA> ss = db.SPRAS.ToList();

            ////foreach (SPRA s in ss)
            ////{
            //try
            //{


            //    TSOPORTET pt = new TSOPORTET();
            //    pt.TSOPORTE_ID = tSOPORTE.ID;                
            //    pt.TXT50 = sp;
            //    pt.SPRAS_ID = txtN;
            //    db.Entry(pt).State = EntityState.Modified;
            //    db.SaveChanges();
            //}
            //catch
            //{
            //}              
            //}



            int pagina = 1317; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID) && b.ID == pagina).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
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
        }

            //ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", tSOPORTE.ID);
            //ViewBag.TSOPORTE_ID = new SelectList(db.TSOPORTEs, "ID", "DESCRIPCION", tSOPORTE.ID);
            return View(tSOPORTE);
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
