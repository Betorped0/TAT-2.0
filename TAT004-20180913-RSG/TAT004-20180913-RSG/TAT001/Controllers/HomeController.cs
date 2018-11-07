using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TAT001.Entities;
using TAT001.Models;
using ClosedXML.Excel;
using TAT001.Services;
using System.Web.Script.Serialization;
using static TAT001.Models.ReportesModel;

namespace TAT001.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Solicitudes
        [Authorize]
        public ActionResult Index(string id)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                int pagina = 101; //ID EN BASE DE DATOS
                string u = User.Identity.Name;
                ////if (pais != null)
                ////    Session["pais"] = pais;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user;
                ViewBag.returnUrl = Request.Url.PathAndQuery;
                ViewBag.nivelUsuario = user.PUESTO_ID;
                ViewBag.sociedadesUsuario = user.SOCIEDADs;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                List<ReportesModel.Concentrado> reporte = new List<Concentrado>();
                if (user.PUESTO_ID == 14)
                {
                    pagina = 1101;
                    //}
                    db.Configuration.LazyLoadingEnabled = false;
                    List<DOCUMENTO> documentoS = db.DOCUMENTOes
                    //.Where(d => comcodessplit.Contains(d.SOCIEDAD_ID) && periodsplit.Contains(d.PERIODO.ToString()) && yearsplit.Contains(d.EJERCICIO))
                    .Include(d => d.CLIENTE)
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
                    .Include(d => d.DOCUMENTOR)
                    .Include(d => d.DOCUMENTOPs)
                    .Include(d => d.DOCUMENTORECs)
                    .Include(d => d.DOCUMENTOTS)
                    .Include(d => d.FLUJOes).ToList();
                    var cuentass = (from C in db.CUENTAs
                                   join cgl in db.CUENTAGLs on C.CARGO equals cgl.ID
                                   //where C.SOCIEDAD_ID == dOCUMENTO.SOCIEDAD_ID
                                   //& C.PAIS_ID == dOCUMENTO.PAIS_ID
                                   //& C.TALL_ID == dOCUMENTO.TALL_ID
                                   //& C.EJERCICIO.ToString() == dOCUMENTO.EJERCICIO
                                   select new {C.SOCIEDAD_ID, C.PAIS_ID, C.TALL_ID, C.EJERCICIO, C.ABONO, C.CARGO, C.CLEARING, C.LIMITE, cgl.NOMBRE }).ToList();
                    foreach (var pais in user.SOCIEDADs)
                    {
                        List<DOCUMENTO> documentos = new List<DOCUMENTO>();
                        if (!string.IsNullOrEmpty(pais.ToString()))
                        {
                            documentos = documentoS.Where(d => d.SOCIEDAD_ID.Equals(pais.BUKRS)).ToList();
                        }

                        foreach (DOCUMENTO dOCUMENTO in documentos)
                        {
                            Concentrado r1 = new Concentrado();
                            r1.documento = dOCUMENTO;
                            if (dOCUMENTO.PAYER_ID == null) continue;
                            //dOCUMENTO.CLIENTE = db.CLIENTEs.Where(a => a.VKORG.Equals(dOCUMENTO.VKORG)
                            //                                        & a.VTWEG.Equals(dOCUMENTO.VTWEG)
                            //                                        & a.SPART.Equals(dOCUMENTO.SPART)
                            //                                        & a.KUNNR.Equals(dOCUMENTO.PAYER_ID)).First();
                            r1.CANAL = db.CANALs.Where(a => a.CANAL1.Equals(dOCUMENTO.CLIENTE.CANAL)).FirstOrDefault();
                            //var cuentas = (from C in db.CUENTAs
                            //               join cgl in db.CUENTAGLs on C.CARGO equals cgl.ID
                            var cuentas = (from C in cuentass
                                           where C.SOCIEDAD_ID == dOCUMENTO.SOCIEDAD_ID
                                           & C.PAIS_ID == dOCUMENTO.PAIS_ID
                                           & C.TALL_ID == dOCUMENTO.TALL_ID
                                           & C.EJERCICIO.ToString() == dOCUMENTO.EJERCICIO
                                           select new { C.ABONO, C.CARGO, C.CLEARING, C.LIMITE, C.NOMBRE }).FirstOrDefault();
                            if (cuentas != null)
                            {
                                r1.CUENTA_ABONO = Convert.ToDecimal(cuentas.GetType().GetProperty("ABONO").GetValue(cuentas, null)); // Convertir a decimal por si es null
                                r1.CUENTA_CARGO = Convert.ToDecimal(cuentas.GetType().GetProperty("CARGO").GetValue(cuentas, null));
                                r1.CUENTA_CLEARING = Convert.ToDecimal(cuentas.GetType().GetProperty("CLEARING").GetValue(cuentas, null));
                                r1.CUENTA_LIMITE = Convert.ToDecimal(cuentas.GetType().GetProperty("LIMITE").GetValue(cuentas, null));
                                r1.CUENTA_CARGO_NOMBRE = cuentas.GetType().GetProperty("NOMBRE").GetValue(cuentas, null).ToString();
                            }
                            var proveedor = dOCUMENTO.DOCUMENTOFs.Select(df => df.PROVEEDOR).FirstOrDefault();
                            r1.PROVEEDOR_NOMBRE = db.PROVEEDORs.Where(x => x.ID.Equals(proveedor)).Select(p => p.NOMBRE).FirstOrDefault();

                            //r1.SEMANA = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear((DateTime)r1.documento.FECHAC, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            r1.SEMANA = (((DateTime)r1.documento.FECHAC).Day + ((int)((DateTime)r1.documento.FECHAC).DayOfWeek)) / 7 + 1;

                            r1.STATUS = dOCUMENTO.ESTATUS_WF;
                            r1.STATUSS1 = (dOCUMENTO.ESTATUS ?? " ") + (dOCUMENTO.ESTATUS_C ?? " ") + (dOCUMENTO.ESTATUS_SAP ?? " ") + (dOCUMENTO.ESTATUS_WF ?? " ");
                            r1.STATUSS3 = (dOCUMENTO.TSOL.PADRE ? "P" : " ");
                            //var queryFlujo = (from f in db.FLUJOes
                            var queryFlujo = (from f in dOCUMENTO.FLUJOes
                                              join wfp in db.WORKFPs on new { f.WORKF_ID, f.WF_VERSION, f.WF_POS } equals new { WORKF_ID = wfp.ID, WF_VERSION = wfp.VERSION, WF_POS = wfp.POS }
                                              join ac in db.ACCIONs on wfp.ACCION_ID equals ac.ID
                                              orderby f.POS descending
                                              where f.NUM_DOC == dOCUMENTO.NUM_DOC
                                              select ac.TIPO
                                              ).FirstOrDefault();
                            r1.STATUSS2 = ((queryFlujo == null) ? " " : queryFlujo.ToString());
                            //r1.STATUSS4 = ((from dr in db.DOCUMENTORECs
                            r1.STATUSS4 = ((from dr in dOCUMENTO.DOCUMENTORECs
                                            where dr.NUM_DOC == dOCUMENTO.NUM_DOC
                                            select dr.NUM_DOC
                                              ).ToList().Count > 0 ? "R" : " ");

                            string estatuss = r1.STATUSS1 + r1.STATUSS2 + r1.STATUSS3 + r1.STATUSS4;
                            if (r1.STATUS == "R")
                            {
                                r1.STATUSS = estatuss.Substring(0, 6);
                                r1.STATUSS += db.USUARIOs.Where(y => y.ID == db.FLUJOes.Where(x => x.NUM_DOC == dOCUMENTO.NUM_DOC & x.ESTATUS == "R").OrderByDescending(a => a.POS).FirstOrDefault().USUARIOA_ID).FirstOrDefault().PUESTO_ID.ToString();
                                r1.STATUSS += estatuss.Substring(6, 1);
                            }
                            else
                            {
                                r1.STATUSS = estatuss.Substring(0, 6) + " " + estatuss.Substring(6, 1); ;
                            }
                            Estatus e = new Estatus();
                            r1.ESTATUS_STRING = e.getText(r1.STATUSS, dOCUMENTO.NUM_DOC);

                            r1.DOCSREFREVERSOS = (from d in db.DOCUMENTOes
                                                  join dr in db.DOCUMENTORs on d.NUM_DOC equals dr.NUM_DOC
                                                  join tr in db.TREVERSATs on dr.TREVERSA_ID equals tr.TREVERSA_ID
                                                  where tr.SPRAS_ID == user.SPRAS_ID
                                                  where d.DOCUMENTO_REF == r1.documento.NUM_DOC
                                                  select new { d, dr, tr }).FirstOrDefault();

                            reporte.Add(r1);
                        }
                    }
                    ViewBag.lista_reporte = reporte;
                }//ADD RSG 07.11.2018
                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".png";
                    PAI pa = db.PAIS.Find(p);
                    if (pa != null)
                    {
                        ViewBag.miles = pa.MILES;//LEJGG 090718
                        ViewBag.dec = pa.DECIMAL;//LEJGG 090718
                    }
                }
                catch
                {
                    //ViewBag.pais = "mx.png";
                    ////return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;

                try//Mensaje de documento creado
                {
                    string p = Session["NUM_DOC"].ToString();
                    ViewBag.NUM_DOC = p;
                    Session["NUM_DOC"] = null;
                }
                catch
                {
                    ViewBag.NUM_DOC = "";
                }

                try//Mensaje de documento creado
                {
                    string error_files = Session["ERROR_FILES"].ToString();
                    ViewBag.ERROR_FILES = error_files;
                    Session["ERROR_FILES"] = null;
                }
                catch
                {
                    ViewBag.ERROR_FILES = "";
                }


                string us = "";
                DateTime fecha = DateTime.Now.Date;
                List<TAT001.Entities.DELEGAR> del = db.DELEGARs.Where(a => a.USUARIOD_ID.Equals(User.Identity.Name) & a.FECHAI <= fecha & a.FECHAF >= fecha & a.ACTIVO == true).ToList();

                if (del.Count > 0)
                {
                    List<USUARIO> users = new List<USUARIO>();
                    foreach (DELEGAR de in del)
                    {
                        users.Add(de.USUARIO);
                    }
                    users.Add(ViewBag.usuario);
                    ViewBag.delegados = users.ToList();

                    if (id != null)
                        us = id;
                    else
                        us = User.Identity.Name;
                    ViewBag.usuariod = us;
                }
                else
                    us = User.Identity.Name;

                List<CSP_DOCUMENTOSXUSER_Result> dOCUMENTOes = db.CSP_DOCUMENTOSXUSER(us, user.SPRAS_ID).ToList();

                //dOCUMENTOes = dOCUMENTOes.Distinct(new DocumentoComparer()).ToList();
                //dOCUMENTOes = dOCUMENTOes.OrderByDescending(a => a.FECHAC).OrderByDescending(a => a.NUM_DOC).ToList();
                //ViewBag.Clientes = db.CLIENTEs.ToList();
                //ViewBag.Cuentas = db.CUENTAs.ToList();
                //ViewBag.DOCF = db.DOCUMENTOFs.ToList();
                //jemo inicio 4/07/2018
                ViewBag.imgnoticia = db.NOTICIAs.Where(x => x.FECHAI <= DateTime.Now && x.FECHAF >= DateTime.Now && x.ACTIVO == true).Select(x => x.PATH).FirstOrDefault();


                List<Documento> listaDocs = new List<Documento>();
                foreach (CSP_DOCUMENTOSXUSER_Result item in dOCUMENTOes)
                {
                    Documento ld = new Documento();
                    ld.BUTTON = item.BUTTON;

                    ld.NUM_DOC = item.NUM_DOC;
                    ld.NUM_DOC_TEXT = item.NUM_DOC_TEXT;
                    ld.SOCIEDAD_ID = item.SOCIEDAD_ID;
                    ld.PAIS_ID = item.PAIS_ID;
                    ld.FECHADD = item.FECHAD.Value.Day + "/" + item.FECHAD.Value.Month + "/" + item.FECHAD.Value.Year;
                    ld.FECHAD = item.FECHAD.Value.Year + "/" + item.FECHAD.Value.Month + "/" + item.FECHAD.Value.Day;
                    ld.HORAC = item.HORAC.Value.ToString().Split('.')[0];
                    ld.PERIODO = item.PERIODO + "";

                    if (item.ESTATUS == "R")
                    {
                        FLUJO flujo = db.FLUJOes.Include("USUARIO").Where(x => x.NUM_DOC == item.NUM_DOC & x.ESTATUS == "R").OrderByDescending(a => a.POS).FirstOrDefault();
                        item.ESTATUSS = item.ESTATUSS.Substring(0, 6) +
                                        (flujo.USUARIO != null ? flujo.USUARIO.PUESTO_ID.ToString() : "") +
                                        item.ESTATUSS.Substring(6, 2);
                    }
                    else
                    {
                        item.ESTATUSS = item.ESTATUSS.Substring(0, 6) + " " + item.ESTATUSS.Substring(6, 2); ;
                    }
                    Estatus e = new Estatus();
                    ld.ESTATUS = e.getText(item.ESTATUSS, ld.NUM_DOC);
                    ld.ESTATUS_CLASS = e.getClass(item.ESTATUSS, ld.NUM_DOC);
                    //ld.ESTATUS = e.getText(item.ESTATUSS);
                    //ld.ESTATUS_CLASS = e.getClass(item.ESTATUSS);

                    ld.PAYER_ID = item.PAYER_ID;

                    ld.CLIENTE = item.NAME1;
                    ld.CANAL = item.CANAL;
                    ld.TSOL = item.TXT020;
                    ld.TALL = item.TXT50;
                    //foreach (CUENTA cuenta in db.CUENTAs.Where(x => x.SOCIEDAD_ID.Equals(item.SOCIEDAD_ID)).ToList())
                    //{
                    //    if (item.TALL != null)
                    //    {
                    //        if (cuenta.TALL_ID == item.TALL.ID)
                    //        {
                    //            ld.CUENTAS = cuenta.CARGO.ToString();
                    //            break;
                    //        }
                    //    }
                    //}
                    try
                    {
                        ld.CUENTAS = item.CARGO + "";
                        ld.CUENTAP = item.CUENTAP;
                        ld.CUENTAPL = item.CUENTAPL;
                        ld.CUENTACL = item.CUENTACL;
                    }
                    catch { }
                    ld.CONCEPTO = item.CONCEPTO;
                    ld.MONTO_DOC_ML = item.MONTO_DOC_MD != null ? Convert.ToDecimal(item.MONTO_DOC_MD).ToString("C") : "";

                    ld.FACTURA = item.FACTURA;
                    ld.FACTURAK = item.FACTURAK;

                    ld.USUARIOC_ID = item.USUARIOD_ID;
                    ld.USUARIOM_ID = item.USUARIOD_ID;

                    if (item.DOCUMENTO_SAP != null)
                    {
                        if (item.PADRE)
                        {
                            ld.NUM_PRO = item.DOCUMENTO_SAP;
                            ld.NUM_AP = "";
                            ld.NUM_NC = "";
                            ld.NUM_REV = "";
                        }
                        else if (item.REVERSO)
                        {
                            ld.NUM_REV = item.DOCUMENTO_SAP;
                            ld.NUM_AP = "";
                            ld.NUM_NC = "";
                            ld.NUM_PRO = "";
                        }
                        else
                        {
                            ld.NUM_NC = item.DOCUMENTO_SAP;
                            ld.NUM_AP = "";
                            ld.NUM_PRO = "";
                            ld.NUM_REV = "";
                        }
                        //<!--NUM_SAP-->
                        ld.BLART = item.BLART;
                        ld.NUM_PAYER = item.KUNNR;
                        ld.NUM_CLIENTE = item.DESCR;
                        ld.NUM_IMPORTE = item.IMPORTE != null ? Convert.ToDecimal(item.IMPORTE).ToString("C") : "";
                        ld.NUM_CUENTA = item.CUENTA_C;
                    }
                    else
                    {
                        //<td></td>
                        //<td></td>
                        //<td></td>
                        //<td></td>
                        //<td></td>
                        //<td></td>
                        //<td></td>
                        //<td></td>
                        //<td></td>
                    }

                    listaDocs.Add(ld);
                }
                return View(listaDocs);

            }
        }
        [HttpGet]
        public ActionResult SelPais(string pais, string returnUrl, string sociedad_id)
        {
            Session["pais"] = pais.ToUpper();
            Session["sociedad_id"] = sociedad_id;
            if (!string.IsNullOrEmpty(returnUrl))
            {

                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult About()
        {
            return View();
        }

        [Authorize]
        public ActionResult Pais(string returnUrl)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                int pagina = 102; //ID EN BASE DE DATOS
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery; ;
                //ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.flag = true;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();


                var p = (from P in db.PAIS.ToList()
                         join C in db.CLIENTEs.Where(x => x.ACTIVO == true).ToList()
                         on P.LAND equals C.LAND
                         join U in db.USUARIOFs.Where(x => x.USUARIO_ID == u & x.ACTIVO == true)
                         on new { C.VKORG, C.VTWEG, C.SPART, C.KUNNR } equals new { U.VKORG, U.VTWEG, U.SPART, U.KUNNR }
                         where P.ACTIVO == true
                         select P).DistinctBy(x => x.LAND);

                List<Delegados> delegados = new List<Delegados>();
                DateTime fecha = DateTime.Now.Date;
                List<TAT001.Entities.DELEGAR> del = db.DELEGARs.Where(a => a.USUARIOD_ID.Equals(User.Identity.Name) & a.FECHAI <= fecha & a.FECHAF >= fecha & a.ACTIVO == true).ToList();
                //List<TAT001.Entities.DELEGAR> del = db.DELEGARs.Where(a => a.USUARIOD_ID.Equals(u)).ToList();
                foreach (DELEGAR de in del)
                {

                    var pd = (from P in db.PAIS.ToList()
                              join C in db.CLIENTEs.Where(x => x.ACTIVO == true).ToList()
                              on P.LAND equals C.LAND
                              join U in db.USUARIOFs.Where(x => x.USUARIO_ID == de.USUARIO_ID & x.ACTIVO == true)
                              on new { C.VKORG, C.VTWEG, C.SPART, C.KUNNR } equals new { U.VKORG, U.VTWEG, U.SPART, U.KUNNR }
                              where P.ACTIVO == true
                              select P).DistinctBy(x => x.LAND).ToList();

                    Delegados delegado = new Delegados();
                    delegado.usuario = de.USUARIO_ID;
                    delegado.nombre = de.USUARIO.NOMBRE + " " + de.USUARIO.APELLIDO_P + " " + de.USUARIO.APELLIDO_M;
                    delegado.LISTA = pd;
                    if (delegado.LISTA.Count > 0)
                        delegados.Add(delegado);
                }
                if (delegados.Count > 0)
                    ViewBag.delegados = delegados;

                ViewBag.returnUrl = returnUrl;


                return View(p.ToList());
            }
            //return View();

        }

        [HttpPost]
        public FileResult Descargar()
        {
            //var dOCUMENTOes = db.DOCUMENTOes.Where(a => a.USUARIOC_ID.Equals(User.Identity.Name)).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.CLIENTE).Include(d => d.PAI).Include(d => d.SOCIEDAD).ToList();
            //var dOCUMENTOVs = db.DOCUMENTOVs.Where(a => a.USUARIOA_ID.Equals(User.Identity.Name)).ToList();
            //var tsol = db.TSOLs.ToList();
            //var tall = db.TALLs.ToList();
            //foreach (DOCUMENTOV v in dOCUMENTOVs)
            //{
            //    DOCUMENTO d = new DOCUMENTO();
            //    var ppd = d.GetType().GetProperties();
            //    var ppv = v.GetType().GetProperties();
            //    foreach (var pv in ppv)
            //    {
            //        foreach (var pd in ppd)
            //        {
            //            if (pd.Name == pv.Name)
            //            {
            //                pd.SetValue(d, pv.GetValue(v));
            //                break;
            //            }
            //        }
            //    }
            //    d.TSOL = tsol.Where(a => a.ID.Equals(d.TSOL_ID)).FirstOrDefault();
            //    d.TALL = tall.Where(a => a.ID.Equals(d.TALL_ID)).FirstOrDefault();
            //    dOCUMENTOes.Add(d);
            //}
            //dOCUMENTOes = dOCUMENTOes.Distinct(new DocumentoComparer()).ToList();
            //dOCUMENTOes = dOCUMENTOes.OrderByDescending(a => a.FECHAC).OrderByDescending(a => a.NUM_DOC).ToList();
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

            string us = "";
            us = User.Identity.Name;

            List<CSP_DOCUMENTOSXUSER_Result> dOCUMENTOes = db.CSP_DOCUMENTOSXUSER(us, user.SPRAS_ID).ToList();

            List<Documento> listaDocs = new List<Documento>();
            foreach (CSP_DOCUMENTOSXUSER_Result item in dOCUMENTOes)
            {
                Documento ld = new Documento();
                ld.BUTTON = item.BUTTON;

                ld.NUM_DOC = item.NUM_DOC;
                ld.NUM_DOC_TEXT = item.NUM_DOC_TEXT;
                ld.SOCIEDAD_ID = item.SOCIEDAD_ID;
                ld.PAIS_ID = item.PAIS_ID;
                ld.FECHADD = item.FECHAD.Value.Day + "/" + item.FECHAD.Value.Month + "/" + item.FECHAD.Value.Year;
                ld.FECHAD = item.FECHAD.Value.Year + "/" + item.FECHAD.Value.Month + "/" + item.FECHAD.Value.Day;
                ld.HORAC = item.HORAC.Value.ToString().Split('.')[0];
                ld.PERIODO = item.PERIODO + "";

                if (item.ESTATUS == "R")
                {
                    FLUJO flujo = db.FLUJOes.Include("USUARIO").Where(x => x.NUM_DOC == item.NUM_DOC & x.ESTATUS == "R").OrderByDescending(a => a.POS).FirstOrDefault();
                    item.ESTATUSS = item.ESTATUSS.Substring(0, 6) +
                                    (flujo.USUARIO != null ? flujo.USUARIO.PUESTO_ID.ToString() : "") +
                                    item.ESTATUSS.Substring(6, 2);
                }
                else
                {
                    item.ESTATUSS = item.ESTATUSS.Substring(0, 6) + " " + item.ESTATUSS.Substring(6, 2); ;
                }
                Estatus e = new Estatus();
                ld.ESTATUS = e.getText(item.ESTATUSS, ld.NUM_DOC);
                ld.ESTATUS_CLASS = e.getClass(item.ESTATUSS, ld.NUM_DOC);
                //ld.ESTATUS = e.getText(item.ESTATUSS);
                //ld.ESTATUS_CLASS = e.getClass(item.ESTATUSS);

                ld.PAYER_ID = item.PAYER_ID;

                ld.CLIENTE = item.NAME1;
                ld.CANAL = item.CANAL;
                ld.TSOL = item.TXT020;
                ld.TALL = item.TXT50;
                //foreach (CUENTA cuenta in db.CUENTAs.Where(x => x.SOCIEDAD_ID.Equals(item.SOCIEDAD_ID)).ToList())
                //{
                //    if (item.TALL != null)
                //    {
                //        if (cuenta.TALL_ID == item.TALL.ID)
                //        {
                //            ld.CUENTAS = cuenta.CARGO.ToString();
                //            break;
                //        }
                //    }
                //}
                try
                {
                    ld.CUENTAS = item.CARGO + "";
                    ld.CUENTAP = item.CUENTAP;
                    ld.CUENTAPL = item.CUENTAPL;
                    ld.CUENTACL = item.CUENTACL;
                }
                catch { }
                ld.CONCEPTO = item.CONCEPTO;
                ld.MONTO_DOC_ML = item.MONTO_DOC_MD != null ? Convert.ToDecimal(item.MONTO_DOC_MD).ToString("C") : "";

                ld.FACTURA = item.FACTURA;
                ld.FACTURAK = item.FACTURAK;

                ld.USUARIOC_ID = item.USUARIOD_ID;
                ld.USUARIOM_ID = item.USUARIOD_ID;

                if (item.DOCUMENTO_SAP != null)
                {
                    if (item.PADRE)
                    {
                        ld.NUM_PRO = item.DOCUMENTO_SAP;
                        ld.NUM_AP = "";
                        ld.NUM_NC = "";
                        ld.NUM_REV = "";
                    }
                    else if (item.REVERSO)
                    {
                        ld.NUM_REV = item.DOCUMENTO_SAP;
                        ld.NUM_AP = "";
                        ld.NUM_NC = "";
                        ld.NUM_PRO = "";
                    }
                    else
                    {
                        ld.NUM_NC = item.DOCUMENTO_SAP;
                        ld.NUM_AP = "";
                        ld.NUM_PRO = "";
                        ld.NUM_REV = "";
                    }
                    //<!--NUM_SAP-->
                    ld.BLART = item.BLART;
                    ld.NUM_PAYER = item.KUNNR;
                    ld.NUM_CLIENTE = item.DESCR;
                    ld.NUM_IMPORTE = item.IMPORTE != null ? Convert.ToDecimal(item.IMPORTE).ToString("C") : "";
                    ld.NUM_CUENTA = item.CUENTA_C;
                }
                else
                {
                    //<td></td>
                    //<td></td>
                    //<td></td>
                    //<td></td>
                    //<td></td>
                    //<td></td>
                    //<td></td>
                    //<td></td>
                    //<td></td>
                }

                listaDocs.Add(ld);
            }
            generarExcelHome(listaDocs.OrderByDescending(t => t.FECHAD).ThenByDescending(t => t.HORAC).ThenByDescending(t => t.NUM_DOC).ToList(), Server.MapPath("~/pdfTemp/"));
            return File(Server.MapPath("~/pdfTemp/Doc" + DateTime.Now.ToShortDateString() + ".xlsx"), "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doc" + DateTime.Now.ToShortDateString() + ".xlsx");
        }

        public void generarExcelHome(List<Documento> lst, string ruta)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            try
            {
                //Creamos el encabezado
                worksheet.Cell("A1").Value = new[]
             {
                  new {
                      BANNER = "Número Solicitud"
                      },
                    };
                worksheet.Cell("B1").Value = new[]
            {
                  new {
                      BANNER = "Company Code"
                      },
                    };
                worksheet.Cell("C1").Value = new[]
            {
                  new {
                      BANNER = "País"
                      },
                    };
                worksheet.Cell("D1").Value = new[]
            {
                  new {
                      BANNER = "Fecha Solicitud"
                      },
                    };
                worksheet.Cell("E1").Value = new[]
            {
                  new {
                      BANNER = "Hora de Solicitud"
                      },
                    };
                worksheet.Cell("F1").Value = new[]
            {
                  new {
                      BANNER = "Período Contable"
                      },
                    };
                worksheet.Cell("G1").Value = new[]
              {
                  new {
                      BANNER = "Estatus"
                      },
                    };
                worksheet.Cell("H1").Value = new[]
            {
                  new {
                      BANNER = "Cliente ID"
                      },
                    };
                worksheet.Cell("I1").Value = new[]
            {
                  new {
                      BANNER = "Cliente"
                      },
                    };
                worksheet.Cell("J1").Value = new[]
                {
                    new {
                        BANNER = "Canal"
                    },
                };
                worksheet.Cell("K1").Value = new[]
            {
                  new {
                      BANNER = "Tipo Solicitud"
                      },
                    };
                worksheet.Cell("L1").Value = new[]
            {
                  new {
                      BANNER = "Clasificación Allowances"
                      },
                    };
                worksheet.Cell("M1").Value = new[]
      {
                  new {
                      BANNER = "Cuenta Contable Gasto"
                      },
                    };
                worksheet.Cell("N1").Value = new[]
{
                  new {
                      BANNER = "Cuenta Contable Pasivo"
                      },
                    };
                worksheet.Cell("O1").Value = new[]
                {
                  new {
                      BANNER = "Cuenta Contable Clearing"
                      },
                    };
                worksheet.Cell("P1").Value = new[]
{
                  new {
                      BANNER = "Descripción Solicitud"
                      },
                    };
                worksheet.Cell("Q1").Value = new[]
      {
                  new {
                      BANNER = "$ Importe Solicitud"
                      },
                    };
                worksheet.Cell("R1").Value = new[]
      {
                  new {
                      BANNER = "Número Factura Proveedor"
                      },
                    };
                worksheet.Cell("S1").Value = new[]
      {
                  new {
                      BANNER = "Número Factura Kellogg"
                      },
                    };
                worksheet.Cell("T1").Value = new[]
                {
                  new {
                      BANNER = "Creado por"
                      },
                    };
                worksheet.Cell("U1").Value = new[]
     {
                  new {
                      BANNER = "Modificado por"
                      },
                    };
                worksheet.Cell("V1").Value = new[]
      {
                  new {
                      BANNER = "Núm. Registro Provisión"
                      },
                    };
                worksheet.Cell("W1").Value = new[]
     {
                  new {
                      BANNER = "Núm. Registro NC/OP"
                      },
                    };
                worksheet.Cell("X1").Value = new[]
  {
                  new {
                      BANNER = "Núm. Registro AP"
                      },
                    };
                worksheet.Cell("Y1").Value = new[]
                {
                  new {
                      BANNER = "Núm. Registro Reverso"
                      },
                    };
                worksheet.Cell("Z1").Value = new[]
                {
                  new {
                      BANNER = "Tipo Registro SAP"
                      },
                    };
                worksheet.Cell("AA1").Value = new[]
                {
                  new {
                      BANNER = "Cliente ID"
                      },
                    };
                worksheet.Cell("AB1").Value = new[]
               {
                  new {
                      BANNER = "Cliente"
                      },
                    };
                worksheet.Cell("AC1").Value = new[]
              {
                  new {
                      BANNER = "$ Importe Moneda Local"
                      },
                    };
                worksheet.Cell("AD1").Value = new[]
              {
                  new {
                      BANNER = "Cuenta Contable Gasto"
                      },
                    };
                for (int i = 2; i <= (lst.Count + 1); i++)
                {
                    worksheet.Cell("A" + i).Value = new[]
                 {
                  new {
                      BANNER       = lst[i-2].NUM_DOC
                      },
                    };
                    worksheet.Cell("B" + i).Value = new[]
                {
                  new {
                      BANNER       = lst[i-2].SOCIEDAD_ID
                      },
                    };
                    worksheet.Cell("C" + i).Value = new[]
                 {
                    new {
                        BANNER       = lst[i-2].PAIS_ID
                        },
                      };
                    worksheet.Cell("D" + i).Value = new[]
                  {
                   new {
                       BANNER       = lst[i-2].FECHAD
                       },
                     };
                    worksheet.Cell("E" + i).Value = new[]
               {
                  new {
                      BANNER       = lst[i-2].HORAC.ToString().Split('.')[0]
                      },
                    };
                    var fx = lst[i - 2].PERIODO;
                    worksheet.Cell("F" + i).Value = new[]
                 {
                  new {
                      BANNER       = fx
                      },
                    };
                    //Verdes
                    if (lst[i - 2].ESTATUS_CLASS == "new badge green darken-1 white-text " || lst[i - 2].ESTATUS_CLASS == "lbl_txt new badge green darken-1 white-text" || lst[i - 2].ESTATUS_CLASS == "lbl_rev new badge green darken-1 white-text")
                    {
                        worksheet.Cell("G" + i).Value = new[]
                    {
                        new
                        {
                            BANNER = lst[i - 2].ESTATUS
                    },
                };
                        worksheet.Range("G" + i + ":G" + i).Style.Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#43A047")).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true);
                    }//grises
                    else if (lst[i - 2].ESTATUS_CLASS == "new badge grey darken-2 white-text")
                    {
                        worksheet.Cell("G" + i).Value = new[]
              {
                        new
                        {
                            BANNER = lst[i - 2].ESTATUS
                        },
                };
                        worksheet.Range("G" + i + ":G" + i).Style.Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#616161")).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true);
                    }
                    else if (lst[i - 2].ESTATUS_CLASS == "new badge yellow darken-1 white-text " || lst[i - 2].ESTATUS_CLASS == "lbl_ts yellow darken-2 white-text new badge" || lst[i - 2].ESTATUS_CLASS == "lbl_soporte new badge yellow darken-2 white-text ")
                    {
                        worksheet.Cell("G" + i).Value = new[]
              {
                        new
                        {
                            BANNER = lst[i - 2].ESTATUS
                        },
                };
                        worksheet.Range("G" + i + ":G" + i).Style.Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#fbc02d")).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true);
                    }//naranjas
                    else if (lst[i - 2].ESTATUS_CLASS == "new badge orange darken-1 white-text " || lst[i - 2].ESTATUS_CLASS == "lbl_borr new badge orange darken-2 white-text")
                    {
                        worksheet.Cell("G" + i).Value = new[]
              {
                        new
                        {
                            BANNER = lst[i - 2].ESTATUS
                        },
                };
                        worksheet.Range("G" + i + ":G" + i).Style.Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#f57c00")).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true);
                    }//Rojos
                    else if (lst[i - 2].ESTATUS_CLASS == "new badge red darken-1 white-text " || lst[i - 2].ESTATUS_CLASS == "lbl_txt new badge red darken-1 white-text " || lst[i - 2].ESTATUS_CLASS == "lbl_cancelled new badge red darken-1 white-text")
                    {
                        worksheet.Cell("G" + i).Value = new[]
                    {
                        new
                        {
                            BANNER = lst[i - 2].ESTATUS
                        },
                };
                        worksheet.Range("G" + i + ":G" + i).Style.Font.SetFontColor(XLColor.White).Fill.SetBackgroundColor(XLColor.FromHtml("#E53935")).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center).Font.SetBold(true);
                    }
                    worksheet.Cell("H" + i).Value = new[]
                {
                    new {
                        BANNER       = lst[i-2].PAYER_ID
                        },
                      };
                    //Para sacar el Nombre "I" y "J"
                    worksheet.Cell("I" + i).Value = new[]
                    {
                                //List Clientes y for para sacar su name 
                                new {
                                    BANNER = lst[i-2].CLIENTE
                                },
                            };

                    worksheet.Cell("J" + i).Value = new[]
                    {
                                //   List Clientes y for para sacar su name
                                new
                                {
                                    BANNER = lst[i-2].CANAL
                                },
                            };
                    worksheet.Cell("K" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].TSOL
                        },
                    };
                    worksheet.Cell("L" + i).Value = new[]
                   {
                        new {
                            BANNER       = lst[i-2].TALL
                        },
                    };

                    worksheet.Cell("M" + i).Value = new[]
                    {
                                new {
                                    BANNER       = lst[i-2].CUENTAP
                                },
                            };
                    worksheet.Cell("N" + i).Value = new[]
                            {
                                new {
                                    BANNER       = lst[i-2].CUENTAPL
                                },
                            };
                    worksheet.Cell("O" + i).Value = new[]
                            {
                                new {
                                    BANNER       = lst[i-2].CUENTACL
                                },
                            };

                    worksheet.Cell("P" + i).Value = new[]
                   {
                        new {
                            BANNER       = lst[i-2].CONCEPTO
                        },
                    };
                    worksheet.Cell("Q" + i).Value = new[]
                   {
                        new {
                            BANNER       = lst[i-2].MONTO_DOC_ML
                        },
                    };
                    worksheet.Cell("R" + i).Value = new[]
                    {
                                new {
                                    BANNER       =lst[i-2].FACTURA
                                },
                            };

                    worksheet.Cell("S" + i).Value = new[]
                    {
                                new {
                                    BANNER       = lst[i-2].FACTURAK
                                },
                            };

                    worksheet.Cell("T" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].USUARIOC_ID
                        },
                    };
                    worksheet.Cell("U" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].USUARIOC_ID
                        },
                    };
                    worksheet.Cell("V" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_PRO
                        },
                    };
                    worksheet.Cell("W" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_NC
                        },
                    };
                    worksheet.Cell("X" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_AP
                        },
                    };
                    worksheet.Cell("Y" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_REV
                        },
                    };
                    worksheet.Cell("Y" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_REV
                        },
                    };
                    worksheet.Cell("Z" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].BLART
                        },
                    };
                    worksheet.Cell("AA" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_PAYER
                        },
                    };
                    worksheet.Cell("AB" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_CLIENTE
                        },
                    };
                    worksheet.Cell("AC" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_IMPORTE
                        },
                    };
                    worksheet.Cell("AD" + i).Value = new[]
                    {
                        new {
                            BANNER       = lst[i-2].NUM_CUENTA
                        },
                    };
                }
                var rt = ruta + @"\Doc" + DateTime.Now.ToShortDateString() + ".xlsx";
                workbook.SaveAs(rt);
            }
            catch (Exception e)
            {
                var ex = e.ToString();
            }

        }

        [HttpGet]
        public JsonResult Clientes(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from N in db.CLIENTEs
                     where N.KUNNR.Contains(Prefix)
                     select new { N.KUNNR, N.NAME1 });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
    }
}
