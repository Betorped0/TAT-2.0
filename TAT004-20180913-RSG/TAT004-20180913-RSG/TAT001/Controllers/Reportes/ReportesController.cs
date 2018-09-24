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
    }
}
