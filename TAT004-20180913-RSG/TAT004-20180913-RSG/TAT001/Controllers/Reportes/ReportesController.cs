using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;
using static TAT001.Models.ReportesModel;

namespace TAT001.Controllers.Reportes
{
    [Authorize]
    public class ReportesController : Controller
    {
        private TAT001Entities db = new TAT001Entities();
        public TAT001.Services.Calendario445 cal = new TAT001.Services.Calendario445();
        // GET: Reportes
        public ActionResult Index()
        {
            return View();
        }

        //GET: Reportes/Allowance
        public ActionResult ReporteIntegracionTs()
        {
            int pagina = 1103; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = false;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.cuentagl = db.CUENTAGLs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();

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
            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);
            return View(dOCUMENTOes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteIntegracionTs(String Form)
        {
            int pagina = 1103; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            string comcode = Request["selectcocode"] as string;
            string[] comcodessplit = comcode.Split(',');
            string accnt = Request["selectaccount"] as string;
            string[] accntssplit = accnt.Split(',');
            int period = Int32.Parse(Request["selectperiod"]);
            string year =  Request["selectyear"];
            decimal decAccnt;
            ViewBag.display = true;
            ViewBag.Calendario = cal;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.cuentagl = db.CUENTAGLs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.cuenta = db.CUENTAs.ToList();
            ViewBag.Consulta = "";

            List<object> queryList = new List<object>();
            List<object> provitions = new List<object>();
            decimal docrefs = 0 ;

            
                foreach (string account in accntssplit)
                {
                foreach (string companyCode in comcodessplit)
                {
                    decimal numDoc;
                    decimal montoDoc;

                    Decimal.TryParse(account, out decAccnt);
                    var queryP = (from cu in db.CUENTAs
                                  join cg in db.CUENTAGLs on cu.ABONO equals cg.ID
                                  join doc in db.DOCUMENTOes on cg.ID equals doc.CUENTAP  //NUM_DOC + DOCUMENTO_SAP PAYER_ID + CLIENTE-NAME1 + TALLT-TXT050 + DOCUMENTO-CONCEPTO --DOCUMENTO-USUARIOD
                                  join docsap in db.DOCUMENTOSAPs on doc.NUM_DOC equals docsap.NUM_DOC
                                  join ta in db.TALLTs on doc.TALL_ID equals ta.TALL_ID
                                  join cli in db.CLIENTEs on new { doc.VKORG, doc.VTWEG, doc.SPART, doc.PAYER_ID } equals new { cli.VKORG, cli.VTWEG, cli.SPART, PAYER_ID = cli.KUNNR }   // NAME1
                                  join fl in db.FLUJOes on doc.NUM_DOC equals fl.NUM_DOC  //FLUJO-COMENTARIO -- FLUJO-USUARIOA
                                  where cu.SOCIEDAD_ID == companyCode.ToString() && doc.PERIODO == period && cg.ID == decAccnt && doc.EJERCICIO == year
                                  select new { cg.ID, cg.NOMBRE, doc.NUM_DOC, doc.DOCUMENTO_SAP, doc.PAYER_ID, doc.CONCEPTO, doc.USUARIOD_ID, cli.NAME1, fl.COMENTARIO, fl.USUARIOA_ID, ta.TALL_ID, ta.TXT50, docsap.FECHAC, doc.MONTO_DOC_MD, doc.PERIODO, doc.EJERCICIO }).Distinct().ToList();



                    var qqueryp2 = (from doc in db.DOCUMENTOes.ToList()
                                    join refe in queryP on doc.DOCUMENTO_REF equals refe.NUM_DOC
                                    select new { refe.NUM_DOC, refe.MONTO_DOC_MD, doc.DOCUMENTO_REF}).ToList();
                    
                    
                    //var docnumb = (from cu in db.cuentas
                    //              join cg in db.cuentagls on cu.abono equals cg.id
                    //              join doc in db.documentoes on cg.id equals doc.cuentap  //num_doc + documento_sap payer_id + cliente-name1 + tallt-txt050 + documento-concepto --documento-usuariod
                    //              join docsap in db.documentosaps on doc.num_doc equals docsap.num_doc
                    //              join ta in db.tallts on doc.tall_id equals ta.tall_id
                    //              join cli in db.clientes on new { doc.vkorg, doc.vtweg, doc.spart, doc.payer_id } equals new { cli.vkorg, cli.vtweg, cli.spart, payer_id = cli.kunnr }   // name1
                    //              join fl in db.flujoes on doc.num_doc equals fl.num_doc  //flujo-comentario -- flujo-usuarioa
                    //              where cu.sociedad_id == companycode.tostring() && doc.periodo == period && cg.id == decaccnt && doc.ejercicio == year
                    //              select new { doc.num_doc}).groupby(o => o.num_doc).take(150).tolist();

                    //foreach (var docNumb1 in docNumb)
                    //{
                    //    var docRef = (from doc in db.DOCUMENTOes
                    //                  where doc.DOCUMENTO_REF == 33
                    //                  select new { doc.MONTO_DOC_MD }).ToList();
                    //    foreach (var monto in docRef)
                    //    {
                    //        Decimal.TryParse(monto.ToString(), out montoDoc);

                    //        docrefs = docrefs + montoDoc;
                    //        provitions.Add(docRef.ToList());
                    //    }
                    //}


                    provitions.Add(qqueryp2);
                    queryList.Add(queryP);
                   
                }
            }
           
            

            ViewBag.Consulta2 = queryList;
            ViewBag.MontoNeg = provitions;
            //var queryP = (from cu in db.CUENTAs
            //              join cg in db.CUENTAGLs on cu.ABONO equals cg.ID
            //              join doc in db.DOCUMENTOes on cg.ID equals doc.CUENTAP  //NUM_DOC + DOCUMENTO_SAP PAYER_ID + CLIENTE-NAME1 + TALLT-TXT050 + DOCUMENTO-CONCEPTO --DOCUMENTO-USUARIOD
            //                 // NAME1
            //              where cu.SOCIEDAD_ID == cocode
            //              select new { cg.ID, cg.NOMBRE, doc.NUM_DOC, doc.DOCUMENTO_SAP, doc.PAYER_ID, doc.CONCEPTO, doc.USUARIOD_ID }).ToList();


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
            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);


            return View(dOCUMENTOes.ToList());
        }



