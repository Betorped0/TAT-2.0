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
                ld = objAList1(dt);

                reader.Close();
            }

            List<Usuarios> uu = new List<Usuarios>();
            List<USUARIO> usuarios = new List<USUARIO>();
            List<CLIENTE> clientes = new List<CLIENTE>();
            List<PUESTO> puesto = new List<PUESTO>();
            List<SOCIEDAD> sociedad = new List<SOCIEDAD>();
            int rowst = ld.Count();
            string[] IDs = new string[rowst];
            int cont2 = 0;
            string[,] tablas = new string[rowst, 11];
            string[,] client = new string[rowst, 2];
            string[] gua = new string[rowst];

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                bool vus = false;
                Usuarios us = new Usuarios();
                Cryptography c = new Cryptography();

                us.KUNNR = da.KUNNR;
                us.KUNNRX = true;
                us.BUNIT = da.BUNIT;
                us.BUNITX = true;
                us.PUESTO_ID = da.PUESTO_ID.ToString();
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

                //Comprobacion de la asignacion de varios clientes
                if (cont2 > 0)
                    if (us.KUNNR != gua[cont2 - 1] && us.BUNIT == "" && us.PUESTO_ID == "" && us.ID == "" && us.NOMBRE == "" && us.APELLIDO_P == "" && us.APELLIDO_M == "" && us.EMAIL == "" && us.SPRAS_ID == "" && us.PASS == "")
                    {
                        vus = true;
                    }
                //Usuario nuevo
                if (vus == false)
                {
                    ////-------------------------------CLIENTE
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
                            tablas[cont2, 0] = da.KUNNR.ToString();
                            gua[cont2] = da.KUNNR.ToString();
                        }
                    }
                    if (!us.KUNNRX)
                    {
                        us.KUNNR = us.KUNNR + "?";
                        messa = cont + ". Error en el cliente<br/>";
                        cont++;
                    }

                    ////-------------------------------COMPANY CODE
                    SOCIEDAD b = sociedad.Where(x => x.BUKRS.Equals(us.BUNIT)).FirstOrDefault();
                    if (b == null)
                    {
                        b = db.SOCIEDADs.Where(x => x.BUKRS.Equals(us.BUNIT) & x.ACTIVO == true).FirstOrDefault();
                        if (b == null)
                            us.BUNITX = false;
                        else
                        {
                            sociedad.Add(b);
                            tablas[cont2, 1] = da.BUNIT.ToString();
                        }
                    }
                    if (!us.BUNITX)
                    {
                        us.BUNIT = us.BUNIT + "?";
                        messa = messa + cont + ". Error en la sociedad<br/>";
                        cont++;
                    }
                    int pues = 0;

                    ////-------------------------------NIVEL
                    if (us.PUESTO_ID != null && us.PUESTO_ID != "")
                        pues = int.Parse(us.PUESTO_ID);

                    PUESTO pi = puesto.Where(x => x.ID == pues & x.ACTIVO == true).FirstOrDefault();
                    if (pi == null)
                    {
                        pi = db.PUESTOes.Where(x => x.ID == pues & x.ACTIVO == true).FirstOrDefault();
                        if (pi == null)
                            us.PUESTO_IDX = false;
                        else
                        {
                            puesto.Add(pi);
                            tablas[cont2, 2] = da.PUESTO_ID.ToString();
                        }
                    }
                    if (!us.PUESTO_IDX)
                    {
                        us.PUESTO_ID = us.PUESTO_ID + "?";
                        messa = messa + cont + ". Error en el nivel<br/>";
                        cont++;
                    }

                    ////-------------------------------USUARIO ID
                    var err = ". Error en el ID de usuario<br/>";
                    if (us.ID == null || us.ID == "")
                        us.IDX = false;
                    else if (IDs.Contains(us.ID))
                        us.IDX = false;
                    else
                    {
                        USUARIO u = db.USUARIOs.Where(xu => xu.ID.Equals(us.ID)).FirstOrDefault();
                        if (u != null)
                        {
                            us.IDX = false;
                            err = ". Usuario duplicado<br/>";
                            client[cont2, 1] = us.ID.ToString();
                            tablas[cont2, 3] = da.ID.ToString();
                            tablas[cont2, 4] = da.NOMBRE.ToString();
                            tablas[cont2, 5] = da.APELLIDO_P.ToString();
                            tablas[cont2, 6] = da.APELLIDO_M.ToString();
                        }
                        else
                        {
                            usuarios.Add(u);
                            IDs[cont2] = us.ID;
                            client[cont2, 1] = us.ID.ToString();
                            tablas[cont2, 3] = da.ID.ToString();
                            tablas[cont2, 4] = da.NOMBRE.ToString();
                            tablas[cont2, 5] = da.APELLIDO_P.ToString();
                            tablas[cont2, 6] = da.APELLIDO_M.ToString();
                        }
                    }

                    if (!us.IDX)
                    {
                        us.ID = us.ID + "?";
                        messa = messa + cont + err;
                        cont++;
                    }

                    ////-------------------------------EMAIL
                    if (ComprobarEmail(us.EMAIL) == false)
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
                    if (us.SPRAS_ID == "")
                    {
                        us.SPRAS_ID = "ES";
                        da.SPRAS_ID = us.SPRAS_ID;
                    }
                    SPRA si = db.SPRAS.Where(x => x.ID.Equals(us.SPRAS_ID) == true).FirstOrDefault();
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
                    da.ID = IDs[cont2 - 1];
                    client[cont2, 1] = da.ID;
                    us.mess = da.mess;
                    tablas[cont2, 10] = messa;
                }

                cont2++;

                uu.Add(us);
            }
            Session["tablas"] = tablas;
            Session["client"] = client;
            Session["rowst"] = rowst;
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

    }
}
