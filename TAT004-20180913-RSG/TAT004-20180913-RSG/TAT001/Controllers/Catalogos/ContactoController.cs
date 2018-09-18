using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    public class ContactoController : Controller
    {
        // GET: Contacto
        public ActionResult Index(string vko, string vtw, string kun, string spa)
        {
            int pagina = 641; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.MIEMBROS.FirstOrDefault().ROL.NOMBRE;
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
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;

                if (vko != null && vtw != null && kun != null && spa != null)
                {
                    var con = db.CONTACTOCs
                            .Where(x => x.VKORG == vko && x.VTWEG == vtw && x.KUNNR == kun && x.SPART == spa && x.ACTIVO == true).ToList();

                    Contactoc conCli = new Contactoc();
                    conCli.vkorg = vko;
                    conCli.vtweg = vtw;
                    conCli.kunnr = kun;
                    conCli.spart = spa;
                    conCli.tabContacto = con;

                    return View(conCli);
                }
                else
                {
                    var con = db.CONTACTOCs.Where(x => x.ACTIVO == true).ToList();

                    Contactoc conCli = new Contactoc();
                    conCli.vkorg = vko;
                    conCli.vtweg = vtw;
                    conCli.kunnr = kun;
                    conCli.spart = spa;
                    conCli.tabContacto = con;

                    return View(conCli);
                }
            }
        }

        [HttpPost]
        public ActionResult Index(string vko, string vtw, string kun, string spa, string aa)
        {
            int pagina = 641; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.MIEMBROS.FirstOrDefault().ROL.NOMBRE;
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
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;

                var con = db.CONTACTOCs
                            .Where(x => x.VKORG == vko && x.VTWEG == vtw && x.KUNNR == kun && x.SPART == spa && x.ACTIVO == true).ToList();

                Contactoc conCli = new Contactoc();
                conCli.kunnr = kun;
                conCli.vkorg = vko;
                conCli.vtweg = vtw;
                conCli.spart = spa;
                conCli.tabContacto = con; ;

                return View(conCli);
            }
        }
        // GET: Contacto/Details/5
        public ActionResult Details(string vko, string vtw, string kun, string spa, int id)
        {
            int pagina = 642; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.MIEMBROS.FirstOrDefault().ROL.NOMBRE;
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
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;

                var cli = db.CONTACTOCs
                          .Where(x => x.VKORG == vko && x.VTWEG == vtw && x.KUNNR == kun && x.SPART == spa && x.ID == id)
                          .First();

                Contactoc cl = new Contactoc();
                cl.vkorg = cli.VKORG;
                cl.vtweg = cli.VTWEG;
                cl.kunnr = cli.KUNNR;
                cl.spart = cli.SPART;
                cl.id = cli.ID.ToString();
                cl.nombre = cli.NOMBRE;
                cl.telefono = cli.PHONE;
                cl.correo = cli.EMAIL;
                cl.defecto = Convert.ToBoolean(cli.DEFECTO);
                return View(cl);
            }
        }

        //// GET: Contacto/Create
        //public ActionResult Create(string vko, string vtw, string kun, string spa)
        //{
        //    Contactoc conCli = new Contactoc();
        //    conCli.kunnr = kun;
        //    conCli.vkorg = vko;
        //    conCli.vtweg = vtw;
        //    conCli.spart = spa;

        //    return View(conCli);
        //}

        // POST: Contacto/Create
        [HttpGet]
        public ActionResult Create(string vko, string vtw, string kun, string spa)
        {
            int pagina = 642; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.MIEMBROS.FirstOrDefault().ROL.NOMBRE;
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
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;

                Contactoc conC = new Contactoc();
                conC.vkorg = vko;
                conC.vtweg = vtw;
                conC.kunnr = kun;
                conC.spart = spa;

                return View(conC);
            }
        }

        [HttpPost]
        public ActionResult Create2(string vko, string vtw, string kun, string spa, Contactoc conC)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                if (conC.defecto == true)
                {
                    var conAct = db.CONTACTOCs
                                   .Where(x => x.VKORG == vko && x.VTWEG == vtw && x.KUNNR == kun && x.SPART == spa && x.DEFECTO == true).ToList();

                    foreach (var item in conAct)
                    {
                        item.DEFECTO = false;
                    }

                    db.SaveChanges();

                    var con = db.CONTACTOCs
                            .Add(new CONTACTOC { NOMBRE = conC.nombre, PHONE = conC.telefono, EMAIL = conC.correo, VKORG = vko, VTWEG = vtw, KUNNR = kun, SPART = spa, ACTIVO = true, DEFECTO = conC.defecto });
                    db.SaveChanges();
                }
                else
                {
                    var con = db.CONTACTOCs
                            .Add(new CONTACTOC { NOMBRE = conC.nombre, PHONE = conC.telefono, EMAIL = conC.correo, VKORG = vko, VTWEG = vtw, KUNNR = kun, SPART = spa, ACTIVO = true, DEFECTO = conC.defecto });
                    db.SaveChanges();
                }
                return RedirectToAction("Index", new { vko = vko, vtw = vtw, kun = kun, spa = spa });
            }
        }

        // GET: Contacto/Edit/5
        public ActionResult Edit(string vko, string vtw, string kun, string spa, int id)
        {
            int pagina = 642; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.MIEMBROS.FirstOrDefault().ROL.NOMBRE;
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
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;

                var con = db.CONTACTOCs
                          .Where(x => x.VKORG == vko && x.VTWEG == vtw && x.KUNNR == kun && x.SPART == spa && x.ID == id)
                          .First();

                Contactoc co = new Contactoc();
                co.vkorg = con.VKORG;
                co.vtweg = con.VTWEG;
                co.kunnr = con.KUNNR;
                co.spart = con.SPART;
                co.id = con.ID.ToString();
                co.nombre = con.NOMBRE;
                co.telefono = con.PHONE;
                co.correo = con.EMAIL;
                co.defecto = Convert.ToBoolean(con.DEFECTO);
                return View(co);
            }
        }

        // POST: Contacto/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Contactoc co)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                try
                {
                    string u = User.Identity.Name;
                    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                    var idCli = db.CONTACTOCs.Where(x => x.ID == id).First();

                    if (co.defecto == true)
                    {
                        var conAct = db.CONTACTOCs
                                       .Where(x => x.VKORG == idCli.VKORG && x.VTWEG == idCli.VTWEG && x.KUNNR == idCli.KUNNR && x.SPART == idCli.SPART && x.DEFECTO == true).ToList();

                        foreach (var item in conAct)
                        {
                            item.DEFECTO = false;
                        }

                        db.SaveChanges();

                        var con = db.CONTACTOCs.Where(x => x.ID == id).First();
                        con.NOMBRE = co.nombre;
                        con.PHONE = co.telefono;
                        con.EMAIL = co.correo;
                        con.DEFECTO = co.defecto;
                        db.SaveChanges();
                    }
                    else
                    {
                        var con = db.CONTACTOCs.Where(x => x.ID == id).First();
                        con.NOMBRE = co.nombre;
                        con.PHONE = co.telefono;
                        con.EMAIL = co.correo;
                        con.DEFECTO = co.defecto;
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", new { vko = idCli.VKORG, vtw = idCli.VTWEG, kun = idCli.KUNNR, spa = idCli.SPART});
                }
                catch
                {
                    return View("Edit");
                }
            }
        }

        // GET: Contacto/Delete/5
        public ActionResult Delete(int id)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                var con = db.CONTACTOCs.Where(x => x.ID == id).First();
                con.ACTIVO = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // POST: Contacto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