        // GET: Reportes/Create
        public ActionResult ReporteProvisiones(string filtroCode)
        {
            int pagina = 1102; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.display = false;

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


            ViewBag.VKORG = new SelectList(db.CLIENTEs, "VKORG", "NAME1");
            ViewBag.CUENTAP = new SelectList(db.CUENTAGLs, "ID", "NOMBRE");
            ViewBag.CUENTAPL = new SelectList(db.CUENTAGLs, "ID", "NOMBRE");
            ViewBag.GALL_ID = new SelectList(db.GALLs, "ID", "DESCRIPCION");
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS");
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT");
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION");
            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION");
            ViewBag.USUARIOC_ID = new SelectList(db.USUARIOs, "ID", "PASS");
            ViewBag.NUM_DOC = new SelectList(db.DOCUMENTOSAPs, "NUM_DOC", "BUKRS");




            //var consultaAnio = (from a in db.DOCUMENTOes
            //                    group a by a.EJERCICIO into EJERCICIO
            //                    select new { EJERCICIO });
            //ViewBag.miconsultaAnio = consultaAnio; ;
            var consultaAnio = (from a in db.DOCUMENTOes select new { a.EJERCICIO }).Distinct().ToList();
            ViewBag.miconsultaAnio = new SelectList(consultaAnio, "EJERCICIO", "EJERCICIO");


            ViewBag.documento = db.DOCUMENTOes.ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();

            //ViewBag.consulta = miConsulta;

            //var miConsulta2 = miConsulta.GroupBy(sociedad => SOCIEDAD_ID);
            //ViewBag.consulta2 = miConsulta2;


            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);
            return View();
        }


