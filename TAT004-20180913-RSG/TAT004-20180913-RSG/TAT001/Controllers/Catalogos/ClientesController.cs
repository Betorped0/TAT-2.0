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
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    public class ClientesController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Clientes
        public ActionResult Index()
        {
            int pagina = 631; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
            //ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
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
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            var cLIENTEs = db.CLIENTEs.Include(c => c.PAI).Include(c => c.TCLIENTE);
            return View(cLIENTEs.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(string vko, string vtw, string spa, string kun)
        {
            int pagina = 632; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
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
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            if (vko == null | vtw == null | spa == null | kun == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CLIENTE cLIENTE = db.CLIENTEs.Find(vko, vtw, spa, kun);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.LAND = new SelectList(db.PAIS, "LAND", "SPRAS");
            ViewBag.PARVW = new SelectList(db.TCLIENTEs, "ID", "ID");
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VKORG,VTWEG,SPART,KUNNR,NAME1,STCD1,STCD2,LAND,REGION,SUBREGION,REGIO,ORT01,STRAS_GP,PSTLZ,CONTAC,CONT_EMAIL,PARVW,PAYER,GRUPO,SPRAS,ACTIVO,BDESCRIPCION,BANNER,CANAL,BZIRK,KONDA,VKGRP,VKBUR,BANNERG")] CLIENTE cLIENTE)
        {
            if (ModelState.IsValid)
            {
                db.CLIENTEs.Add(cLIENTE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LAND = new SelectList(db.PAIS, "LAND", "SPRAS", cLIENTE.LAND);
            ViewBag.PARVW = new SelectList(db.TCLIENTEs, "ID", "ID", cLIENTE.PARVW);
            return View(cLIENTE);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(string vko, string vtw, string spa, string kun)
        {
            int pagina = 632; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
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
                //return RedirectToAction("Pais", "Home");
            }
            Session["spras"] = user.SPRAS_ID;

            if (vko == null | vtw == null | spa == null | kun == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CLIENTE cLIENTE = db.CLIENTEs.Find(vko, vtw, spa, kun);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            ViewBag.LAND = new SelectList(db.PAIS, "LAND", "LANDX", cLIENTE.LAND);
            ViewBag.PARVW = new SelectList(db.TCLIENTEs, "ID", "ID", cLIENTE.PARVW);
            return View(cLIENTE);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VKORG,VTWEG,SPART,KUNNR,NAME1,STCD1,STCD2,LAND,REGION,SUBREGION,REGIO,ORT01,STRAS_GP,PSTLZ,CONTAC,CONT_EMAIL,PARVW,PAYER,GRUPO,SPRAS,ACTIVO,BDESCRIPCION,BANNER,CANAL,BZIRK,KONDA,VKGRP,VKBUR,BANNERG")] CLIENTE cLIENTE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cLIENTE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LAND = new SelectList(db.PAIS, "LAND", "SPRAS", cLIENTE.LAND);
            ViewBag.PARVW = new SelectList(db.TCLIENTEs, "ID", "ID", cLIENTE.PARVW);
            return View(cLIENTE);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CLIENTE cLIENTE = db.CLIENTEs.Find(id);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            return View(cLIENTE);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CLIENTE cLIENTE = db.CLIENTEs.Find(id);
            db.CLIENTEs.Remove(cLIENTE);
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
                ld = ObjAList1(dt);

                reader.Close();
            }

            List<Clientes> cc = new List<Clientes>();
            List<USUARIO> usuarios = new List<USUARIO>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            List<SOCIEDAD> sociedades = new List<SOCIEDAD>();
            List<PAI> paises = new List<PAI>();
            List<PROVEEDOR> proveedores = new List<PROVEEDOR>();
            List<CANAL> canales = new List<CANAL>();
            List<CONTACTOC> contactos = new List<CONTACTOC>();

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                Clientes cl = new Clientes();
                Cryptography c = new Cryptography();
                string[] ids = new string[8];
                bool[] idsx = new bool[8];

                cl.BUKRS = da.BUKRS;
                cl.BUKRSX = true;
                cl.LAND = da.LAND;
                cl.LANDX = true;
                cl.KUNNR = da.KUNNR;
                cl.KUNNRX = true;
                cl.CLIENTE_N = da.CLIENTE_N;
                ids[0] = da.ID_US0;
                idsx[0] = true;
                ids[1] = da.ID_US1;
                idsx[1] = true;
                ids[2] = da.ID_US2;
                idsx[2] = true;
                ids[3] = da.ID_US3;
                idsx[3] = true;
                ids[4] = da.ID_US4;
                idsx[4] = true;
                ids[5] = da.ID_US5;
                idsx[5] = true;
                ids[6] = da.ID_US6;
                idsx[6] = true;
                ids[7] = da.ID_US7;
                idsx[7] = true;
                cl.ID_PROVEEDOR = da.ID_PROVEEDOR;
                cl.BANNER = da.BANNER;
                cl.BANNERG = da.BANNERG;
                cl.CANAL = da.CANAL;
                cl.EXPORTACION = da.EXPORTACION;
                cl.CONTACTO = da.CONTACTO;
                cl.CONTACTOE = da.CONTACTOE;
                cl.CONTACTOEX = true;

                ////-------------------------------CoCode
                SOCIEDAD s = db.SOCIEDADs.Where(x => x.BUKRS.Equals(cl.BUKRS) & x.ACTIVO == true).FirstOrDefault();
                if (s == null)
                    cl.BUKRSX = false;
                else
                    sociedades.Add(s);
                if (!cl.BUKRSX)
                {
                    cl.BUKRS = cl.BUKRS + "?";
                    messa = cont + ". Error con el CoCode<br/>";
                    cont++;
                }

                ////-------------------------------Pais
                PAI p = db.PAIS.Where(x => x.LAND.Equals(cl.LAND)).FirstOrDefault();
                if (p == null)
                    cl.LANDX = false;
                else
                    paises.Add(p);
                if (!cl.LANDX)
                {
                    cl.LAND = cl.LAND + "?";
                    messa = cont + ". Error con el Pais<br/>";
                    cont++;
                }

                ////-------------------------------CLIENTE
                CLIENTE k = db.CLIENTEs.Where(x => x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true).FirstOrDefault();
                if (k == null)
                    cl.KUNNRX = false;
                else
                    clientes.Add(k);
                if (!cl.KUNNRX)
                {
                    cl.KUNNR = cl.KUNNR + "?";
                    messa = cont + ". Error con el Cliente<br/>";
                    cont++;
                }

                ////-------------------------------Niveles
                for (int i = 0; i < 8; i++)
                {
                    if (ids[i] != null && ids[i] != "")
                    {
                        var usuario = ids[i];
                        USUARIO u = db.USUARIOs.Where(x => x.ID.Equals(usuario) & x.ACTIVO == true).FirstOrDefault();
                        if (u != null)
                            idsx[i] = false;
                        else
                            usuarios.Add(u);
                        if ((ids[i] == "" && ids[i] == null) && (i == 1 || i == 7))
                            idsx[i] = false;
                    }
                    if (!idsx[i])
                    {
                        ids[i] = ids[i] + "?";
                        messa = messa + cont + ". Error en el nivel " + i;
                        cont++;
                    }
                }
                cl.ID_US0 = ids[0];
                cl.ID_US0X = idsx[0];
                cl.ID_US1 = ids[1];
                cl.ID_US1X = idsx[1];
                cl.ID_US2 = ids[2];
                cl.ID_US2X = idsx[2];
                cl.ID_US3 = ids[3];
                cl.ID_US3X = idsx[3];
                cl.ID_US4 = ids[4];
                cl.ID_US4X = idsx[4];
                cl.ID_US5 = ids[5];
                cl.ID_US5X = idsx[5];
                cl.ID_US6 = ids[6];
                cl.ID_US6X = idsx[6];
                cl.ID_US7 = ids[7];
                cl.ID_US7X = idsx[7];

                ////-------------------------------EMAIL
                if (ComprobarEmail(cl.CONTACTOE) == false)
                {
                    cl.CONTACTOEX = false;
                }
                if (!cl.CONTACTOEX)
                {
                    cl.CONTACTOE = cl.CONTACTOE + "?";
                    messa = messa + cont + ". Error en el correo<br/>";
                    cont++;
                }

                da.MESS = messa;
                cl.MESS = da.MESS;

                cc.Add(cl);
            }
            JsonResult jl = Json(cc, JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult Agregar()
        {
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();

            var coc = Request["coc"].Split(',');
            var pai = Request["pai"].Split(',');
            var cli = Request["cli"].Split(',');
            var noc = Request["noc"].Split(',');
            var ni0 = Request["ni0"].Split(',');
            var ni1 = Request["ni1"].Split(',');
            var ni2 = Request["ni2"].Split(',');
            var ni3 = Request["ni3"].Split(',');
            var ni4 = Request["ni4"].Split(',');
            var ni5 = Request["ni5"].Split(',');
            var ni6 = Request["ni6"].Split(',');
            var ni7 = Request["ni7"].Split(',');
            var ven = Request["ven"].Split(',');
            var ban = Request["ban"].Split(',');
            var baa = Request["baa"].Split(',');
            var can = Request["can"].Split(',');
            var exp = Request["exp"].Split(',');
            var con = Request["con"].Split(',');
            var eco = Request["eco"].Split(',');
            var mes = Request["mes"].Split(',');

            var rows = coc.Length;
            string[,] tablas = new string[rows, 20];

            for (int i = 0; i < rows; i++)
            {
                tablas[i, 0] = coc[i];
                tablas[i, 1] = pai[i];
                tablas[i, 2] = cli[i];
                tablas[i, 3] = noc[i];
                tablas[i, 4] = ni0[i];
                tablas[i, 5] = ni1[i];
                tablas[i, 6] = ni2[i];
                tablas[i, 7] = ni3[i];
                tablas[i, 8] = ni4[i];
                tablas[i, 9] = ni5[i];
                tablas[i, 10] = ni6[i];
                tablas[i, 11] = ni7[i];
                tablas[i, 12] = ven[i];
                tablas[i, 13] = ban[i];
                tablas[i, 14] = baa[i];
                tablas[i, 15] = can[i];
                tablas[i, 16] = exp[i];
                tablas[i, 17] = con[i];
                tablas[i, 18] = eco[i];
                tablas[i, 19] = mes[i];
            }

            ld = ObjAList2(tablas, rows);
            int cont = 0;

            foreach (DET_AGENTE1 da in ld)
            {
                CLIENTEF cl = new CLIENTEF();
                CONTACTOC co = new CONTACTOC();

                if (da.MESS == null || da.MESS == "")
                {
                    cl.VKORG = da.VKORG;
                    cl.VTWEG = da.VTWEG;
                    cl.SPART = da.SPART;
                    cl.KUNNR = da.KUNNR;

                    CLIENTEF s = db.CLIENTEFs.Where(x => x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true).FirstOrDefault();
                    if (s == null)
                    {
                        cl.VERSION = 1;
                        cl.FECHAC = DateTime.Today;
                        cl.FECHAM = null;
                        cl.ACTIVO = true;
                    }
                    else
                    {
                        cl.VERSION = int.Parse((from x in db.CLIENTEFs where x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true select x.VERSION).ToString())+1;
                        cl.FECHAC = null;
                        cl.FECHAM = DateTime.Today;
                        s.ACTIVO = false;
                    }
                    cl.USUARIO0_ID = da.ID_US0;
                    cl.USUARIO1_ID = da.ID_US1;
                    cl.USUARIO2_ID = da.ID_US2;
                    cl.USUARIO3_ID = da.ID_US3;
                    cl.USUARIO4_ID = da.ID_US4;
                    cl.USUARIO5_ID = da.ID_US5;
                    cl.USUARIO6_ID = da.ID_US6;
                    cl.USUARIO7_ID = da.ID_US7;
                    co.NOMBRE = da.CONTACTO;
                    co.EMAIL = da.CONTACTOE;
                    co.VKORG = da.VKORG;
                    co.VTWEG = da.VTWEG;
                    co.SPART = da.SPART;
                    co.KUNNR = da.KUNNR;

                    db.CLIENTEFs.Add(cl);
                    db.CONTACTOCs.Add(co);
                    db.SaveChanges();
                    cont++;
                }
            }

            JsonResult jl = Json(cont, JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult Comprobar()
        {
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();

            var coc = Request["coc"].Split(',');
            var pai = Request["pai"].Split(',');
            var cli = Request["cli"].Split(',');
            var noc = Request["noc"].Split(',');
            var ni0 = Request["ni0"].Split(',');
            var ni1 = Request["ni1"].Split(',');
            var ni2 = Request["ni2"].Split(',');
            var ni3 = Request["ni3"].Split(',');
            var ni4 = Request["ni4"].Split(',');
            var ni5 = Request["ni5"].Split(',');
            var ni6 = Request["ni6"].Split(',');
            var ni7 = Request["ni7"].Split(',');
            var ven = Request["ven"].Split(',');
            var ban = Request["ban"].Split(',');
            var baa = Request["baa"].Split(',');
            var can = Request["can"].Split(',');
            var exp = Request["exp"].Split(',');
            var con = Request["con"].Split(',');
            var eco = Request["eco"].Split(',');

            var rows = coc.Length;
            string[,] tablas = new string[rows, 19];

            for (int i = 0; i < rows; i++)
            {
                tablas[i, 0] = coc[i];
                tablas[i, 1] = pai[i];
                tablas[i, 2] = cli[i];
                tablas[i, 3] = noc[i];
                tablas[i, 4] = ni0[i];
                tablas[i, 5] = ni1[i];
                tablas[i, 6] = ni2[i];
                tablas[i, 7] = ni3[i];
                tablas[i, 8] = ni4[i];
                tablas[i, 9] = ni5[i];
                tablas[i, 10] = ni6[i];
                tablas[i, 11] = ni7[i];
                tablas[i, 12] = ven[i];
                tablas[i, 13] = ban[i];
                tablas[i, 14] = baa[i];
                tablas[i, 15] = can[i];
                tablas[i, 16] = exp[i];
                tablas[i, 17] = con[i];
                tablas[i, 18] = eco[i];
            }

            ld = ObjAList2(tablas, rows);

            List<Clientes> cc = new List<Clientes>();
            List<USUARIO> usuarios = new List<USUARIO>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            List<SOCIEDAD> sociedades = new List<SOCIEDAD>();
            List<PAI> paises = new List<PAI>();
            List<PROVEEDOR> proveedores = new List<PROVEEDOR>();
            List<CANAL> canales = new List<CANAL>();
            List<CONTACTOC> contactos = new List<CONTACTOC>();

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                Clientes cl = new Clientes();
                Cryptography c = new Cryptography();
                string[] ids = new string[8];
                bool[] idsx = new bool[8];

                cl.BUKRS = da.BUKRS;
                cl.BUKRSX = true;
                cl.LAND = da.LAND;
                cl.LANDX = true;
                cl.KUNNR = da.KUNNR;
                cl.KUNNRX = true;
                cl.CLIENTE_N = da.CLIENTE_N;
                ids[0] = da.ID_US0;
                idsx[0] = true;
                ids[1] = da.ID_US1;
                idsx[1] = true;
                ids[2] = da.ID_US2;
                idsx[2] = true;
                ids[3] = da.ID_US3;
                idsx[3] = true;
                ids[4] = da.ID_US4;
                idsx[4] = true;
                ids[5] = da.ID_US5;
                idsx[5] = true;
                ids[6] = da.ID_US6;
                idsx[6] = true;
                ids[7] = da.ID_US7;
                idsx[7] = true;
                cl.ID_PROVEEDOR = da.ID_PROVEEDOR;
                cl.BANNER = da.BANNER;
                cl.BANNERG = da.BANNERG;
                cl.CANAL = da.CANAL;
                cl.EXPORTACION = da.EXPORTACION;
                cl.CONTACTO = da.CONTACTO;
                cl.CONTACTOE = da.CONTACTOE;
                cl.CONTACTOEX = true;

                ////-------------------------------CoCode
                SOCIEDAD s = db.SOCIEDADs.Where(x => x.BUKRS.Equals(cl.BUKRS) & x.ACTIVO == true).FirstOrDefault();
                if (s == null)
                    cl.BUKRSX = false;
                else
                    sociedades.Add(s);
                if (!cl.BUKRSX)
                {
                    cl.BUKRS = cl.BUKRS + "?";
                    messa = cont + ". Error con el CoCode<br/>";
                    cont++;
                }

                ////-------------------------------Pais
                PAI p = db.PAIS.Where(x => x.LAND.Equals(cl.LAND)).FirstOrDefault();
                if (p == null)
                    cl.LANDX = false;
                else
                    paises.Add(p);
                if (!cl.LANDX)
                {
                    cl.LAND = cl.LAND + "?";
                    messa = cont + ". Error con el Pais<br/>";
                    cont++;
                }

                ////-------------------------------CLIENTE
                CLIENTE k = db.CLIENTEs.Where(x => x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true).FirstOrDefault();
                if (k == null)
                    cl.KUNNRX = false;
                else
                    clientes.Add(k);
                if (!cl.KUNNRX)
                {
                    cl.KUNNR = cl.KUNNR + "?";
                    messa = cont + ". Error con el Cliente<br/>";
                    cont++;
                }

                ////-------------------------------Niveles
                for (int i = 0; i < 8; i++)
                {
                    if (ids[i] != null && ids[i] != "")
                    {
                        var usuario = ids[i];
                        USUARIO u = db.USUARIOs.Where(x => x.ID.Equals(usuario) & x.ACTIVO == true).FirstOrDefault();
                        if (u != null)
                            idsx[i] = false;
                        else
                            usuarios.Add(u);
                        if ((ids[i] == "" && ids[i] == null) && (i == 1 || i == 7))
                            idsx[i] = false;
                    }
                    if (!idsx[i])
                    {
                        ids[i] = ids[i] + "?";
                        messa = messa + cont + ". Error en el nivel " + i;
                        cont++;
                    }
                }
                cl.ID_US0 = ids[0];
                cl.ID_US0X = idsx[0];
                cl.ID_US1 = ids[1];
                cl.ID_US1X = idsx[1];
                cl.ID_US2 = ids[2];
                cl.ID_US2X = idsx[2];
                cl.ID_US3 = ids[3];
                cl.ID_US3X = idsx[3];
                cl.ID_US4 = ids[4];
                cl.ID_US4X = idsx[4];
                cl.ID_US5 = ids[5];
                cl.ID_US5X = idsx[5];
                cl.ID_US6 = ids[6];
                cl.ID_US6X = idsx[6];
                cl.ID_US7 = ids[7];
                cl.ID_US7X = idsx[7];

                ////-------------------------------EMAIL
                if (ComprobarEmail(cl.CONTACTOE) == false)
                {
                    cl.CONTACTOEX = false;
                }
                if (!cl.CONTACTOEX)
                {
                    cl.CONTACTOE = cl.CONTACTOE + "?";
                    messa = messa + cont + ". Error en el correo<br/>";
                    cont++;
                }

                da.MESS = messa;
                cl.MESS = da.MESS;

                cc.Add(cl);
            }
            JsonResult jl = Json(cc, JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult Borrar()
        {
            List<DET_AGENTE1> ld = new List<DET_AGENTE1>();

            var bor = Request["bor"].Split(',');
            var coc = Request["coc"].Split(',');
            var pai = Request["pai"].Split(',');
            var cli = Request["cli"].Split(',');
            var noc = Request["noc"].Split(',');
            var ni0 = Request["ni0"].Split(',');
            var ni1 = Request["ni1"].Split(',');
            var ni2 = Request["ni2"].Split(',');
            var ni3 = Request["ni3"].Split(',');
            var ni4 = Request["ni4"].Split(',');
            var ni5 = Request["ni5"].Split(',');
            var ni6 = Request["ni6"].Split(',');
            var ni7 = Request["ni7"].Split(',');
            var ven = Request["ven"].Split(',');
            var ban = Request["ban"].Split(',');
            var baa = Request["baa"].Split(',');
            var can = Request["can"].Split(',');
            var exp = Request["exp"].Split(',');
            var con = Request["con"].Split(',');
            var eco = Request["eco"].Split(',');

            var rows = coc.Length - bor.Length;
            string[,] tablas = new string[rows, 19];

            for (int i = 0; i < coc.Length; i++)
            {
                if (i != int.Parse(bor[i]))
                {
                    tablas[i, 0] = coc[i];
                    tablas[i, 1] = pai[i];
                    tablas[i, 2] = cli[i];
                    tablas[i, 3] = noc[i];
                    tablas[i, 4] = ni0[i];
                    tablas[i, 5] = ni1[i];
                    tablas[i, 6] = ni2[i];
                    tablas[i, 7] = ni3[i];
                    tablas[i, 8] = ni4[i];
                    tablas[i, 9] = ni5[i];
                    tablas[i, 10] = ni6[i];
                    tablas[i, 11] = ni7[i];
                    tablas[i, 12] = ven[i];
                    tablas[i, 13] = ban[i];
                    tablas[i, 14] = baa[i];
                    tablas[i, 15] = can[i];
                    tablas[i, 16] = exp[i];
                    tablas[i, 17] = con[i];
                    tablas[i, 18] = eco[i];
                }
            }

            ld = ObjAList2(tablas, rows);

            List<Clientes> cc = new List<Clientes>();
            List<USUARIO> usuarios = new List<USUARIO>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            List<SOCIEDAD> sociedades = new List<SOCIEDAD>();
            List<PAI> paises = new List<PAI>();
            List<PROVEEDOR> proveedores = new List<PROVEEDOR>();
            List<CANAL> canales = new List<CANAL>();
            List<CONTACTOC> contactos = new List<CONTACTOC>();

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                Clientes cl = new Clientes();
                Cryptography c = new Cryptography();
                string[] ids = new string[8];
                bool[] idsx = new bool[8];

                cl.BUKRS = da.BUKRS;
                cl.BUKRSX = true;
                cl.LAND = da.LAND;
                cl.LANDX = true;
                cl.KUNNR = da.KUNNR;
                cl.KUNNRX = true;
                cl.CLIENTE_N = da.CLIENTE_N;
                ids[0] = da.ID_US0;
                idsx[0] = true;
                ids[1] = da.ID_US1;
                idsx[1] = true;
                ids[2] = da.ID_US2;
                idsx[2] = true;
                ids[3] = da.ID_US3;
                idsx[3] = true;
                ids[4] = da.ID_US4;
                idsx[4] = true;
                ids[5] = da.ID_US5;
                idsx[5] = true;
                ids[6] = da.ID_US6;
                idsx[6] = true;
                ids[7] = da.ID_US7;
                idsx[7] = true;
                cl.ID_PROVEEDOR = da.ID_PROVEEDOR;
                cl.BANNER = da.BANNER;
                cl.BANNERG = da.BANNERG;
                cl.CANAL = da.CANAL;
                cl.EXPORTACION = da.EXPORTACION;
                cl.CONTACTO = da.CONTACTO;
                cl.CONTACTOE = da.CONTACTOE;
                cl.CONTACTOEX = true;

                ////-------------------------------CoCode
                SOCIEDAD s = db.SOCIEDADs.Where(x => x.BUKRS.Equals(cl.BUKRS) & x.ACTIVO == true).FirstOrDefault();
                if (s == null)
                    cl.BUKRSX = false;
                else
                    sociedades.Add(s);
                if (!cl.BUKRSX)
                {
                    cl.BUKRS = cl.BUKRS + "?";
                    messa = cont + ". Error con el CoCode<br/>";
                    cont++;
                }

                ////-------------------------------Pais
                PAI p = db.PAIS.Where(x => x.LAND.Equals(cl.LAND)).FirstOrDefault();
                if (p == null)
                    cl.LANDX = false;
                else
                    paises.Add(p);
                if (!cl.LANDX)
                {
                    cl.LAND = cl.LAND + "?";
                    messa = cont + ". Error con el Pais<br/>";
                    cont++;
                }

                ////-------------------------------CLIENTE
                CLIENTE k = db.CLIENTEs.Where(x => x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true).FirstOrDefault();
                if (k == null)
                    cl.KUNNRX = false;
                else
                    clientes.Add(k);
                if (!cl.KUNNRX)
                {
                    cl.KUNNR = cl.KUNNR + "?";
                    messa = cont + ". Error con el Cliente<br/>";
                    cont++;
                }

                ////-------------------------------Niveles
                for (int i = 0; i < 8; i++)
                {
                    if (ids[i] != null && ids[i] != "")
                    {
                        var usuario = ids[i];
                        USUARIO u = db.USUARIOs.Where(x => x.ID.Equals(usuario) & x.ACTIVO == true).FirstOrDefault();
                        if (u != null)
                            idsx[i] = false;
                        else
                            usuarios.Add(u);
                        if ((ids[i] == "" && ids[i] == null) && (i == 1 || i == 7))
                            idsx[i] = false;
                    }
                    if (!idsx[i])
                    {
                        ids[i] = ids[i] + "?";
                        messa = messa + cont + ". Error en el nivel " + i;
                        cont++;
                    }
                }
                cl.ID_US0 = ids[0];
                cl.ID_US0X = idsx[0];
                cl.ID_US1 = ids[1];
                cl.ID_US1X = idsx[1];
                cl.ID_US2 = ids[2];
                cl.ID_US2X = idsx[2];
                cl.ID_US3 = ids[3];
                cl.ID_US3X = idsx[3];
                cl.ID_US4 = ids[4];
                cl.ID_US4X = idsx[4];
                cl.ID_US5 = ids[5];
                cl.ID_US5X = idsx[5];
                cl.ID_US6 = ids[6];
                cl.ID_US6X = idsx[6];
                cl.ID_US7 = ids[7];
                cl.ID_US7X = idsx[7];

                ////-------------------------------EMAIL
                if (ComprobarEmail(cl.CONTACTOE) == false)
                {
                    cl.CONTACTOEX = false;
                }
                if (!cl.CONTACTOEX)
                {
                    cl.CONTACTOE = cl.CONTACTOE + "?";
                    messa = messa + cont + ". Error en el correo<br/>";
                    cont++;
                }

                da.MESS = messa;
                cl.MESS = da.MESS;

                cc.Add(cl);
            }
            JsonResult jl = Json(cc, JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult Actualizar()
        {
            List<Clientes> cc = new List<Clientes>();
            Clientes cl = new Clientes();
            CLIENTEF cf = new CLIENTEF();

            var cli = Request["cli"];
            var ni0 = Request["ni0"];
            cl.KUNNRX = true;

            if (cli != null)
            {
                cl.KUNNR = Completa(cli, 10);
                CLIENTEF k = db.CLIENTEFs.Where(x => x.KUNNR.Equals(cl.KUNNR)).FirstOrDefault();
                if (k == null)
                    cl.KUNNRX = false;
                else
                {
                    cl.BUKRS = (from x in db.SOCIEDADs join j in db.CLIENTEs on x.REGION equals j.REGION where j.KUNNR.Equals(cl.KUNNR) select x.BUKRS).ToString();

                    //cl.VERSION = int.Parse((from x in db.CLIENTEFs where x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true select x.VERSION).ToString()) + 1
                }
                if (!cl.KUNNRX)
                {
                    cl.KUNNR = cl.KUNNR + "?";
                    cl.MESS = "El cliente no existe";
                }
            }

            else if (ni0 != null)
            {

            }

            JsonResult jl = Json(cc, JsonRequestBehavior.AllowGet);
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

        private string Completa(string s, int longitud)
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

        private List<DET_AGENTE1> ObjAList1(DataTable dt)
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
                    doc.BUKRS = dt.Rows[i][0].ToString().ToUpper();
                }
                catch (Exception e)
                {
                    doc.BUKRS = null;
                } //CoCode
                try
                {
                    doc.LAND = dt.Rows[i][1].ToString().ToUpper();
                }
                catch (Exception e)
                {
                    doc.LAND = null;
                } //Pais
                try
                {
                    doc.KUNNR = dt.Rows[i][2].ToString();
                    doc.KUNNR = Completa(doc.KUNNR, 10);

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
                } //Cliente
                try
                {
                    doc.CLIENTE_N = dt.Rows[i][3].ToString();
                }
                catch (Exception e)
                {
                    doc.CLIENTE_N = null;
                } //Nombre Cliente
                try
                {
                    doc.ID_US0 = dt.Rows[i][4].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US0 = null;
                } //Nivel 0
                try
                {
                    doc.ID_US1 = dt.Rows[i][5].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US1 = null;
                } //Nivel 1
                try
                {
                    doc.ID_US2 = dt.Rows[i][6].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US2 = null;
                } //Nivel 2
                try
                {
                    doc.ID_US3 = dt.Rows[i][7].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US3 = null;
                } //Nivel 3
                try
                {
                    doc.ID_US4 = dt.Rows[i][8].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US4 = null;
                } //Nivel 4
                try
                {
                    doc.ID_US5 = dt.Rows[i][9].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US5 = null;
                } //Nivel 5
                try
                {
                    doc.ID_US6 = dt.Rows[i][10].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US6 = null;
                } //Nivel 6
                try
                {
                    doc.ID_US7 = dt.Rows[i][11].ToString();
                }
                catch (Exception e)
                {
                    doc.ID_US7 = null;
                } //Nivel 7
                try
                {
                    doc.ID_PROVEEDOR = dt.Rows[i][12].ToString();
                    doc.ID_PROVEEDOR = Completa(doc.ID_PROVEEDOR, 10);
                }
                catch (Exception e)
                {
                    doc.ID_PROVEEDOR = null;
                } //Vendor
                try
                {
                    doc.BANNER = dt.Rows[i][13].ToString();
                }
                catch (Exception e)
                {
                    doc.BANNER = null;
                } //Banner
                try
                {
                    doc.BANNERG = dt.Rows[i][14].ToString();
                }
                catch (Exception e)
                {
                    doc.BANNERG = null;
                } //Banner Agrupador
                try
                {
                    doc.CANAL = dt.Rows[i][15].ToString();
                }
                catch (Exception e)
                {
                    doc.CANAL = null;
                } //Canal
                try
                {
                    doc.EXPORTACION = dt.Rows[i][16].ToString();
                }
                catch (Exception e)
                {
                    doc.EXPORTACION = null;
                } //Canal
                try
                {
                    doc.CONTACTO = dt.Rows[i][17].ToString();
                }
                catch (Exception e)
                {
                    doc.CONTACTO = null;
                } //Contacto
                try
                {
                    doc.CONTACTOE = dt.Rows[i][18].ToString();
                }
                catch (Exception e)
                {
                    doc.CONTACTOE = null;
                } //Email de contacto

                ld.Add(doc);
                pos++;
            }
            return ld;
        }

        private List<DET_AGENTE1> ObjAList2(string[,] dt, int rowsc)
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

                try
                {
                    doc.BUKRS = dt[i, 0].ToUpper();
                }
                catch (Exception e)
                {
                    doc.BUKRS = null;
                } //CoCode
                try
                {
                    doc.LAND = dt[i, 1].ToUpper();
                }
                catch (Exception e)
                {
                    doc.LAND = null;
                } //Pais
                try
                {
                    doc.KUNNR = dt[i, 2];
                    doc.KUNNR = Completa(doc.KUNNR, 10);

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
                } //Cliente
                try
                {
                    doc.CLIENTE_N = dt[i, 3];
                }
                catch (Exception e)
                {
                    doc.CLIENTE_N = null;
                } //Nombre Cliente
                try
                {
                    doc.ID_US0 = dt[i, 4];
                }
                catch (Exception e)
                {
                    doc.ID_US0 = null;
                } //Nivel 0
                try
                {
                    doc.ID_US1 = dt[i, 5];
                }
                catch (Exception e)
                {
                    doc.ID_US1 = null;
                } //Nivel 1
                try
                {
                    doc.ID_US2 = dt[i, 6];
                }
                catch (Exception e)
                {
                    doc.ID_US2 = null;
                } //Nivel 2
                try
                {
                    doc.ID_US3 = dt[i, 7];
                }
                catch (Exception e)
                {
                    doc.ID_US3 = null;
                } //Nivel 3
                try
                {
                    doc.ID_US4 = dt[i, 8];
                }
                catch (Exception e)
                {
                    doc.ID_US4 = null;
                } //Nivel 4
                try
                {
                    doc.ID_US5 = dt[i, 9];
                }
                catch (Exception e)
                {
                    doc.ID_US5 = null;
                } //Nivel 5
                try
                {
                    doc.ID_US6 = dt[i, 10];
                }
                catch (Exception e)
                {
                    doc.ID_US6 = null;
                } //Nivel 6
                try
                {
                    doc.ID_US7 = dt[i, 11];
                }
                catch (Exception e)
                {
                    doc.ID_US7 = null;
                } //Nivel 7
                try
                {
                    doc.ID_PROVEEDOR = dt[i, 12];
                    doc.ID_PROVEEDOR = Completa(doc.ID_PROVEEDOR, 10);
                }
                catch (Exception e)
                {
                    doc.ID_PROVEEDOR = null;
                } //Vendor
                try
                {
                    doc.BANNER = dt[i, 13];
                    doc.BANNER = Completa(doc.BANNER, 10);
                }
                catch (Exception e)
                {
                    doc.BANNER = null;
                } //Banner
                try
                {
                    doc.BANNERG = dt[i, 14];
                    doc.BANNERG = Completa(doc.BANNERG, 10);
                }
                catch (Exception e)
                {
                    doc.BANNERG = null;
                } //Banner Agrupador
                try
                {
                    doc.CANAL = dt[i, 15];
                }
                catch (Exception e)
                {
                    doc.CANAL = null;
                } //Canal
                try
                {
                    doc.EXPORTACION = dt[i, 16];
                }
                catch (Exception e)
                {
                    doc.EXPORTACION = null;
                } //Exportacion
                try
                {
                    doc.CONTACTO = dt[i, 17];
                }
                catch (Exception e)
                {
                    doc.CONTACTO = null;
                } //Contacto
                try
                {
                    doc.CONTACTOE = dt[i, 18];
                }
                catch (Exception e)
                {
                    doc.CONTACTOE = null;
                } //Email de contacto
                try
                {
                    doc.MESS = dt[i, 19];
                }
                catch (Exception e)
                {
                    doc.MESS = null;
                } //Mensaje

                ld.Add(doc);
                pos++;
            }
            return ld;
        }

        public partial class DET_AGENTE1 : IEquatable<DET_AGENTE1>
        {
            public string BUKRS { get; set; }
            public string LAND { get; set; }
            public string KUNNR { get; set; }
            public string VKORG { get; set; }
            public string VTWEG { get; set; }
            public string SPART { get; set; }
            public string CLIENTE_N { get; set; }
            public string ID_US0 { get; set; }
            public string ID_US1 { get; set; }
            public string ID_US2 { get; set; }
            public string ID_US3 { get; set; }
            public string ID_US4 { get; set; }
            public string ID_US5 { get; set; }
            public string ID_US6 { get; set; }
            public string ID_US7 { get; set; }
            public string ID_PROVEEDOR { get; set; }
            public string BANNER { get; set; }
            public string BANNERG { get; set; }
            public string CANAL { get; set; }
            public string EXPORTACION { get; set; }
            public string CONTACTO { get; set; }
            public string CONTACTOE { get; set; }
            public string MESS { get; set; }
            public int POS { get; set; }

            public virtual CLIENTE CLIENTE { get; set; }
            public virtual USUARIO USUARIO { get; set; }

            public bool Equals(DET_AGENTE1 other)
            {
                throw new NotImplementedException();
            }
        }

    }
}
