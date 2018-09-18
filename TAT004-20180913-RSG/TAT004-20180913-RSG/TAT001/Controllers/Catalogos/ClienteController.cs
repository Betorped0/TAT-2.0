using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;


namespace TAT001.Controllers.Catalogos
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            int pagina = 631; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
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

                return View(db.CLIENTEs.ToList());
            }
        }

        // GET: Cliente/Details/5
        public ActionResult Details(string id)
        {
            int pagina = 632; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
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

                var cli = db.CLIENTEs
                          .Where(x => x.KUNNR == id)
                          .First();

                Cliente cl = new Cliente();
                cl.nombre = cli.NAME1;
                cl.identificadorF = cli.STCD1;
                cl.identificadorF2 = cli.STCD2;
                cl.pais = cli.LAND;
                cl.estado = cli.REGIO;
                cl.municipio = cli.ORT01;
                cl.direccion = cli.STRAS_GP;
                cl.cp = cli.PSTLZ;
                cl.contacC = cli.CONTAC;
                cl.contacE = cli.CONT_EMAIL;
                cl.parvw = cli.PARVW;
                cl.payer = cli.PAYER;
                cl.grupo = cli.GRUPO;
                cl.spras = cli.SPRAS;
                cl.banner = cli.BANNER;
                cl.canal = cli.CANAL;
                cl.salesDisc = cli.BZIRK;
                cl.priceGro = cli.KONDA;
                cl.salesGro = cli.VKGRP;
                cl.salesOff = cli.VKBUR;
                return View(cl);
            }
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(string id)
        {
            int pagina = 632; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
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

                var cli = db.CLIENTEs
                          .Where(x => x.KUNNR == id)
                          .First();

                Cliente cl = new Cliente();
                cl.nombre = cli.NAME1;
                cl.identificadorF = cli.STCD1;
                cl.identificadorF2 = cli.STCD2;
                cl.pais = cli.LAND;
                cl.estado = cli.REGIO;
                cl.municipio = cli.ORT01;
                cl.direccion = cli.STRAS_GP;
                cl.cp = cli.PSTLZ;
                cl.contacC = cli.CONTAC;
                cl.contacE = cli.CONT_EMAIL;
                cl.parvw = cli.PARVW;
                cl.payer = cli.PAYER;
                cl.grupo = cli.GRUPO;
                cl.spras = cli.SPRAS;
                cl.banner = cli.BANNER;
                cl.canal = cli.CANAL;
                cl.salesDisc = cli.BZIRK;
                cl.priceGro = cli.KONDA;
                cl.salesGro = cli.VKGRP;
                cl.salesOff = cli.VKBUR;
                return View(cl);
            }
        }

        // POST: Cliente/Edit/5
        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Cliente cl)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                try
                {
                    string u = User.Identity.Name;
                    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();


                    var cli = db.CLIENTEs.Where(x => x.KUNNR == id).First();
                    cli.NAME1 = cl.nombre;
                    cli.STCD1 = cl.identificadorF;
                    cli.STCD2 = cl.identificadorF2;
                    cli.LAND = cl.pais;
                    cli.REGIO = cl.estado;
                    cli.ORT01 = cl.municipio;
                    cli.STRAS_GP = cl.direccion;
                    cli.PSTLZ = cl.cp;
                    cli.CONTAC = cl.contacC;
                    cli.CONT_EMAIL = cl.contacE;
                    cli.PARVW = cl.parvw;
                    cli.PAYER = cl.payer;
                    cli.GRUPO = cl.grupo;
                    cli.SPRAS = cl.spras;
                    cli.BANNER = cl.banner;
                    cli.CANAL = cl.canal;
                    cli.BZIRK = cl.salesDisc;
                    cli.KONDA = cl.priceGro;
                    cli.VKGRP = cl.salesGro;
                    cli.VKBUR = cl.salesOff;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cliente/Delete/5
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