        // POST: Reportes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteProvisiones(string filtroCode, int filtroPeriodo, string miconsultaAnio)
        {

            //if (ModelState.IsValid)
            //{
            //    db.DOCUMENTOes.Add(dOCUMENTO);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            int pagina = 1102; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.display = true;

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

            //ViewBag.VKORG = new SelectList(db.CLIENTEs, "VKORG", "NAME1", dOCUMENTO.VKORG);
            //ViewBag.CUENTAP = new SelectList(db.CUENTAGLs, "ID", "NOMBRE", dOCUMENTO.CUENTAP);
            //ViewBag.CUENTAPL = new SelectList(db.CUENTAGLs, "ID", "NOMBRE", dOCUMENTO.CUENTAPL);
            //ViewBag.GALL_ID = new SelectList(db.GALLs, "ID", "DESCRIPCION", dOCUMENTO.GALL_ID);
            //ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS", dOCUMENTO.PAIS_ID);
            //ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", dOCUMENTO.SOCIEDAD_ID);
            //ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION", dOCUMENTO.TALL_ID);
            //ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", dOCUMENTO.TSOL_ID);
            //ViewBag.USUARIOC_ID = new SelectList(db.USUARIOs, "ID", "PASS", dOCUMENTO.USUARIOC_ID);
            //ViewBag.NUM_DOC = new SelectList(db.DOCUMENTOSAPs, "NUM_DOC", "BUKRS", dOCUMENTO.NUM_DOC);

            ViewBag.documento = db.DOCUMENTOes.ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();

            var code = Request["filtroCode"];
            var periodo = Request["filtroPeriodo"];
            var anio = Request["filtroAnio"];
            //var cocode = code.Request["filtroCode"];
            string[] cocodesplit = code.Split(',');

            List<object> lista = new List<object>();
            List<object> rema = new List<object>();
            List<object> perio = new List<object>();
            List<object> comen = new List<object>();

            ViewBag.miles = ",";
            ViewBag.decimales = ".";


            foreach (string item in cocodesplit)
            {
                var miConsulta = (from x in db.DOCUMENTOes

                                  join CLIENTE in db.CLIENTEs on new { x.VKORG, x.VTWEG, x.SPART, x.PAYER_ID } equals new { CLIENTE.VKORG, CLIENTE.VTWEG, CLIENTE.SPART, PAYER_ID = CLIENTE.KUNNR }
                                  join FLUJO in db.FLUJOes on x.NUM_DOC equals FLUJO.NUM_DOC
                                  join CUENTAGL in db.CUENTAGLs on x.CUENTAP equals CUENTAGL.ID
                                  join DOCUMENSAP in db.DOCUMENTOSAPs on x.NUM_DOC equals DOCUMENSAP.NUM_DOC

                                  where x.SOCIEDAD_ID == item.ToString() && x.PERIODO == filtroPeriodo && x.EJERCICIO == miconsultaAnio && FLUJO.POS == 2

                                  select new
                                  {
                                      x.SOCIEDAD_ID,
                                      x.PAIS_ID,
                                      x.NUM_DOC,
                                      x.PAYER_ID,
                                      CLIENTE.NAME1,
                                      CLIENTE.CANAL,
                                      x.CONCEPTO,
                                      x.FECHAC,
                                      x.MONTO_DOC_MD,
                                      x.MONTO_DOC_ML2,
                                      DOCUMENSAP.IMPORTE,
                                      fechasap = DOCUMENSAP.FECHAC,
                                      x.CUENTAP,
                                      CUENTAGL.NOMBRE,
                                      x.CUENTAPL,
                                      x.DOCUMENTO_SAP,
                                      x.USUARIOC_ID,
                                      x.USUARIOD_ID,
                                      FLUJO.USUARIOA_ID,
                                      FLUJO.COMENTARIO,
                                      x.EJERCICIO,
                                      x.PERIODO
                                  }).Distinct().ToList();

                var montos = (from doc in db.DOCUMENTOes.ToList()
                              join refe in miConsulta on doc.DOCUMENTO_REF equals refe.NUM_DOC
                              select new { refe.NUM_DOC, refe.MONTO_DOC_MD, doc.DOCUMENTO_REF }).ToList();

                var period = (from doc in db.DOCUMENTOes.ToList()
                              join per in miConsulta on doc.PERIODO equals per.PERIODO
                              select new { per.NUM_DOC, per.MONTO_DOC_MD, doc.DOCUMENTO_REF }).ToList();

                List<Comentarios> comentarios = (from ff in db.FLUJOes
                                                 join j in
                                                 (from d in db.DOCUMENTOes
                                                  join f in db.FLUJOes on d.NUM_DOC equals f.NUM_DOC
                                                  where f.COMENTARIO != null
                                                  group f by d.NUM_DOC into g
                                                  select new { NUM_DOC = g.Key, POS = g.Max(p => p.POS) })
                                                  on new { ff.NUM_DOC, ff.POS } equals new { j.NUM_DOC, j.POS }
                                                 select new Comentarios { NUM_DOC = ff.NUM_DOC, COMENTARIOS = ff.COMENTARIO }).ToList();

                lista.Add(miConsulta);
                rema.Add(montos);
                perio.Add(period);
                comen.Add(comentarios);
            }
            ViewBag.miConsulSplit = lista;
            ViewBag.remanente = rema;
            ViewBag.peri = perio;
            ViewBag.ultimo = comen;

            //CONSULTA DEL FILTRO AÑO
            var consultaAnio = (from a in db.DOCUMENTOes select new { a.EJERCICIO }).Distinct().ToList();
            ViewBag.miconsultaAnio = new SelectList(consultaAnio, "EJERCICIO", "EJERCICIO");


            ViewBag.documento = db.DOCUMENTOes.ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            //ViewBag.consulta = miConsulta;


            //var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP).Where(a => a.SOCIEDAD_ID == SOCIEDAD_ID);
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // REPORTE 1 - CONCENTRADO
        public ActionResult ReporteConcentrado()
        {
            int pagina = 1101;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = false;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.paises = db.PAIS.ToList();
            ViewBag.anios = string.Empty;//TODO: Reserch which it's the anio value
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //SWALLOW
            }
            Session["spras"] = user.SPRAS_ID;
            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);
            return View(dOCUMENTOes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteConcentrado(string Form)
        {
            string[] comcodessplit = { };
            string[] yearsplit = { };
            string[] periodsplit = { };
            string[] paissplit = { };
            string[] campossplit;
            int pagina = 1101;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            string comcode = Request["selectcocode"] as string;

            //Co. Code
            if (!string.IsNullOrEmpty(comcode))
            {
                comcodessplit = comcode.Split(',');
            }

            //Quarter
            string year = Request["selectyear"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                yearsplit = year.Split(',');
            }

            //Period
            string period = Request["selectperiod"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                periodsplit = period.Split(',');
            }

            //Pais
            string pais = Request["selectpais"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                paissplit = pais.Split(',');
            }

            //Campos
            string campos = Request["selectcampos"] as string;
            campossplit = null;
            if (!string.IsNullOrEmpty(campos))
            {
                campossplit = campos.Split(',');
            }
            ViewBag.campos = campossplit;
            ViewBag.camposstring = campos;

            ViewBag.display = true;
            ViewBag.Calendario = cal;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.cuentagl = db.CUENTAGLs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.cuenta = db.CUENTAs.ToList();
            ViewBag.paises = db.PAIS.ToList();
            ViewBag.anios = string.Empty;//TODO: Reserch which it's the anio value

            List<ReportesModel.Concentrado> reporte = new List<Concentrado>();
            List<DOCUMENTO> documentos = db.DOCUMENTOes
                .Where(d => comcodessplit.Contains(d.SOCIEDAD_ID) && periodsplit.Contains(d.PERIODO.ToString()) && yearsplit.Contains(d.EJERCICIO))
                .Include(d => d.CLIENTE)
                .Where(d => paissplit.Contains(d.CLIENTE.LAND))
                .Include(d => d.CARTAs)
                .Include(d => d.CUENTAGL)
                .Include(d => d.CUENTAGL1)
                .Include(d => d.GALL)
                .Include(d => d.PAI)
                .Include(d => d.SOCIEDAD)
                .Include(d => d.TALL)
                .Include(d => d.TSOL)
                .Include(d => d.USUARIO)
                .Include(d => d.DOCUMENTOAs)
                .Include(d => d.DOCUMENTOFs)
                .Include(d => d.DOCUMENTOLs)
                .Include(d => d.DOCUMENTONs)
                .Include(d => d.DOCUMENTOPs)
                .Include(d => d.DOCUMENTORECs)
                .Include(d => d.DOCUMENTOTS)
                .Include(d => d.FLUJOes).ToList();
            foreach (DOCUMENTO dOCUMENTO in documentos)
            {
                Concentrado r1 = new Concentrado();
                if (dOCUMENTO.PAYER_ID == null) continue;
                //dOCUMENTO.CLIENTE = db.CLIENTEs.Where(a => a.VKORG.Equals(dOCUMENTO.VKORG)
                //                                        & a.VTWEG.Equals(dOCUMENTO.VTWEG)
                //                                        & a.SPART.Equals(dOCUMENTO.SPART)
                //                                        & a.KUNNR.Equals(dOCUMENTO.PAYER_ID)).First();
                r1.CANAL = db.CANALs.Where(a => a.CANAL1.Equals(dOCUMENTO.CLIENTE.CANAL)).FirstOrDefault();
                var cuentas = (from C in db.CUENTAs
                               join cgl in db.CUENTAGLs on C.CARGO equals cgl.ID
                               where C.SOCIEDAD_ID == dOCUMENTO.SOCIEDAD_ID
                               & C.PAIS_ID == dOCUMENTO.PAIS_ID
                               & C.TALL_ID == dOCUMENTO.TALL_ID
                               & C.EJERCICIO.ToString() == dOCUMENTO.EJERCICIO
                               select new { C.ABONO, C.CARGO, C.CLEARING, C.LIMITE, cgl.NOMBRE }).FirstOrDefault();
                if (cuentas != null)
                {
                    r1.CUENTA_ABONO = Convert.ToDecimal(cuentas.GetType().GetProperty("ABONO").GetValue(cuentas, null)); // Convertir a decimal por si es null
                    r1.CUENTA_CARGO = Convert.ToDecimal(cuentas.GetType().GetProperty("CARGO").GetValue(cuentas, null));
                    r1.CUENTA_CLEARING = Convert.ToDecimal(cuentas.GetType().GetProperty("CLEARING").GetValue(cuentas, null));
                    r1.CUENTA_LIMITE = Convert.ToDecimal(cuentas.GetType().GetProperty("LIMITE").GetValue(cuentas, null));
                    r1.CUENTA_CARGO_NOMBRE = cuentas.GetType().GetProperty("NOMBRE").GetValue(cuentas, null).ToString();
                }
                Presupuesto pres = new Presupuesto();
                r1.PRESUPUESTO = pres.getPresupuesto(dOCUMENTO.CLIENTE.KUNNR);
                var proveedor = dOCUMENTO.DOCUMENTOFs.Select(df => df.PROVEEDOR).FirstOrDefault();
                r1.PROVEEDOR_NOMBRE = db.PROVEEDORs.Where(x => x.ID.Equals(proveedor)).Select(p => p.NOMBRE).FirstOrDefault();
                //dOCUMENTO.DOCUMENTOF = db.DOCUMENTOFs.Where(a => a.NUM_DOC.Equals(dOCUMENTO.NUM_DOC)).ToList();
                //var vbFl = db.FLUJOes.Where(a => a.NUM_DOC.Equals(dOCUMENTO.NUM_DOC)).OrderBy(a => a.POS).ToList();
                //FLUJO fvbfl = new FLUJO();
                //var flng = db.FLUJNEGOes.Where(a => a.NUM_DOC.Equals(dOCUMENTO.NUM_DOC)).ToList();
                //if (flng.Count > 0)
                //{
                //    for (int i = 0; i < flng.Count; i++)
                //    {
                //        var kn = flng[i].KUNNR;
                //        var clName = db.CLIENTEs.Where(c => c.KUNNR == kn).Select(s => s.NAME1).FirstOrDefault();
                //        fvbfl = new FLUJO();
                //        fvbfl.NUM_DOC = flng[i].NUM_DOC;
                //        fvbfl.FECHAC = flng[i].FECHAC;
                //        fvbfl.FECHAM = flng[i].FECHAM;
                //        fvbfl.USUARIOA_ID = clName;// + "(Cliente)";
                //        fvbfl.COMENTARIO = flng[i].COMENTARIO;
                //        vbFl.Add(fvbfl);
                //    }
                //}
                //r1.workflow = vbFl;
                //string usuariodel = "";
                //DateTime fecha = DateTime.Now.Date;
                //List<TAT001.Entities.DELEGAR> del = db.DELEGARs.Where(a => a.USUARIOD_ID.Equals(User.Identity.Name) & a.FECHAI <= fecha & a.FECHAF >= fecha & a.ACTIVO == true).ToList();
                //FLUJO f = db.FLUJOes.Where(a => a.NUM_DOC.Equals(dOCUMENTO.NUM_DOC) & a.ESTATUS.Equals("P")).FirstOrDefault();
                //r1.acciones = f;
                //List<DOCUMENTOA> archivos = db.DOCUMENTOAs.Where(x => x.NUM_DOC.Equals(dOCUMENTO.NUM_DOC) & x.ACTIVO == true).ToList();//RSG 15.05.2018
                //r1.files = archivos;
                //if (f != null)
                //    if (f.USUARIOA_ID != null)
                //    {
                //        if (del.Count > 0)
                //        {
                //            DELEGAR dell = del.Where(a => a.USUARIO_ID.Equals(f.USUARIOA_ID)).FirstOrDefault();
                //            if (dell != null)
                //                usuariodel = dell.USUARIO_ID;
                //            else
                //                usuariodel = User.Identity.Name;
                //        }
                //        else
                //            usuariodel = User.Identity.Name;

                //        if (f.USUARIOA_ID.Equals(usuariodel))
                //            ViewBag.accion = db.WORKFPs.Where(a => a.ID.Equals(f.WORKF_ID) & a.POS.Equals(f.WF_POS) & a.VERSION.Equals(f.WF_VERSION)).FirstOrDefault().ACCION.TIPO;
                //    }
                //    else
                //    {
                //        ViewBag.accion = db.WORKFPs.Where(a => a.ID.Equals(f.WORKF_ID) & a.POS.Equals(f.WF_POS) & a.VERSION.Equals(f.WF_VERSION)).FirstOrDefault().ACCION.TIPO;
                //    }
                r1.documento = dOCUMENTO;
                //r1.pais = dOCUMENTO.PAIS_ID + ".png"; //RSG 29.09.2018
                //r1.flujo = db.FLUJOes.Where(a => a.NUM_DOC.Equals(dOCUMENTO.NUM_DOC)).OrderByDescending(a => a.POS).FirstOrDefault();
                //r1.ts = db.TS_FORM.Where(a => a.BUKRS_ID.Equals(r1.documento.SOCIEDAD_ID) & a.LAND_ID.Equals(r1.documento.PAIS_ID)).ToList();
                //r1.tts = db.DOCUMENTOTS.Where(a => a.NUM_DOC.Equals(r1.documento.NUM_DOC)).ToList();

                //if (r1.documento.DOCUMENTO_REF != null)
                //    r1.Title += r1.documento.DOCUMENTO_REF + "-";
                //r1.Title += dOCUMENTO.NUM_DOC;

                //r1.cartap = db.CARTAPs.Where(i => i.NUM_DOC == dOCUMENTO.NUM_DOC).ToList();

                //Models.PresupuestoModels carga = new Models.PresupuestoModels();
                //r1.ultMod = carga.consultarUCarga();

                //r1.TSOL_RELA = db.TSOLs.Where(a => a.ESTATUS == "M" & a.PADRE == false).ToList();
                //r1.miles = r1.documento.PAI.MILES;//LEJGG 090718
                //r1.dec = r1.documento.PAI.DECIMAL;//LEJGG 090718
                reporte.Add(r1);
            }
            ViewBag.lista_reporte = reporte;
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //SWALLOW
            }
            Session["spras"] = user.SPRAS_ID;
            return View();
        }

