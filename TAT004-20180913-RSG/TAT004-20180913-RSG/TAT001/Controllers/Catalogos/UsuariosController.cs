using ClosedXML.Excel;
using ExcelDataReader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Filters;
using TAT001.Models;
using TAT001.Models.Dao;
using TAT001.Services;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    [LoginActive]
    public class UsuariosController : Controller
    {
        private TAT001Entities db = new TAT001Entities();
        private UsuarioLogin usuValidateLogin = new UsuarioLogin();


        //--------------------DAO's-----------
        readonly SociedadesDao sociedadesDao=new SociedadesDao();

        // GET: Usuarios
        public ActionResult Index()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            string us = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(us)).FirstOrDefault();
            ViewBag.nivelUsuario = user.PUESTO_ID;
            int pagina = 601; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);

            var uSUARIOs = db.USUARIOs.Where(u => u.ACTIVO == true).Include(u => u.PUESTO).Include(u => u.SPRA);
            UsuarioNuevo un = new UsuarioNuevo();
            un.L = uSUARIOs;
            return View(un);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(string id)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            int pagina = 604; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            ViewBag.nivelUsuario = userz.PUESTO_ID;
            ViewBag.idUsuario = userz.ID;
            string spra = Session["spras"].ToString();
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            var ni = (from x in db.PUESTOTs
                      join a in db.PUESTOes on x.PUESTO_ID equals a.ID
                      where x.PUESTO_ID == uSUARIO.PUESTO_ID & x.SPRAS_ID.Equals(spra) & a.ACTIVO == true
                      select x.PUESTO_ID).FirstOrDefault();
            ViewBag.PUESTO_ID1 = (from x in db.DET_APROBH where x.PUESTOC_ID == ni & x.ACTIVO == true select x.SOCIEDAD_ID).FirstOrDefault();
            List<SOCIEDAD> sociedades = db.USUARIOs.Where(a => a.ID.Equals(uSUARIO.ID)).FirstOrDefault().SOCIEDADs.ToList();
            string[] sociedad = new string[sociedades.Count];
            for (int i = 0; i < sociedades.Count; i++)
            {
                sociedad[i] = sociedades[i].BUKRS;
            }
            ViewBag.ID = uSUARIO.ID;
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION", uSUARIO.SPRAS_ID);
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            ViewBag.BUNIT1 = sociedad;
            ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            ViewBag.SOCIEDADES = db.SOCIEDADs;
            ViewBag.PAISES = db.PAIS.Where(a => a.SOCIEDAD_ID != null & a.ACTIVO == true).ToList();
            ViewBag.APROBADORES = db.DET_APROB.Where(a => a.BUKRS.Equals("KCMX") & a.PUESTOC_ID == uSUARIO.PUESTO_ID).ToList();
            return View(uSUARIO);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            int pagina = 602; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);

            string spra = ViewBag.spras_id;
            //Roles a los cuales no se pueden agregar varios cocodes, solo uni
            ViewBag.ROLESCC = (from x in db.DET_APROBH where x.ACTIVO == true select x.PUESTOC_ID).Distinct().ToArray();
            ViewBag.SOCIEDADES = new List<SOCIEDAD>( FnCommon.obtenerCoCodes());
            //ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50");           
            //ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION");

            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PASS,NOMBRE,APELLIDO_P,APELLIDO_M,EMAIL,SPRAS_ID,ACTIVO,PUESTO_ID,MANAGER,BACKUP_ID,BUNIT,ROL")] Usuario uSUARIO)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            int pagina = 602; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION");
            
            ViewBag.ROLESCC = (from x in db.DET_APROBH where x.ACTIVO == true select x.PUESTOC_ID).Distinct().ToArray();
            ViewBag.SOCIEDADES = new List<SOCIEDAD>(FnCommon.obtenerCoCodes());

            if (ModelState.IsValid)
            {           
                if (!ExisteUsuario(uSUARIO.ID))
                {
                    if (!String.IsNullOrEmpty(uSUARIO.PASS) & !String.IsNullOrEmpty(uSUARIO.MANAGER))
                    {
                        if (uSUARIO.PASS == uSUARIO.MANAGER)
                        {

                            if (ComprobarEmail(uSUARIO.EMAIL) != false & !String.IsNullOrEmpty(uSUARIO.EMAIL))
                            {
                                Cryptography c = new Cryptography();
                                uSUARIO.PASS = c.Encrypt(uSUARIO.PASS);
                                USUARIO u = new USUARIO();

                                if (uSUARIO.ID != null && !db.USUARIOs.Any(x => x.ID == uSUARIO.ID))
                                {
                                    try
                                    {
                                        u.ID = uSUARIO.ID.Trim();
                                        u.PASS = uSUARIO.PASS;
                                        u.NOMBRE = uSUARIO.NOMBRE;
                                        u.APELLIDO_P = uSUARIO.APELLIDO_P;
                                        u.APELLIDO_M = uSUARIO.APELLIDO_M;
                                        u.EMAIL = uSUARIO.EMAIL;
                                        u.SPRAS_ID = uSUARIO.SPRAS_ID;
                                        u.ACTIVO = true;
                                        u.PUESTO_ID = uSUARIO.PUESTO_ID;
                                        u.MANAGER = uSUARIO.MANAGER;
                                        u.BACKUP_ID = uSUARIO.BACKUP_ID;
                                        string comcode1 = Request["selectcocode"] as string;
                                        if (comcode1==null)
                                            u.BUNIT = uSUARIO.BUNIT;
                                        else
                                            u.BUNIT = comcode1.Split(',')[0];

                                        db.USUARIOs.Add(u);
                                        db.SaveChanges();
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }
                                    string comcode = Request["selectcocode"] as string;
                                    USUARIO us = db.USUARIOs.Where(x => x.ID == uSUARIO.ID).FirstOrDefault();
                                    var re = (from x in db.DET_APROBH where x.PUESTOC_ID == uSUARIO.PUESTO_ID & x.ACTIVO == true select x.SOCIEDAD_ID).FirstOrDefault();
                                    int num = 1;


                                    if (comcode != null)
                                    {
                                        num = comcode.Split(',').Length;
                                    }
                                    if (re != null)
                                    {
                                        db.Entry(us).State = EntityState.Modified;
                                        db.SaveChanges();

                                        try
                                        {
                                            SOCIEDAD soc = db.SOCIEDADs.Where(x => x.BUKRS == us.BUNIT).First();
                                            us.SOCIEDADs.Add(soc);
                                            db.Entry(us).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        catch (Exception e)
                                        {
                                            db.Entry(us).State = EntityState.Detached;
                                        }

                                    }
                                    else
                                    {
                                        string[] codes = new string[num];
                                        if (comcode == null)
                                        {
                                            codes = uSUARIO.BUNIT.Split(',');
                                        }
                                        else
                                        {
                                            codes = comcode.Split(',');
                                        }
                                        us.BUNIT = codes[0];
                                        db.Entry(us).State = EntityState.Modified;
                                        db.SaveChanges();

                                        foreach (var da in codes)
                                        {
                                            try
                                            {
                                                SOCIEDAD soc = db.SOCIEDADs.Where(x => x.BUKRS == da).First();
                                                us.SOCIEDADs.Add(soc);
                                                db.Entry(us).State = EntityState.Modified;
                                                db.SaveChanges();
                                            }
                                            catch (Exception e)
                                            {
                                                db.Entry(us).State = EntityState.Detached;
                                            }
                                        }
                                    }
                                    try
                                    {
                                        SOCIEDAD soc = db.SOCIEDADs.Where(x => x.BUKRS == us.BUNIT).First();
                                        if (!us.SOCIEDADs.Any(x => x.BUKRS == us.BUNIT))
                                        {
                                            us.SOCIEDADs.Add(soc);
                                        }

                                        db.Entry(us).State = EntityState.Modified;
                                        db.SaveChanges();

                                    }
                                    catch (Exception e)
                                    {
                                    }

                                    return RedirectToAction("Index");

                            }
                            else
                            {
                                ViewBag.Error = "El correo no es correcto";
                            }
                        }
                        else
                        {
                            TempData["MensajePass"] = "La contraseña no coincide";
                        }

                    }
                }
                else
                {
                    TempData["MensajeUsuario"] = "El usuario ya existe. Introduzca un ID de usuario diferente";
                    return View(uSUARIO);
                }
            }

            return View(uSUARIO);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(string id)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            int pagina = 603; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            
            var usu = User.Identity.Name;
            USUARIO usu2 = db.USUARIOs.Where(x => x.ID.Equals(usu)).FirstOrDefault();
            ViewBag.nivelUsuario = usu2.PUESTO_ID;
            ViewBag.idUsuario = usu2.ID;
            if (usu2.PUESTO_ID == 1 || usu2.PUESTO_ID == 8)
            {
                ViewBag.admin = "si";
            }
            else
            {
                ViewBag.admin = "no";
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
            string spra = ViewBag.spras_id;
            var ni = (from x in db.PUESTOTs
                      join a in db.PUESTOes on x.PUESTO_ID equals a.ID
                      where x.PUESTO_ID == uSUARIO.PUESTO_ID & x.SPRAS_ID.Equals(spra) & a.ACTIVO == true
                      select x.PUESTO_ID).FirstOrDefault();
            ViewBag.PUESTO_ID1 = (from x in db.DET_APROBH where x.PUESTOC_ID == ni & x.ACTIVO == true select x.SOCIEDAD_ID).FirstOrDefault();
            List<SOCIEDAD> sociedades = db.USUARIOs.Where(a => a.ID.Equals(uSUARIO.ID)).FirstOrDefault().SOCIEDADs.ToList();
            string[] sociedad = new string[sociedades.Count];
            for (int i = 0; i < sociedades.Count; i++)
            {
                sociedad[i] = sociedades[i].BUKRS;
            }
            var usf = (from x in db.USUARIOFs
                       where x.USUARIO_ID.Equals(uSUARIO.ID)
                       select x.KUNNR).ToArray();
            var ucl = (from x in db.CLIENTEFs
                       where x.USUARIO0_ID.Equals(uSUARIO.ID) | x.USUARIO1_ID.Equals(uSUARIO.ID) | x.USUARIO2_ID.Equals(uSUARIO.ID) | x.USUARIO3_ID.Equals(uSUARIO.ID)
                         | x.USUARIO4_ID.Equals(uSUARIO.ID) | x.USUARIO5_ID.Equals(uSUARIO.ID) | x.USUARIO6_ID.Equals(uSUARIO.ID) | x.USUARIO7_ID.Equals(uSUARIO.ID)
                       select x.KUNNR).ToArray();
            if (ucl.Length == 0 && usf.Length == 0)
            {
                ViewBag.flujo = "si";
                ViewBag.ROLESCC = (from x in db.DET_APROBH where x.ACTIVO == true select x.PUESTOC_ID).Distinct().ToArray();
                ViewBag.SOCIEDADES = new List<SOCIEDAD>(FnCommon.obtenerCoCodes());
            }
            else
            {
                ViewBag.flujo = "no";
            }
            ViewBag.ID = uSUARIO.ID;
            //ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "DESCRIPCION", uSUARIO.SPRAS_ID);
            //ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            //ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            //ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            ViewBag.SOCIEDADES = new List<SOCIEDAD>(FnCommon.obtenerCoCodes());
            ViewBag.sociedad = JsonConvert.SerializeObject(sociedad, Formatting.Indented);
            //ViewBag.PAISES = db.PAIS;
            //ViewBag.sociedad = JsonConvert.SerializeObject(sociedad, Formatting.Indented);
            //ViewBag.APROBADORES = db.DET_APROB.Where(a => a.BUKRS.Equals("KCMX") & a.PUESTOC_ID == uSUARIO.PUESTO_ID).ToList();
            return View(uSUARIO);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PASS,NOMBRE,APELLIDO_P,APELLIDO_M,EMAIL,SPRAS_ID,ACTIVO,PUESTO_ID,MANAGER,BACKUP_ID,BUNIT")] USUARIO uSUARIO)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            int pagina = 603; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            string spra = ViewBag.spras_id;
            if (ModelState.IsValid)
            {
                if (ComprobarEmail(uSUARIO.EMAIL) != false & uSUARIO.EMAIL != null & uSUARIO.EMAIL != "")
                {
                    USUARIO us = db.USUARIOs.Where(x => x.ID == uSUARIO.ID).FirstOrDefault();
                    //db.Entry(us).State = EntityState.Detached;
                    //db.Entry(uSUARIO).State = EntityState.Detached;
                    var ni = (from x in db.PUESTOTs
                              join a in db.PUESTOes on x.PUESTO_ID equals a.ID
                              where x.PUESTO_ID == uSUARIO.PUESTO_ID & x.SPRAS_ID.Equals(spra) & a.ACTIVO == true
                              select x.PUESTO_ID).FirstOrDefault();
                    var re = (from x in db.DET_APROBH where x.PUESTOC_ID == ni & x.ACTIVO == true select x.SOCIEDAD_ID).FirstOrDefault();
                    List<SOCIEDAD> sociedades = db.USUARIOs.Where(a => a.ID.Equals(uSUARIO.ID)).FirstOrDefault().SOCIEDADs.ToList();
                    int num = 1;
                    string comcode = Request["selectcocode"] as string;

                    if (comcode != null)
                    {
                        num = comcode.Split(',').Length;
                    }



                    foreach (var da in sociedades)
                    {
                        try
                        {
                            us.SOCIEDADs.Remove(da);
                            db.Entry(us).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            db.Entry(us).State = EntityState.Detached;
                        }
                    }

                    us.ID = uSUARIO.ID;
                    us.PASS = uSUARIO.PASS;
                    us.NOMBRE = uSUARIO.NOMBRE;
                    us.APELLIDO_P = uSUARIO.APELLIDO_P;
                    us.APELLIDO_M = uSUARIO.APELLIDO_M;
                    us.ACTIVO = true;
                    us.EMAIL = uSUARIO.EMAIL;
                    us.SPRAS_ID = uSUARIO.SPRAS_ID;
                    us.PUESTO_ID = uSUARIO.PUESTO_ID;
                    us.MANAGER = uSUARIO.MANAGER;
                    us.BACKUP_ID = uSUARIO.BACKUP_ID;
                    us.BUNIT = uSUARIO.BUNIT;

                    if (re != null)
                    {
                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();

                        try
                        {
                            SOCIEDAD soc = db.SOCIEDADs.Where(x => x.BUKRS == us.BUNIT).First();
                            us.SOCIEDADs.Add(soc);
                            db.Entry(us).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            db.Entry(us).State = EntityState.Detached;
                        }

                    }
                    else
                    {
                        string[] codes = new string[num];
                        if (comcode == null)
                        {
                            codes = uSUARIO.BUNIT.Split(',');
                        }
                        else
                        {
                            codes = comcode.Split(',');
                        }
                        us.BUNIT = codes[0];
                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();

                        foreach (var da in codes)
                        {
                            try
                            {
                                SOCIEDAD soc = db.SOCIEDADs.Where(x => x.BUKRS == da).First();
                                us.SOCIEDADs.Add(soc);
                                db.Entry(us).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {
                                db.Entry(us).State = EntityState.Detached;
                            }
                        }
                    }
                    if (userz.PUESTO_ID == 1 || userz.PUESTO_ID == 8)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Details", new { id = uSUARIO.ID });
                    }
                }
                else
                {
                    ViewBag.Error = "El correo no es correcto";
                }
            }
            //using (TAT001Entities db = new TAT001Entities())
            //{
            //    string u = User.Identity.Name;
            //    //string u = "admin";
            //    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            //    ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            //    ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            //    ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            //    ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            //    pagina = pagina - 1;
            //    ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            //    try
            //    {
            //        string p = Session["pais"].ToString();
            //        ViewBag.pais = p + ".png";
            //    }
            //    catch
            //    {
            //        //ViewBag.pais = "mx.png";
            //        //return RedirectToAction("Pais", "Home");
            //    }
            //    Session["spras"] = user.SPRAS_ID;
            //}
            ViewBag.SPRAS_ID = new SelectList(db.SPRAS, "ID", "ID", uSUARIO.SPRAS_ID);
            ViewBag.PUESTO_ID = new SelectList(db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(spra)), "PUESTO_ID", "TXT50", uSUARIO.PUESTO_ID);
            ViewBag.BUNIT = new SelectList(db.SOCIEDADs, "BUKRS", "BUKRS", uSUARIO.BUNIT);
            ViewBag.ROLES = db.ROLTs.Where(a => a.SPRAS_ID.Equals(spra));
            return View(uSUARIO);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(string id)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            //int pagina = 603; //ID EN BASE DE DATOS
            //using (TAT001Entities db = new TAT001Entities())
            //{
            //    string u = User.Identity.Name;
            //    //string u = "admin";
            //    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            //    ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            //    ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            //    ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            //    ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            //    pagina = pagina - 1;
            //    ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            //    try
            //    {
            //        string p = Session["pais"].ToString();
            //        ViewBag.pais = p + ".png";
            //    }
            //    catch
            //    {
            //        //ViewBag.pais = "mx.png";
            //        //return RedirectToAction("Pais", "Home");
            //    }
            //    Session["spras"] = user.SPRAS_ID;
            //}
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
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            USUARIO uSUARIO = db.USUARIOs.Find(id);
            uSUARIO.ACTIVO = false;
            db.Entry(uSUARIO).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Usuarios/Edit/5
        public ActionResult Pass(string id)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            int pagina = 605; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            //using (TAT001Entities db = new TAT001Entities())
            //{
            //    string u = User.Identity.Name;
            //    //string u = "admin";
            //    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            //    ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            //    ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            //    ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            //    ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            //    ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            //    try
            //    {
            //        string p = Session["pais"].ToString();
            //        ViewBag.pais = p + ".png";
            //    }
            //    catch
            //    {
            //        //ViewBag.pais = "mx.png";
            //        //return RedirectToAction("Pais", "Home");
            //    }
            //    Session["spras"] = user.SPRAS_ID;
            //}
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var usu = User.Identity.Name;
            USUARIO usu2 = db.USUARIOs.Where(x => x.ID.Equals(usu)).FirstOrDefault();
            if (usu2.PUESTO_ID == 1 || usu2.PUESTO_ID == 8)
            {
                ViewBag.admin = true;
            }
            else
            {
                ViewBag.admin = false;
            }
            Pass uSUARIO = new Pass();
            uSUARIO.ID = id;
            return View(uSUARIO);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Pass(/*[Bind(Include = "ID,pass,npass1,npass2")] */Pass pp)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            var usu = User.Identity.Name;
            USUARIO usu2 = db.USUARIOs.Where(x => x.ID.Equals(usu)).FirstOrDefault();
            Pass pass = new Pass();
            pass.ID = Request.Form.Get("ID");
            pass.pass = Request.Form.Get("pass");
            pass.npass1 = Request.Form.Get("npass1");
            pass.npass2 = Request.Form.Get("npass2");
            USUARIO us = db.USUARIOs.Find(pass.ID);
            Cryptography c = new Cryptography();
            string pass_a = c.Decrypt(us.PASS);
            if ((pass.pass.Equals(pass_a) && usu2.ID.Equals(pass.ID)) || (usu2.PUESTO_ID == 1 || usu2.PUESTO_ID == 8))
            {
                if (pass.npass1.Equals(pass.npass2))
                {
                    if (ModelState.IsValid)
                    {
                        us.PASS = c.Encrypt(pass.npass1);
                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = us.ID });
                    }
                }
                else
                {
                    TempData["MensajePass"] = "Los datos no coinciden";
                }
            }
            else
            {
                ViewBag.message = "Los datos no coinciden";
            }
            int pagina = 605; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            //using (TAT001Entities db = new TAT001Entities())
            //{
            //    string u = User.Identity.Name;
            //    //string u = "admin";
            //    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            //    ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            //    ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            //    ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            //    ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //    ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            //    ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

            //    try
            //    {
            //        string p = Session["pais"].ToString();
            //        ViewBag.pais = p + ".png";
            //    }
            //    catch
            //    {
            //        //ViewBag.pais = "mx.png";
            //        //return RedirectToAction("Pais", "Home");
            //    }
            //    Session["spras"] = user.SPRAS_ID;
            //}
            return View(pass);
        }
        public ActionResult AgregarRol(string id)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
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
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
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
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
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
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            int pagina = 608;
            string u = User.Identity.Name;
            FnCommon.ObtenerConfPage(db, pagina, u, this.ControllerContext.Controller);
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
            return View();
        }

        [HttpPost]
        public ActionResult Carga(IEnumerable<HttpPostedFileBase> files)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadExcel()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
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
            int rowst = ld.Count;
            string[] IDs = new string[rowst];
            int cont2 = 0;
            int cont3 = 0;
            int cont4 = 0;
            string[,] tablas = new string[rowst, 11];
            string[,] client = new string[rowst, 2];
            string[,] tabla1 = new string[rowst, 11];
            string[,] admins = new string[rowst, 2];
            string[,] usuariosoc = new string[rowst, 2];
            //string[] gua = new string[rowst];
            //string[] gua1 = new string[rowst];
            string p = FnCommon.ObtenerSprasId(db,User.Identity.Name) ;
            string usuario_id = "";
            int puesto_id = 0;

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                string sel = "";
                bool vus = false;
                Usuarios us = new Usuarios();
                Cryptography c = new Cryptography();

                us.KUNNR = da.KUNNR.Replace(" ", "");
                us.KUNNRX = true;
                us.BUNIT = da.BUNIT.Replace(" ", "");
                us.BUNITX = true;
                us.PUESTO_ID = da.PUESTO_ID.ToString();
                us.PUESTO_IDX = true;
                us.ID = da.ID.Replace(" ", "");
                us.IDX = true;
                us.NOMBRE = da.NOMBRE;
                us.APELLIDO_P = da.APELLIDO_P;
                us.APELLIDO_M = da.APELLIDO_M;
                us.EMAIL = da.EMAIL.Replace(" ", "");
                us.EMAILX = true;
                us.SPRAS_ID = da.SPRAS_ID;
                us.SPRAS_IDX = true;
                us.PASS = da.PASS;
                
                string men = ". Error en el nivel<br/>";
                if (us.PUESTO_ID != null && us.PUESTO_ID != "")
                {
                    puesto_id = int.Parse(us.PUESTO_ID);
                }

                var ni = (from x in db.PUESTOTs
                          join a in db.PUESTOes on x.PUESTO_ID equals a.ID
                          where x.PUESTO_ID == puesto_id && x.SPRAS_ID.Equals(p) && a.ACTIVO.Value
                          select x.PUESTO_ID).FirstOrDefault();
               
                var re = (from x in db.DET_APROBH where x.PUESTOC_ID == ni && x.ACTIVO select x.SOCIEDAD_ID).FirstOrDefault();
                //Comprobacion de la asignacion de varios clientes
                if (cont2 > 0 && us.KUNNR != ""  && (us.BUNIT == "" || us.BUNIT != "") && us.PUESTO_ID == "" && us.ID == "" && us.NOMBRE == "" && us.APELLIDO_P == "" && us.APELLIDO_M == "" && us.EMAIL == "" && us.SPRAS_ID == "" && us.PASS == "")
                    {
                        vus = true;
                        sel = "venta";
                    
                }
                //Comprobacion de la asignacion de varios co code
                if (cont3 > 0 && us.BUNIT != "" && (us.KUNNR == "" || us.KUNNR != "") && us.PUESTO_ID == "" && us.ID == "" && us.NOMBRE == "" && us.APELLIDO_P == "" && us.APELLIDO_M == "" && us.EMAIL == "" && us.SPRAS_ID == "" && us.PASS == "")
                    {
                        vus = true;
                        sel = "super";
                    
                }

                // Validacion del tipo de usuario
                if ((re != null && re != "")  || (puesto_id == 8 && us.KUNNR != ""))
                {
                    sel = "venta";
                }
                else if ((re == null || re == "") )
                {
                    sel = "super";
                }

                //Validacion de datos
                if (sel == "venta")
                {
                    //Usuario nuevo con cliente
                    if (!vus)
                    {
                        ////-------------------------------CLIENTE
                        var error = "";
                        CLIENTE k = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(us.KUNNR) && cc.ACTIVO).FirstOrDefault();
                        for (int i = cont2; i >= 0; i--)
                        {
                            if (client[i, 0] == us.KUNNR && client[i, 1] == us.ID)
                            {
                                us.KUNNRX = false;
                                error = ". Registro duplicado<br/>";
                            }
                        }
                        if (k == null)
                        {
                            us.KUNNRX = false;
                            error = ". Error en el cliente<br/>";
                        }
                        else
                        {
                            clientes.Add(k);
                            client[cont2, 0] = us.KUNNR.ToString();
                            tablas[cont2, 0] = da.KUNNR.ToString();
                            //gua[cont2] = da.KUNNR.ToString();
                        }
                        if (!us.KUNNRX)
                        {
                            us.KUNNR = us.KUNNR + "?";
                            messa = cont + error;
                            cont++;
                        }

                        ////-------------------------------COMPANY CODE
                        SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();

                        if (b == null)
                        {
                            us.BUNITX = false;
                        }
                        else
                        {
                            sociedad.Add(b);
                            tablas[cont2, 1] = da.BUNIT.ToString();
                            admins[cont2, 0] = da.BUNIT.ToString();
                            usuariosoc[cont4, 0] = da.BUNIT.ToString();
                        }
                        if (!us.BUNITX)
                        {
                            us.BUNIT = us.BUNIT + "?";
                            messa = messa + cont + ". Error en la sociedad<br/>";
                            cont++;
                        }

                        ////-------------------------------NIVEL

                        PUESTO pi = db.PUESTOes.Where(x => x.ID == puesto_id && x.ACTIVO.Value).FirstOrDefault();
                        if (pi == null)
                            us.PUESTO_IDX = false;
                        else
                        {
                            puesto.Add(pi);
                            tablas[cont2, 2] = da.PUESTO_ID.ToString();

                        }
                        if (!us.PUESTO_IDX)
                        {
                            us.PUESTO_ID = us.PUESTO_ID + "?";
                            messa = messa + cont + men;
                            cont++;
                        }

                        ////-------------------------------USUARIO ID
                        bool existeUsuario = false;
                        var err = ". Error en el ID de usuario<br/>";
                        if (us.ID == null || us.ID == "")
                            us.IDX = false;
                        else if (IDs.Contains(us.ID))
                        {
                            us.IDX = false;
                            err = ". ID de usuario duplicado<br/>";
                        }
                        else
                        {
                            USUARIO u = db.USUARIOs.Where(xu => xu.ID.Equals(us.ID)).FirstOrDefault();
                            if (u != null)
                            {
                                existeUsuario = true;
                                err = ". El usuario ya existe<br/>";
                                IDs[cont4] = us.ID;
                                usuario_id = us.ID;
                                client[cont2, 1] = us.ID.ToString();
                                tablas[cont2, 3] = da.ID.ToString();
                                tablas[cont2, 4] = da.NOMBRE.ToString();
                                tablas[cont2, 5] = da.APELLIDO_P.ToString();
                                tablas[cont2, 6] = da.APELLIDO_M.ToString();
                                usuariosoc[cont4, 1] = us.ID.ToString();
                                us.ID = us.ID + "!";
                            }
                            else
                            {
                                usuarios.Add(u);
                                IDs[cont4] = us.ID;
                                usuario_id = us.ID;
                                client[cont2, 1] = us.ID.ToString();
                                tablas[cont2, 3] = da.ID.ToString();
                                tablas[cont2, 4] = da.NOMBRE.ToString();
                                tablas[cont2, 5] = da.APELLIDO_P.ToString();
                                tablas[cont2, 6] = da.APELLIDO_M.ToString();
                                usuariosoc[cont4, 1] = us.ID.ToString();
                            }
                        }

                        if (!us.IDX|| existeUsuario)
                        {
                            if (!us.IDX) {
                                us.ID = us.ID + "?";
                            }
                            messa = messa + cont + err;
                            cont++;
                        }

                        ////-------------------------------EMAIL
                        if (!ComprobarEmail(us.EMAIL))
                        {
                            us.EMAILX = false;
                        }
                        else
                            tablas[cont2, 7] = da.EMAIL.ToString();
                        if (!us.EMAILX)
                        {
                            us.EMAIL = us.EMAIL + "?";
                            messa = messa + cont + ". Error en el correo<br/>";
                            cont++;
                        }

                        ////-------------------------------IDIOMA
                        SPRA si = db.SPRAS.FirstOrDefault(x => x.ID.Equals(us.SPRAS_ID));
                        if (si == null)
                        {
                            us.SPRAS_IDX = false;
                        }
                        else
                        {
                            tablas[cont2, 8] = da.SPRAS_ID.ToString();
                            tablas[cont2, 9] = c.Encrypt(da.PASS.ToString());
                        }
                        if (!us.SPRAS_IDX)
                        {
                            us.SPRAS_ID = us.SPRAS_ID + "?";
                            messa = messa + cont + ". Error en el idioma<br/>";
                            cont++;
                        }

                        da.mess = messa;
                        us.mess = da.mess;
                        tablas[cont2, 10] = messa;
                    }
                    //Asignacion de mas clientes
                    else if (vus)
                    {
                        ////-------------------------------COMPANY CODE
                        var error = "";
                        for (int i = cont2; i >= 0; i--)
                        {
                            if (admins[i, 0] == us.BUNIT && admins[i, 1] == usuario_id)
                            {
                                us.BUNITX = false;
                                error = ". Registro duplicado<br/>";
                            }
                        }
                        SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();
                        if (b != null)
                        {
                            sociedad.Add(b);
                            admins[cont2, 0] = da.BUNIT.ToString();
                            usuariosoc[cont4, 0] = da.BUNIT.ToString();
                        }
                        if (!us.BUNITX)
                        {
                            us.BUNIT = us.BUNIT + "?";
                            messa = messa + cont + error;
                            cont++;
                        }
                        ////-------------------------------CLIENTE
                        error = "";
                        CLIENTE k = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(us.KUNNR) && cc.ACTIVO).FirstOrDefault();
                        for (int i = cont2; i >= 0; i--)
                        {
                            if (client[i, 0] == us.KUNNR && client[i, 1] == usuario_id)
                            {
                                us.KUNNRX = false;
                                error = ". Registro duplicado<br/>";
                            }
                        }
                        if (k == null)
                        {
                            us.KUNNRX = false;
                            error = ". Error en el ciente";
                        }
                        else
                        {
                            clientes.Add(k);
                            client[cont2, 0] = us.KUNNR.ToString();

                        }
                        if (!us.KUNNRX)
                        {
                            us.KUNNR = us.KUNNR + "?";
                            messa = messa+cont + error;
                            cont++;
                        }
                        client[cont2, 1] = usuario_id;
                        admins[cont2, 1] = usuario_id;
                        usuariosoc[cont4, 1] = usuario_id;
                        da.mess = messa;
                        us.mess = da.mess;
                        tablas[cont2, 10] = messa;
                    }
                    cont2++;
                }
                else if (sel == "super")
                {
                    //Usuario nuevo con Co Code
                    if (!vus)
                    {
                        ////-------------------------------CLIENTE
                        if (us.KUNNR != null && us.KUNNR != "")
                        {
                            us.KUNNRX = false;
                        }
                        else
                        {
                            tabla1[cont3, 0] = da.KUNNR.ToString();
                        }
                        if (!us.KUNNRX)
                        {
                            us.KUNNR = us.KUNNR + "?";
                            messa = cont + ". Este usuario no acepta clientes<br/>";
                            cont++;
                        }

                        ////-------------------------------COMPANY CODE
                        var error = "";
                        SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();
                        for (int i = cont3; i >= 0; i--)
                        {
                            if (admins[i, 0] == us.BUNIT && admins[i, 1] == us.ID)
                            {
                                us.BUNITX = false;
                                error = ". Registro duplicado<br/>";
                            }
                        }
                        if (b == null)
                        {
                            us.BUNITX = false;
                            error = ". Error en la Sociedad<br/>";
                        }
                        else
                        {
                            sociedad.Add(b);
                            admins[cont3, 0] = da.BUNIT.ToString();
                            tabla1[cont3, 1] = da.BUNIT.ToString();
                            //gua1[cont3] = da.BUNIT.ToString();
                            usuariosoc[cont4, 0] = da.BUNIT.ToString();
                        }
                        if (!us.BUNITX)
                        {
                            us.BUNIT = us.BUNIT + "?";
                            messa = messa + cont + error;
                            cont++;
                        }

                        ////-------------------------------NIVEL

                        PUESTO pi = db.PUESTOes.Where(x => x.ID == puesto_id && x.ACTIVO == true).FirstOrDefault();
                        if (pi == null)
                            us.PUESTO_IDX = false;
                        else
                        {
                            puesto.Add(pi);
                            tabla1[cont3, 2] = da.PUESTO_ID.ToString();
                        }
                        if (!us.PUESTO_IDX)
                        {
                            us.PUESTO_ID = us.PUESTO_ID + "?";
                            messa = messa + cont + men;
                            cont++;
                        }

                        ////-------------------------------USUARIO ID
                        bool existeUsuario = false;
                        var err = ". Error en el ID de usuario<br/>";
                        if (us.ID == null || us.ID == "")
                            us.IDX = false;
                        else if (IDs.Contains(us.ID))
                        {
                            us.IDX = false;
                            err = ". ID de usuario duplicado<br/>";
                        }
                        else
                        {
                            USUARIO u = db.USUARIOs.Where(xu => xu.ID.Equals(us.ID)).FirstOrDefault();
                            if (u != null)
                            {
                                existeUsuario = true;
                                err = ". El usuario ya existe<br/>";
                                IDs[cont4] = us.ID;
                                usuario_id= us.ID;
                                admins[cont3, 1] = us.ID.ToString();
                                tabla1[cont3, 3] = da.ID.ToString();
                                tabla1[cont3, 4] = da.NOMBRE.ToString();
                                tabla1[cont3, 5] = da.APELLIDO_P.ToString();
                                tabla1[cont3, 6] = da.APELLIDO_M.ToString();
                                usuariosoc[cont4, 1] = da.ID.ToString();
                                us.ID = us.ID + "!";
                            }
                            else
                            {
                                usuarios.Add(u);
                                IDs[cont4] = us.ID;
                                usuario_id = us.ID;
                                admins[cont3, 1] = us.ID.ToString();
                                tabla1[cont3, 3] = da.ID.ToString();
                                tabla1[cont3, 4] = da.NOMBRE.ToString();
                                tabla1[cont3, 5] = da.APELLIDO_P.ToString();
                                tabla1[cont3, 6] = da.APELLIDO_M.ToString();
                                usuariosoc[cont4, 1] = da.ID.ToString();
                            }
                        }

                        if (!us.IDX|| existeUsuario)
                        {
                            if (!us.IDX) {
                                us.ID = us.ID + "?";
                            }
                            messa = messa + cont + err;
                            cont++;
                        }

                        ////-------------------------------EMAIL
                        if (!ComprobarEmail(us.EMAIL) )
                        {
                            us.EMAILX = false;
                        }
                        else
                            tabla1[cont3, 7] = da.EMAIL.ToString();
                        if (!us.EMAILX)
                        {
                            us.EMAIL = us.EMAIL + "?";
                            messa = messa + cont + ". Error en el correo<br/>";
                            cont++;
                        }

                        ////-------------------------------IDIOMA
                        SPRA si = db.SPRAS.FirstOrDefault(x => x.ID.Equals(us.SPRAS_ID));
                        if (si == null)
                        {
                            us.SPRAS_IDX = false;
                        }
                        else
                        {
                            tabla1[cont3, 8] = da.SPRAS_ID.ToString();
                            tabla1[cont3, 9] = c.Encrypt(da.PASS.ToString());
                        }
                        if (!us.SPRAS_IDX)
                        {
                            us.SPRAS_ID = us.SPRAS_ID + "?";
                            messa = messa + cont + ". Error en el idioma<br/>";
                            cont++;
                        }

                        da.mess = messa;
                        us.mess = da.mess;
                        tablas[cont3, 10] = messa;
                    }
                    //Asignacion de mas Co Codes
                    else if (vus)
                    {
                        ////-------------------------------CLIENTE
                        if (us.KUNNR != null && us.KUNNR != "")
                        {
                            us.KUNNRX = false;
                        }
                        if (!us.KUNNRX)
                        {
                            us.KUNNR = us.KUNNR + "?";
                            messa = messa+cont + ". Este usuario no acepta clientes<br/>";
                            cont++;
                        }
                        var error = "";
                        SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();
                        for (int i = cont3; i >= 0; i--)
                        {
                            if (admins[i, 0] == us.BUNIT && admins[i, 1] == usuario_id)
                            {
                                us.BUNITX = false;
                                error = ". Registro duplicado<br/>";
                            }
                        }
                        if (b == null)
                        {
                            us.BUNITX = false;
                            error = ". Error en la sociedad";
                        }
                        else
                        {
                            sociedad.Add(b);
                            admins[cont3, 0] = da.BUNIT.ToString();
                            usuariosoc[cont4, 0] = da.BUNIT.ToString();
                        }
                        if (!us.BUNITX)
                        {
                            us.BUNIT = us.BUNIT + "?";
                            messa = messa + cont + error;
                            cont++;
                        }
                        da.mess = messa;
                        admins[cont3, 1] = usuario_id;
                        usuariosoc[cont4, 1] = usuario_id;
                        us.mess = da.mess;
                        tabla1[cont3, 10] = messa;
                    }
                    cont3++;
                }

                uu.Add(us);
                cont4++;
            }
            Session["tablas"] = tablas;
            Session["tabla1"] = tabla1;
            Session["usuariosoc"] = usuariosoc;
            Session["client"] = client;
            Session["admins"] = admins;
            Session["rowst"] = cont2;
            Session["rows1"] = cont3;
            JsonResult jl = Json(uu, JsonRequestBehavior.AllowGet);
            return jl;
        }

        public bool ExisteUsuario(string user)
        {
            var existeusuario = db.USUARIOs.Where(t => t.ID == user).SingleOrDefault();
            if (existeusuario == null)
                return false;
            else
                return true;
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

                    CLIENTE u = clientes.FirstOrDefault(x => x.KUNNR.Equals(doc.KUNNR));
                    if (u == null)
                    {
                        u = db.CLIENTEs.FirstOrDefault(cc => cc.KUNNR.Equals(doc.KUNNR) && cc.ACTIVO);
                        if (u == null)
                            doc.VKORG = null;
                        else
                            clientes.Add(u);
                    }

                    CLIENTE c = clientes.FirstOrDefault(cc => cc.KUNNR.Equals(doc.KUNNR) && cc.ACTIVO);
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
        public JsonResult Agregar()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }
            string[,] tablas = (string[,])Session["tablas"];
            int rowst = (int)Session["rowst"];
            string[,] tabla1 = (string[,])Session["tabla1"];
            string[,] admins = (string[,])Session["admins"];
            string[,] usuariosoc = (string[,])Session["usuariosoc"];
            int rows1 = (int)Session["rows1"];
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();
            List<DET_AGENTE1> ld1 = new List<DET_AGENTE1>();
            ld = ObjAList2(tablas, rowst);
            int cont = 0;

            foreach (DET_AGENTE1 da in ld)
            {
                USUARIO us = new USUARIO();

                if (da.mess == null || da.mess == "")
                {
                    if (da.ID != null)
                    {
                        ////---------------------------- USUARIO
                        try
                        {
                            us.ID = da.ID.Trim();
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
                        catch (Exception e)
                        {
                        }
                    }
                }
            }

            //List<DET_AGENTE1> ld1 = new List<DET_AGENTE1>();
            ld1 = ObjAList3();
            string usuario_id = null;
            foreach (DET_AGENTE1 da in ld1)
            {
                if (!String.IsNullOrEmpty(da.ID))
                {
                    usuario_id = da.ID;
                }
                USUARIOF uf = new USUARIOF();

                if (da.mess == null || da.mess == "")
                {
                    ////---------------------------- USUARIOF
                    try
                    {
                        uf.USUARIO_ID = usuario_id;
                        uf.VKORG = da.VKORG;
                        uf.VTWEG = da.VTWEG;
                        uf.SPART = da.SPART;
                        uf.KUNNR = da.KUNNR;
                        uf.ACTIVO = true;
                        uf.USUARIOC_ID = null;
                        uf.FECHAC = DateTime.Today;
                        uf.USUARIOM_ID = null;
                        uf.FECHAM = null;

                        db.USUARIOFs.Add(uf);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            ld = ObjAList2(tabla1, rows1);
            foreach (DET_AGENTE1 da in ld)
            {
                USUARIO us = new USUARIO();

                if (da.mess == null || da.mess == "")
                {
                    if (da.ID != null && !db.USUARIOs.Any(x=>x.ID==da.ID))
                    {
                        try
                        {
                            us.ID = da.ID.Trim();
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
                        catch (Exception e)
                        {
                        }
                    }
                }
            }

            ////---------------------------- Co Codes
            ld = ObjAList4();
            usuario_id = null;
            foreach (DET_AGENTE1 da in ld)
            {
                if (!String.IsNullOrEmpty(da.ID))
                {
                     usuario_id = da.ID;
                }
                USUARIO us = db.USUARIOs.Where(x => x.ID == usuario_id).First();

                if (da.mess == null || da.mess == "")
                {
                    try
                    { 
                        SOCIEDAD soc = db.SOCIEDADs.Where(x => x.BUKRS == da.BUNIT).First();
                        if (!us.SOCIEDADs.Any(x => x.BUKRS == da.BUNIT))
                        {
                            us.SOCIEDADs.Add(soc);
                        }

                        db.Entry(us).State = EntityState.Modified;
                        db.SaveChanges();
                        cont++;
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            JsonResult jl = Json(cont, JsonRequestBehavior.AllowGet);
            return jl;
        }

        private List<DET_AGENTE1> ObjAList2(string [,] dt, int rowsc)
        {

            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            var rows = 0;
            var pos = 1;

            for (int i = rows; i < rowsc; i++)
            {
                DET_AGENTE1 doc = new DET_AGENTE1();

                string a = Convert.ToString(pos);

                doc.POS = Convert.ToInt32(a);

                doc.KUNNR = dt[i, 0];
                doc.KUNNR = completa(doc.KUNNR, 10);

                CLIENTE cliente = clientes.FirstOrDefault(x => x.KUNNR.Equals(doc.KUNNR) && x.ACTIVO);
                if (cliente == null)
                {
                    doc.VKORG = null;
                }
                else
                {
                    doc.VKORG = cliente.VKORG;
                    doc.VTWEG = cliente.VTWEG;
                    doc.SPART = cliente.SPART;
                }


                doc.BUNIT = dt[i, 1];
                doc.PUESTO_ID = (!string.IsNullOrEmpty(dt[i, 2]) ? (int?)int.Parse(dt[i, 2]) : null);
                doc.ID = dt[i, 3];
                doc.NOMBRE = dt[i, 4];
                doc.APELLIDO_P = dt[i, 5];
                doc.APELLIDO_M = dt[i, 6];
                doc.EMAIL = dt[i, 7];
                doc.SPRAS_ID = dt[i, 8];
                doc.PASS = dt[i, 9];
                doc.mess = dt[i, 10];

                ld.Add(doc);
                pos++;
            }
            return ld;
        }

        private List<DET_AGENTE1> ObjAList3()
        {

            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();
            List<CLIENTE> clientes = new List<CLIENTE>();

            var dt = (string[,])Session["client"];
            var rowsc = (int)Session["rowst"];
            var rows = 0;
            var pos = 1;

            for (int i = rows; i < rowsc; i++)
            {
                DET_AGENTE1 doc = new DET_AGENTE1();

                string a = Convert.ToString(pos);

                doc.POS = Convert.ToInt32(a);
                try
                {
                    doc.KUNNR = dt[i, 0];
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
                    doc.ID = dt[i, 1];
                }
                catch (Exception e)
                {
                    doc.ID = null;
                }
                ld.Add(doc);
                pos++;
            }
            return ld;
        }

        private List<DET_AGENTE1> ObjAList4()
        {
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();

            var dt = (string[,])Session["usuariosoc"];
            var pos = 1;

            for (int i = 0; i < (dt.Length / dt.Rank); i++)
            {
                DET_AGENTE1 doc = new DET_AGENTE1();
                string id = dt[i, 1];
                string a = Convert.ToString(pos);


                if (!String.IsNullOrEmpty(id))
                {
                    doc.ID = id;
                    doc.BUNIT = dt[i, 0];
                    doc.POS = Convert.ToInt32(a);
                    ld.Add(doc);
                    pos++;
                }
            }
            return ld;
        }

        [HttpPost]
        public JsonResult Comprobar()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }

            var cli = Request["cli"];
            var com = Request["com"];
            var niv = Request["niv"];
            var usc = Request["usc"];
            var nom = Request["nom"];
            var app = Request["app"];
            var apm = Request["apm"];
            var ema = Request["ema"];
            var idi = Request["idi"];
            var pas = Request["pas"];

            if (cli == null && com == null && niv == null && usc == null && nom == null && app == null && apm == null && ema == null && idi == null && pas == null)
            {
                cli = "";com = "";niv = "";usc = "";nom = "";app = "";apm = "";ema = "";idi = "";pas = "";
            }
            int rowst = cli.Split(',').Length;
            string[,] tablas = new string[rowst, 11];
            string[,] client = new string[rowst, 2];
            string[,] tabla1 = new string[rowst, 11];
            string[,] admins = new string[rowst, 2];
            string[,] usuariosoc = new string[rowst, 2];

            string[,] compara = new string[rowst, 11];
            for(int i=0;i<rowst;i++)
            {
                    compara[i, 0] = cli.Split(',')[i];
                    compara[i, 1] = com.Split(',')[i];
                    compara[i, 2] = niv.Split(',')[i];
                    compara[i, 3] = usc.Split(',')[i];
                    compara[i, 4] = nom.Split(',')[i];
                    compara[i, 5] = app.Split(',')[i];
                    compara[i, 6] = apm.Split(',')[i];
                    compara[i, 7] = ema.Split(',')[i];
                    compara[i, 8] = idi.Split(',')[i];
                    compara[i, 9] = pas.Split(',')[i];
            }

            List<DET_AGENTE1> ld = ObjAList2(compara, rowst);

            List<Usuarios> uu = new List<Usuarios>();
            List<USUARIO> usuarios = new List<USUARIO>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            List<PUESTO> puesto = new List<PUESTO>();
            List<SOCIEDAD> sociedad = new List<SOCIEDAD>();
            int cont2 = 0;
            int cont3 = 0;
            int cont4 = 0;
            //string[] gua1 = new string[rowst];
            //string[] gua = new string[rowst];
            string[] IDs = new string[rowst];
            string p = FnCommon.ObtenerSprasId(db,User.Identity.Name);
            string usuario_id = "";
            int puesto_id = 0;

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                string sel = "";
                bool vus = false;
                Usuarios us = new Usuarios();
                Cryptography c = new Cryptography();

                us.KUNNR = da.KUNNR.Replace(" ", "");
                us.KUNNRX = true;
                us.BUNIT = da.BUNIT.Replace(" ", "");
                us.BUNITX = true;
                us.PUESTO_ID = da.PUESTO_ID.ToString();
                us.PUESTO_IDX = true;
                us.ID = da.ID.Replace(" ", "");
                us.IDX = true;
                us.NOMBRE = da.NOMBRE;
                us.APELLIDO_P = da.APELLIDO_P;
                us.APELLIDO_M = da.APELLIDO_M;
                us.EMAIL = da.EMAIL.Replace(" ", "");
                us.EMAILX = true;
                us.SPRAS_ID = da.SPRAS_ID;
                us.SPRAS_IDX = true;
                us.PASS = da.PASS;

                if (us.KUNNR != "" || us.BUNIT != "" || us.PUESTO_ID != "" || us.ID != "" || us.NOMBRE != "" || us.APELLIDO_P != "" || us.APELLIDO_M != "" || us.EMAIL != "" || us.SPRAS_ID != "" || us.PASS != "")
                {

                    string men = ". Error en el nivel<br/>";
                    if (us.PUESTO_ID != null && us.PUESTO_ID != "")
                    {
                        puesto_id = int.Parse(us.PUESTO_ID);
                    }

                    var ni = (from x in db.PUESTOTs
                              join a in db.PUESTOes on x.PUESTO_ID equals a.ID
                              where x.PUESTO_ID == puesto_id && x.SPRAS_ID.Equals(p) && (a.ACTIVO!=null && a.ACTIVO.Value)
                              select x.PUESTO_ID).FirstOrDefault();
                    var re = (from x in db.DET_APROBH where x.PUESTOC_ID == ni && x.ACTIVO select x.SOCIEDAD_ID).FirstOrDefault();

                    //Comprobacion de la asignacion de varios clientes
                    if (cont2 > 0 && us.KUNNR != ""&& (us.BUNIT == "" || us.BUNIT != "") && us.PUESTO_ID == "" && us.ID == "" && us.NOMBRE == "" && us.APELLIDO_P == "" && us.APELLIDO_M == "" && us.EMAIL == "" && us.SPRAS_ID == "" && us.PASS == "")
                        {
                            vus = true;
                            sel = "venta";
                        
                    }
                    // Comprobacion de la asignacion de varios co code
                    if (cont3 > 0  && us.BUNIT != "" && (us.KUNNR == "" || us.KUNNR != "") && us.PUESTO_ID == "" && us.ID == "" && us.NOMBRE == "" && us.APELLIDO_P == "" && us.APELLIDO_M == "" && us.EMAIL == "" && us.SPRAS_ID == "" && us.PASS == "")
                        {
                            vus = true;
                            sel = "super";
                        
                    }

                    // Validacion del tipo de usuario
                    if (((re != null && re != "") ) || (puesto_id == 8 && us.KUNNR != ""))
                    {
                        sel = "venta";
                    }
                    else if ((re == null || re == "") )
                    {
                        sel = "super";
                    }

                    //Validacion de datos
                    if (sel == "venta")
                    {
                        //Usuario nuevo con cliente
                        if (!vus )
                        {
                            ////-------------------------------CLIENTE
                            var error = "";
                            CLIENTE k = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(us.KUNNR) && cc.ACTIVO).FirstOrDefault();
                            for (int i = cont2; i >= 0; i--)
                            {
                                if (client[i, 0] == us.KUNNR && client[i, 1] == us.ID)
                                {
                                    us.KUNNRX = false;
                                    error = ". Registro duplicado<br/>";
                                }
                            }
                            if (k == null)
                            {
                                us.KUNNRX = false;
                                error = ". Error en el cliente<br/>";
                            }
                            else
                            {
                                clientes.Add(k);
                                client[cont2, 0] = us.KUNNR.ToString();
                                tablas[cont2, 0] = da.KUNNR.ToString();
                                //gua[cont2] = da.KUNNR.ToString();
                            }
                            if (!us.KUNNRX)
                            {
                                us.KUNNR = us.KUNNR + "?";
                                messa = cont + error;
                                cont++;
                            }

                            ////-------------------------------COMPANY CODE
                            SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();

                            if (b == null)
                            {
                                us.BUNITX = false;
                            }
                            else
                            {
                                sociedad.Add(b);
                                admins[cont2, 0] = da.BUNIT.ToString();
                                usuariosoc[cont4, 0] = da.BUNIT.ToString();
                                tablas[cont2, 1] = da.BUNIT.ToString();
                            }
                            if (!us.BUNITX)
                            {
                                us.BUNIT = us.BUNIT + "?";
                                messa = messa + cont + ". Error en la sociedad<br/>";
                                cont++;
                            }
                            ////-------------------------------NIVEL
                            PUESTO pi = db.PUESTOes.Where(x => x.ID == puesto_id && x.ACTIVO == true).FirstOrDefault();
                            if (pi == null)
                                us.PUESTO_IDX = false;
                            else
                            {
                                puesto.Add(pi);
                                tablas[cont2, 2] = da.PUESTO_ID.ToString();

                            }
                            if (!us.PUESTO_IDX)
                            {
                                us.PUESTO_ID = us.PUESTO_ID + "?";
                                messa = messa + cont + men;
                                cont++;
                            }

                            ////-------------------------------USUARIO ID
                            bool existeUsuario = false;
                            var err = ". Error en el ID de usuario<br/>";
                            if (string.IsNullOrEmpty(us.ID))
                                us.IDX = false;
                            else if (IDs.Contains(us.ID))
                            {
                                us.IDX = false;
                                err = ". ID de usuario duplicado<br/>";
                            }
                            else
                            {
                                USUARIO u = db.USUARIOs.FirstOrDefault(xu => xu.ID.Equals(us.ID));
                                if (u != null)
                                {
                                    existeUsuario = true;
                                    err = ". El usuario ya existe<br/>";
                                    IDs[cont4] = us.ID;
                                    usuario_id = us.ID;
                                    admins[cont2, 1] = us.ID.ToString();
                                    client[cont2, 1] = us.ID.ToString();
                                    tablas[cont2, 3] = da.ID.ToString();
                                    tablas[cont2, 4] = da.NOMBRE.ToString();
                                    tablas[cont2, 5] = da.APELLIDO_P.ToString();
                                    tablas[cont2, 6] = da.APELLIDO_M.ToString();
                                    usuariosoc[cont4, 1] = us.ID.ToString();
                                    us.ID = us.ID + "!";
                                }
                                else
                                {
                                    usuarios.Add(u);
                                    IDs[cont4] = us.ID;
                                    usuario_id = us.ID;
                                    admins[cont2, 1] = us.ID.ToString();
                                    client[cont2, 1] = us.ID.ToString();
                                    tablas[cont2, 3] = da.ID.ToString();
                                    tablas[cont2, 4] = da.NOMBRE.ToString();
                                    tablas[cont2, 5] = da.APELLIDO_P.ToString();
                                    tablas[cont2, 6] = da.APELLIDO_M.ToString();
                                    usuariosoc[cont4, 1] = us.ID.ToString();
                                }
                            }

                            if (!us.IDX || existeUsuario)
                            {
                                if (!us.IDX)
                                    us.ID = us.ID + "?";
                                messa = messa + cont + err;
                                cont++;
                            }

                            ////-------------------------------EMAIL
                            if (!ComprobarEmail(us.EMAIL))
                            {
                                us.EMAILX = false;
                            }
                            else
                                tablas[cont2, 7] = da.EMAIL.ToString();
                            if (!us.EMAILX)
                            {
                                us.EMAIL = us.EMAIL + "?";
                                messa = messa + cont + ". Error en el correo<br/>";
                                cont++;
                            }

                            ////-------------------------------IDIOMA
                            SPRA si = db.SPRAS.FirstOrDefault(x => x.ID.Equals(us.SPRAS_ID));
                            if (si == null)
                            {
                                us.SPRAS_IDX = false;
                            }
                            else
                            {
                                tablas[cont2, 8] = da.SPRAS_ID.ToString();
                                tablas[cont2, 9] = c.Encrypt(da.PASS.ToString());
                            }
                            if (!us.SPRAS_IDX)
                            {
                                us.SPRAS_ID = us.SPRAS_ID + "?";
                                messa = messa + cont + ". Error en el idioma<br/>";
                                cont++;
                            }

                            da.mess = messa;
                            us.mess = da.mess;
                            tablas[cont2, 10] = messa;

                        }
                        //Asignacion de mas clientes
                        else if (vus )
                        {
                            ////-------------------------------COMPANY CODE
                           var error = "";
                            for (int i = cont2; i >= 0; i--)
                            {
                                if (admins[i, 0] == us.BUNIT && admins[i, 1] == usuario_id)
                                {
                                    us.BUNITX = false;
                                    error = ". Registro duplicado<br/>";
                                }
                            }
                            SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();
                            if (b != null)
                            {
                                sociedad.Add(b);
                                admins[cont2, 0] = da.BUNIT.ToString();
                                usuariosoc[cont4, 0] = da.BUNIT.ToString();
                            }
                            if (!us.BUNITX)
                            {
                                us.BUNIT = us.BUNIT + "?";
                                messa = messa + cont + error;
                                cont++;
                            }
                            ////-------------------------------CLIENTE
                            error = "";
                            string pais_id = (sociedad.Any()? sociedad.Last().LAND : "");
                            CLIENTE k = db.CLIENTEs.Where(cc => cc.KUNNR.Equals(us.KUNNR)&& cc.LAND== pais_id && cc.ACTIVO ).FirstOrDefault();
                            for (int i = cont2; i >= 0; i--)
                            {
                                if (client[i, 0] == us.KUNNR && client[i, 1] == usuario_id)
                                {
                                    us.KUNNRX = false;
                                    error = ". Registro duplicado<br/>";
                                }
                            }
                            if (k == null)
                            {
                                us.KUNNRX = false;
                                error = ". Error en el ciente";
                            }
                            else
                            {
                                clientes.Add(k);
                                client[cont2, 0] = us.KUNNR.ToString();

                            }
                            if (!us.KUNNRX)
                            {
                                us.KUNNR = us.KUNNR + "?";
                                messa = messa + cont + error;
                                cont++;
                            }
                            
                            client[cont2, 1] = usuario_id;
                            admins[cont2, 1] = usuario_id;
                            usuariosoc[cont4, 1] = usuario_id;
                            da.mess = messa;
                            us.mess = da.mess;
                            tablas[cont2, 10] = messa;
                        }
                        cont2++;
                    }
                    else if (sel == "super")
                    {
                        //Usuario nuevo con Co Code
                        if (!vus)
                        {
                            ////-------------------------------CLIENTE
                            if (us.KUNNR != null && us.KUNNR != "")
                            {
                                us.KUNNRX = false;
                            }
                            else
                            {
                                tabla1[cont3, 0] = da.KUNNR.ToString();
                            }
                            if (!us.KUNNRX)
                            {
                                us.KUNNR = us.KUNNR + "?";
                                messa = cont + ". Este usuario no acepta clientes<br/>";
                                cont++;
                            }

                            ////-------------------------------COMPANY CODE
                            var error = "";
                            SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO).FirstOrDefault();
                            for (int i = cont3; i >= 0; i--)
                            {
                                if (admins[i, 0] == us.BUNIT && admins[i, 1] == us.ID)
                                {
                                    us.BUNITX = false;
                                    error = ". Registro duplicado<br/>";
                                }
                            }
                            if (b == null)
                            {
                                us.BUNITX = false;
                                error = ". Error en la Sociedad<br/>";
                            }
                            else
                            {
                                sociedad.Add(b);
                                admins[cont3, 0] = da.BUNIT.ToString();
                                tabla1[cont3, 1] = da.BUNIT.ToString();
                                //gua1[cont3] = da.BUNIT.ToString();
                                usuariosoc[cont4, 0] = da.BUNIT.ToString();
                            }
                            if (!us.BUNITX)
                            {
                                us.BUNIT = us.BUNIT + "?";
                                messa = messa + cont + error;
                                cont++;
                            }

                            ////-------------------------------NIVEL

                            PUESTO pi = db.PUESTOes.Where(x => x.ID == puesto_id && x.ACTIVO == true).FirstOrDefault();
                            if (pi == null)
                                us.PUESTO_IDX = false;
                            else
                            {
                                puesto.Add(pi);
                                tabla1[cont3, 2] = da.PUESTO_ID.ToString();
                            }
                            if (!us.PUESTO_IDX)
                            {
                                us.PUESTO_ID = us.PUESTO_ID + "?";
                                messa = messa + cont + men;
                                cont++;
                            }

                            ////-------------------------------USUARIO ID
                            bool existeUsuario = false;
                            var err = ". Error en el ID de usuario<br/>";
                            if (string.IsNullOrEmpty(us.ID))
                                us.IDX = false;
                            else if (IDs.Contains(us.ID))
                            {
                                us.IDX = false;
                                err = ". ID de usuario duplicado<br/>";
                            }
                            else
                            {
                                USUARIO u = db.USUARIOs.FirstOrDefault(xu => xu.ID.Equals(us.ID));
                                if (u != null)
                                {
                                    existeUsuario = true;
                                    err = ". El usuario ya existe<br/>";
                                    IDs[cont4] = us.ID;
                                    usuario_id = us.ID;
                                    admins[cont3, 1] = us.ID.ToString();
                                    tabla1[cont3, 3] = da.ID.ToString();
                                    tabla1[cont3, 4] = da.NOMBRE.ToString();
                                    tabla1[cont3, 5] = da.APELLIDO_P.ToString();
                                    tabla1[cont3, 6] = da.APELLIDO_M.ToString();
                                    usuariosoc[cont4, 1] = da.ID.ToString();
                                    us.ID = us.ID + "!";
                                }
                                else
                                {
                                    usuarios.Add(u);
                                    IDs[cont4] = us.ID;
                                    usuario_id = us.ID;
                                    admins[cont3, 1] = us.ID.ToString();
                                    tabla1[cont3, 3] = da.ID.ToString();
                                    tabla1[cont3, 4] = da.NOMBRE.ToString();
                                    tabla1[cont3, 5] = da.APELLIDO_P.ToString();
                                    tabla1[cont3, 6] = da.APELLIDO_M.ToString();
                                    usuariosoc[cont4, 1] = da.ID.ToString();
                                }
                            }

                            if (!us.IDX || existeUsuario)
                            {
                                if (!us.IDX)
                                {
                                    us.ID = us.ID + "?";
                                }
                                messa = messa + cont + err;
                                cont++;
                            }

                            ////-------------------------------EMAIL
                            if (!ComprobarEmail(us.EMAIL) )
                            {
                                us.EMAILX = false;
                            }
                            else
                                tabla1[cont3, 7] = da.EMAIL.ToString();
                            if (!us.EMAILX)
                            {
                                us.EMAIL = us.EMAIL + "?";
                                messa = messa + cont + ". Error en el correo<br/>";
                                cont++;
                            }

                            ////-------------------------------IDIOMA
                            SPRA si = db.SPRAS.FirstOrDefault(x => x.ID.Equals(us.SPRAS_ID));
                            if (si == null)
                            {
                                us.SPRAS_IDX = false;
                            }
                            else
                            {
                                tabla1[cont3, 8] = da.SPRAS_ID.ToString();
                                tabla1[cont3, 9] = c.Encrypt(da.PASS.ToString());
                            }
                            if (!us.SPRAS_IDX)
                            {
                                us.SPRAS_ID = us.SPRAS_ID + "?";
                                messa = messa + cont + ". Error en el idioma<br/>";
                                cont++;
                            }

                            da.mess = messa;
                            us.mess = da.mess;
                            tablas[cont3, 10] = messa;
                        }
                        //Asignacion de mas Co Codes
                        else if (vus )
                        {
                            ////-------------------------------CLIENTE
                            if (us.KUNNR != null && us.KUNNR != "")
                            {
                                us.KUNNRX = false;
                            }
                            if (!us.KUNNRX)
                            {
                                us.KUNNR = us.KUNNR + "?";
                                messa = messa + cont + ". Este usuario no acepta clientes<br/>";
                                cont++;
                            }
                            var error = "";
                            SOCIEDAD b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) && x.ACTIVO ).FirstOrDefault();
                            for (int i = cont3; i >= 0; i--)
                            {
                                if (admins[i, 0] == us.BUNIT && admins[i, 1] == usuario_id)
                                {
                                    us.BUNITX = false;
                                    error = ". Registro duplicado<br/>";
                                }
                            }
                            if (b == null)
                            {
                                us.BUNITX = false;
                                error = ". Error en la sociedad";
                            }
                            else
                            {
                                sociedad.Add(b);
                                admins[cont3, 0] = da.BUNIT.ToString();
                                usuariosoc[cont4, 0] = da.BUNIT.ToString();
                            }
                            if (!us.BUNITX)
                            {
                                us.BUNIT = us.BUNIT + "?";
                                messa = messa + cont + error;
                                cont++;
                            }
                            da.mess = messa;
                            admins[cont3, 1] = usuario_id;
                            usuariosoc[cont4, 1] = usuario_id;
                            us.mess = da.mess;
                            tabla1[cont3, 10] = messa;
                        }
                        cont3++;
                    }
                    uu.Add(us);
                    cont4++;
                }
            }
            Session["tablas"] = tablas;
            Session["tabla1"] = tabla1;
            Session["usuariosoc"] = usuariosoc;
            Session["client"] = client;
            Session["admins"] = admins;
            Session["rowst"] = cont2;
            Session["rows1"] = cont3;

            JsonResult jl = Json(uu, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        public JsonResult Guardar()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }
            string[,] tablas = (string[,])Session["tablas"];
            string[,] tabla1 = (string[,])Session["tabla1"];

            int rows1 = (int)Session["rows1"];
            int rowst = (int)Session["rowst"];
            List<DET_AGENTE1> ld = ObjAList2(tablas, rowst);
            List<DET_AGENTE1> ld1 = ObjAList2(tabla1, rows1);
            List<DET_AGENTE1> usuariosGuardar = new List<DET_AGENTE1>();
            foreach (DET_AGENTE1 da in ld)
            {
                if (da.ID != null)
                {
                    usuariosGuardar.Add(da);
                }
            }
            foreach (DET_AGENTE1 da in ld1)
            {
                if (da.ID != null && !usuariosGuardar.Any(x=>x.ID.Trim() == da.ID.Trim()))
                {
                    usuariosGuardar.Add(da);
                }
            }


            ////---------------------------- USUARIO
            foreach (DET_AGENTE1 da in usuariosGuardar)
            {
                USUARIO us = new USUARIO();

                try
                {
                    us.ID = da.ID.Trim();
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
                    if (db.USUARIOs.Any(x => x.ID == us.ID))
                    {
                        us.PASS = (from x in db.USUARIOs where x.ID.Equals(da.ID) && x.ACTIVO == true select x.PASS).FirstOrDefault().ToString();
                        db.Entry(us).State = EntityState.Modified;
                    }
                    else
                    {
                        db.USUARIOs.Add(us);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Log.ErrorLogApp(e, "Usuarios", "Guardar");
                }
                
            }

            ////---------------------------- USUARIOF
            ld = ObjAList3();
            foreach (DET_AGENTE1 da in usuariosGuardar)
            {
               //Se eliminan clientes.
               List<USUARIOF> clientesU= db.USUARIOFs.Where(x=>x.USUARIO_ID==da.ID).ToList();         
                foreach (USUARIOF clienteU in clientesU)
                {
                    if (!ld.Any(x => x.ID == da.ID && x.KUNNR == clienteU.KUNNR)) {
                        db.USUARIOFs.Remove(clienteU);
                    }
                }    
            }
                foreach (DET_AGENTE1 da in ld)
            {
                try
                {
                    USUARIOF uf = new USUARIOF
                    {
                        USUARIO_ID = da.ID.Trim(),
                        VKORG = da.VKORG,
                        VTWEG = da.VTWEG,
                        SPART = da.SPART,
                        KUNNR = da.KUNNR,
                        ACTIVO = true,
                        USUARIOC_ID = null,
                        FECHAC = DateTime.Today,
                        USUARIOM_ID = null,
                        FECHAM = null
                    };
                    if (db.USUARIOFs.Any(x => x.KUNNR.Equals(da.KUNNR) && x.USUARIO_ID.Equals(da.ID)))
                    {
                        db.Entry(uf).State = EntityState.Modified;
                    }
                    else
                    {
                        db.USUARIOFs.Add(uf);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Log.ErrorLogApp(e, "Usuarios", "Guardar");
                }
            }
        
       

            ////---------------------------- Co Codes
            ld = ObjAList4();
            foreach (DET_AGENTE1 da in usuariosGuardar)
            {
                //Se eliminan Sociedades.
                USUARIO us = db.USUARIOs.Include(x => x.SOCIEDADs).First(x => x.ID == da.ID);
                List<SOCIEDAD> sociedadesU = sociedadesDao.ListaSociedades(TATConstantes.ACCION_LISTA_SOCPORUSUARIO,null,da.ID);
                foreach (SOCIEDAD sociedadU in sociedadesU)
                {
                    if (!ld.Any(x => x.ID == da.ID && x.BUNIT == sociedadU.BUKRS))
                    {
                        us.SOCIEDADs.Remove(us.SOCIEDADs.First(x =>  x.BUKRS == sociedadU.BUKRS));
                    }
                }
                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();
            }
            foreach (DET_AGENTE1 da in ld)
            {
                try
                {
                    USUARIO us = db.USUARIOs.Include(x=>x.SOCIEDADs).First(x => x.ID == da.ID);
                    SOCIEDAD soc = db.SOCIEDADs.FirstOrDefault(x => x.BUKRS == da.BUNIT);
                    if (!us.SOCIEDADs.Any(x => x.BUKRS == da.BUNIT) && soc != null)
                    {
                        us.SOCIEDADs.Add(soc);
                    }

                    db.Entry(us).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    Log.ErrorLogApp(e, "Usuarios", "Guardar");
                }
            }

            JsonResult jl = Json("", JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult AgregarT()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }
            List<Usuarios> cc = new List<Usuarios>();
            
            var uscs = Request["usc"];
            string usc = "";
            if (uscs != null)
            {
                usc = uscs.Split(',')[0].ToString();
            }
            var uscx = true;
            int rowst = 0;
            if (!string.IsNullOrEmpty(usc))
            {
                USUARIO u = db.USUARIOs.Where(xu => xu.ID.Equals(usc) && (xu.ACTIVO != null && xu.ACTIVO.Value)).FirstOrDefault();
                if (u == null)
                    uscx = false;
                else
                {
                    List<SOCIEDAD> sociedadesU = u.SOCIEDADs.ToList();
                    List<USUARIOF> clientesU = db.USUARIOFs.Where(x=> x.USUARIO_ID == usc && (x.ACTIVO != null && x.ACTIVO.Value)).ToList();
                    int i = 0;
                    if (!sociedadesU.Any())
                    {
                        AgregarUsuario(ref rowst,ref cc,u,i,0);
                    }
                    else {
                        foreach (SOCIEDAD sociedadU in sociedadesU)
                        {
                            int j = 0;
                            if (clientesU.Any(x => x.CLIENTE.LAND == sociedadU.LAND))
                            {
                                foreach (USUARIOF clienteU in clientesU.Where(x => x.CLIENTE.LAND == sociedadU.LAND))
                                {
                                    AgregarUsuario(ref rowst, ref cc, u, i, j,sociedadU,clienteU);
                                    i++;
                                    j++;
                                }
                            }
                            else
                            {
                                AgregarUsuario(ref rowst, ref cc, u, i, j, sociedadU);
                                i++;
                                j++;
                            }
                            
                        }
                    }
                    
                }
                if (!uscx)
                {
                    Usuarios ul = new Usuarios();
                    ul.KUNNR = "";
                    ul.BUNIT = "";
                    ul.PUESTO_ID = "";
                    ul.ID = usc + "?";
                    ul.NOMBRE = "";
                    ul.APELLIDO_P = "";
                    ul.APELLIDO_M = "";
                    ul.EMAIL = "";
                    ul.SPRAS_ID = "";
                    ul.PASS = "";
                    ul.mess = "El usuario no existe";

                    rowst++;
                    cc.Add(ul);
                }
            }

            int cont2 = rowst;
            int cont3 = rowst;
            string[,] tablas = new string[rowst, 11];
            string[,] client = new string[rowst, 2];
            string[,] tabla1 = new string[rowst, 11];
            string[,] admins = new string[rowst, 2];
            string[,] usuariosoc = new string[rowst, 2];

            Session["tablas"] = tablas;
            Session["tabla1"] = tabla1;
            Session["usuariosoc"] = usuariosoc;
            Session["client"] = client;
            Session["admins"] = admins;
            Session["rowst"] = cont2;
            Session["rows1"] = cont3;

            JsonResult jl = Json(cc, JsonRequestBehavior.AllowGet);
            return jl;
        }
         void AgregarUsuario(ref int  rowst,ref List<Usuarios> cc, USUARIO u , int i,int j,SOCIEDAD sociedadU=null,USUARIOF clienteU=null)
        {
            Usuarios ul = new Usuarios();
            ul.KUNNR = (clienteU!=null? clienteU.KUNNR:"");
            ul.BUNIT = "";
            ul.PUESTO_ID = "";
            ul.ID = "";
            ul.NOMBRE = "";
            ul.APELLIDO_P = "";
            ul.APELLIDO_M = "";
            ul.EMAIL = "";
            ul.SPRAS_ID = "";
            ul.PASS = "";
            ul.mess = "";
            var com = "";
            if (j == 0 && sociedadU!=null)
            {
                ul.BUNIT = sociedadU.BUKRS;
            }
            if (i == 0)
            {
                ul.ID = u.ID;
                if (sociedadU == null&& clienteU == null)
                {
                    ul.BUNIT = u.BUNIT;
                }
                ul.PUESTO_ID = u.PUESTO_ID.ToString();
                com = u.NOMBRE;
                if (com != null)
                    ul.NOMBRE = com;
                com = u.APELLIDO_P;
                if (com != null)
                    ul.APELLIDO_P = com;
                com = u.APELLIDO_M;
                if (com != null)
                    ul.APELLIDO_M = com;
                com = u.EMAIL;
                if (com != null)
                    ul.EMAIL = com;
                com = u.SPRAS_ID;
                if (com != null)
                    ul.SPRAS_ID = com;
                ul.ID = ul.ID + "!";
                ul.mess = "1. El usuario ya existe<br/>";
            }
            rowst++;
            cc.Add(ul);
        }

        [HttpPost]
        public FileResult Descargar()
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return null;
            }
            var uSuario = db.USUARIOs.ToList();
            generarExcelHome(uSuario, Server.MapPath("~/pdfTemp/"));
            return File(Server.MapPath("~/pdfTemp/Usuarios_" + DateTime.Now.ToShortDateString() + ".xlsx"), "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Usuarios_" + DateTime.Now.ToShortDateString() + ".xlsx");
        }

        public void generarExcelHome(List<USUARIO> lst, string ruta)
        {
            string spra = Session["spras"].ToString();
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            try
            {
                worksheet.Cell("A1").Value = new[] { new { BANNER = "Id" }, };
                worksheet.Cell("B1").Value = new[] { new { BANNER = "Nombre" }, };
                worksheet.Cell("C1").Value = new[] { new { BANNER = "Email" }, };
                worksheet.Cell("D1").Value = new[] { new { BANNER = "Rol" }, };

                for (int i = 2; i <= (lst.Count + 1); i++)
                {
                    var pues = lst[i - 2].PUESTO_ID;
                    var puesto = db.PUESTOTs.Where(x => x.PUESTO_ID == pues && x.SPRAS_ID.Equals(spra)).Select(x => x.TXT50).FirstOrDefault();
                    worksheet.Cell("A" + i).Value = new[] { new { BANNER = lst[i - 2].ID }, };
                    worksheet.Cell("B" + i).Value = new[] { new { BANNER = (lst[i - 2].NOMBRE + " " + lst[i - 2].APELLIDO_P + " " + lst[i - 2].APELLIDO_M) }, };
                    worksheet.Cell("C" + i).Value = new[] { new { BANNER = lst[i - 2].EMAIL }, };
                    worksheet.Cell("D" + i).Value = new[] { new { BANNER = puesto }, };
                }
                var rt = ruta + @"\Usuarios_" + DateTime.Now.ToShortDateString() + ".xlsx";
                workbook.SaveAs(rt);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
            }
        }
        


        public JsonResult Idioma(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from x in db.SPRAS
                     where x.ID.Contains(Prefix)
                     select new { x.ID, x.DESCRIPCION }).ToList();

            if (c.Count == 0)
            {
                var c2 = (from x in db.SPRAS
                          where x.DESCRIPCION.Contains(Prefix)
                          select new { x.ID, x.DESCRIPCION }).ToList();
                c.AddRange(c2);
            }

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        public JsonResult Nivel(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";
            string p = db.USUARIOs.Find(User.Identity.Name).SPRAS_ID;
            
            var c = (from x in db.PUESTOTs
                     join a in db.PUESTOes on x.PUESTO_ID equals a.ID
                     where x.TXT50.Contains(Prefix) & x.SPRAS_ID.Equals(p) & a.ACTIVO == true
                     group x by new { x.PUESTO_ID, x.TXT50 } into g
                     select new { ID = g.Key.PUESTO_ID, TEXTO = g.Key.TXT50 }).ToList();

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);

            return cc;
        }

        public JsonResult getUsuarios(string Prefix, string ID)
        {
            var usuario = db.USUARIOs.Where(t => t.ID == ID).SingleOrDefault();
            //Se comenta el filtro de puesto y sociedad, solo buscara los usuarios activos
            var usuarios = db.USUARIOs.Where(x => x.ACTIVO == true /*&& x.PUESTO_ID == usuario.PUESTO_ID && x.BUNIT == usuario.BUNIT*/).ToList();
            usuarios.Remove(usuario);
            var backups = db.DELEGARs.Where(t => t.USUARIO_ID == usuario.ID && t.ACTIVO == true).ToList();
            foreach (var b in backups)
            {
                var usr = db.USUARIOs.Where(t => t.ID == b.USUARIOD_ID).SingleOrDefault();
                usuarios.Remove(usr);
            }
            //var ac_usuarios = usuarios.Select(x => new { x.ID, DESCRIPCION = x.ID + "-" + x.NOMBRE+" "+x.APELLIDO_P });
            if (Prefix == null)
                Prefix = "";

            var c = (from x in usuarios
                     where x.ID.Contains(Prefix)
                     select new { x.ID, DESCRIPCION = x.ID + " - " + x.NOMBRE + " " + x.APELLIDO_P }).ToList();

            if (c.Count == 0)
            {
                var c2 = (from x in db.USUARIOs
                          where x.NOMBRE.Contains(Prefix)
                          select new { x.ID, DESCRIPCION = x.ID + " - " + x.NOMBRE + " " + x.APELLIDO_P }).ToList();
                c.AddRange(c2);
            }

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        public ActionResult AddBackup(string ID)
        {
            int pagina = 606; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            var usuario = db.USUARIOs.Where(t => t.ID == ID).SingleOrDefault();
            //Se comenta el filtro de puesto y sociedad, solo buscara los usuarios activos
            var usuarios = db.USUARIOs.Where(x => x.ACTIVO == true /*&& x.PUESTO_ID == usuario.PUESTO_ID && x.BUNIT == usuario.BUNIT*/).ToList();
            usuarios.Remove(usuario);
            var backups = db.DELEGARs.Where(t => t.USUARIO_ID == ID && t.ACTIVO==true).ToList();
            foreach(var b in backups)
            {
                var usr=db.USUARIOs.Where(t => t.ID == b.USUARIOD_ID).SingleOrDefault();
                usuarios.Remove(usr);
            }
            if(backups.Count>0)
            ViewBag.ultimoback = Convert.ToDateTime(backups.OrderByDescending(t => t.FECHAF).First().FECHAF).AddDays(1).ToString("dd/MM/yyyy");
            //if (usuarios.Count() > 0)
                ViewBag.USUARIOD_ID = new SelectList(usuarios.ToList(), "ID", "ID", "");
            //else
              //  ViewBag.USUARIOD_ID = new SelectList(new List<string> { "No data" });
            DELEGAR usuarioback = new DELEGAR();
            usuarioback.ACTIVO = true;
            usuarioback.USUARIO_ID = ID;
            return View(usuarioback);
        }

        [HttpPost]
        public ActionResult AddBackup([Bind(Include = "USUARIO_ID,USUARIOD_ID,FECHAI,FECHAF,ACTIVO")]DELEGAR delegar)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            int pagina = 606; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina, User.Identity.Name, this.ControllerContext.Controller);
            var usuario = db.USUARIOs.Where(t => t.ID == delegar.USUARIO_ID).SingleOrDefault();
            //Se comenta el filtro de puesto y sociedad, solo buscara los usuarios activos
            var usuarios = db.USUARIOs.Where(x => x.ACTIVO == true /*&& x.PUESTO_ID == usuario.PUESTO_ID && x.BUNIT == usuario.BUNIT*/).ToList();
            usuarios.Remove(usuario);
            var backups = db.DELEGARs.Where(t => t.USUARIO_ID == delegar.USUARIO_ID && t.ACTIVO == true).ToList();
            foreach (var b in backups)
            {
                var usr = db.USUARIOs.Where(t => t.ID == b.USUARIOD_ID).SingleOrDefault();
                usuarios.Remove(usr);
            }
            if(backups.Count>0)
            ViewBag.ultimoback = Convert.ToDateTime(backups.OrderByDescending(t => t.FECHAF).First().FECHAF).AddDays(1).ToString("dd/MM/yyyy");
            //if (usuarios.Count() > 0)
            ViewBag.USUARIOD_ID = new SelectList(usuarios.ToList(), "ID", "ID", "");
            if (ModelState.IsValid)
            {
                var ultdelegados = db.DELEGARs.Where(t => t.USUARIO_ID == delegar.USUARIO_ID && t.ACTIVO==true).OrderByDescending(t => t.FECHAF).FirstOrDefault();
                if (ultdelegados != null)
                {
                    if (ultdelegados.FECHAF < delegar.FECHAI)
                    {
                        try
                        {
                            DELEGAR delegado = new DELEGAR { ACTIVO = delegar.ACTIVO, FECHAF = delegar.FECHAF, FECHAI = delegar.FECHAI, USUARIOD_ID = delegar.USUARIOD_ID, USUARIO_ID = delegar.USUARIO_ID };
                            var delegadosanteriores = db.DELEGARs.Where(t => t.USUARIO_ID == delegar.USUARIO_ID).ToList();
                            foreach (var de in delegadosanteriores)
                            {
                                if (de.FECHAF < DateTime.Now)
                                    de.ACTIVO = false;
                            }
                            db.DELEGARs.Add(delegado);
                            db.SaveChanges();
                            return RedirectToAction("Details", new { id = delegar.USUARIO_ID });
                        }
                        catch (Exception e)
                        {
                            TempData["MessageBackupRepetido"] = "Mensaje";
                            return View(delegar);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("MessageFechaBackup", "La fecha del backup se mezcla con otro backrup activo");
                        return View(delegar);
                    }
                }
                else
                {
                    try
                    {
                        DELEGAR delegado = new DELEGAR { ACTIVO = delegar.ACTIVO, FECHAF = delegar.FECHAF, FECHAI = delegar.FECHAI, USUARIOD_ID = delegar.USUARIOD_ID, USUARIO_ID = delegar.USUARIO_ID };
                        var delegadosanteriores = db.DELEGARs.Where(t => t.USUARIO_ID == delegar.USUARIO_ID).ToList();
                        foreach (var de in delegadosanteriores)
                        {
                            if (de.FECHAF < DateTime.Now)
                                de.ACTIVO = false;
                        }
                        db.DELEGARs.Add(delegado);
                        db.SaveChanges();
                        return RedirectToAction("Details", new { id = delegar.USUARIO_ID });
                    }
                    catch (Exception e)
                    {
                        TempData["MessageBackupRepetido"] = "Mensaje";
                        return View(delegar);
                    }
                }
            }
            else
            {
                return View(delegar);
            }
        }

        public ActionResult EditBackup(string id,string idd, string fi, string ff)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            int pagina = 607; //ID EN BASE DE DATOS
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
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(831) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

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
                
                //Para las version de las fechas
                var arrF = fi.Split('/');
                var dtgd = arrF[1] + '/' + arrF[0] + '/' + arrF[2];
                DateTime dt = DateTime.Parse(dtgd);
                var arrFf = ff.Split('/');
                var dtgff = arrFf[1] + '/' + arrFf[0] + '/' + arrFf[2];
                DateTime dff = DateTime.Parse(dtgff);
                var deledit = db.DELEGARs
                          .Where(x => x.USUARIO_ID == id && x.USUARIOD_ID == idd && x.FECHAI == dt&& x.FECHAF==dff).FirstOrDefault();
            
            return View(deledit);

        }

        [HttpPost]
        public ActionResult EditBackup([Bind(Include = "USUARIO_ID,USUARIOD_ID,FECHAI,FECHAF,ACTIVO")]DELEGAR delegar)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(delegar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = delegar.USUARIO_ID });
            }
            else
            {
                return View(delegar);
            }
        }

        public ActionResult DeleteBackup(string id, string idd, string fi, string ff)
        {
            string uz = User.Identity.Name;
            var userz = db.USUARIOs.Where(a => a.ID.Equals(uz)).FirstOrDefault();
            if (!usuValidateLogin.validaUsuario(userz.ID))
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            //Para las version de las fechas
            var arrF = fi.Split('/');
            var dtgd = arrF[1] + '/' + arrF[0] + '/' + arrF[2];
            DateTime dt = DateTime.Parse(dtgd);
            var arrFf = ff.Split('/');
            var dtgff = arrFf[1] + '/' + arrFf[0] + '/' + arrFf[2];
            DateTime dff = DateTime.Parse(dtgff);
            var deledit = db.DELEGARs
                      .Where(x => x.USUARIO_ID == id && x.USUARIOD_ID == idd && x.FECHAI == dt && x.FECHAF == dff).FirstOrDefault();
            deledit.ACTIVO = false;
            db.SaveChanges();
            return RedirectToAction("Details", new { id=id});

        }
    }
}

