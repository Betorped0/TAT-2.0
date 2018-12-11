﻿using ClosedXML.Excel;
using ExcelDataReader;
using PagedList;
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
using TAT001.Common;
using TAT001.Entities;
using TAT001.Filters;
using TAT001.Models;
using TAT001.Services;

namespace TAT001.Controllers.Catalogos
{
    [Authorize]
    [LoginActive]
    public class ClientesController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();
        readonly UsuarioLogin usuValidateLogin = new UsuarioLogin();

        // GET: Clientes       
        public ActionResult Index()
        {
            int pagina_id = 631; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            ViewBag.pais = Session["pais"]!=null? Session["pais"].ToString() + ".png" :null;


            ClienteViewModel viewModel = new ClienteViewModel
            {
                pageSizes = FnCommon.ObtenerCmbPageSize()
            };
            ObtenerListado(ref viewModel);

            return View(viewModel);
        }

        public ActionResult List(string colOrden, string ordenActual, int? numRegistros = 10, int? pagina = 1, string buscar = "")
        {
            if (!usuValidateLogin.validaUsuario(User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }
            int pagina_id = 631; //ID EN BASE DE DATOS
            ClienteViewModel viewModel = new ClienteViewModel();
            ObtenerListado(ref viewModel,colOrden,ordenActual,numRegistros,pagina,buscar);
            FnCommon.ObtenerTextos(db,pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            return View(viewModel);
        }
        void ObtenerListado(ref ClienteViewModel viewModel, string colOrden="", string ordenActual="", int? numRegistros = 10, int? pagina = 1, string buscar = "")
        {
            int pageIndex = pagina.Value;
            List<CLIENTE> clientes = db.CLIENTEs.Include(c => c.PAI).Include(c => c.TCLIENTE).ToList();
            viewModel.ordenActual = colOrden;
            viewModel.numRegistros = numRegistros.Value;
            viewModel.buscar = buscar;

            if (!String.IsNullOrEmpty(buscar))
            {
                clientes = clientes.Where(x =>
                String.Concat(x.KUNNR, x.NAME1, (x.SUBREGION ?? ""), x.LAND, x.PAI.LANDX, x.PARVW, x.PAYER, (x.CANAL == null ? "" : x.CANAL))
                .ToLower().Contains(buscar.ToLower()))
                .ToList();
            }
            switch (colOrden)
            {
                case "KUNNR":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.KUNNR).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.KUNNR).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
                case "NAME1":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.NAME1).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.NAME1).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "SUBREGION":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.SUBREGION).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.SUBREGION).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "LAND":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.LAND).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.LAND).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "PARVW":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.PARVW).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.PARVW).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "PAYER":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.PAYER).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.PAYER).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "CANAL":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.CANAL).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.CANAL).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                case "ACTIVO":
                    if (colOrden.Equals(ordenActual))
                        viewModel.clientes = clientes.OrderByDescending(m => m.ACTIVO).ToPagedList(pageIndex, viewModel.numRegistros);
                    else
                        viewModel.clientes = clientes.OrderBy(m => m.ACTIVO).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;

                default:
                    viewModel.clientes = clientes.OrderBy(m => m.KUNNR).ToPagedList(pageIndex, viewModel.numRegistros);
                    break;
            }
        }

        public ActionResult VerFlujo(string vko, string vtw, string spa, string kun, int version)
        {
            if (!usuValidateLogin.validaUsuario(User.Identity.Name))
            {
                FormsAuthentication.SignOut();
                return Json(new
                {
                    redirectUrl = Url.Action("Index", "Home"),
                    isRedirect = true
                });
            }
            int pagina_id = 604; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            var flujo = db.CLIENTEFs.Find(vko, vtw, spa, kun, version);
            if (flujo.USUARIO1_ID != null)
            {
                var usu1 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO1_ID).SingleOrDefault();
                ViewBag.Usuario1 = usu1 != null ? flujo.USUARIO1_ID + " - " + usu1.NOMBRE + " " + usu1.APELLIDO_P : "";
            }
            if (flujo.USUARIO2_ID != null)
            {
                var usu2 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO2_ID).SingleOrDefault();
                ViewBag.Usuario2 = usu2 != null ? flujo.USUARIO2_ID + " - " + usu2.NOMBRE + " " + usu2.APELLIDO_P : "";
            }
            if (flujo.USUARIO3_ID != null)
            {
                var usu3 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO3_ID).SingleOrDefault();
                ViewBag.Usuario3 = usu3 != null ? flujo.USUARIO3_ID + " - " + usu3.NOMBRE + " " + usu3.APELLIDO_P : "";
            }
            if (flujo.USUARIO4_ID != null)
            {
                var usu4 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO4_ID).SingleOrDefault();
                ViewBag.Usuario4 = usu4 != null ? flujo.USUARIO4_ID + " - " + usu4.NOMBRE + " " + usu4.APELLIDO_P : "";
            }
            if (flujo.USUARIO5_ID != null)
            {
                var usu5 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO5_ID).SingleOrDefault();
                ViewBag.Usuario5 = usu5 != null ? flujo.USUARIO5_ID + " - " + usu5.NOMBRE + " " + usu5.APELLIDO_P : "";
            }
            if (flujo.USUARIO6_ID != null)
            {
                var usu6 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO6_ID).SingleOrDefault();
                ViewBag.Usuario6 = usu6 != null ? flujo.USUARIO6_ID+" - "+ usu6.NOMBRE + " " + usu6.APELLIDO_P : "";
            }
            if (flujo.USUARIO7_ID != null)
            {
                var usu7 = db.USUARIOs.Where(t => t.ID == flujo.USUARIO7_ID).SingleOrDefault();
                ViewBag.Usuario7 = usu7 != null ? flujo.USUARIO7_ID + " - " + usu7.NOMBRE + " " + usu7.APELLIDO_P : "";
            }
            return View(flujo);
        }
        
        // GET: Clientes/Details/5
        public ActionResult Details(string vko, string vtw, string spa, string kun)
        {
            int pagina_id = 632; //ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }

            if (vko == null || vtw == null || spa == null || kun == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CLIENTE cLIENTE = db.CLIENTEs.Find(vko, vtw, spa, kun);
            if (cLIENTE == null)
            {
                return HttpNotFound();
            }
            if (!String.IsNullOrEmpty(cLIENTE.CANAL))
            {
                CANAL canaldsc = db.CANALs.Where(t => t.CANAL1 == cLIENTE.CANAL).SingleOrDefault();
                if (canaldsc != null)
                    ViewBag.CanalDsc = canaldsc.CANAL1 + "-" + canaldsc.CDESCRIPCION;
            }
            if (!String.IsNullOrEmpty(cLIENTE.PROVEEDOR_ID))
            {
                PROVEEDOR proveedordsc = db.PROVEEDORs.Where(p => p.ID == cLIENTE.PROVEEDOR_ID).SingleOrDefault();
                if (proveedordsc != null)
                    ViewBag.ProveedorDsc = proveedordsc.ID + "-" + proveedordsc.NOMBRE;
            }
            else
            {
                ViewBag.ProveedorDsc = "";
            }
            return View(cLIENTE);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(string vko, string vtw, string spa, string kun)
        {
            int pagina_id = 635; //ID EN BASE DE DATOS PARA EL TITULO
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);
            
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                //return RedirectToAction("Pais", "Home");
            }

            if (vko == null || vtw == null || spa == null || kun == null)
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

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VKORG,VTWEG,SPART,KUNNR,NAME1,STCD1,STCD2,LAND,REGION,SUBREGION,REGIO,ORT01,STRAS_GP,PSTLZ,CONTAC,CONT_EMAIL,PARVW,PAYER,GRUPO,SPRAS,ACTIVO,BDESCRIPCION,BANNER, PROVEEDOR_ID,CANAL,BZIRK,KONDA,VKGRP,VKBUR,BANNERG")] CLIENTE cLIENTE)
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
            int pagina = 631; //ID EN BASE DE DATOS PARA EL TITULO
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
                if (dt.Columns.Count!=19)
                {
                    return Json("NO VALIDO", JsonRequestBehavior.AllowGet); 
                }
                ld = ObjAList1(dt);

                reader.Close();
            }

            List<Clientes> cc = new List<Clientes>();
            List<Clientes> ccErr = new List<Clientes>();

            foreach (DET_AGENTE1 da in ld)
            {
                int cont = 1;
                string messa = "";
                Clientes cl = new Clientes();
                Cryptography c = new Cryptography();
                string[] ids = new string[8];
                bool[] idsx = new bool[8];

                cl.BUKRS = da.BUKRS.Replace(" ", "");
                cl.BUKRSX = true;
                cl.LAND = da.LAND.Replace(" ", "");
                cl.LANDX = true;
                cl.KUNNR = da.KUNNR.Replace(" ", "");
                cl.KUNNRX = true;
                cl.CLIENTE_N = da.CLIENTE_N;
                ids[0] = da.ID_US0.Replace(" ", "");
                idsx[0] = true;
                ids[1] = da.ID_US1.Replace(" ", "");
                idsx[1] = true;
                ids[2] = da.ID_US2.Replace(" ", "");
                idsx[2] = true;
                ids[3] = da.ID_US3.Replace(" ", "");
                idsx[3] = true;
                ids[4] = da.ID_US4.Replace(" ", "");
                idsx[4] = true;
                ids[5] = da.ID_US5.Replace(" ", "");
                idsx[5] = true;
                ids[6] = da.ID_US6.Replace(" ", "");
                idsx[6] = true;
                ids[7] = da.ID_US7.Replace(" ", "");
                idsx[7] = true;
                cl.ID_PROVEEDOR = da.ID_PROVEEDOR.Replace(" ", "");
                cl.ID_PROVEEDORX = true;
                cl.BANNER = da.BANNER.Replace(" ", "");
                cl.BANNERG = da.BANNERG.Replace(" ", "");
                cl.CANAL = da.CANAL.Replace(" ", "");
                cl.CANALX = true;
                cl.EXPORTACION = da.EXPORTACION.Replace(" ", "");
                cl.CONTACTO = da.CONTACTO;
                cl.CONTACTOE = da.CONTACTOE.Replace(" ", "");
                cl.CONTACTOEX = true;

                ////-------------------------------CoCode
                if (!db.PAIS.Any(x => x.SOCIEDAD_ID==cl.BUKRS & x.ACTIVO))
                {
                        cl.BUKRSX = false;
                    cl.BUKRS = cl.BUKRS + "?";
                    messa = cont + ". Error con el CoCode<br/>";
                    cont++;
                }

                ////-------------------------------Pais
                if (!db.PAIS.Any(x => x.LAND==cl.LAND & x.ACTIVO))
                {
                    cl.LANDX = false;
                    cl.LAND = cl.LAND + "?";
                    messa = cont + ". Error con el Pais<br/>";
                    cont++;
                }

                ////-------------------------------CLIENTE
                if (da.VKORG==null)
                {
                    cl.KUNNRX = false;
                    cl.KUNNR = cl.KUNNR + "?";
                    messa = cont + ". Error con el Cliente<br/>";
                    cont++;
                }

                ////-------------------------------NOMBRE DEL CLIENTE
                if (string.IsNullOrEmpty(cl.CLIENTE_N) )
                {
                    cl.CLIENTE_N = cl.CLIENTE_N + "?";
                    messa = cont + ". Error con el Nombre del Cliente<br/>";
                    cont++;
                }

                ////-------------------------------Niveles
                for (int i = 0; i < 8; i++)
                {
                    if (ids[i] != null && ids[i] != "")
                    {
                        var usuario = ids[i];
                        if (!db.USUARIOs.Any(x => x.ID==usuario & x.ACTIVO==true))
                            idsx[i] = false;
                        else if (string.IsNullOrEmpty(ids[i]) && (i == 1 || i == 6))
                            idsx[i] = false;
                        var puesto = db.USUARIOs.Where(x => x.ID == usuario & x.ACTIVO == true).Select(x => x.PUESTO_ID).FirstOrDefault();
                        if (puesto == 1 || puesto == 14)
                        {
                            idsx[i] = false;
                        }
                        if ((puesto != 8 && i == 6) || (puesto != 9 && i == 7))
                        {
                            idsx[i] = false;
                        }
                    }
                    if (!idsx[i])
                    {
                        ids[i] = ids[i] + "?";
                        messa = messa + cont + ". Error en el nivel " + i + "<br/>";
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

                ////-------------------------------ID_PROVEEDOR
                    if (!string.IsNullOrEmpty(cl.ID_PROVEEDOR) && !db.PROVEEDORs.Any(x => x.ID==cl.ID_PROVEEDOR))
                    {
                        cl.ID_PROVEEDORX = false;
                        cl.ID_PROVEEDOR = cl.ID_PROVEEDOR + "?";
                        messa = messa + cont + ". Error en el vendor<br/>";
                        cont++;
                    }
                
                ////-------------------------------CANAL
                if (!string.IsNullOrEmpty(cl.CANAL) && !db.CANALs.Any(x => x.CANAL1 == cl.CANAL))
                {
                        cl.CANALX = false;
                        cl.CANAL = cl.CANAL + "?";
                        messa = messa + cont + ". Error en el canal<br/>";
                        cont++;
                }

                ////-------------------------------EMAIL
                    if (!string.IsNullOrEmpty(cl.CONTACTO) && !ComprobarEmail(cl.CONTACTOE) )
                    {
                        cl.CONTACTOEX = false;
                        cl.CONTACTOE = cl.CONTACTOE + "?";
                        messa = messa + cont + ". Error en el correo<br/>";
                        cont++;
                    }
                

                da.MESS = messa;
                cl.MESS = da.MESS;
                if (string.IsNullOrEmpty(messa)) {
                    cc.Add(cl);
                }
                else
                {
                    ccErr.Add(cl);
                }
            }
            ccErr.AddRange(cc);
            return Json(ccErr, JsonRequestBehavior.AllowGet);
            //  return View("CargaList",cc);
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
                if (noc[i] == "")
                { tablas[i, 3] = null; }
                else
                { tablas[i, 3] = noc[i]; }
                if (ni0[i] == "")
                { tablas[i, 4] = null; }
                else
                { tablas[i, 4] = ni0[i]; }
                tablas[i, 5] = ni1[i];
                if (ni2[i] == "")
                { tablas[i, 6] = null; }
                else
                { tablas[i, 6] = ni2[i]; }
                if (ni3[i] == "")
                { tablas[i, 7] = null; }
                else
                { tablas[i, 7] = ni3[i]; }
                if (ni3[i] == "")
                { tablas[i, 8] = null; }
                else
                { tablas[i, 8] = ni4[i]; }
                if (ni5[i] == "")
                { tablas[i, 9] = null; }
                else
                { tablas[i, 9] = ni5[i]; }
                tablas[i, 10] = ni6[i];
                if (ni7[i] == "")
                { tablas[i, 11] = null; }
                else
                { tablas[i, 11] = ni7[i]; }
                if (ven[i] == "")
                { tablas[i, 12] = null; }
                else
                { tablas[i, 12] = ven[i]; }
                if (ban[i] == "")
                { tablas[i, 13] = null; }
                else
                { tablas[i, 13] = ban[i]; }
                if (baa[i] == "")
                { tablas[i, 14] = null; }
                else
                { tablas[i, 14] = baa[i]; }
                if (can[i] == "")
                { tablas[i, 15] = null; }
                else
                { tablas[i, 15] = can[i]; }
                if (exp[i] == "")
                { tablas[i, 16] = null; }
                else
                { tablas[i, 16] = exp[i]; }
                if (con[i] == "")
                { tablas[i, 17] = null; }
                else
                { tablas[i, 17] = con[i]; }
                if (eco[i] == "")
                { tablas[i, 18] = null; }
                else
                { tablas[i, 18] = eco[i]; }
                tablas[i, 19] = mes[i];
            }

            ld = ObjAList2(tablas, rows);
            int cont = 0;

            foreach (DET_AGENTE1 da in ld)
            {
                CLIENTEF cl = new CLIENTEF();

                if (da.MESS == null || da.MESS == "")
                {
                    ////Agregar a CLIENTEF
                    cl.VKORG = da.VKORG;
                    cl.VTWEG = da.VTWEG;
                    cl.SPART = da.SPART;
                    cl.KUNNR = da.KUNNR;
                    List<CLIENTEF> clientesF = db.CLIENTEFs.Where(x => x.KUNNR.Equals(cl.KUNNR)).ToList();
                    CLIENTE cl1 = db.CLIENTEs.Where(x => x.KUNNR.Equals(cl.KUNNR)).FirstOrDefault();
                    if (!clientesF.Any())
                    {
                        cl.VERSION = 1;
                        cl.FECHAC = DateTime.Today;
                        cl.FECHAM = null;
                    }
                    else
                    {
                        cl.VERSION = clientesF.Count + 1;
                        cl.FECHAC = null;
                        cl.FECHAM = DateTime.Today;
                        //Actualizar a 0 Actiovo
                        CLIENTEF clienteF = clientesF.FirstOrDefault(x=>x.ACTIVO);
                        if (clienteF!=null) {
                            clienteF.ACTIVO = false;
                            db.Entry(clienteF).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    cl.USUARIO0_ID = (!string.IsNullOrEmpty(da.ID_US0) ?da.ID_US0.Trim():null);
                    cl.USUARIO1_ID = (!string.IsNullOrEmpty(da.ID_US1) ? da.ID_US1.Trim() : null);
                    cl.USUARIO2_ID = (!string.IsNullOrEmpty(da.ID_US2) ? da.ID_US2.Trim() : null);
                    cl.USUARIO3_ID = (!string.IsNullOrEmpty(da.ID_US3) ? da.ID_US3.Trim() : null);
                    cl.USUARIO4_ID = (!string.IsNullOrEmpty(da.ID_US4) ? da.ID_US4.Trim() : null);
                    cl.USUARIO5_ID = (!string.IsNullOrEmpty(da.ID_US5) ? da.ID_US5.Trim() : null);
                    cl.USUARIO6_ID = (!string.IsNullOrEmpty(da.ID_US6) ? da.ID_US6.Trim() : null);
                    cl.USUARIO7_ID = (!string.IsNullOrEmpty(da.ID_US7) ? da.ID_US7.Trim() : null);
                    cl.ACTIVO = true;

                    ////Modificar a CLIENTE
                    cl1.NAME1 = (!string.IsNullOrEmpty(da.CLIENTE_N) ? da.CLIENTE_N.Trim() : cl1.NAME1);
                    cl1.PROVEEDOR_ID = (!string.IsNullOrEmpty(da.ID_PROVEEDOR) ? da.ID_PROVEEDOR.Trim() : cl1.PROVEEDOR_ID);
                    cl1.LAND = (!string.IsNullOrEmpty(da.LAND) ? da.LAND.Trim() : cl1.LAND);
                    cl1.BANNER = (!string.IsNullOrEmpty(da.BANNER) ? da.BANNER.Trim() : cl1.BANNER);
                    cl1.BANNERG = (!string.IsNullOrEmpty(da.BANNERG) ? da.BANNERG.Trim() : cl1.BANNERG);
                    cl1.CANAL = (!string.IsNullOrEmpty(da.CANAL) ? da.CANAL.Trim() : cl1.CANAL);
                    cl1.EXPORTACION = (!string.IsNullOrEmpty(da.EXPORTACION) ? da.EXPORTACION.Trim() : cl1.EXPORTACION);
                    cl1.CONTAC = (!string.IsNullOrEmpty(da.CONTACTO) ? da.CONTACTO.Trim() : cl1.CONTAC);
                    cl1.CONT_EMAIL = (!string.IsNullOrEmpty(da.CONTACTOE ) ? da.CONTACTOE.Trim() : cl1.CONT_EMAIL);
                    db.Entry(cl1).State = EntityState.Modified;
                    
                    ////Agregar a contacto
                    if (da.CONTACTO != null)
                    {
                        CONTACTOC co = new CONTACTOC();
                        db.CONTACTOCs.Where(x => (x.DEFECTO != null && x.DEFECTO.Value) && x.VKORG == da.VKORG
                        && x.SPART== da.SPART && x.KUNNR == da.KUNNR).ToList().ForEach(x=>
                        {
                            x.DEFECTO = false;
                            db.Entry(x).State = EntityState.Modified;
                        });
                        if (!db.CONTACTOCs.Any(x=>x.EMAIL== (da.CONTACTOE != null ? da.CONTACTOE.Trim() : null) && x.NOMBRE== (da.CONTACTO != null ? da.CONTACTO.Trim() : null) && x.KUNNR==da.KUNNR))
                        {
                            co.NOMBRE = (!string.IsNullOrEmpty(da.CONTACTO) ? da.CONTACTO.Trim() : null);
                            co.EMAIL = (!string.IsNullOrEmpty(da.CONTACTOE) ? da.CONTACTOE.Trim() : null);
                            co.VKORG = da.VKORG;
                            co.VTWEG = da.VTWEG;
                            co.SPART = da.SPART;
                            co.KUNNR = da.KUNNR;
                            co.ACTIVO = true;
                            co.DEFECTO = true;
                            db.CONTACTOCs.Add(co);
                        }
                        else
                        {
                            co = db.CONTACTOCs.FirstOrDefault(x => x.EMAIL == da.CONTACTOE.Trim() && x.NOMBRE == da.CONTACTO.Trim() && x.KUNNR == da.KUNNR);
                            co.DEFECTO = true;
                            co.ACTIVO = true;
                            db.Entry(co).State = EntityState.Modified;
                        }

                    }
                    ////Guardar cambios en db
                    db.CLIENTEFs.Add(cl);
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

                cl.BUKRS = da.BUKRS.Replace(" ", "");
                cl.BUKRSX = true;
                cl.LAND = da.LAND.Replace(" ", "");
                cl.LANDX = true;
                cl.KUNNR = da.KUNNR.Replace(" ", "");
                cl.KUNNRX = true;
                cl.CLIENTE_N = da.CLIENTE_N;
                ids[0] = da.ID_US0.Replace(" ", "");
                idsx[0] = true;
                ids[1] = da.ID_US1.Replace(" ", "");
                idsx[1] = true;
                ids[2] = da.ID_US2.Replace(" ", "");
                idsx[2] = true;
                ids[3] = da.ID_US3.Replace(" ", "");
                idsx[3] = true;
                ids[4] = da.ID_US4.Replace(" ", "");
                idsx[4] = true;
                ids[5] = da.ID_US5.Replace(" ", "");
                idsx[5] = true;
                ids[6] = da.ID_US6.Replace(" ", "");
                idsx[6] = true;
                ids[7] = da.ID_US7.Replace(" ", "");
                idsx[7] = true;
                cl.ID_PROVEEDOR = da.ID_PROVEEDOR.Replace(" ", "");
                cl.ID_PROVEEDORX = true;
                cl.BANNER = da.BANNER.Replace(" ", "");
                cl.BANNERG = da.BANNERG.Replace(" ", "");
                cl.CANAL = da.CANAL.Replace(" ", "");
                cl.CANALX = true;
                cl.EXPORTACION = da.EXPORTACION.Replace(" ", "");
                cl.CONTACTO = da.CONTACTO;
                cl.CONTACTOE = da.CONTACTOE.Replace(" ", "");
                cl.CONTACTOEX = true;

                ////-------------------------------CoCode
                PAI s = paises.Where(x => x.SOCIEDAD_ID.Equals(cl.BUKRS)).FirstOrDefault();
                if (s == null)
                {
                    s = db.PAIS.Where(x => x.SOCIEDAD_ID.Equals(cl.BUKRS) & x.ACTIVO == true).FirstOrDefault();
                    if (s == null)
                        cl.BUKRSX = false;
                    else
                        paises.Add(s);
                }
                if (!cl.BUKRSX)
                {
                    cl.BUKRS = cl.BUKRS + "?";
                    messa = cont + ". Error con el CoCode<br/>";
                    cont++;
                }

                ////-------------------------------Pais
                PAI p = paises.Where(x => x.LAND.Equals(cl.LAND)).FirstOrDefault();
                if (p == null)
                {
                    p = db.PAIS.Where(x => x.LAND.Equals(cl.LAND) & x.ACTIVO == true).FirstOrDefault();
                    if (p == null)
                        cl.LANDX = false;
                    else
                        paises.Add(p);
                }
                if (!cl.LANDX)
                {
                    cl.LAND = cl.LAND + "?";
                    messa = cont + ". Error con el Pais<br/>";
                    cont++;
                }

                ////-------------------------------CLIENTE
                CLIENTE k = clientes.Where(x => x.KUNNR.Equals(cl.KUNNR)).FirstOrDefault();
                if (k == null)
                {
                    k = db.CLIENTEs.Where(x => x.KUNNR.Equals(cl.KUNNR) & x.ACTIVO == true).FirstOrDefault();
                    if (k == null)
                        cl.KUNNRX = false;
                    else
                    {
                        clientes.Add(k);
                        if (cl.CLIENTE_N == "" || cl.CLIENTE_N == null)
                        {
                            var ncli = (from x in db.CLIENTEs where x.KUNNR.Equals(cl.KUNNR) select x.NAME1).FirstOrDefault();
                            if (ncli == null || ncli == "")
                            {
                                cl.CLIENTE_N = "";
                            }
                            else
                            {
                                cl.CLIENTE_N = ncli;
                            }
                        }
                    }
                }
                if (!cl.KUNNRX)
                {
                    cl.KUNNR = cl.KUNNR + "?";
                    messa = cont + ". Error con el Cliente<br/>";
                    cont++;
                }

                ////-------------------------------NOMBRE DEL CLIENTE
                if (cl.CLIENTE_N == null || cl.CLIENTE_N == "")
                {
                    cl.CLIENTE_N = cl.CLIENTE_N + "?";
                    messa = cont + ". Error con el Nombre del Cliente<br/>";
                    cont++;
                }

                ////-------------------------------Niveles
                for (int i = 0; i < 8; i++)
                {
                    if (ids[i] != null && ids[i] != "")
                    {
                        var usuario = ids[i];
                        if (!db.USUARIOs.Any(x => x.ID == usuario & x.ACTIVO == true))
                            idsx[i] = false;
                        else if (string.IsNullOrEmpty(ids[i]) && (i == 1 || i == 6))
                            idsx[i] = false;
                        var puesto = db.USUARIOs.Where(x => x.ID == usuario & x.ACTIVO == true).Select(x => x.PUESTO_ID).FirstOrDefault();
                        if (puesto == 1 || puesto == 14)
                        {
                            idsx[i] = false;
                        }
                        if ((puesto != 8 && i == 6) || (puesto != 9 && i == 7))
                        {
                            idsx[i] = false;
                        }
                    }
                    if (!idsx[i])
                    {
                        ids[i] = ids[i] + "?";
                        messa = messa + cont + ". Error en el nivel " + i + "<br/>";
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

                ////-------------------------------ID_PROVEEDOR
                if (cl.ID_PROVEEDOR != null && cl.ID_PROVEEDOR != "")
                {
                    PROVEEDOR pr = db.PROVEEDORs.Where(x => x.ID.Equals(cl.ID_PROVEEDOR)).FirstOrDefault();
                    if (pr == null)
                    {
                        cl.ID_PROVEEDORX = false;
                    }
                }
                if (!cl.ID_PROVEEDORX)
                {
                    cl.ID_PROVEEDOR = cl.ID_PROVEEDOR + "?";
                    messa = messa + cont + ". Error en el vendor<br/>";
                    cont++;
                }
                ////-------------------------------CANAL
                if (cl.CANAL != null && cl.CANAL != "")
                {
                    CANAL ca = db.CANALs.Where(x => x.CANAL1.Equals(cl.CANAL)).FirstOrDefault();
                    if (ca == null)
                    {
                        cl.CANALX = false;
                    }
                }
                if (!cl.CANALX)
                {
                    cl.CANAL = cl.CANAL + "?";
                    messa = messa + cont + ". Error en el canal<br/>";
                    cont++;
                }

                ////-------------------------------EMAIL
                if (cl.CONTACTO != null && cl.CONTACTO != "")
                {
                    if (ComprobarEmail(cl.CONTACTOE) == false)
                    {
                        cl.CONTACTOEX = false;
                    }
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
            List<Clientes> cc = new List<Clientes>();
            
            var cli = Request["cli"];

            if (!string.IsNullOrEmpty(cli))
            {
                Clientes cl = new Clientes();
                cli = Completa(cli, 10);
                CLIENTE k = db.CLIENTEs.Where(x => x.KUNNR.Equals(cli) && x.ACTIVO).FirstOrDefault();
                cl.KUNNRX = true;
                cl.BUKRS = "";
                cl.LAND = "";
                cl.KUNNR = "";
                cl.CLIENTE_N = "";
                cl.ID_US0 = "";
                cl.ID_US1 = "";
                cl.ID_US2 = "";
                cl.ID_US3 = "";
                cl.ID_US4 = "";
                cl.ID_US5 = "";
                cl.ID_US6 = "";
                cl.ID_US7 = "";
                cl.ID_PROVEEDOR = "";
                cl.BANNER = "";
                cl.BANNERG = "";
                cl.CANAL = "";
                cl.EXPORTACION = "";
                cl.CONTACTO = "";
                cl.CONTACTOE = "";
                cl.MESS = "";

                if (k == null)
                    cl.KUNNRX = false;
                else
                {
                    CLIENTEF clienteF = db.CLIENTEFs.FirstOrDefault(x=>x.KUNNR.Equals(cli) && x.ACTIVO);
                    var com = "";
                    com = k.LAND;
                    if (com != null)
                        cl.LAND = com;
                    com = (from x in db.PAIS where x.LAND.Equals(k.LAND) && x.ACTIVO select x.SOCIEDAD_ID).FirstOrDefault();
                    if (com != null)
                        cl.BUKRS = com;
                    cl.KUNNR = cli;
                    com = k.NAME1;
                    if (com != null)
                        cl.CLIENTE_N = com;
                    com = clienteF?.USUARIO0_ID;
                    if (com != null)
                        cl.ID_US0 = com;
                    com = clienteF?.USUARIO1_ID;
                    if (com != null)
                        cl.ID_US1 = com;
                    com = clienteF?.USUARIO2_ID;
                    if (com != null)
                        cl.ID_US2 = com;
                    com = clienteF?.USUARIO3_ID;
                    if (com != null)
                        cl.ID_US3 = com;
                    com = clienteF?.USUARIO4_ID;
                    if (com != null)
                        cl.ID_US4 = com;
                    com = clienteF?.USUARIO5_ID;
                    if (com != null)
                        cl.ID_US5 = com;
                    com = clienteF?.USUARIO6_ID;
                    if (com != null)
                        cl.ID_US6 = com;
                    com = clienteF?.USUARIO7_ID;
                    if (com != null)
                        cl.ID_US7 = com;
                    com = k.PROVEEDOR_ID;
                    if (com != null)
                        cl.ID_PROVEEDOR = com;
                    com = k.BANNER;
                    if (com != null)
                        cl.BANNER = com;
                    com = k.BANNERG;
                    if (com != null)
                        cl.BANNERG = "";
                    com = k.CANAL;
                    if (com != null)
                        cl.CANAL = com;
                    com = k.EXPORTACION;
                    if (com != null)
                        cl.EXPORTACION = com;
                    com = k.CONTAC;
                    if (com != null)
                        cl.CONTACTO = com;
                    com = k.CONT_EMAIL;
                    if (com != null)
                        cl.CONTACTOE = com;
                }
                if (!cl.KUNNRX)
                {
                    cl.KUNNR = cli + "?";
                    cl.MESS = "El cliente no existe";
                }
                cc.Add(cl);
            }
            
            JsonResult jl = Json(cc, JsonRequestBehavior.AllowGet);
            return jl;
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
            var cLiente = db.CLIENTEs.ToList();
            generarExcelHome(cLiente, Server.MapPath("~/pdfTemp/"));
            return File(Server.MapPath("~/pdfTemp/Clientes_" + DateTime.Now.ToShortDateString() + ".xlsx"), "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clientes_" + DateTime.Now.ToShortDateString() + ".xlsx");
        }

        public void generarExcelHome(List<CLIENTE> lst, string ruta)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            try
            {
                worksheet.Cell("A1").Value = new[] { new { BANNER = "Id Cliente" }, };
                worksheet.Cell("B1").Value = new[] { new { BANNER = "Nombre" }, };
                worksheet.Cell("C1").Value = new[] { new { BANNER = "Región" }, };
                worksheet.Cell("D1").Value = new[] { new { BANNER = "País" }, };
                worksheet.Cell("E1").Value = new[] { new { BANNER = "Tipo de Cliente" }, };
                worksheet.Cell("F1").Value = new[] { new { BANNER = "Payer" }, };
                worksheet.Cell("G1").Value = new[] { new { BANNER = "Canal" }, };
                worksheet.Cell("H1").Value = new[] { new { BANNER = "Estatus" }, };
                worksheet.Cell("I1").Value = new[] { new { BANNER = "Contacto" }, };
                worksheet.Cell("J1").Value = new[] { new { BANNER = "Email" }, };

                for (int i = 2; i <= (lst.Count + 1); i++)
                {
                    var NOMBRE = "";
                    var EMAIL = "";
                    var pais = lst[i - 2].LAND;
                    var kunnr = lst[i - 2].KUNNR;
                    var pais2 = db.PAIS.Where(X => X.LAND.Equals(pais)).Select(x => x.LANDX).FirstOrDefault();
                    var contacto = db.CONTACTOCs.Where(x => x.KUNNR == kunnr && x.ACTIVO == true).ToArray();
                    if (contacto != null)
                    {
                        for (int j = 0; j < contacto.Length; j++)
                        {
                            if (contacto[j].DEFECTO == true)
                            {
                                NOMBRE = NOMBRE + contacto[j].NOMBRE + "*/";
                                EMAIL = EMAIL + contacto[j].EMAIL + "*/";
                            }
                            else
                            {
                                NOMBRE = NOMBRE + contacto[j].NOMBRE + "/";
                                EMAIL = EMAIL + contacto[j].EMAIL + "/";
                            }
                        }
                    }
                    worksheet.Cell("A" + i).Value = new[] { new { BANNER = lst[i - 2].KUNNR.TrimStart('0') }, };
                    worksheet.Cell("B" + i).Value = new[] { new { BANNER = lst[i - 2].NAME1 }, };
                    worksheet.Cell("C" + i).Value = new[] { new { BANNER = lst[i - 2].SUBREGION }, };
                    worksheet.Cell("D" + i).Value = new[] { new { BANNER = pais2 }, };
                    worksheet.Cell("E" + i).Value = new[] { new { BANNER = lst[i - 2].PARVW }, };
                    worksheet.Cell("F" + i).Value = new[] { new { BANNER = lst[i - 2].PAYER.TrimStart('0') }, };
                    worksheet.Cell("G" + i).Value = new[] { new { BANNER = lst[i - 2].CANAL }, };
                    worksheet.Cell("H" + i).Value = new[] { new { BANNER = lst[i - 2].ACTIVO? "Activo":"Inactivo" }, };
                    worksheet.Cell("I" + i).Value = new[] { new { BANNER = NOMBRE.TrimEnd('/') }, };
                    worksheet.Cell("J" + i).Value = new[] { new { BANNER = EMAIL.TrimEnd('/') }, };
                }
                var rt = ruta + @"\Clientes_" + DateTime.Now.ToShortDateString() + ".xlsx";
                workbook.SaveAs(rt);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
            }
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

            var rowsc = dt.Rows.Count;
            var columnsc = dt.Columns.Count;
            var rows = 1;
            var pos = 1;

            for (int i = rows; i < rowsc; i++)
            {
                DET_AGENTE1 doc = new DET_AGENTE1();
                CLIENTE existeCliente = null;
                doc.POS = Convert.ToInt32(pos);

                //CoCode
                doc.BUKRS = (dt.Rows[i][0]!=null? dt.Rows[i][0].ToString().ToUpper():null);
                //Pais
                doc.LAND = dt.Rows[i][1].ToString().ToUpper();
                //Cliente
                doc.KUNNR = dt.Rows[i][2].ToString();
                doc.KUNNR = Completa(doc.KUNNR, 10);

                existeCliente = db.CLIENTEs.Where(cc => cc.KUNNR == doc.KUNNR & cc.ACTIVO).FirstOrDefault();
                if (!String.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                {
                    doc.CLIENTE_N = dt.Rows[i][3].ToString().Replace(',', ' ').ToUpper();
                }
                else if (existeCliente != null)
                {
                    doc.CLIENTE_N = (existeCliente.NAME1 == null ? "" : existeCliente.NAME1.Replace(',', ' '));
                }
                if (existeCliente == null)
                    doc.VKORG = null;
                else
                {
                    doc.VKORG = existeCliente.VKORG;
                    doc.VTWEG = existeCliente.VTWEG;
                    doc.SPART = existeCliente.SPART;
                }
                //Manager
                doc.ID_US0 = (dt.Rows[i][4] != null ? dt.Rows[i][4].ToString().ToUpper() : null);
                //Nivel 1
                doc.ID_US1 = (dt.Rows[i][5] != null ? dt.Rows[i][5].ToString().ToUpper() : null);
                //Nivel 2
                doc.ID_US2 = (dt.Rows[i][6] != null ? dt.Rows[i][6].ToString().ToUpper() : null);
                //Nivel 3
                doc.ID_US3 = (dt.Rows[i][7] != null ? dt.Rows[i][7].ToString().ToUpper() : null);
                //Nivel 4
                doc.ID_US4 = (dt.Rows[i][8] != null ? dt.Rows[i][8].ToString().ToUpper() : null);
                //Nivel 5
                doc.ID_US5 = (dt.Rows[i][9] != null ? dt.Rows[i][9].ToString().ToUpper() : null);
                //Nivel 6
                doc.ID_US6 = (dt.Rows[i][10] != null ? dt.Rows[i][10].ToString().ToUpper() : null);
                //Nivel 7
                doc.ID_US7 = (dt.Rows[i][11] != null ? dt.Rows[i][11].ToString().ToUpper() : null);

                if (!String.IsNullOrEmpty(dt.Rows[i][12].ToString()))
                //if (string.IsNullOrEmpty(dt.Rows[i][12]==null?"": dt.Rows[i][12].ToString()))
                {
                    doc.ID_PROVEEDOR = dt.Rows[i][12].ToString();
                }
                else
                {
                    if (existeCliente == null)
                    {
                        doc.ID_PROVEEDOR = "";
                    }
                    else
                    {
                        doc.ID_PROVEEDOR = (existeCliente.PROVEEDOR_ID == null ? "" : existeCliente.PROVEEDOR_ID);
                    }
                }
                doc.ID_PROVEEDOR = Completa(doc.ID_PROVEEDOR, 10);
                //Banner

                if (!String.IsNullOrEmpty(dt.Rows[i][13].ToString()))
                //if(string.IsNullOrEmpty(dt.Rows[i][13] == null ? "" : dt.Rows[i][13].ToString()))
                {
                    doc.BANNER = dt.Rows[i][13].ToString();
                }
                else
                {
                    if (existeCliente == null)
                    {
                        doc.BANNER = "";
                    }
                    else
                    {
                        doc.BANNER = (existeCliente.BANNER == null ? "" : existeCliente.BANNER);
                    }
                }
                doc.BANNER = Completa(doc.BANNER, 10);
                //Banner Agrupador
                if (!String.IsNullOrEmpty(dt.Rows[i][14].ToString()))
                //if (string.IsNullOrEmpty(dt.Rows[i][14] == null ? "" : dt.Rows[i][14].ToString()))
                {
                    doc.BANNERG = dt.Rows[i][14].ToString();
                }
                else
                {
                    if (existeCliente == null)
                    {
                        doc.BANNERG = "";
                    }
                    else
                    {
                        doc.BANNERG = (existeCliente.BANNERG == null ? "" : existeCliente.BANNERG);
                    }
                }
                doc.BANNERG = Completa(doc.BANNERG, 10);

                //Canal
                //if (string.IsNullOrEmpty(dt.Rows[i][15] == null ? "" : dt.Rows[i][15].ToString()))
                if (!String.IsNullOrEmpty(dt.Rows[i][15].ToString()))
                {
                    doc.CANAL = dt.Rows[i][15].ToString();
                }
                else
                {
                    if (existeCliente == null)
                    {
                        doc.CANAL = "";
                    }
                    else
                    {
                        doc.CANAL = (existeCliente.CANAL == null ? "" : existeCliente.CANAL);
                    }
                }
                //EXPORTACION
                if (!String.IsNullOrEmpty(dt.Rows[i][16].ToString()))
                //if (string.IsNullOrEmpty(dt.Rows[i][16] == null ? "" : dt.Rows[i][16].ToString()))
                {
                    doc.EXPORTACION = dt.Rows[i][16].ToString();
                }
                else
                {
                    if (existeCliente == null)
                    {
                        doc.EXPORTACION = "";
                    }
                    else
                    {
                        doc.EXPORTACION = (existeCliente.EXPORTACION == null ? "" : existeCliente.EXPORTACION);
                    }
                }
                //CONTACTO
                doc.CONTACTO = (dt.Rows[i][17] != null ? dt.Rows[i][17].ToString().ToUpper() : null);
                //Contacto Emal
               
                doc.CONTACTOE = (dt.Rows[i][18]!= null ? dt.Rows[i][18].ToString().ToUpper() : null);


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
        
        public JsonResult Usuario8(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from x in db.USUARIOs
                     where x.ID.Contains(Prefix) && x.ACTIVO == true && x.PUESTO_ID == 8
                     select new { x.ID, x.NOMBRE, x.APELLIDO_P }).ToList();

            if (c.Count == 0)
            {
                var c2 = (from x in db.USUARIOs
                          where x.NOMBRE.Contains(Prefix) && x.ACTIVO == true && x.PUESTO_ID == 8
                          select new { x.ID, x.NOMBRE, x.APELLIDO_P }).ToList();
                c.AddRange(c2);
                if (c2.Count == 0)
                {
                    var c3 = (from x in db.USUARIOs
                              where x.APELLIDO_P.Contains(Prefix) && x.ACTIVO == true && x.PUESTO_ID == 8
                              select new { x.ID, x.NOMBRE, x.APELLIDO_P }).ToList();
                    c.AddRange(c3);
                }
            }

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        public JsonResult Usuario9(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from x in db.USUARIOs
                     where x.ID.Contains(Prefix) && x.ACTIVO == true && x.PUESTO_ID == 9
                     select new { x.ID, x.NOMBRE, x.APELLIDO_P }).ToList();

            if (c.Count == 0)
            {
                var c2 = (from x in db.USUARIOs
                          where x.NOMBRE.Contains(Prefix) && x.ACTIVO == true && x.PUESTO_ID == 9
                          select new { x.ID, x.NOMBRE, x.APELLIDO_P }).ToList();
                c.AddRange(c2);
                if (c2.Count == 0)
                {
                    var c3 = (from x in db.USUARIOs
                              where x.APELLIDO_P.Contains(Prefix) && x.ACTIVO == true && x.PUESTO_ID == 9
                              select new { x.ID, x.NOMBRE, x.APELLIDO_P }).ToList();
                    c.AddRange(c3);
                }
            }

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        

       
        
    }
}