        //public PRESUPUESTO_MOD getPresupuesto(string kunnr)
        //{
        //    PRESUPUESTO_MOD pm = new PRESUPUESTO_MOD();
        //    try
        //    {
        //        if (kunnr == null)
        //            kunnr = "";

        //        //Obtener presupuesto
        //        string mes = DateTime.Now.Month.ToString();
        //        var presupuesto = db.CSP_PRESU_CLIENT(cLIENTE: kunnr, pERIODO: mes).Select(p => new { DESC = p.DESCRIPCION.ToString(), VAL = p.VALOR.ToString() }).ToList();
        //        string clien = db.CLIENTEs.Where(x => x.KUNNR == kunnr).Select(x => x.BANNERG).First();
        //        if (presupuesto != null)
        //        {
        //            if (String.IsNullOrEmpty(clien))
        //            {
        //                pm.P_CANAL = presupuesto[0].VAL;
        //                pm.P_BANNER = presupuesto[1].VAL;
        //                pm.PC_C = (float.Parse(presupuesto[4].VAL) + float.Parse(presupuesto[5].VAL) + float.Parse(presupuesto[6].VAL)).ToString();
        //                pm.PC_A = presupuesto[8].VAL;
        //                pm.PC_P = presupuesto[9].VAL;
        //                pm.PC_T = presupuesto[10].VAL;
        //                pm.CONSU = (float.Parse(presupuesto[1].VAL) - float.Parse(presupuesto[10].VAL)).ToString();
        //            }
        //            else
        //            {
        //                pm.P_CANAL = presupuesto[0].VAL;
        //                pm.P_BANNER = presupuesto[0].VAL;
        //                pm.PC_C = (float.Parse(presupuesto[4].VAL) + float.Parse(presupuesto[5].VAL) + float.Parse(presupuesto[6].VAL)).ToString();
        //                pm.PC_A = presupuesto[8].VAL;
        //                pm.PC_P = presupuesto[9].VAL;
        //                pm.PC_T = presupuesto[10].VAL;
        //                pm.CONSU = (float.Parse(presupuesto[0].VAL) - float.Parse(presupuesto[10].VAL)).ToString();
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return pm;
        //}

