using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.HtmlControls;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    public class UsuariosController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Usuarios
        public ActionResult Index()
        {
            int pagina = 601; //ID EN BASE DE DATOS
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
            string spra = Session["spras"].ToString();
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50");
            ViewBag.ROLs = new SelectList(db.ROLs, "ID", "ID");
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION");
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS");

            var uSUARIOs = db.USUARIOs.Include(u => u.PUESTO).Include(u => u.SPRA);
            UsuarioNuevo un = new UsuarioNuevo();
            un.L = uSUARIOs;
            return View(un);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(string id)
        {
            int pagina = 603; //ID EN BASE DE DATOS
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
                pagina = pagina - 1;
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
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PUESTO_ID = new SelectList(db.PUESTOes, "ID", "ID", uSUARIO.PUESTO_ID);
            string spra = Session["spras"].ToString();
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "ID", uSUARIO.SPRAS_ID);
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            ViewBag.SOCIEDADES = db.SOCIEDADs;
            ViewBag.PAISES = db.PAIS.Where(a => a.SOCIEDAD_ID != null & a.ACTIVO == true).ToList();
            ViewBag.APROBADORES = db.DET_APROB.Where(a => a.BUKRS.Equals("KCMX") & a.PUESTOC_ID == uSUARIO.PUESTO_ID).ToList();
            return View(uSUARIO);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            int pagina = 602; //ID EN BASE DE DATOS
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
            string spra = Session["spras"].ToString();
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50");
            ViewBag.ROLs = new SelectList(db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra)), "ROL_ID", "TXT50");
            //ViewBag.ROLs = new SelectList(db.ROLs, "ID", "ID");
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION");
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS");
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PASS,NOMBRE,APELLIDO_P,APELLIDO_M,EMAIL,SPRAS_ID,ACTIVO,PUESTO_ID,MANAGER,BACKUP_ID,BUNIT,ROL")] Usuario uSUARIO)
        {
            if (ModelState.IsValid)
            {
                if (uSUARIO.PASS != null & uSUARIO.MANAGER != null)
                {
                    if (uSUARIO.PASS != "" & uSUARIO.MANAGER != "")
                    {
                        if (uSUARIO.PASS == uSUARIO.MANAGER)
                        {
                            Cryptography c = new Cryptography();
                            uSUARIO.PASS = c.Encrypt(uSUARIO.PASS);
                            USUARIO u = new USUARIO();
                            var ppd = u.GetType().GetProperties();
                            var ppv = uSUARIO.GetType().GetProperties();
                            foreach (var pv in ppv)
                            {
                                foreach (var pd in ppd)
                                {
                                    if (pd.Name == pv.Name)
                                    {
                                        pd.SetValue(u, pv.GetValue(uSUARIO));
                                        break;
                                    }
                                }
                            }
                            db.USUARIOs.Add(u);

                            ////MIEMBRO m = new MIEMBRO();
                            ////m.ROL_ID = int.Parse(Request.Form["ROLs"].ToString());
                            ////m.USUARIO_ID = uSUARIO.ID;
                            ////m.ACTIVO = true;
                            ////db.MIEMBROS.Add(m);

                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ViewBag.Error = "La contraseña no coincide";
                        }
                    }
                }
            }

            int pagina = 602; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
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
            string spra = Session["spras"].ToString();
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50");
            ViewBag.ROLs = new SelectList(db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra)), "ROL_ID", "TXT50");
            //ViewBag.ROLs = new SelectList(db.ROLs, "ID", "ID");
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION");
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS");

            return View(uSUARIO);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            int pagina = 603; //ID EN BASE DE DATOS
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
                pagina = pagina - 1;
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
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PUESTO_ID = new SelectList(db.PUESTOes, "ID", "ID", uSUARIO.PUESTO_ID);
            string spra = Session["spras"].ToString();
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "ID", uSUARIO.SPRAS_ID);
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            ViewBag.SOCIEDADES = db.SOCIEDADs;
            ViewBag.PAISES = db.PAIS;
            ViewBag.APROBADORES = db.DET_APROB.Where(a => a.BUKRS.Equals("KCMX") & a.PUESTOC_ID == uSUARIO.PUESTO_ID).ToList();
            return View(uSUARIO);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PASS,NOMBRE,APELLIDO_P,APELLIDO_M,EMAIL,SPRAS_ID,ACTIVO,PUESTO_ID,MANAGER,BACKUP_ID,BUNIT")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSUARIO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = uSUARIO.ID });
                //return RedirectToAction("Index");
            }
            int pagina = 603; //ID EN BASE DE DATOS
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
                pagina = pagina - 1;
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
            string spra = Session["spras"].ToString();
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "ID", uSUARIO.SPRAS_ID);
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            return View(uSUARIO);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            db.USUARIOs.Remove(uSUARIO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Usuarios/Edit/5
        public ActionResult Pass(string id)
        {
            int pagina = 604; //ID EN BASE DE DATOS
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
            Pass uSUARIO = new Pass();
            uSUARIO.ID = id;
            return View(uSUARIO);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Pass(/*[Bind(Include = "ID,pass,npass1,npass2")] */Pass pp)
        {
            Pass pass = new Pass();
            pass.ID = Request.Form.Get("ID");
            pass.pass = Request.Form.Get("pass");
            pass.npass1 = Request.Form.Get("npass1");
            pass.npass2 = Request.Form.Get("npass2");
            USUARIO us = db.USUARIOs.Find(pass.ID);
            Cryptography c = new Cryptography();
            string pass_a = c.Decrypt(us.PASS);
            if (pass.pass.Equals(pass_a))
            {
                if (pass.npass1.Equals(pass.npass2))
                {
                    if (ModelState.IsValid)
                    {
                        us.PASS = c.Encrypt(pass.npass1);
                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.message = "Los datos no coinciden";
                }
            }
            else
            {
                ViewBag.message = "Los datos no coinciden";
            }
            int pagina = 604; //ID EN BASE DE DATOS
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
            return View(pass);
        }
        public ActionResult AgregarRol(string id)
        {
            int pagina = 603; //ID EN BASE DE DATOS
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
                pagina = pagina - 1;
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
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            //ViewBag.PUESTO_ID = new SelectList(db.PUESTOes, "ID", "ID", uSUARIO.PUESTO_ID);
            string spra = Session["spras"].ToString();
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "ID", uSUARIO.SPRAS_ID);
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            ViewBag.SOCIEDADES = db.SOCIEDADs;
            ViewBag.PAISES = db.PAIS;
            return View(uSUARIO);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarRol(USUARIO u)
        {
            int rol = Int32.Parse(Request.Form["txt_rol"].ToString());
            string pais = Request.Form["txt_pai"].ToString().Split('-')[0];
            string vkorg = Request.Form["txt_vkor"].ToString();
            string vtweg = Request.Form["txt_vtwe"].ToString();
            string spart = Request.Form["txt_spar"].ToString();
            string kunnr = Request.Form["txt_clie"].ToString();
            string soc = Request.Form["txt_pai"].ToString().Split('-')[1];

            //MIEMBRO m = db.MIEMBROS.Where(a => a.USUARIO_ID.Equals(u.ID) & a.ROL_ID == rol).FirstOrDefault();
            //if (m == null)
            //{
            //    m = new MIEMBRO();
            //    m.ROL_ID = rol;
            //    m.USUARIO_ID = u.ID;
            //    m.ACTIVO = true;
            //    db.MIEMBROS.Add(m);
            //}

            ////List<DET_APROB> dd = db.DET_APROB.Where(a => a.PUESTOC_ID == u.PUESTO_ID & a.BUKRS.Equals(soc)).ToList();
            //GAUTORIZACION ga = new GAUTORIZACION();
            //ga.LAND = pais;
            //ga.BUKRS = soc;
            //ga.CLAVE = pais;
            //ga.NOMBRE = soc;
            ////db.GAUTORIZACIONs.Add(ga);
            //USUARIO user = db.USUARIOs.Find(u.ID);
            //user.GAUTORIZACIONs.Add(ga);
            //db.Entry(user).State = EntityState.Modified;
            //db.SaveChanges();

            if (vkorg != "" && vtweg != "" && spart != "")
            {

                DET_APROBH dh = db.DET_APROBH.Where(a => a.SOCIEDAD_ID.Equals(soc) & a.PUESTOC_ID == u.PUESTO_ID).OrderByDescending(a => a.VERSION).FirstOrDefault();
                if (dh != null)
                {
                    DET_AGENTEH dah = new DET_AGENTEH();
                    //dah.SOCIEDAD_ID = dh.SOCIEDAD_ID;
                    //dah.PUESTOC_ID = (int)u.PUESTO_ID;
                    //dah.VERSION = dh.VERSION;
                    //dah.AGROUP_ID = (int)ga.ID;
                    //dah.USUARIOC_ID = u.ID;
                    //dah.ACTIVO = true;
                    //db.DET_AGENTEH.Add(dah);
                    //db.SaveChanges();

                    List<DET_APROBP> ddp = db.DET_APROBP.Where(a => a.SOCIEDAD_ID.Equals(soc) & a.PUESTOC_ID == u.PUESTO_ID & a.VERSION == dh.VERSION).ToList();
                    foreach (DET_APROBP dp in ddp)
                    {
                        DET_AGENTEC dap = new DET_AGENTEC();
                        dap.USUARIOC_ID = u.ID;
                        dap.PAIS_ID = pais;
                        dap.VKORG = vkorg;
                        dap.VTWEG = vtweg;
                        dap.SPART = spart;
                        dap.KUNNR = kunnr;
                        dap.VERSION = dah.VERSION;
                        dap.POS = dp.POS;
                        dap.USUARIOA_ID = Request.Form["txt_p-" + dp.POS].ToString();
                        try
                        {
                            string pre = Request.Form["txt_presup-" + dp.POS].ToString();
                            if (pre == "on")
                                dap.PRESUPUESTO = true;

                        }
                        catch
                        {
                            dap.PRESUPUESTO = false;
                        }
                        try
                        {
                            string mon = Request.Form["txt_monto-" + dp.POS].ToString();
                            if (mon != "")
                                dap.MONTO = decimal.Parse(mon);

                        }
                        catch
                        {
                            dap.MONTO = null;
                        }

                        //dap.PRESUPUESTO = dp.PRESUPUESTO;
                        dap.ACTIVO = true;
                        db.DET_AGENTEC.Add(dap);
                        //dgp.Add(dap);

                        ////string us = dap.USUARIOA_ID;
                        ////USUARIO uu = db.USUARIOs.Find(us);
                        ////uu.GAUTORIZACIONs.Add(ga);
                        ////db.Entry(uu).State = EntityState.Modified;

                        ////MIEMBRO mi = db.MIEMBROS.Where(a => a.USUARIO_ID.Equals(uu.ID) & a.ROL_ID == 2).FirstOrDefault();
                        ////if (mi == null)
                        ////{
                        ////    mi = new MIEMBRO();
                        ////    mi.ROL_ID = 2;
                        ////    mi.USUARIO_ID = uu.ID;
                        ////    mi.ACTIVO = true;
                        ////    db.MIEMBROS.Add(mi);
                        ////}

                    }

                    TAX_LAND tl = db.TAX_LAND.Where(a => a.SOCIEDAD_ID.Equals(soc) & a.ACTIVO == true).FirstOrDefault();
                    if (tl != null)
                    {
                        DET_TAXEO dt = new DET_TAXEO();
                        dt.SOCIEDAD_ID = soc;
                        dt.PAIS_ID = pais;
                        dt.PUESTOC_ID = dah.PUESTOC_ID;
                        dt.USUARIOC_ID = dah.USUARIOC_ID;
                        dt.VERSION = dah.VERSION;
                        dt.PUESTOA_ID = 9;
                        dt.USUARIOA_ID = Request.Form["txt_p-9"].ToString();
                        dt.ACTIVO = true;
                        db.DET_TAXEO.Add(dt);
                    }

                    db.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { id = u.ID });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificarRol(USUARIO u)
        {
            //int rol = Int32.Parse(Request.Form["txt_rol"].ToString());
            string pais = Request.Form["item.PAIS_ID"].ToString();
            string vkorg = Request.Form["item.VKORG"].ToString();
            string vtweg = Request.Form["item.VTWEG"].ToString();
            string spart = Request.Form["item.SPART"].ToString();
            string kunnr = Request.Form["item.KUNNR"].ToString();

            //DET_AGENTEH dh = db.DET_AGENTEH.Where(a => a.SOCIEDAD_ID.Equals(soc) & a.PUESTOC_ID == u.PUESTO_ID & a.AGROUP_ID == (agroup)).OrderByDescending(a => a.VERSION).FirstOrDefault();
            DET_AGENTEC dh = db.DET_AGENTEC.Where(a => a.USUARIOC_ID.Equals(u.ID) & a.PAIS_ID.Equals(pais) & a.VKORG.Equals(vkorg) & a.VTWEG.Equals(vtweg) & a.SPART.Equals(spart) & a.KUNNR.Equals(kunnr) & a.POS == 1 & a.ACTIVO == true).OrderByDescending(a => a.VERSION).FirstOrDefault();
            if (dh != null)
            {

                List<DET_AGENTEC> ddp = db.DET_AGENTEC.Where(a => a.USUARIOC_ID.Equals(u.ID) & a.PAIS_ID.Equals(pais) & a.VKORG.Equals(vkorg) & a.VTWEG.Equals(vtweg)
                                        & a.SPART.Equals(spart) & a.KUNNR.Equals(kunnr) & a.VERSION == dh.VERSION & a.ACTIVO == true).ToList();
                foreach (DET_AGENTEC dp in ddp)
                {
                    //DET_AGENTEP dap = new DET_AGENTEP();
                    //dap.SOCIEDAD_ID = dh.SOCIEDAD_ID;
                    //dap.PUESTOC_ID = dh.PUESTOC_ID;
                    //dap.VERSION = dh.VERSION;
                    //dap.AGROUP_ID = dh.AGROUP_ID;
                    //dap.POS = dp.POS;
                    //dap.PUESTOA_ID = dp.PUESTOA_ID;
                    dp.USUARIOA_ID = Request.Form[pais + "-" + kunnr + "-" + dp.POS].ToString();
                    try
                    {
                        string isMonto = Request.Form[pais + "-" + kunnr + "-" + dp.POS + "-IsMonto"].ToString();
                        string monto = Request.Form[pais + "-" + kunnr + "-" + dp.POS + "-monto"].ToString();
                        if (monto != "")
                            dp.MONTO = decimal.Parse(monto);
                        else
                            dp.MONTO = null;
                    }
                    catch
                    {
                        dp.MONTO = null;
                    }
                    try
                    {
                        string presu = Request.Form[pais + "-" + kunnr + "-" + dp.POS + "-presup"].ToString();
                        if (presu == "on")
                            dp.PRESUPUESTO = true;
                        else
                            dp.PRESUPUESTO = false;
                    }
                    catch
                    {
                        dp.PRESUPUESTO = false;
                    }
                    dp.MONTO = dp.MONTO;
                    dp.PRESUPUESTO = dp.PRESUPUESTO;
                    dp.ACTIVO = true;
                    db.Entry(dp).State = EntityState.Modified;

                }
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = u.ID });

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Carga()
        {
            int pagina = 601;
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
            return View();
        }
        [HttpPost]
        public ActionResult Carga(IEnumerable<HttpPostedFileBase> files)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadExcel()
        {
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();


            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                string extension = System.IO.Path.GetExtension(file.FileName);
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream);
                DataSet result = reader.AsDataSet();
                DataTable dt = result.Tables[0];
                ld = objAList1(dt);

                reader.Close();
            }

            List<Usuarios> uu = new List<Usuarios>();
            List<USUARIO> usuarios = new List<USUARIO>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            List<PUESTO> puesto = new List<PUESTO>();
            List<SOCIEDAD> sociedad = new List<SOCIEDAD>();
            int rowst = ld.Count();
            int rowst2 = 1;
            string[] IDs = new string[rowst];
            int cont2= 0;
            string[,] tablas = new string[rowst, 11];
            string[,] client = new string[rowst,2];
            string[] gua = new string[rowst];

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = null;
                bool vus = false;
                Usuarios us = new Usuarios();

                us.KUNNR = da.KUNNR;
                us.KUNNRX = true;
                us.BUNIT = da.BUNIT;
                us.BUNITX = true;
                us.PUESTO_ID = da.PUESTO_ID;
                us.PUESTO_IDX = true;
                us.ID = da.ID;
                us.IDX = true;
                us.NOMBRE = da.NOMBRE;
                us.APELLIDO_P = da.APELLIDO_P;
                us.APELLIDO_M = da.APELLIDO_M;
                us.EMAIL = da.EMAIL;
                us.EMAILX = true;
                us.SPRAS_ID = da.SPRAS_ID;
                us.SPRAS_IDX = true;
                us.PASS = da.PASS;

                tablas[cont2, 0] = da.KUNNR.ToString();
                gua[cont2] = da.KUNNR.ToString();
                tablas[cont2, 1] = da.BUNIT.ToString();
                tablas[cont2, 2] = da.PUESTO_ID.ToString();
                tablas[cont2, 3] = da.ID.ToString();
                tablas[cont2, 4] = da.NOMBRE.ToString();
                tablas[cont2, 5] = da.APELLIDO_P.ToString();
                tablas[cont2, 6] = da.APELLIDO_M.ToString();
                tablas[cont2, 7] = da.EMAIL.ToString();
                tablas[cont2, 8] = da.SPRAS_ID.ToString();
                tablas[cont2, 9] = da.PASS.ToString();

                if(cont2>0)
                if (us.KUNNR != gua[cont2 - 1] && us.BUNIT == "" && us.PUESTO_ID == null && us.ID == "" && us.NOMBRE == "" && us.APELLIDO_P == "" && us.APELLIDO_M == "" && us.EMAIL == "" && us.SPRAS_ID == "" && us.PASS == "")
                {
                    vus = true;
                }

                if (vus == false)
                {
                    CLIENTE k = clientes.Where(x => x.KUNNR.Equals(us.KUNNR)).FirstOrDefault();
                    if (k == null)
                    {
                        k = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(us.KUNNR) & cc.ACTIVO == true).FirstOrDefault();
                        if (k == null)
                            us.KUNNRX = false;
                        else
                        {
                            clientes.Add(k);
                            client[cont2, 0] = us.KUNNR.ToString();
                        }
                    }
                    if (!us.KUNNRX)
                    {
                        us.KUNNR = "<span class='red white-text'>" + us.KUNNR + "</span>";
                        messa = cont + ". Error en el cliente \n\r";
                        cont++;
                    }
                }
                else
                {
                    CLIENTE k = clientes.Where(x => x.KUNNR.Equals(us.KUNNR)).FirstOrDefault();
                    if (k == null)
                    {
                        k = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(us.KUNNR) & cc.ACTIVO == true).FirstOrDefault();
                        if (k == null)
                            us.KUNNRX = false;
                        else
                        {
                            clientes.Add(k);
                            client[cont2, 0] = us.KUNNR.ToString();
                        }
                    }
                }

                if (vus == false)
                {
                    SOCIEDAD b = sociedad.Where(x => x.BUKRS.Equals(us.BUNIT)).FirstOrDefault();
                    if (b == null)
                    {
                        b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) & x.ACTIVO == true).FirstOrDefault();
                        if (b == null)
                            us.BUNITX = false;
                        else
                            sociedad.Add(b);
                    }
                    if (!us.BUNITX)
                    {
                        us.BUNIT = "<span class='red white-text'>" + us.BUNIT + "</span>";
                        messa = messa + cont + ". Error en la sociedad \n\r";
                        cont++;
                    }

                    PUESTO pi = puesto.Where(x => x.ID == us.PUESTO_ID).FirstOrDefault();
                    if (pi == null)
                    {
                        pi = db.PUESTOes.Where(x => x.ID == us.PUESTO_ID & x.ACTIVO == true).FirstOrDefault();
                        if (pi == null)
                            us.PUESTO_IDX = false;
                        else
                            puesto.Add(pi);
                    }
                    if (!us.PUESTO_IDX)
                    {
                        us.PUESTO_ID = us.PUESTO_ID;
                        messa = messa + cont + ". Error en el nivel \n\r";
                        cont++;
                    }
                }
                if (vus == false)
                {
                    if (us.ID == null || us.ID == "")
                        us.IDX = false;
                    else if (IDs.Contains(us.ID))
                        us.IDX = false;
                    else
                    {
                        USUARIO u = db.USUARIOs.Where(xu => xu.ID.Equals(us.ID)).FirstOrDefault();
                        if (u != null)
                            us.IDX = false;
                        else
                        {
                            usuarios.Add(u);
                            IDs[cont2] = us.ID;
                            client[cont2, 1] = us.ID.ToString();
                        }
                    }

                    if (!us.IDX)
                    {
                        us.ID = "<span class='red white-text'>" + us.ID + "</span>";
                        messa = messa + cont + ". Error en el ID de usuario \n\r";
                        cont++;
                    }
                }
                else
                {
                    client[cont2, 1] = us.ID.ToString();
                }

                if (vus == false)
                {
                    if (ComprobarEmail(us.EMAIL) == false)
                        us.EMAILX = false;
                    if (!us.EMAILX)
                    {
                        us.EMAIL = "<span class='red white-text'>" + us.EMAIL + "</span>";
                        messa = messa + cont + ". Error en el correo \n\r";
                        cont++;
                    }

                    if (us.SPRAS_ID == "")
                        us.SPRAS_ID = "ES";
                    SPRA si = db.SPRAS.Where(x => x.ID.Equals(us.SPRAS_ID) == true).FirstOrDefault();
                    if (si == null)
                        us.SPRAS_IDX = false;
                    if (!us.SPRAS_IDX)
                    {
                        us.SPRAS_ID = "<span class='red white-text'>" + us.SPRAS_ID + "</span>";
                        messa = messa + cont + ". Error en el idioma \n\r";
                        cont++;
                    }

                    da.mess = messa;
                    us.mess = da.mess;
                    tablas[cont2, 10] = messa;
                    rowst2++;
                }
                cont2++;

                uu.Add(us);
            }
            Session["tablas"] = tablas;
            Session["client"] = client;
            Session["rowst"] = rowst;
            Session["rowst2"] = rowst2;
            JsonResult jl = Json(uu, JsonRequestBehavior.AllowGet);
            return jl;
        }

        public static bool ComprobarEmail(string email)
        {
            String sFormato;
            sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, sFormato))
            {
                if (Regex.Replace(email, sFormato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private string completa(string s, int longitud)
        {
            string cadena = "";
            try
            {
                long a = Int64.Parse(s);
                int l = a.ToString().Length;
                for (int i = l; i < longitud; i++)
                {
                    cadena += "0";
                }
                cadena += a.ToString();
            }
            catch
            {
                cadena = s;
            }
            return cadena;
        }

        private List<DET_AGENTE1> objAList1(DataTable dt)
        {

            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();
            List<CLIENTE> clientes = new List<CLIENTE>();

            var rowsc = dt.Rows.Count;
            var columnsc = dt.Columns.Count;
            var rows = 1;
            var pos = 1;

            for (int i = rows; i < rowsc; i++)
            {
                DET_AGENTE1 doc = new DET_AGENTE1();

                string a = Convert.ToString(pos);

                doc.POS = Convert.ToInt32(a);
                try
                {
                    doc.KUNNR = dt.Rows[i][0].ToString();
                    doc.KUNNR = completa(doc.KUNNR, 10);

                    CLIENTE u = clientes.Where(x => x.KUNNR.Equals(doc.KUNNR)).FirstOrDefault();
                    if (u == null)
                    {
                        u = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(doc.KUNNR) & cc.ACTIVO == true).FirstOrDefault();
                        if (u == null)
                            doc.VKORG = null;
                        else
                            clientes.Add(u);
                    }

                    CLIENTE c = clientes.Where(cc => cc.KUNNR.Equals(doc.KUNNR) & cc.ACTIVO == true).FirstOrDefault();
                    if (c != null)
                    {
                        doc.VKORG = c.VKORG;
                        doc.VTWEG = c.VTWEG;
                        doc.SPART = c.SPART;
                    }
                    else
                    {
                        doc.VKORG = null;
                    }
                }
                catch (Exception e)
                {
                    doc.KUNNR = null;
                }
                try
                {
                    doc.BUNIT = dt.Rows[i][1].ToString();
                }
                catch (Exception e)
                {
                    doc.BUNIT = null;
                }
                try
                {
                    doc.PUESTO_ID = int.Parse(dt.Rows[i][2].ToString());
                }
                catch (Exception e)
                {
                    doc.PUESTO_ID = null;
                }
                try
                {
                    doc.ID = dt.Rows[i][3].ToString();
                }
                catch (Exception e)
                {
                    doc.ID = null;
                }
                try
                {
                    doc.NOMBRE = dt.Rows[i][4].ToString();
                }
                catch (Exception e)
                {
                    doc.NOMBRE = null;
                }
                try
                {
                    doc.APELLIDO_P = dt.Rows[i][5].ToString();
                }
                catch (Exception e)
                {
                    doc.APELLIDO_P = null;
                }
                try
                {
                    doc.APELLIDO_M = dt.Rows[i][6].ToString();
                }
                catch (Exception e)
                {
                    doc.APELLIDO_M = null;
                }
                try
                {
                    doc.EMAIL = dt.Rows[i][7].ToString();
                }
                catch (Exception e)
                {
                    doc.EMAIL = null;
                }
                try
                {
                    doc.SPRAS_ID = dt.Rows[i][8].ToString().ToUpper();
                }
                catch (Exception e)
                {
                    doc.SPRAS_ID = null;
                }
                try
                {
                    doc.PASS = dt.Rows[i][9].ToString();
                }
                catch (Exception e)
                {
                    doc.PASS = null;
                }
                try
                {
                    doc.mess = dt.Rows[i][10].ToString();
                }
                catch (Exception e)
                {
                    doc.mess = null;
                }

                ld.Add(doc);
                pos++;
            }
            return ld;
        }

        public partial class DET_AGENTE1 : IEquatable<DET_AGENTE1>
        {
            public string KUNNR { get; set; }
            public string VKORG { get; set; }
            public string VTWEG { get; set; }
            public string SPART { get; set; }
            public string BUNIT { get; set; }
            public Nullable<int> PUESTO_ID { get; set; }
            public string ID { get; set; }
            public string NOMBRE { get; set; }
            public string APELLIDO_P { get; set; }
            public string APELLIDO_M { get; set; }
            public string EMAIL { get; set; }
            public string SPRAS_ID { get; set; }
            public string PASS { get; set; }
            public string mess { get; set; }
            public int POS { get; set; }

            public virtual CLIENTE CLIENTE { get; set; }
            public virtual USUARIO USUARIO { get; set; }

            public bool Equals(DET_AGENTE1 other)
            {
                throw new NotImplementedException();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Agregar()
        {
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();
            var rowsc = (int)Session["rowst"];
            var tablas = (string[,])Session["tablas"];
            ld = ObjAList2();

            int cont = 1;

            foreach (DET_AGENTE1 da in ld)
            {
                USUARIO us = new USUARIO();
                USUARIOF uf = new USUARIOF();

                if (da.mess == null)
                {

                    ////---------------------------- USUARIO
                    us.ID = da.ID;
                    us.PASS = da.PASS;
                    us.NOMBRE = da.NOMBRE;
                    us.APELLIDO_P = da.APELLIDO_P;
                    us.APELLIDO_M = da.APELLIDO_M;
                    us.EMAIL = da.EMAIL;
                    us.SPRAS_ID = da.SPRAS_ID;
                    us.ACTIVO = true;
                    us.PUESTO_ID = da.PUESTO_ID;
                    us.MANAGER = null;
                    us.BACKUP_ID = null;
                    us.BUNIT = da.BUNIT;

                    db.USUARIOs.Add(us);
                    db.SaveChanges();

                    cont++;
                }
                    //////---------------------------- USUARIOF
                    //uf.USUARIO_ID = da.ID;
                    //uf.VKORG = da.VKORG;
                    //uf.VTWEG = da.VTWEG;
                    //uf.SPART = da.SPART;
                    //uf.KUNNR = da.KUNNR;
                    //uf.ACTIVO = true;
                    //uf.USUARIOC_ID = null;
                    //uf.FECHAC = DateTime.Today;
                    //uf.USUARIOM_ID = null;
                    //uf.FECHAM = null;

                    //db.USUARIOFs.Add(uf);
                    //db.SaveChanges();

                    //cont++;
                //}
            }
            //JsonResult jl = Json(null, JsonRequestBehavior.AllowGet);
            //return jl;
            return View(Index());
        }

        private List<DET_AGENTE1> ObjAList2()
        {

            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();
            List<CLIENTE> clientes = new List<CLIENTE>();

            var dt = (string[,])Session["tablas"];
            var rowsc = (int)Session["rowst"];
            var rows = 1;
            var pos = 1;

            for (int i = rows; i < rowsc; i++)
            {
                DET_AGENTE1 doc = new DET_AGENTE1();

                string a = Convert.ToString(pos);

                doc.POS = Convert.ToInt32(a);
                try
                {
                    doc.KUNNR = dt[i,0];
                    doc.KUNNR = completa(doc.KUNNR, 10);

                    CLIENTE u = clientes.Where(x => x.KUNNR.Equals(doc.KUNNR)).FirstOrDefault();
                    if (u == null)
                    {
                        u = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(doc.KUNNR) & cc.ACTIVO == true).FirstOrDefault();
                        if (u == null)
                            doc.VKORG = null;
                        else
                            clientes.Add(u);
                    }

                    CLIENTE c = clientes.Where(cc => cc.KUNNR.Equals(doc.KUNNR) & cc.ACTIVO == true).FirstOrDefault();
                    if (c != null)
                    {
                        doc.VKORG = c.VKORG;
                        doc.VTWEG = c.VTWEG;
                        doc.SPART = c.SPART;
                    }
                    else
                    {
                        doc.VKORG = null;
                    }
                }
                catch (Exception e)
                {
                    doc.KUNNR = null;
                }
                try
                {
                    doc.BUNIT = dt[i,1];
                }
                catch (Exception e)
                {
                    doc.BUNIT = null;
                }
                try
                {
                    doc.PUESTO_ID = int.Parse(dt[i,2]);
                }
                catch (Exception e)
                {
                    doc.PUESTO_ID = null;
                }
                try
                {
                    doc.ID = dt[i,3];
                }
                catch (Exception e)
                {
                    doc.ID = null;
                }
                try
                {
                    doc.NOMBRE = dt[i,4];
                }
                catch (Exception e)
                {
                    doc.NOMBRE = null;
                }
                try
                {
                    doc.APELLIDO_P = dt[i,5];
                }
                catch (Exception e)
                {
                    doc.APELLIDO_P = null;
                }
                try
                {
                    doc.APELLIDO_M = dt[i,6];
                }
                catch (Exception e)
                {
                    doc.APELLIDO_M = null;
                }
                try
                {
                    doc.EMAIL = dt[i,7];
                }
                catch (Exception e)
                {
                    doc.EMAIL = null;
                }
                try
                {
                    doc.SPRAS_ID = dt[i,8];
                }
                catch (Exception e)
                {
                    doc.SPRAS_ID = null;
                }
                try
                {
                    doc.PASS = dt[i,9];
                }
                catch (Exception e)
                {
                    doc.PASS = null;
                }
                try
                {
                    doc.mess = dt[i,10];
                }
                catch (Exception e)
                {
                    doc.mess = null;
                }

                ld.Add(doc);
                pos++;
            }
            return ld;
        }
    }
}

