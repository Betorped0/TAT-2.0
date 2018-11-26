using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Filters;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    [LoginActive]
    public class SociedadesController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Sociedades
        public ActionResult Index()
        {
            ObtenerConfPage(920);//ID EN BASE DE DATOS
            var sOCIEDADs = db.SOCIEDADs;
            return View(sOCIEDADs.ToList());
        }

        // GET: Sociedades/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObtenerConfPage(923);
            SOCIEDAD sOCIEDAD = db.SOCIEDADs.Find(id);
            if (sOCIEDAD == null)
            {
                return HttpNotFound();
            }
            return View(sOCIEDAD);
        }

        // GET: Sociedades/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObtenerConfPage(922);
            SOCIEDAD sOCIEDAD = db.SOCIEDADs.Find(id);
            if (sOCIEDAD == null)
            {
                return HttpNotFound();
            }
            //if (sOCIEDAD.REGION != null)
              //  sOCIEDAD.REGION= sOCIEDAD.REGION.TrimEnd();
            //ViewBag.REGION = new SelectList(db.REGIONs.Where(x=>x.SOCIEDAD==id).ToList(), "REGION1", "REGION1", sOCIEDAD.REGION!=null?sOCIEDAD.REGION.TrimEnd():"");

            return View(sOCIEDAD);
        }

        // POST: Sociedades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BUKRS,BUTXT,ORT01,LAND,SUBREGIO,WAERS,SPRAS,NAME1,KTOPL,ACTIVO,REGION")] SOCIEDAD sOCIEDAD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sOCIEDAD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ObtenerConfPage(922);
            //if (sOCIEDAD.REGION != null)
            //    sOCIEDAD.REGION = sOCIEDAD.REGION.TrimEnd();
            //ViewBag.REGION = new SelectList(db.REGIONs.Where(x => x.SOCIEDAD == sOCIEDAD.BUKRS).ToList(), "REGION1", "REGION1", sOCIEDAD.REGION != null ? sOCIEDAD.REGION.TrimEnd() : "");
            return View(sOCIEDAD);
        }

        public ActionResult Flujos(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObtenerConfPage(923);
            List<DET_APROBH> flujos = db.DET_APROBH.Where(t=>t.SOCIEDAD_ID==id).ToList();
            ViewBag.coCode = id;
            return View(flujos);
        }
        public ActionResult CreateF(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObtenerConfPage(923);
            var flujos = db.DET_APROBH.Where(t => t.SOCIEDAD_ID == id && t.ACTIVO).Select(t=>t.PUESTOC_ID).Distinct().ToList();
            var lan =ViewBag.usuario.SPRAS_ID;
            var puestos = db.PUESTOes.Where(t => t.ACTIVO == true && !flujos.Contains(t.ID)).ToList();
            var sl_puestos = puestos.Select(x => new { x.ID,Puesto= x.PUESTOTs.Count>0? x.PUESTOTs.Where(t=>t.SPRAS_ID==lan).FirstOrDefault().TXT50 : ""}).ToList();
            ViewBag.PUESTOC_ID = new SelectList(sl_puestos, "ID", "Puesto");
            var version= db.DET_APROBH.Where(t => t.SOCIEDAD_ID == id).OrderByDescending(t=>t.VERSION).FirstOrDefault();

            DET_APROBH dET_APROBP = new DET_APROBH { SOCIEDAD_ID = id, ACTIVO=true, VERSION=version!=null?version.VERSION:1 };
            return View(dET_APROBP);
        }
        public ActionResult EditF(string id, int pid, int v)
        {
            if (id == null || pid==0||v==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObtenerConfPage(923);
            DET_APROBH flujo = db.DET_APROBH.Where(t => t.SOCIEDAD_ID == id && t.PUESTOC_ID==pid && t.VERSION==v).SingleOrDefault();

            return View(flujo);
        }
        [HttpPost]
        public ActionResult EditF([Bind(Include = "ACTIVO")]DET_APROBH modelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Flujos", new {modelo.SOCIEDAD_ID });
            }
            ObtenerConfPage(923);
            return View(modelo);
        }
        public ActionResult MAFlujos(string id, int pid, int v)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ObtenerConfPage(923);
            List<DET_APROBP> flujos = db.DET_APROBP.Where(t => t.SOCIEDAD_ID == id && t.PUESTOC_ID==pid && t.VERSION==v).ToList();

            return View(flujos);
        }
        void ObtenerConfPage(int pagina)//ID EN BASE DE DATOS
        {
            var user = ObtenerUsuario();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

        }
        USUARIO ObtenerUsuario()
        {
            string u = User.Identity.Name;
            return db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
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
