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
    [Authorize]
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