        // FIN REPORTE 1 - CONCENTRADO

        // REPORTE 4.1 - ALLOWANCESPL
        public ActionResult ReporteAllowancesPL()
        {
            int pagina = 1104;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = false;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.canales = db.CANALs.ToList();
            ViewBag.payers = string.Empty; //TODO: Research which it's the payers value
            ViewBag.categories = string.Empty; //TODO: Research which it's the categories value
            ViewBag.quarter = string.Empty;//TODO: Research which it's the q value
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //SWALLOW
            }
            Session["spras"] = user.SPRAS_ID;
            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);
            return View(dOCUMENTOes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteAllowancesPL(string Form)
        {
            int pagina = 1104;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

            //Co. Codes
            string[] comcodessplit;
            string comcode = Request["selectcocode"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                comcodessplit = comcode.Split(',');
            }

            //Quarters
            string[] quartersplit;
            string quarter = Request["selectq"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                quartersplit = quarter.Split(',');
            }

            //Periods
            string[] periodsplit;
            string period = Request["selectperiod"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                periodsplit = period.Split(',');
            }

            //Payers
            string[] payersplit;
            string payer = Request["selectpayer"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                payersplit = payer.Split(',');
            }

            //Categories
            string[] categorysplit;
            string category = Request["selectcategory"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                categorysplit = category.Split(',');
            }

            string year = Request["selectyear"];
            string canal = Request["selectcanal"];

            ViewBag.display = true;
            ViewBag.Calendario = cal;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.cuentagl = db.CUENTAGLs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.canales = db.CANALs.ToList();
            ViewBag.payers = string.Empty; //TODO: Research which it's the payers value
            ViewBag.categories = string.Empty; //TODO: Research which it's the categories value
            ViewBag.quarter = string.Empty;//TODO: Research which it's the q value

            List<object> queryList = new List<object>();
            List<object> provitions = new List<object>();

            /*
             AQUI VA EL QUERY
             */

            ViewBag.Consulta2 = queryList;
            ViewBag.MontoNeg = provitions;

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //SWALLOW
            }
            Session["spras"] = user.SPRAS_ID;
            return View();
        }
        // FIN REPORTE 4.1 - ALLOWANCESPL

        // REPORTE 4.2 - ALLOWANCESB
        public ActionResult ReporteAllowancesB()
        {
            int pagina = 1105;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = false;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.quarteruarter = string.Empty;//TODO: Reserch which it's the q value
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //SWALLOW
            }
            Session["spras"] = user.SPRAS_ID;
            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);
            return View(dOCUMENTOes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteAllowancesB(string Form)
        {
            string[] comcodessplit;
            string[] quartersplit;
            string[] periodsplit;
            int pagina = 1105;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            string comcode = Request["selectcocode"] as string;

            //Co. Code
            if (!string.IsNullOrEmpty(comcode))
            {
                comcodessplit = comcode.Split(',');
            }

            //Quarter
            string quarter = Request["selectquarter"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                quartersplit = quarter.Split(',');
            }

            //Period
            string period = Request["selectperiod"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                periodsplit = period.Split(',');
            }

            string year = Request["selectyear"];
            ViewBag.display = true;
            ViewBag.Calendario = cal;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.cuentagl = db.CUENTAGLs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.cuenta = db.CUENTAs.ToList();
            ViewBag.quarter = string.Empty;//TODO: Reserch which it's the q value

            List<object> queryList = new List<object>();
            List<object> provitions = new List<object>();


            /*
             AQUI VA EL QUERY
             */

            ViewBag.Consulta2 = queryList;
            ViewBag.MontoNeg = provitions;

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
                //SWALLOW
            }
            Session["spras"] = user.SPRAS_ID;
            return View();
        }
        // FIN REPORTE 4.2 - ALLOWANCESB

        // REPORTE 5 - MRLTS
        public ActionResult ReporteMRLTS()
        {
            int pagina = 1106;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = false;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.dperiodo = db.PERIODOes.ToList();
            ViewBag.aperiodo = db.PERIODOes.ToList();

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
            }
            Session["spras"] = user.SPRAS_ID;
            var dOCUMENTOes = db.DOCUMENTOes.Include(d => d.CLIENTE).Include(d => d.CUENTAGL).Include(d => d.CUENTAGL1).Include(d => d.GALL).Include(d => d.PAI).Include(d => d.SOCIEDAD).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.DOCUMENTOSAP);
            return View(dOCUMENTOes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteMRLTS(string Form)
        {
            int pagina = 1106;
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = true;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.cuentagl = db.CUENTAGLs.ToList();
            ViewBag.cuenta = db.CUENTAs.ToList();
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
            }
            Session["spras"] = user.SPRAS_ID;
            // ViewBag.Calendario = cal;
            // ViewBag.Consulta = "";
            // EVALUAR FILTROS
            string[] comcodessplit = new string[] { };
            string comcode = Request["selectcocode"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                comcodessplit = comcode.Split(',');
            }
            int dperiod = Int32.Parse(Request["selectdperiod"]);
            int aperiod = Int32.Parse(Request["selectaperiod"]);
            string year = Request["selectyear"];
            ViewBag.dperiodo = db.PERIODOes.ToList();
            ViewBag.aperiodo = db.PERIODOes.ToList();

            //SELECT d.SOCIEDAD_ID, p.LANDX, d.NUM_DOC, d.FECHAC, d.PERIODO, d.EJERCICIO, d.DOCUMENTO_SAP, tst.TXT020 AS TSOLT_TXT020, d.ESTATUS_SAP, d.CONCEPTO
            //, d.FECHAI_VIG, d.FECHAF_VIG, d.PAYER_ID, c.NAME1, d.TSOL_ID, MONTO_DOC_MD, MONEDA_ID, TIPO_CAMBIO, d.*
            //FROM DOCUMENTO d
            //INNER JOIN DOCUMENTOSAP ds ON d.NUM_DOC = ds.NUM_DOC
            //INNER JOIN PAIS p ON d.PAIS_ID = p.LAND
            //INNER JOIN CLIENTE c ON d.PAYER_ID = c.KUNNR
            //INNER JOIN TSOL ts ON d.TSOL_ID = ts.ID
            //INNER JOIN USUARIO u ON d.USUARIOC_ID = u.ID
            //INNER JOIN TSOLT tst ON ts.ID = tst.TSOL_ID AND u.SPRAS_ID = tst.SPRAS_ID
            //WHERE d.EJERCICIO = 2018 AND d.PERIODO BETWEEN 4 AND 7 AND d.SOCIEDAD_ID IN('KCMX')

            var queryP = (from d in db.DOCUMENTOes
                          join ds in db.DOCUMENTOSAPs on d.NUM_DOC equals ds.NUM_DOC
                          join p in db.PAIS on d.PAIS_ID equals p.LAND
                          join c in db.CLIENTEs on new { d.VKORG, d.VTWEG, d.SPART, d.PAYER_ID } equals new { c.VKORG, c.VTWEG, c.SPART, PAYER_ID = c.KUNNR }
                          join ts in db.TSOLs on d.TSOL_ID equals ts.ID
                          join us in db.USUARIOs on d.USUARIOC_ID equals us.ID
                          join tst in db.TSOLTs on new { TSOL_ID = ts.ID, us.SPRAS_ID } equals new { tst.TSOL_ID, tst.SPRAS_ID }
                          join pt in db.PUESTOTs on new { us.SPRAS_ID, PUESTO_ID = (int)(us.PUESTO_ID) } equals new { pt.SPRAS_ID, pt.PUESTO_ID }
                          orderby d.FECHAC descending // , d.NUM_DOC ascending, f.WF_POS ascending
                          where ((d.PERIODO >= dperiod && d.PERIODO <= aperiod) || (d.PERIODO >= aperiod && d.PERIODO <= dperiod))
                              && d.EJERCICIO == year
                              && (!string.IsNullOrEmpty(comcode) ? comcodessplit.Contains(d.SOCIEDAD_ID) : true)
                          select new MRLTS
                          {
                              CO_CODE = d.SOCIEDAD_ID,
                              PAIS = p.LANDX,
                              NUMERO_SOLICITUD = d.NUM_DOC,
                              FECHA_SOLICITUD = (DateTime)d.FECHAC,
                              PERIODO_CONTABLE = (Int32)d.PERIODO,
                              NUMERO_DOCUMENTO_SAP = d.DOCUMENTO_SAP,
                              //NUMERO_REVERSO_SAP =
                              //FECHA_REVERSO =
                              //PERIODO_CONTABLE_REVERSO =
                              //COMENTARIOS_REVERSO_PROVISION =
                              TIPO_SOLICITUD = tst.TXT020,
                              STATUS = d.ESTATUS_SAP,
                              CONCEPTO_SOLICITUD = d.CONCEPTO,
                              DE = (DateTime)d.FECHAI_VIG,
                              A = (DateTime)d.FECHAF_VIG,
                              //CLASIFICACION = 
                              NUMERO_CLIENTE = d.PAYER_ID,
                              CLIENTE = c.NAME1,
                              MONTO = (decimal)d.MONTO_DOC_MD,
                              MONEDA = d.MONEDA_ID,
                              TIPO_CAMBIO = (decimal)d.TIPO_CAMBIO
                          }).ToList();

            ViewBag.tabla_reporte = queryP;

            //ViewBag.tabla_reporte = queryP.GroupBy(registro => new { registro.NUMERO_SOLICITUD, registro.WF_POS })
            //    .Select(grupo => new
            //    {
            //        trackings = grupo.OrderBy(x => x.FECHA)
            //    })
            //    .Select(grupo_ordenado => new
            //    {
            //        tracking = calcularValoresGrupo(grupo_ordenado.trackings)
            //    }).ToList();
            return View();
        }
        // FIN REPORTE 5 - MRLTS

        // REPORTE 6 - TRACKING TS
        public ActionResult ReporteTrackingTS()
        {
            int pagina = 1107; // ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = false;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.UsuarioF = db.USUARIOs.ToList();
            ViewBag.Cliente = db.CLIENTEs.Select(x => x.KUNNR).ToList();
            ViewBag.Tds = db.TSOLs.ToList();
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
            }
            Session["spras"] = user.SPRAS_ID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteTrackingTS(string Form)
        {
            int pagina = 1107; // ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.display = true;
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.sociedad = db.SOCIEDADs.ToList();
            ViewBag.periodo = db.PERIODOes.ToList();
            ViewBag.UsuarioF = db.USUARIOs.ToList();
            ViewBag.Cliente = db.CLIENTEs.Select(x => x.KUNNR).ToList();
            ViewBag.Tds = db.TSOLs.ToList();
            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".svg";
            }
            catch
            {
            }
            Session["spras"] = user.SPRAS_ID;
            //ViewBag.Calendario = cal;
            //ViewBag.cuentagl = db.CUENTAGLs.ToList();
            //ViewBag.cuenta = db.CUENTAs.ToList();
            //ViewBag.Consulta = "";
            // EVALUAR FILTROS
            string[] comcodessplit = new string[] { };
            string comcode = Request["selectcocode"] as string;
            if (!string.IsNullOrEmpty(comcode))
            {
                comcodessplit = comcode.Split(',');
            }
            int period = Int32.Parse(Request["selectperiod"]);
            string year = Request["selectyear"];
            string usuarioF = (string)Request["selectUsuarioF"];
            string clienteF = (string)Request["selectCliente"];
            string solicitudF = (string)Request["selectTipoSolicitud"];

            var queryP = (from d in db.DOCUMENTOes
                          join f in db.FLUJOes on d.NUM_DOC equals f.NUM_DOC
                          join p in db.PAIS on d.PAIS_ID equals p.LAND
                          join c in db.CLIENTEs on new { d.VKORG, d.VTWEG, d.SPART, d.PAYER_ID } equals new { c.VKORG, c.VTWEG, c.SPART, PAYER_ID = c.KUNNR }
                          join ts in db.TSOLs on d.TSOL_ID equals ts.ID
                          join us in db.USUARIOs on f.USUARIOA_ID equals us.ID
                          join tst in db.TSOLTs on new { TSOL_ID = ts.ID, us.SPRAS_ID } equals new { tst.TSOL_ID, tst.SPRAS_ID }
                          join pt in db.PUESTOTs on new { us.SPRAS_ID, PUESTO_ID = (int)(us.PUESTO_ID) } equals new { pt.SPRAS_ID, pt.PUESTO_ID }
                          join wfp in db.WORKFPs on new { f.WORKF_ID, f.WF_VERSION, f.WF_POS } equals new { WORKF_ID = wfp.ID, WF_VERSION = wfp.VERSION, WF_POS = wfp.POS }
                          join ac in db.ACCIONs on wfp.ACCION_ID equals ac.ID
                          orderby d.FECHAC descending // , d.NUM_DOC ascending, f.WF_POS ascending
                          where d.PERIODO == period
                              && d.EJERCICIO == year
                              && (!string.IsNullOrEmpty(usuarioF) ? d.USUARIOC_ID == usuarioF : true)
                              && (!string.IsNullOrEmpty(solicitudF) ? d.TSOL_ID == solicitudF : true)
                              && (!string.IsNullOrEmpty(clienteF) ? d.PAYER_ID == clienteF : true)
                              && (!string.IsNullOrEmpty(comcode) ? comcodessplit.Contains(d.SOCIEDAD_ID) : true)
                          select new TrackingTS
                          {
                              WF_POS = f.WF_POS,
                              NUMERO_SOLICITUD = d.NUM_DOC,
                              CO_CODE = d.SOCIEDAD_ID,
                              PAIS = p.LANDX,
                              NUMERO_CLIENTE = d.PAYER_ID,
                              CLIENTE = c.NAME1,
                              TIPO_SOLICITUD = tst.TXT50,
                              ESTATUS = f.ESTATUS,
                              ESTATUS_C = d.ESTATUS_C,
                              ESTATUS_SAP = d.ESTATUS_SAP,
                              ESTATUS_WF = d.ESTATUS_WF,
                              TIPO = ac.TIPO,
                              PADRE = ts.PADRE,
                              FECHA = (DateTime)f.FECHAC,
                              PERIODO = (Int32)d.PERIODO,
                              ANIO = d.EJERCICIO,
                              USUARIO = f.USUARIOA_ID,
                              COMENTARIO = f.COMENTARIO,
                              ROL = pt.TXT50
                              //, STATUS_STRING = statusToString(f.ESTATUS, d.ESTATUS_C, d.ESTATUS_SAP, d.ESTATUS_WF, ac.TIPO, ts.PADRE)
                          }).ToList();

            ViewBag.tabla_reporte = queryP.GroupBy(registro => new { registro.NUMERO_SOLICITUD, registro.WF_POS })
                .Select(grupo => new
                {
                    trackings = grupo.OrderBy(x => x.FECHA)
                })
                .Select(grupo_ordenado => new
                {
                    tracking = calcularValoresGrupo(grupo_ordenado.trackings)
                }).ToList();
            return View();
        }

        private TrackingTS calcularValoresGrupo(IOrderedEnumerable<TrackingTS> grupo)
        {
            TrackingTS ultimo = grupo.Last();
            TrackingTS primero = grupo.First();
            ultimo.NUMERO_CORRECCIONES = grupo.Count();
            ultimo.TIEMPO_TRANSCURRIDO = (ultimo.FECHA - primero.FECHA).TotalHours; // ToDo: Restar sábados, domingos y días feriados
            ultimo.SEMANA = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(primero.FECHA, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return ultimo;
        }
        // FIN REPORTE 6 - TRACKING TS

    }
}
