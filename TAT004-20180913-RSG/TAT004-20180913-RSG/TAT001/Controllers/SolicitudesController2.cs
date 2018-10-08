﻿using ExcelDataReader;
using Newtonsoft.Json;
using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;

namespace TAT001.Controllers
{
    [Authorize]
    public class SolicitudesController2 : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Solicitudes
        public ActionResult Index()
        {
            int pagina = 201; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
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
                    //ViewBag.pais = "mx.png";
                    ////return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }

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



            var dOCUMENTOes = db.DOCUMENTOes.Where(a => a.USUARIOC_ID.Equals(User.Identity.Name)).Include(d => d.TALL).Include(d => d.TSOL).Include(d => d.USUARIO).Include(d => d.CLIENTE).Include(d => d.PAI).Include(d => d.SOCIEDAD).ToList();
            var dOCUMENTOVs = db.DOCUMENTOVs.Where(a => a.USUARIOA_ID.Equals(User.Identity.Name)).ToList();
            var tsol = db.TSOLs.ToList();
            var tall = db.TALLs.ToList();
            foreach (DOCUMENTOV v in dOCUMENTOVs)
            {
                DOCUMENTO d = new DOCUMENTO();
                var ppd = d.GetType().GetProperties();
                var ppv = v.GetType().GetProperties();
                foreach (var pv in ppv)
                {
                    foreach (var pd in ppd)
                    {
                        if (pd.Name == pv.Name)
                        {
                            pd.SetValue(d, pv.GetValue(v));
                            break;
                        }
                    }
                }
                d.TSOL = tsol.Where(a => a.ID.Equals(d.TSOL_ID)).FirstOrDefault();
                d.TALL = tall.Where(a => a.ID.Equals(d.TALL_ID)).FirstOrDefault();
                //d.ESTADO = db.STATES.Where(a => a.ID.Equals(v.ESTADO)).FirstOrDefault().NAME;
                //d.CIUDAD = db.CITIES.Where(a => a.ID.Equals(v.CIUDAD)).FirstOrDefault().NAME;
                dOCUMENTOes.Add(d);
            }
            dOCUMENTOes = dOCUMENTOes.Distinct(new DocumentoComparer()).ToList();
            return View(dOCUMENTOes);
        }

        // GET: Solicitudes/Details/5
        public ActionResult Details(decimal id, string pais)
        {
            int pagina = 203; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title += " ";
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".png";
                }
                catch
                {
                    if (pais != "")
                    {
                        ViewBag.pais = pais + ".png";
                        Session["pais"] = pais;
                    }
                    else
                    {
                        //ViewBag.pais = "mx.png";
                        ////return RedirectToAction("Pais", "Home");
                    }
                }
                Session["spras"] = user.SPRAS_ID;
            }
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCUMENTO dOCUMENTO = db.DOCUMENTOes.Find(id);
            if (dOCUMENTO == null)
            {
                return HttpNotFound();
            }
            dOCUMENTO.CLIENTE = db.CLIENTEs.Where(a => a.VKORG.Equals(dOCUMENTO.VKORG)
                                                    & a.VTWEG.Equals(dOCUMENTO.VTWEG)
                                                    & a.SPART.Equals(dOCUMENTO.SPART)
                                                    & a.KUNNR.Equals(dOCUMENTO.PAYER_ID)).First();
            dOCUMENTO.DOCUMENTOF = db.DOCUMENTOFs.Where(a => a.NUM_DOC.Equals(dOCUMENTO.NUM_DOC)).ToList();

            ViewBag.workflow = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id)).OrderBy(a => a.POS).ToList();

            FLUJO f = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id) & a.ESTATUS.Equals("P")).FirstOrDefault();
            ViewBag.acciones = f;
            List<DOCUMENTOA> archivos = db.DOCUMENTOAs.Where(x => x.NUM_DOC.Equals(id) & x.ACTIVO == true).ToList();//RSG 15.05.2018
            ViewBag.files = archivos;
            if (f != null)
                if (f.USUARIOA_ID != null)
                {
                    if (f.USUARIOA_ID.Equals(User.Identity.Name))
                        ViewBag.accion = db.WORKFPs.Where(a => a.ID.Equals(f.WORKF_ID) & a.POS.Equals(f.WF_POS) & a.VERSION.Equals(f.WF_VERSION)).FirstOrDefault().ACCION.TIPO;
                }
                else
                {
                    ViewBag.accion = db.WORKFPs.Where(a => a.ID.Equals(f.WORKF_ID) & a.POS.Equals(f.WF_POS) & a.VERSION.Equals(f.WF_VERSION)).FirstOrDefault().ACCION.TIPO;
                }
            DocumentoFlujo DF = new DocumentoFlujo();
            DF.D = dOCUMENTO;
            DF.F = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id)).OrderByDescending(a => a.POS).FirstOrDefault();
            //DF.F.ESTATUS = "";
            ViewBag.ts = db.TS_FORM.Where(a => a.BUKRS_ID.Equals(DF.D.SOCIEDAD_ID) & a.LAND_ID.Equals(DF.D.PAIS_ID)).ToList();
            ViewBag.tts = db.DOCUMENTOTS.Where(a => a.NUM_DOC.Equals(DF.D.NUM_DOC)).ToList();

            if (DF.D.DOCUMENTO_REF != null)
                ViewBag.Title += DF.D.DOCUMENTO_REF + "-";
            ViewBag.Title += id;

            ViewBag.Relacionados = db.DOCUMENTOes.Where(a => a.DOCUMENTO_REF == DF.D.NUM_DOC).ToList();

            return View(DF);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(IEnumerable<HttpPostedFileBase> files_soporte, string[] labels_soporte)
        {
            decimal num_doc = decimal.Parse(Request.Form["D.NUM_DOC"].ToString());
            var res = "";
            string errorMessage = "";
            int numFiles = 0;
            //Checar si hay archivos para subir
            foreach (HttpPostedFileBase file in files_soporte)
            {
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        numFiles++;
                    }
                }
            }
            if (numFiles > 0)
            {
                string url = ConfigurationManager.AppSettings["URL_SAVE"];
                //Crear el directorio
                var dir = createDir(url, num_doc.ToString());

                //Evaluar que se creo el directorio
                if (dir.Equals(""))
                {

                    int i = 0;
                    int indexlabel = 0;
                    int max = 0;
                    if (db.DOCUMENTOAs.Where(a => a.NUM_DOC.Equals(num_doc)).Count() > 0)
                        max = db.DOCUMENTOAs.Where(a => a.NUM_DOC.Equals(num_doc)).Max(a => a.POS);//RSG 15.05.2018
                    foreach (HttpPostedFileBase file in files_soporte)
                    {
                        string errorfiles = "";
                        var clasefile = "";
                        try
                        {
                            clasefile = labels_soporte[indexlabel];
                        }
                        catch (Exception ex)
                        {
                            clasefile = "";
                        }
                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                string path = "";
                                string filename = file.FileName;
                                errorfiles = "";
                                res = SaveFile(file, url, num_doc.ToString(), out errorfiles, out path);

                                if (errorfiles == "")
                                {
                                    DOCUMENTOA doc = new DOCUMENTOA();
                                    var ext = System.IO.Path.GetExtension(filename);
                                    i++;
                                    doc.NUM_DOC = num_doc;
                                    doc.POS = i;
                                    doc.TIPO = ext.Replace(".", "");
                                    try
                                    {
                                        var clasefileM = clasefile.ToUpper();
                                        doc.CLASE = clasefileM.Substring(0, 3);
                                    }
                                    catch (Exception e)
                                    {
                                        doc.CLASE = "";
                                    }
                                    if (max > 0)
                                    {
                                        doc.POS = max + 1;
                                        DOCUMENTOA da = db.DOCUMENTOAs.Where(a => a.NUM_DOC.Equals(num_doc) & a.CLASE.Equals(doc.CLASE)).FirstOrDefault();
                                        if (da != null)
                                        {
                                            da.ACTIVO = false;
                                            db.Entry(da).State = EntityState.Modified;
                                        }
                                    }
                                    doc.STEP_WF = 1;
                                    doc.USUARIO_ID = User.Identity.Name;
                                    doc.PATH = path;
                                    doc.ACTIVO = true;
                                    try
                                    {
                                        db.DOCUMENTOAs.Add(doc);
                                        db.SaveChanges();
                                    }
                                    catch (Exception e)
                                    {
                                        errorfiles = "" + filename;
                                    }
                                }
                            }
                        }
                        indexlabel++;
                        if (errorfiles != "")
                        {
                            errorMessage += "Error con el archivo " + errorfiles;
                        }



                    }
                }
                else
                {
                    errorMessage = dir;
                }

                ////errorString = errorMessage;
                //Guardar número de documento creado
                Session["ERROR_FILES"] = errorMessage;
            }
            if (errorMessage == "")
            {

                FLUJO actual = db.FLUJOes.Where(a => a.NUM_DOC.Equals(num_doc)).OrderByDescending(a => a.POS).FirstOrDefault();
                FLUJO flujo = actual;
                flujo.ESTATUS = "A";
                flujo.FECHAM = DateTime.Now;
                flujo.COMENTARIO = "";
                flujo.USUARIOA_ID = User.Identity.Name;
                ProcesaFlujo2 pf = new ProcesaFlujo2();
                string c = pf.procesa(flujo, "");
                if (c.Equals("0"))//Aprobado
                {
                    return RedirectToAction("Details", "Solicitudes", new { id = flujo.NUM_DOC });
                }
                else if (c.Equals("1"))//CORREO
                {
                    return RedirectToAction("Enviar", "Mails", new { id = flujo.NUM_DOC, index = false });

                }
                else if (c.Equals("2"))//CORREO DE FIN DE WORKFLOW
                {
                    return RedirectToAction("Enviar", "Mails", new { id = flujo.NUM_DOC, index = false });
                }
                else if (c.Equals("3"))//Rechazado
                {
                    return RedirectToAction("Details", "Solicitudes", new { id = flujo.NUM_DOC });
                }
                else
                {
                    TempData["error"] = c;
                    return RedirectToAction("Details", "Solicitudes", new { id = flujo.NUM_DOC });
                }
            }

            return RedirectToAction("Details");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Taxeo()
        {
            decimal num_doc = decimal.Parse(Request.Form["D.NUM_DOC"].ToString());
            string conc = Request.Form["txt_concepto"].ToString();
            DOCUMENTO d = db.DOCUMENTOes.Find(num_doc);
            d.CONCEPTO_ID = int.Parse(conc);
            db.Entry(d).State = EntityState.Modified;
            db.SaveChanges();

            FLUJO f = db.FLUJOes.Where(a => a.NUM_DOC.Equals(num_doc)).OrderByDescending(a => a.POS).FirstOrDefault();
            ProcesaFlujo2 p = new ProcesaFlujo2();
            f.ESTATUS = "A";
            p.procesa(f, "");


            return RedirectToAction("Details", new { id = num_doc });
        }

        // GET: Solicitudes
        public ActionResult Reversa(decimal id)
        {
            int pagina = 201; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                //string u = "admin";
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
                    //ViewBag.pais = "mx.png";
                    ////return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
            }
            DOCUMENTO d = db.DOCUMENTOes.Find(id);
            return View(d);
        }
        // GET: Solicitudes/Create
        [HttpGet]
        public ActionResult Create(string id_d, string tsol)
        {

            string dates = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime theTime = DateTime.ParseExact(dates, //"06/04/2018 12:00:00 a.m."
                                        "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None);

            var relacionada_neg = "";
            var relacionada_dis = "";

            DOCUMENTO d = new DOCUMENTO();
            string errorString = "";
            int pagina = 202; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string p = "";
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                List<TREVERSAT> ldocr = new List<TREVERSAT>();
                decimal rel = 0;
                try
                {
                    if (id_d == null || id_d.Equals(""))
                    {
                        throw new Exception();
                    }
                    rel = Convert.ToDecimal(id_d);
                    ViewBag.relacionada = "prelacionada";
                    ViewBag.relacionadan = rel + "";

                }
                catch
                {
                    rel = 0;
                    ViewBag.relacionada = "";
                    ViewBag.relacionadan = "";
                }

                //Obtener los valores de tsols
                List<TSOL> tsols_val = new List<TSOL>();
                List<TSOLT_MODBD> tsols_valbd = new List<TSOLT_MODBD>();
                try
                {
                    tsols_val = db.TSOLs.ToList();
                    tsols_valbd = tsols_val.Select(tsv => new TSOLT_MODBD
                    {
                        ID = tsv.ID,
                        FACTURA = tsv.FACTURA
                    }).ToList();
                }
                catch (Exception e)
                {

                }
                var tsols_valbdjs = JsonConvert.SerializeObject(tsols_valbd, Formatting.Indented);
                ViewBag.TSOL_VALUES = tsols_valbdjs;

                //Validar si es una reversa
                string isrn = "";
                string isr = "";
                var freversa = (dynamic)null;
                try
                {
                    if (tsol == null || tsol.Equals(""))
                    {
                        throw new Exception();
                    }
                    TSOL ts = tsols_val.Where(tsb => tsb.TSOLR == tsol).FirstOrDefault();
                    if (ts != null)
                    {
                        isrn = "X";
                        isr = "preversa";
                        freversa = theTime.ToString("yyyy-MM-dd"); ;
                        //Obtener los tipos de reversas
                        try
                        {
                            //ldocr = db.TREVERSAs.Where(t => t.ACTIVO == true)
                            //    .Join(
                            //    db.TREVERSATs.Where(tt => tt.SPRAS_ID == user.SPRAS_ID),
                            //    t => t.ID,
                            //    tt => tt.TREVERSA_ID,
                            //    (t, tt) => new TREVERSAT
                            //    {
                            //        SPRAS_ID = tt.SPRAS_ID,
                            //        TREVERSA_ID = tt.TREVERSA_ID,
                            //        TXT100 = tt.TXT100
                            //    }).ToList();
                            //ldocr = db.TREVERSATs.Where(tt => tt.SPRAS_ID == user.SPRAS_ID).ToList();
                            ldocr = db.TREVERSATs.Where(a => a.TREVERSA.ACTIVO == true && a.SPRAS_ID == user.SPRAS_ID).ToList();
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
                catch (Exception e)
                {
                    isrn = "";
                    isr = "";
                    freversa = "";
                }

                ViewBag.reversa = isr;
                ViewBag.reversan = isrn;
                ViewBag.FECHAD_REV = freversa;
                ViewBag.TREVERSA = new SelectList(ldocr, "TREVERSA_ID", "TXT100");

                if (rel == 0)
                {
                    try//RSG 15.05.2018
                    {
                        p = Session["pais"].ToString();
                        ViewBag.pais = p + ".png";
                    }
                    catch
                    {
                        //ViewBag.pais = "mx.png";
                        return RedirectToAction("Pais", "Home");//
                    }
                }

                Session["spras"] = user.SPRAS_ID;

                List<TSOLT_MOD> list_sol = new List<TSOLT_MOD>();
                //tipo de solicitud
                if (ViewBag.reversa == "preversa")
                {
                    list_sol = tsols_val.Where(sol => sol.TSOLR == null)
                                        .Join(
                                        db.TSOLTs.Where(solt => solt.SPRAS_ID == user.SPRAS_ID),
                                        sol => sol.ID,
                                        solt => solt.TSOL_ID,
                                        (sol, solt) => new TSOLT_MOD
                                        {
                                            SPRAS_ID = solt.SPRAS_ID,
                                            TSOL_ID = solt.TSOL_ID,
                                            TEXT = solt.TSOL_ID + " " + solt.TXT020
                                        })
                                    .ToList();
                }
                else
                {
                    list_sol = tsols_val.Where(sol => sol.ESTATUS != "X")
                                        .Join(
                                        db.TSOLTs.Where(solt => solt.SPRAS_ID == user.SPRAS_ID),
                                        sol => sol.ID,
                                        solt => solt.TSOL_ID,
                                        (sol, solt) => new TSOLT_MOD
                                        {
                                            SPRAS_ID = solt.SPRAS_ID,
                                            TSOL_ID = solt.TSOL_ID,
                                            TEXT = solt.TSOL_ID + " " + solt.TXT020
                                        })
                                    .ToList();
                }



                //Obtener los documentos relacionados
                List<DOCUMENTO> docsrel = new List<DOCUMENTO>();

                SOCIEDAD id_bukrs = new SOCIEDAD();
                var id_pais = new PAI();
                var id_waers = db.MONEDAs.Where(m => m.ACTIVO == true).ToList();

                List<TAT001.Models.GALL_MOD> list_grupo = new List<GALL_MOD>();
                //Grupos de solicitud
                list_grupo = db.GALLs.Where(g => g.ACTIVO == true)
                                .Join(
                                db.GALLTs.Where(gt => gt.SPRAS_ID == user.SPRAS_ID),
                                g => g.ID,
                                gt => gt.GALL_ID,
                                (g, gt) => new GALL_MOD
                                {
                                    SPRAS_ID = gt.SPRAS_ID,
                                    GALL_ID = gt.GALL_ID,
                                    TEXT = g.ID + " " + gt.TXT50
                                }).ToList();

                List<DOCUMENTOA> archivos = new List<DOCUMENTOA>();
                if (rel > 0)
                {
                    d = db.DOCUMENTOes.Where(doc => doc.NUM_DOC == rel).FirstOrDefault();
                    docsrel = db.DOCUMENTOes.Where(docr => docr.DOCUMENTO_REF == rel).ToList();
                    id_bukrs = db.SOCIEDADs.Where(soc => soc.BUKRS == d.SOCIEDAD_ID && soc.ACTIVO == true).FirstOrDefault();
                    id_pais = db.PAIS.Where(pais => pais.LAND.Equals(d.PAIS_ID)).FirstOrDefault();//RSG 15.05.2018
                    d.DOCUMENTO_REF = rel;
                    relacionada_neg = d.TIPO_TECNICO;
                    ViewBag.TSOL_ANT = d.TSOL_ID;

                    if (d != null)
                    {

                        d.TSOL_ID = tsol;
                        ViewBag.TSOL_ID = new SelectList(list_sol, "TSOL_ID", "TEXT", selectedValue: d.TSOL_ID);
                        ViewBag.GALL_ID = new SelectList(list_grupo, "GALL_ID", "TEXT", selectedValue: d.GALL_ID);
                        TSOLT_MOD tsmod = new TSOLT_MOD();
                        try
                        {
                            tsmod = list_sol.Where(id => id.TSOL_ID.Equals(d.TSOL_ID)).FirstOrDefault();

                        }
                        catch
                        {
                            tsmod.TEXT = "";
                        }
                        ViewBag.TSOL_IDI = tsmod.TEXT.ToString();
                        TAT001.Models.GALL_MOD gall_mod = list_grupo.Where(id => id.GALL_ID.Equals(d.GALL_ID)).FirstOrDefault();
                        ViewBag.GALL_IDI = gall_mod.TEXT;
                        ViewBag.GALL_IDI_VAL = gall_mod.GALL_ID;
                        archivos = db.DOCUMENTOAs.Where(x => x.NUM_DOC.Equals(d.NUM_DOC)).ToList();

                        List<DOCUMENTOP> docpl = db.DOCUMENTOPs.Where(docp => docp.NUM_DOC == d.NUM_DOC).ToList();//Documentos que se obtienen de la provisión
                        List<DOCUMENTOP> docsrelp = new List<DOCUMENTOP>();
                        //Obtener los documentos de la relacionada
                        if (docsrel.Count > 0)
                        {
                            docsrelp = docsrel
                                .Join(
                                db.DOCUMENTOPs,
                                docsl => docsl.NUM_DOC,
                                docspl => docspl.NUM_DOC,
                                (docsl, docspl) => new DOCUMENTOP
                                {
                                    NUM_DOC = docspl.NUM_DOC,
                                    POS = docspl.POS,
                                    MATNR = docspl.MATNR,
                                    MATKL = docspl.MATKL,
                                    CANTIDAD = docspl.CANTIDAD,
                                    MONTO = docspl.MONTO,
                                    PORC_APOYO = docspl.PORC_APOYO,
                                    MONTO_APOYO = docspl.MONTO_APOYO,
                                    PRECIO_SUG = docspl.PRECIO_SUG,
                                    VOLUMEN_EST = docspl.VOLUMEN_EST,
                                    VOLUMEN_REAL = docspl.VOLUMEN_REAL,
                                    APOYO_REAL = docspl.APOYO_REAL,
                                    APOYO_EST = docspl.APOYO_EST,
                                    VIGENCIA_DE = docspl.VIGENCIA_DE,
                                    VIGENCIA_AL = docspl.VIGENCIA_AL
                                }).ToList();
                        }
                        d.NUM_DOC = 0;
                        List<TAT001.Models.DOCUMENTOP_MOD> docsp = new List<DOCUMENTOP_MOD>();
                        var dis = "";
                        for (int j = 0; j < docpl.Count; j++)
                        {
                            try
                            {
                                //Documentos de la provisión
                                DOCUMENTOP_MOD docP = new DOCUMENTOP_MOD();
                                docP.NUM_DOC = d.NUM_DOC;
                                docP.POS = docpl[j].POS;
                                docP.MATNR = docpl[j].MATNR;
                                if (j == 0 && docP.MATNR == "")
                                {
                                    relacionada_dis = "C";
                                }
                                docP.MATKL = docpl[j].MATKL;
                                docP.MATKL_ID = docpl[j].MATKL;
                                docP.CANTIDAD = 1;
                                docP.MONTO = docpl[j].MONTO;
                                docP.PORC_APOYO = docpl[j].PORC_APOYO;
                                docP.MONTO_APOYO = docpl[j].MONTO_APOYO;
                                docP.PRECIO_SUG = docpl[j].PRECIO_SUG;
                                docP.VOLUMEN_EST = docpl[j].VOLUMEN_EST;
                                docP.VIGENCIA_DE = docpl[j].VIGENCIA_DE;
                                docP.VIGENCIA_AL = docpl[j].VIGENCIA_AL;
                                docP.APOYO_EST = docpl[j].APOYO_EST;
                                docP.APOYO_REAL = docpl[j].APOYO_REAL;

                                //Verificar si hay materiales en las relacionadas
                                if (docsrelp.Count > 0)
                                {
                                    List<DOCUMENTOP> docrel = new List<DOCUMENTOP>();

                                    if (docP.MATNR != null && docP.MATNR != "")
                                    {
                                        docrel = docsrelp.Where(docrell => docrell.MATNR == docP.MATNR).ToList();
                                    }
                                    else
                                    {
                                        docrel = docsrelp.Where(docrell => docrell.MATKL == docP.MATKL_ID).ToList();
                                        dis = "C";
                                    }

                                    for (int k = 0; k < docrel.Count; k++)
                                    {
                                        //Relacionada se obtiene el 
                                        decimal docr_vr = Convert.ToDecimal(docrel[k].VOLUMEN_REAL);
                                        decimal docr_ar = Convert.ToDecimal(docrel[k].APOYO_REAL);

                                        docP.VOLUMEN_EST -= docr_vr;
                                        docP.APOYO_EST -= docr_ar;

                                        if (dis == "C")
                                        {
                                            //decimal docr_vr = Convert.ToDecimal(docrel[k].);
                                            //decimal docr_ar = Convert.ToDecimal(docrel[k].APOYO_REAL);
                                        }

                                    }
                                }

                                //Siempre tiene que ser igual a 0
                                if (docP.VOLUMEN_EST < 0)
                                {
                                    docP.VOLUMEN_EST = 0;
                                }
                                if (docP.APOYO_EST < 0)
                                {
                                    docP.APOYO_EST = 0;
                                }

                                docP.MATNR = docpl[j].MATNR.TrimStart('0');//RSG 07.06.2018
                                docsp.Add(docP);
                            }
                            catch (Exception e)
                            {

                            }
                        }

                        d.DOCUMENTOP = docsp;
                    }
                }
                else
                {
                    ViewBag.TSOL_ID = new SelectList(list_sol, "TSOL_ID", "TEXT");
                    ViewBag.GALL_ID = new SelectList(list_grupo, "GALL_ID", "TEXT");
                    ViewBag.TSOL_IDI = "";
                    ViewBag.GALL_IDI = "";
                    //id_bukrs = db.SOCIEDADs.Where(soc => soc.LAND.Equals(p) && soc.ACTIVO == true).FirstOrDefault();//RSG 15.05.2018
                    id_pais = db.PAIS.Where(pais => pais.LAND.Equals(p)).FirstOrDefault();//RSG 15.05.2018
                    id_bukrs = db.SOCIEDADs.Where(soc => soc.BUKRS.Equals(id_pais.SOCIEDAD_ID) && soc.ACTIVO == true).FirstOrDefault();//RSG 15.05.2018
                    ViewBag.TSOL_ANT = "";
                }

                ViewBag.files = archivos;

                //Select clasificación
                //var id_clas = db.TALLs.Where(t => t.ACTIVO == true)
                //                .Join(
                //                db.TALLTs.Where(tallt => tallt.SPRAS_ID == user.SPRAS_ID),
                //                tall => tall.ID,
                //                tallt => tallt.TALL_ID,
                //                (tall, tallt) => tallt)
                //            .ToList();

                List<TAT001.Entities.GALL> id_clas = new List<TAT001.Entities.GALL>();
                ViewBag.TALL_ID = new SelectList(id_clas, "TALL_ID", "TXT50");

                //Datos del país
                //var id_pais = db.PAIS.Where(pais => pais.LAND.Equals(id_bukrs.LAND)).FirstOrDefault();//RSG 15.05.2018

                var id_states = (from st in db.STATES
                                 join co in db.COUNTRIES
                                 on st.COUNTRY_ID equals co.ID
                                 where co.SORTNAME.Equals(id_pais.LAND)
                                 select new
                                 {
                                     st.ID,
                                     st.NAME,
                                     st.COUNTRY_ID
                                 }).ToList();



                List<TAT001.Entities.CITy> id_city = new List<TAT001.Entities.CITy>();

                ViewBag.SOCIEDAD_ID = id_bukrs;
                ViewBag.PAIS_ID = id_pais;
                ViewBag.STATE_ID = "";// new SelectList(id_states, "ID", dataTextField: "NAME");
                ViewBag.CITY_ID = "";// new SelectList(id_city, "ID", dataTextField: "NAME");
                ViewBag.MONEDA = new SelectList(id_waers, "WAERS", dataTextField: "WAERS", selectedValue: id_bukrs.WAERS); //Duda si cambia en la relacionada

                //Información del cliente
                var id_clientes = db.CLIENTEs.Where(c => c.LAND.Equals(p) && c.ACTIVO == true).ToList();

                ViewBag.PAYER_ID = new SelectList(id_clientes, "KUNNR", dataTextField: "NAME1");

                //Información de categorías
                var id_cat = db.CATEGORIAs.Where(c => c.ACTIVO == true)
                                .Join(
                                db.CATEGORIATs.Where(ct => ct.SPRAS_ID == user.SPRAS_ID),
                                c => c.ID,
                                ct => ct.CATEGORIA_ID,
                                (c, ct) => new
                                {
                                    ct.CATEGORIA_ID,
                                    TEXT = ct.TXT50
                                }).ToList();

                id_cat.RemoveRange(0, id_cat.Count - 1);//RSG 28.05.2018
                ViewBag.CATEGORIA_ID = new SelectList(id_cat, "CATEGORIA_ID", "TEXT");
                List<TAT001.Entities.CITy> id_cityy = new List<TAT001.Entities.CITy>();
                ViewBag.BASE_ID = new SelectList(id_cityy, "CATEGORIA_ID", "TEXT");

                d.SOCIEDAD_ID = id_bukrs.BUKRS;
                d.PAIS_ID = id_pais.LAND;//RSG 18.05.2018
                d.MONEDA_ID = id_bukrs.WAERS;
                var date = DateTime.Now.Date;
                TAT001.Entities.TCAMBIO tcambio = new TAT001.Entities.TCAMBIO();
                try
                {
                    tcambio = db.TCAMBIOs.Where(t => t.FCURR.Equals(id_bukrs.WAERS) && t.TCURR.Equals("USD") && t.GDATU.Equals(date)).FirstOrDefault();
                    if (tcambio == null)
                    {
                        var max = db.TCAMBIOs.Where(t => t.FCURR.Equals(id_bukrs.WAERS) && t.TCURR.Equals("USD")).Max(a => a.GDATU);
                        tcambio = db.TCAMBIOs.Where(t => t.FCURR.Equals(id_bukrs.WAERS) && t.TCURR.Equals("USD") && t.GDATU.Equals(max)).FirstOrDefault();
                    }
                    decimal con = Convert.ToDecimal(tcambio.UKURS);
                    var cons = con.ToString("0.##");

                    ViewBag.tcambio = cons;
                }
                catch (Exception e)
                {
                    errorString = e.Message + "detail: conversion " + id_bukrs.WAERS + " to " + "USD" + " in date " + DateTime.Now.Date;
                    ViewBag.tcambio = "";
                }


            }

            d.PERIODO = Convert.ToInt32(DateTime.Now.ToString("MM"));
            d.EJERCICIO = Convert.ToString(DateTime.Now.Year);

            d.FECHAD = theTime;
            ViewBag.FECHAD = theTime.ToString("yyyy-MM-dd");
            ViewBag.PERIODO = d.PERIODO;
            ViewBag.EJERCICIO = d.EJERCICIO;
            ViewBag.STCD1 = "";
            ViewBag.PARVW = "";
            ViewBag.UNAFACTURA = "false";
            ViewBag.MONTO_DOC_ML2 = "";
            ViewBag.error = errorString;
            ViewBag.NAME1 = "";
            ViewBag.notas_soporte = "";

            //Prueba para agregar soporte a la tabla ahora información

            //DOCUMENTOF DF1 = new DOCUMENTOF();

            //DF1.POS = 1;
            //DF1.FACTURA = "FF1";
            //DF1.PROVEEDOR = "PP1";
            //DF1.FACTURAK = "FFK1";

            //DOCUMENTOF DF2 = new DOCUMENTOF();

            //DF2.POS = 2;
            //DF2.FACTURA = "FF2";
            //DF2.PROVEEDOR = "1000000001";
            //DF2.FACTURAK = "FFK2";

            //List<DOCUMENTOF> LD = new List<DOCUMENTOF>() { DF1, DF2 };

            //d.DOCUMENTOF = LD;

            ViewBag.SEL_NEG = relacionada_neg;
            ViewBag.SEL_DIS = relacionada_dis;
            ViewBag.BMONTO_APOYO = "";

            //----------------------------RSG 18.05.2018
            string spras = Session["spras"].ToString();
            ViewBag.PERIODOS = new SelectList(db.PERIODOTs.Where(a => a.SPRAS_ID == spras).ToList(), "PERIODO_ID", "TXT50", DateTime.Now.Month);
            List<string> anios = new List<string>();
            int mas = 10;
            for (int i = 0; i < mas; i++)
            {
                anios.Add((DateTime.Now.Year + i).ToString());
            }
            ViewBag.ANIOS = new SelectList(anios, DateTime.Now.Year.ToString());
            d.SOCIEDAD = db.SOCIEDADs.Find(d.SOCIEDAD_ID);
            //----------------------------RSG 18.05.2018

            return View(d);
        }

        // POST: Solicitudes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NUM_DOC,TSOL_ID,TALL_ID,SOCIEDAD_ID,PAIS_ID,ESTADO,CIUDAD,PERIODO," +
            "EJERCICIO,TIPO_TECNICO,TIPO_RECURRENTE,CANTIDAD_EV,USUARIOC_ID,FECHAD,FECHAC,ESTATUS,ESTATUS_C,ESTATUS_SAP," +
            "ESTATUS_WF,DOCUMENTO_REF,NOTAS,MONTO_DOC_MD,MONTO_FIJO_MD,MONTO_BASE_GS_PCT_MD,MONTO_BASE_NS_PCT_MD,MONTO_DOC_ML," +
            "MONTO_FIJO_ML,MONTO_BASE_GS_PCT_ML,MONTO_BASE_NS_PCT_ML,MONTO_DOC_ML2,MONTO_FIJO_ML2,MONTO_BASE_GS_PCT_ML2," +
            "MONTO_BASE_NS_PCT_ML2,IMPUESTO,FECHAI_VIG,FECHAF_VIG,ESTATUS_EXT,SOLD_TO_ID,PAYER_ID,GRUPO_CTE_ID,CANAL_ID," +
            "MONEDA_ID,TIPO_CAMBIO,NO_FACTURA,FECHAD_SOPORTE,METODO_PAGO,NO_PROVEEDOR,PASO_ACTUAL,AGENTE_ACTUAL,FECHA_PASO_ACTUAL," +
            "VKORG,VTWEG,SPART,HORAC,FECHAC_PLAN,FECHAC_USER,HORAC_USER,CONCEPTO,PORC_ADICIONAL,PAYER_NOMBRE,PAYER_EMAIL," +
            "MONEDAL_ID,MONEDAL2_ID,TIPO_CAMBIOL,TIPO_CAMBIOL2,DOCUMENTOP, DOCUMENTOF, DOCUMENTOREC, GALL_ID")] DOCUMENTO dOCUMENTO,
                IEnumerable<HttpPostedFileBase> files_soporte, string notas_soporte, string[] labels_soporte, string unafact, string FECHAD_REV, string TREVERSA, string select_neg, string select_dis, string bmonto_apoyo)
        {

            bool prueba = true;
            string errorString = "";
            SOCIEDAD id_bukrs = new SOCIEDAD();
            string p = "";
            if (ModelState.IsValid && prueba == true)
            //if (ModelState.IsValid)
            {
                try
                {
                    //Obtener datos ocultos o deshabilitados                    
                    try
                    {
                        p = Session["pais"].ToString();
                        ViewBag.pais = p + ".png";
                    }
                    catch
                    {
                        ViewBag.pais = "mx.png";
                        //return RedirectToAction("Pais", "Home");
                    }
                    //try
                    //{
                    //    decimal refe = Convert.ToDecimal(Session["rel"].ToString());
                    //    dOCUMENTO.DOCUMENTO_REF = refe;
                    //    Session["rel"] = null;
                    //}
                    //catch (Exception e)
                    //{
                    //    dOCUMENTO.DOCUMENTO_REF = null;
                    //}
                    DOCUMENTO d = new DOCUMENTO();
                    if (dOCUMENTO.DOCUMENTO_REF > 0)
                    {
                        d = db.DOCUMENTOes.Where(doc => doc.NUM_DOC == dOCUMENTO.DOCUMENTO_REF).FirstOrDefault();
                        //dOCUMENTO.TSOL_ID = d.TSOL_ID;
                        id_bukrs = db.SOCIEDADs.Where(soc => soc.BUKRS == d.SOCIEDAD_ID).FirstOrDefault();
                        dOCUMENTO.ESTADO = d.ESTADO;
                        dOCUMENTO.CIUDAD = d.CIUDAD;
                        dOCUMENTO.PAYER_ID = d.PAYER_ID;
                        dOCUMENTO.CONCEPTO = d.CONCEPTO;
                        dOCUMENTO.NOTAS = d.NOTAS;
                        dOCUMENTO.FECHAI_VIG = d.FECHAI_VIG;
                        dOCUMENTO.FECHAF_VIG = d.FECHAF_VIG;
                        dOCUMENTO.PAYER_NOMBRE = d.PAYER_NOMBRE;
                        dOCUMENTO.PAYER_EMAIL = d.PAYER_EMAIL;
                        dOCUMENTO.TIPO_CAMBIO = d.TIPO_CAMBIO;
                        dOCUMENTO.GALL_ID = d.GALL_ID;
                        //Obtener el país
                        dOCUMENTO.PAIS_ID = d.PAIS_ID;//RSG 15.05.2018
                    }
                    else
                    {
                        id_bukrs = db.SOCIEDADs.Where(soc => soc.LAND.Equals(p)).FirstOrDefault();
                        //Obtener el país
                        dOCUMENTO.PAIS_ID = p.ToUpper();//RSG 15.05.2018
                    }
                    //Tipo técnico
                    dOCUMENTO.TIPO_TECNICO = select_neg;

                    //Obtener el número de documento
                    decimal N_DOC = getSolID(dOCUMENTO.TSOL_ID);
                    dOCUMENTO.NUM_DOC = N_DOC;

                    //Obtener SOCIEDAD_ID                     
                    dOCUMENTO.SOCIEDAD_ID = id_bukrs.BUKRS;

                    ////Obtener el país
                    //dOCUMENTO.PAIS_ID = p.ToUpper();

                    //CANTIDAD_EV > 1 si son recurrentes
                    dOCUMENTO.CANTIDAD_EV = 1;

                    //Obtener usuarioc
                    USUARIO u = db.USUARIOs.Find(User.Identity.Name);//RSG 02/05/2018
                    dOCUMENTO.PUESTO_ID = u.PUESTO_ID;//RSG 02/05/2018
                    dOCUMENTO.USUARIOC_ID = User.Identity.Name;

                    //Fechac
                    dOCUMENTO.FECHAC = DateTime.Now;

                    //Horac
                    dOCUMENTO.HORAC = DateTime.Now.TimeOfDay;

                    //FECHAC_PLAN
                    dOCUMENTO.FECHAC_PLAN = DateTime.Now.Date;

                    //FECHAC_USER
                    dOCUMENTO.FECHAC_USER = DateTime.Now.Date;

                    //HORAC_USER
                    dOCUMENTO.HORAC_USER = DateTime.Now.TimeOfDay;

                    //Estatus
                    dOCUMENTO.ESTATUS = "N";

                    //Estatus wf
                    dOCUMENTO.ESTATUS_WF = "P";

                    ///////////////////Montos
                    //MONTO_DOC_MD
                    var MONTO_DOC_MD = dOCUMENTO.MONTO_DOC_MD;
                    dOCUMENTO.MONTO_DOC_MD = Convert.ToDecimal(MONTO_DOC_MD);

                    //Obtener el monto de la sociedad
                    dOCUMENTO.MONTO_DOC_ML = getValSoc(id_bukrs.WAERS, dOCUMENTO.MONEDA_ID, Convert.ToDecimal(dOCUMENTO.MONTO_DOC_MD), out errorString);
                    if (!errorString.Equals(""))
                    {
                        throw new Exception();
                    }

                    //MONTO_DOC_ML2 
                    var MONTO_DOC_ML2 = dOCUMENTO.MONTO_DOC_ML2;
                    dOCUMENTO.MONTO_DOC_ML2 = Convert.ToDecimal(MONTO_DOC_ML2);

                    //MONEDAL_ID moneda de la sociedad
                    dOCUMENTO.MONEDAL_ID = id_bukrs.WAERS;

                    //MONEDAL2_ID moneda en USD
                    dOCUMENTO.MONEDAL2_ID = "USD";

                    //Tipo cambio de la moneda de la sociedad TIPO_CAMBIOL
                    dOCUMENTO.TIPO_CAMBIOL = getUkurs(id_bukrs.WAERS, dOCUMENTO.MONEDA_ID, out errorString);

                    //Tipo cambio dolares TIPO_CAMBIOL2
                    dOCUMENTO.TIPO_CAMBIOL2 = getUkursUSD(dOCUMENTO.MONEDA_ID, "USD", out errorString);
                    if (!errorString.Equals(""))
                    {
                        throw new Exception();
                    }
                    //Obtener datos del payer
                    CLIENTE payer = getCliente(dOCUMENTO.PAYER_ID);

                    dOCUMENTO.VKORG = payer.VKORG;
                    dOCUMENTO.VTWEG = payer.VTWEG;
                    dOCUMENTO.SPART = payer.SPART;


                    //Guardar el documento
                    db.DOCUMENTOes.Add(dOCUMENTO);
                    db.SaveChanges();

                    //Actualizar el rango
                    updateRango(dOCUMENTO.TSOL_ID, dOCUMENTO.NUM_DOC);

                    //Redireccionar al inicio
                    //Guardar número de documento creado
                    Session["NUM_DOC"] = dOCUMENTO.NUM_DOC;

                    //Validar si es una reversa
                    var revn = "";
                    try
                    {
                        if (dOCUMENTO.TSOL_ID == null || dOCUMENTO.TSOL_ID.Equals(""))
                        {
                            throw new Exception();
                        }

                        TSOL ts = db.TSOLs.Where(tsb => tsb.ID == dOCUMENTO.TSOL_ID).FirstOrDefault();
                        if (ts != null)
                        {
                            revn = "";
                            //DateTime theTime = (dynamic)null;
                            DateTime dates = DateTime.Now;
                            try
                            {
                                //dates = DateTime.Now;
                                //theTime  = DateTime.ParseExact(FECHAD_REV, //"06/04/2018 12:00:00 a.m."
                                //            "yyyy-MM-dd",
                                //            System.Globalization.CultureInfo.InvariantCulture,
                                //            System.Globalization.DateTimeStyles.None);
                            }
                            catch (Exception e)
                            {
                            }
                            //Si es una reversa
                            try
                            {
                                if (TREVERSA != null)
                                {
                                    DOCUMENTOR docr = new DOCUMENTOR();
                                    docr.NUM_DOC = dOCUMENTO.NUM_DOC;
                                    docr.TREVERSA_ID = Convert.ToInt32(TREVERSA);
                                    docr.USUARIOC_ID = User.Identity.Name;
                                    docr.FECHAC = dates;
                                    docr.COMENTARIO = notas_soporte.ToString();

                                    db.DOCUMENTORs.Add(docr);
                                    db.SaveChanges();

                                    revn = "X";
                                }

                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }

                    if (revn == "")
                    {
                        //Guardar las notas
                        if (notas_soporte != null && notas_soporte != "")
                        {
                            DOCUMENTON doc_notas = new DOCUMENTON();
                            doc_notas.NUM_DOC = dOCUMENTO.NUM_DOC;
                            doc_notas.POS = 1;
                            doc_notas.STEP = 1;
                            doc_notas.USUARIO_ID = dOCUMENTO.USUARIOC_ID;
                            doc_notas.TEXTO = notas_soporte.ToString();

                            db.DOCUMENTONs.Add(doc_notas);
                            db.SaveChanges();
                        }
                    }



                    //Guardar los documentos p para el documento guardado
                    try
                    {
                        //Agregar materiales existentes para evitar que en la vista se hayan agregado o quitado
                        List<DOCUMENTOP> docpl = new List<DOCUMENTOP>();
                        if (dOCUMENTO.DOCUMENTO_REF > 0)
                        {
                            docpl = db.DOCUMENTOPs.Where(docp => docp.NUM_DOC == dOCUMENTO.DOCUMENTO_REF).ToList();

                            for (int j = 0; j < docpl.Count; j++)
                            {
                                try
                                {
                                    DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();
                                    var cat = "";

                                    if (docpl[j].MATNR != null && docpl[j].MATNR != "")
                                    {
                                        string mmatnr = docpl[j].MATNR.TrimStart('0');//RSG 07.06.2018
                                        docmod = dOCUMENTO.DOCUMENTOP.Where(docp => docp.MATNR == mmatnr).FirstOrDefault();
                                    }
                                    else
                                    {
                                        docmod = dOCUMENTO.DOCUMENTOP.Where(docp => docp.MATKL_ID == docpl[j].MATKL).FirstOrDefault();
                                        cat = "C";
                                    }
                                    DOCUMENTOP docP = new DOCUMENTOP();
                                    //Si lo encuentra meter valores de la base de datos y vista
                                    if (docmod != null)
                                    {
                                        docP.NUM_DOC = dOCUMENTO.NUM_DOC;
                                        docP.POS = docmod.POS;
                                        if (docmod.MATNR == null || docmod.MATNR == "")
                                        {
                                            docmod.MATNR = "";
                                        }
                                        docP.MATNR = docmod.MATNR;
                                        docP.MATNR = completaMaterial(docP.MATNR);//RSG 07.06.2018
                                        docP.MATKL = docmod.MATKL_ID;
                                        docP.CANTIDAD = 1;
                                        docP.MONTO = docmod.MONTO;
                                        docP.PORC_APOYO = docmod.PORC_APOYO;
                                        //docP.MONTO_APOYO = docmod.MONTO_APOYO;
                                        docP.MONTO_APOYO = docP.MONTO * (docP.PORC_APOYO / 100);
                                        docP.MONTO_APOYO = Math.Round(docP.MONTO_APOYO, 2);//RSG 16.05.2018
                                        docP.PRECIO_SUG = docmod.PRECIO_SUG;
                                        docP.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                        docP.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                        docP.VIGENCIA_DE = docpl[j].VIGENCIA_DE;
                                        docP.VIGENCIA_AL = docpl[j].VIGENCIA_AL;
                                        docP.APOYO_EST = docmod.APOYO_EST;
                                        docP.APOYO_REAL = docmod.APOYO_REAL;


                                    }
                                    else
                                    {
                                        docP.NUM_DOC = dOCUMENTO.NUM_DOC;
                                        docP.POS = docpl[j].POS;
                                        docP.MATNR = docpl[j].MATNR;
                                        docP.MATKL = docpl[j].MATKL;
                                        docP.CANTIDAD = 1;
                                        docP.MONTO = docpl[j].MONTO;
                                        //docP.PORC_APOYO = docpl[j].PORC_APOYO;
                                        docP.MONTO_APOYO = docP.MONTO * (docpl[j].PORC_APOYO / 100);
                                        docP.MONTO_APOYO = docpl[j].MONTO_APOYO;
                                        docP.PRECIO_SUG = docpl[j].PRECIO_SUG;
                                        docP.VOLUMEN_EST = docpl[j].VOLUMEN_EST;
                                        docP.VOLUMEN_REAL = docpl[j].VOLUMEN_REAL;
                                        docP.VIGENCIA_DE = docpl[j].VIGENCIA_DE;
                                        docP.VIGENCIA_AL = docpl[j].VIGENCIA_AL;
                                        docP.APOYO_EST = docpl[j].APOYO_EST;
                                        docP.APOYO_REAL = docpl[j].APOYO_REAL;
                                    }

                                    //RSG 05.06.2018
                                    if (cat == "C")
                                    {
                                        decimal pos = docpl[j].POS;
                                        List<DOCUMENTOM> mm = db.DOCUMENTOMs.Where(a => a.NUM_DOC == dOCUMENTO.DOCUMENTO_REF & a.POS_ID == pos).ToList();
                                        foreach (DOCUMENTOM mn in mm)
                                        {
                                            DOCUMENTOM ii = new DOCUMENTOM();
                                            ii.NUM_DOC = dOCUMENTO.NUM_DOC;
                                            ii.POS_ID = mn.POS_ID;
                                            ii.POS = mn.POS;
                                            ii.MATNR = mn.MATNR;
                                            ii.APOYO_EST = (docP.APOYO_REAL + docP.APOYO_EST) / mm.Count;
                                            ii.APOYO_REAL = ii.APOYO_EST;
                                            ii.PORC_APOYO = mn.PORC_APOYO;
                                            ii.VIGENCIA_A = mn.VIGENCIA_A;
                                            ii.VIGENCIA_DE = mn.VIGENCIA_DE;
                                            docP.DOCUMENTOMs.Add(ii);
                                        }
                                    }

                                    //Agregarlo a la bd
                                    db.DOCUMENTOPs.Add(docP);
                                    db.SaveChanges();//RSG
                                }
                                catch (Exception e)
                                {

                                }

                            }
                        }
                        else
                        {
                            for (int j = 0; j < dOCUMENTO.DOCUMENTOP.Count; j++)
                            {
                                try
                                {
                                    DOCUMENTOP docP = new DOCUMENTOP();

                                    docP.NUM_DOC = dOCUMENTO.NUM_DOC;
                                    docP.POS = dOCUMENTO.DOCUMENTOP.ElementAt(j).POS;
                                    if (dOCUMENTO.DOCUMENTOP.ElementAt(j).MATNR == null)
                                    {
                                        dOCUMENTO.DOCUMENTOP.ElementAt(j).MATNR = "";
                                    }
                                    docP.MATNR = dOCUMENTO.DOCUMENTOP.ElementAt(j).MATNR;
                                    docP.MATNR = completaMaterial(docP.MATNR);//RSG 07.06.2018
                                    docP.MATKL = dOCUMENTO.DOCUMENTOP.ElementAt(j).MATKL_ID;
                                    docP.CANTIDAD = 1;
                                    docP.MONTO = dOCUMENTO.DOCUMENTOP.ElementAt(j).MONTO;
                                    docP.PORC_APOYO = dOCUMENTO.DOCUMENTOP.ElementAt(j).PORC_APOYO;
                                    docP.MONTO_APOYO = dOCUMENTO.DOCUMENTOP.ElementAt(j).MONTO_APOYO;
                                    docP.PRECIO_SUG = dOCUMENTO.DOCUMENTOP.ElementAt(j).PRECIO_SUG;
                                    docP.VOLUMEN_EST = dOCUMENTO.DOCUMENTOP.ElementAt(j).VOLUMEN_EST;
                                    docP.VOLUMEN_REAL = dOCUMENTO.DOCUMENTOP.ElementAt(j).VOLUMEN_REAL;
                                    docP.VIGENCIA_DE = dOCUMENTO.DOCUMENTOP.ElementAt(j).VIGENCIA_DE;
                                    docP.VIGENCIA_AL = dOCUMENTO.DOCUMENTOP.ElementAt(j).VIGENCIA_AL;
                                    docP.APOYO_EST = dOCUMENTO.DOCUMENTOP.ElementAt(j).APOYO_EST;
                                    docP.APOYO_REAL = dOCUMENTO.DOCUMENTOP.ElementAt(j).APOYO_REAL;

                                    db.DOCUMENTOPs.Add(docP);
                                    db.SaveChanges();//RSG

                                    //If matnr es "" agregar los materiales de la categoría
                                    List<DOCUMENTOM> docml = new List<DOCUMENTOM>();
                                    if (docP.MATNR == "")
                                    {
                                        docml = addCatItems(dOCUMENTO.PAYER_ID, docP.MATKL, dOCUMENTO.SOCIEDAD_ID, dOCUMENTO.NUM_DOC, Convert.ToInt16(docP.POS), docP.VIGENCIA_DE, docP.VIGENCIA_AL);
                                    }
                                    //Obtener el apoyo real o estimado para cada material
                                    var cantmat = docml.Count;
                                    //Obtener apoyo estimado
                                    decimal apoyo_esti = 0;
                                    decimal apoyo_real = 0;
                                    try
                                    {
                                        apoyo_esti = Convert.ToDecimal(docP.APOYO_EST) / cantmat;

                                    }
                                    catch (Exception e)
                                    {
                                        apoyo_esti = 0;
                                    }

                                    try
                                    {
                                        apoyo_real = Convert.ToDecimal(docP.APOYO_REAL) / cantmat;

                                    }
                                    catch (Exception e)
                                    {
                                        apoyo_real = 0;
                                    }
                                    for (int k = 0; k < docml.Count; k++)
                                    {
                                        try
                                        {
                                            DOCUMENTOM docM = new DOCUMENTOM();
                                            docM = docml[k];
                                            docM.POS = k + 1;
                                            docM.APOYO_REAL = apoyo_real;
                                            docM.APOYO_EST = apoyo_esti;

                                            db.DOCUMENTOMs.Add(docM);
                                            db.SaveChanges();//RSG
                                        }
                                        catch (Exception e)
                                        {

                                        }
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {

                    }

                    //Guardar los documentos f para el documento guardado
                    try
                    {
                        for (int j = 0; j < dOCUMENTO.DOCUMENTOF.Count; j++)
                        {
                            try
                            {
                                DOCUMENTOF docF = new DOCUMENTOF();
                                docF = dOCUMENTO.DOCUMENTOF[j];
                                docF.NUM_DOC = dOCUMENTO.NUM_DOC;

                                db.DOCUMENTOFs.Add(docF);
                                db.SaveChanges();
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }
                    //Guardar registros de recurrencias  RSG 28.05.2018------------------
                    if (dOCUMENTO.DOCUMENTOREC != null)
                        if (dOCUMENTO.DOCUMENTOREC.Count > 0)
                        {
                            foreach (DOCUMENTOREC drec in dOCUMENTO.DOCUMENTOREC)
                            {
                                drec.NUM_DOC = dOCUMENTO.NUM_DOC;
                                if (drec.POS == 1)
                                {
                                    drec.DOC_REF = drec.NUM_DOC;
                                    drec.ESTATUS = "P";
                                    dOCUMENTO.FECHAI_VIG = drec.FECHAF;
                                    dOCUMENTO.FECHAF_VIG = drec.FECHAF.Value.AddMonths(1).AddDays(-1);
                                    dOCUMENTO.TIPO_RECURRENTE = "M";
                                    foreach (DOCUMENTOP po in dOCUMENTO.DOCUMENTOPs)
                                    {
                                        po.VIGENCIA_DE = dOCUMENTO.FECHAI_VIG;
                                        po.VIGENCIA_AL = dOCUMENTO.FECHAF_VIG;
                                    }
                                }
                                if (drec.MONTO_BASE == null) //RSG 31.05.2018-------------------
                                    drec.MONTO_BASE = 0;
                                if (drec.PORC == null) //RSG 31.05.2018-------------------
                                    drec.PORC = 0;
                                dOCUMENTO.DOCUMENTORECs.Add(drec);
                            }
                            db.SaveChanges();
                        }//Guardar registros de recurrencias  RSG 28.05.2018-------------------

                    //Guardar los documentos cargados en la sección de soporte
                    var res = "";
                    string errorMessage = "";
                    int numFiles = 0;
                    //Checar si hay archivos para subir
                    try
                    {
                        foreach (HttpPostedFileBase file in files_soporte)
                        {
                            if (file != null)
                            {
                                if (file.ContentLength > 0)
                                {
                                    numFiles++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    if (numFiles > 0)
                    {
                        //Obtener las variables con los datos de sesión y ruta
                        string url = ConfigurationManager.AppSettings["URL_SAVE"];
                        //Crear el directorio
                        var dir = createDir(url, dOCUMENTO.NUM_DOC.ToString());

                        //Evaluar que se creo el directorio
                        if (dir.Equals(""))
                        {

                            int i = 0;
                            int indexlabel = 0;
                            foreach (HttpPostedFileBase file in files_soporte)
                            {
                                string errorfiles = "";
                                var clasefile = "";
                                try
                                {
                                    clasefile = labels_soporte[indexlabel];
                                }
                                catch (Exception ex)
                                {
                                    clasefile = "";
                                }
                                if (file != null)
                                {
                                    if (file.ContentLength > 0)
                                    {
                                        string path = "";
                                        string filename = file.FileName;
                                        errorfiles = "";
                                        res = SaveFile(file, url, dOCUMENTO.NUM_DOC.ToString(), out errorfiles, out path);

                                        if (errorfiles == "")
                                        {
                                            DOCUMENTOA doc = new DOCUMENTOA();
                                            var ext = System.IO.Path.GetExtension(filename);
                                            i++;
                                            doc.NUM_DOC = dOCUMENTO.NUM_DOC;
                                            doc.POS = i;
                                            doc.TIPO = ext.Replace(".", "");
                                            try
                                            {
                                                var clasefileM = clasefile.ToUpper();
                                                doc.CLASE = clasefileM.Substring(0, 3);
                                            }
                                            catch (Exception e)
                                            {
                                                doc.CLASE = "";
                                            }
                                            doc.STEP_WF = 1;
                                            doc.USUARIO_ID = dOCUMENTO.USUARIOC_ID;
                                            doc.PATH = path;
                                            doc.ACTIVO = true;
                                            try
                                            {
                                                db.DOCUMENTOAs.Add(doc);
                                                db.SaveChanges();
                                            }
                                            catch (Exception e)
                                            {
                                                errorfiles = "" + filename;
                                            }

                                        }
                                    }
                                }
                                indexlabel++;
                                if (errorfiles != "")
                                {
                                    errorMessage += "Error con el archivo " + errorfiles;
                                }



                            }
                        }
                        else
                        {
                            errorMessage = dir;
                        }

                        errorString = errorMessage;
                        //Guardar número de documento creado
                        Session["ERROR_FILES"] = errorMessage;
                    }
                    ProcesaFlujo2 pf = new ProcesaFlujo2();
                    //db.DOCUMENTOes.Add(dOCUMENTO);
                    //db.SaveChanges();

                    USUARIO user = db.USUARIOs.Where(a => a.ID.Equals(User.Identity.Name)).FirstOrDefault();
                    ////int rol = user.MIEMBROS.FirstOrDefault().ROL_ID;
                    try
                    {
                        //WORKFV wf = db.WORKFHs.Where(a => a.BUKRS.Equals(dOCUMENTO.SOCIEDAD_ID) & a.ROL_ID == rol).FirstOrDefault().WORKFVs.OrderByDescending(a => a.VERSION).FirstOrDefault();
                        WORKFV wf = db.WORKFHs.Where(a => a.TSOL_ID.Equals(dOCUMENTO.TSOL_ID)).FirstOrDefault().WORKFVs.OrderByDescending(a => a.VERSION).FirstOrDefault();
                        if (wf != null)
                        {
                            WORKFP wp = wf.WORKFPs.OrderBy(a => a.POS).FirstOrDefault();
                            FLUJO f = new FLUJO();
                            f.WORKF_ID = wf.ID;
                            f.WF_VERSION = wf.VERSION;
                            f.WF_POS = wp.POS;
                            f.NUM_DOC = dOCUMENTO.NUM_DOC;
                            f.POS = 1;
                            f.LOOP = 1;
                            f.USUARIOA_ID = dOCUMENTO.USUARIOC_ID;
                            f.ESTATUS = "I";
                            f.FECHAC = DateTime.Now;
                            f.FECHAM = DateTime.Now;
                            string c = pf.procesa(f, "");
                            if (c == "1")
                            {
                                //Email em = new Email();
                                //em.enviaMail(f.NUM_DOC, true);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        if (errorString == "")
                        {
                            errorString = ee.Message.ToString();
                        }
                        ViewBag.error = errorString;
                    }
                    //---------------------------------------------------------------------------------TODO CORRECTO
                    if (dOCUMENTO.DOCUMENTO_REF > 0)
                    {
                        if (dOCUMENTO.TSOL_ID != "CPR")
                        {
                            List<DOCUMENTO> dd = db.DOCUMENTOes.Where(a => a.DOCUMENTO_REF == (dOCUMENTO.DOCUMENTO_REF)).ToList();
                            List<DOCUMENTOP> ddr = db.DOCUMENTOPs.Where(a => a.NUM_DOC == (dOCUMENTO.DOCUMENTO_REF)).ToList();
                            ////decimal total = 0;
                            decimal[] totales = new decimal[ddr.Count()];
                            foreach (DOCUMENTOP dr in ddr)
                            {
                                //totales[(int)dr.POS - 1] = dr.VOLUMEN_EST * dr.MONTO_APOYO;
                                totales[(int)dr.POS - 1] = (decimal)dr.APOYO_EST;
                                foreach (DOCUMENTO d1 in dd)
                                {
                                    foreach (DOCUMENTOP dp in d1.DOCUMENTOPs)
                                    {
                                        if (dr.POS == dp.POS)
                                        {
                                            //var suma2 = dp.VOLUMEN_REAL * dp.MONTO_APOYO;
                                            var suma2 = dp.APOYO_REAL;

                                            totales[(int)dr.POS - 1] = totales[(int)dr.POS - 1] - (decimal)suma2;
                                        }
                                    }
                                }
                            }
                            foreach (decimal dec in totales)
                            {
                                if (dec > 0)
                                    return RedirectToAction("Reversa", new { id = dOCUMENTO.DOCUMENTO_REF });
                            }
                        }
                        DOCUMENTO referencia = db.DOCUMENTOes.Find(dOCUMENTO.DOCUMENTO_REF);
                        referencia.ESTATUS = "C";
                        db.Entry(referencia).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    if (errorString == "")
                    {
                        errorString = e.Message.ToString();
                    }
                    ViewBag.error = errorString;

                }
            }


            int pagina = 202; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                //Obtener datos ocultos o deshabilitados                    
                try
                {
                    p = Session["pais"].ToString();
                    ViewBag.pais = p + ".png";
                }
                catch
                {
                    ViewBag.pais = "mx.png";
                    //return RedirectToAction("Pais", "Home");
                }
                string u = User.Identity.Name;
                //string u = "admin";
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

                //tipo de solicitud
                var id_sol = db.TSOLs.Where(sol => sol.ESTATUS != "X")
                                    .Join(
                                    db.TSOLTs.Where(solt => solt.SPRAS_ID == user.SPRAS_ID),
                                    sol => sol.ID,
                                    solt => solt.TSOL_ID,
                                    (sol, solt) => new
                                    {
                                        solt.SPRAS_ID,
                                        solt.TSOL_ID,
                                        TEXT = solt.TSOL_ID + " " + solt.TXT020
                                    })
                                .ToList();

                ViewBag.TSOL_ID = new SelectList(id_sol, "TSOL_ID", "TEXT", selectedValue: dOCUMENTO.TSOL_ID);

                //                      

                //Select clasificación
                var id_clas = db.TALLs.Where(t => t.ACTIVO == true)
                                .Join(
                                db.TALLTs.Where(tallt => tallt.SPRAS_ID == user.SPRAS_ID),
                                tall => tall.ID,
                                tallt => tallt.TALL_ID,
                                (tall, tallt) => tallt)
                            .ToList();

                var id_clas_sel = db.TALLs.Where(t => t.ID == dOCUMENTO.TALL_ID).FirstOrDefault().GALL_ID;

                //Grupos de solicitud
                var id_grupo = db.GALLs.Where(g => g.ACTIVO == true)
                                .Join(
                                db.GALLTs.Where(gt => gt.SPRAS_ID == user.SPRAS_ID),
                                g => g.ID,
                                gt => gt.GALL_ID,
                                (g, gt) => new
                                {
                                    gt.SPRAS_ID,
                                    gt.GALL_ID,
                                    TEXT = g.ID + " " + gt.TXT50
                                }).ToList();

                var id_grupo_sel = id_grupo.Where(g => g.GALL_ID == id_clas_sel).FirstOrDefault().GALL_ID;

                ViewBag.GALL_ID = new SelectList(id_grupo, "GALL_ID", "TEXT", selectedValue: id_grupo_sel);

                //ViewBag.tall = db.TALLs.ToList();
                ViewBag.TALL_ID = new SelectList(id_clas, "TALL_ID", "TXT50", selectedValue: dOCUMENTO.TALL_ID);
                //Datos del país
                id_bukrs = db.SOCIEDADs.Where(soc => soc.LAND.Equals(p) && soc.ACTIVO == true).FirstOrDefault();
                var id_pais = db.PAIS.Where(pais => pais.LAND.Equals(id_bukrs.LAND)).FirstOrDefault();

                var id_states = (from st in db.STATES
                                 join co in db.COUNTRIES
                                 on st.COUNTRY_ID equals co.ID
                                 where co.SORTNAME.Equals(id_pais.LAND)
                                 select new
                                 {
                                     st.ID,
                                     st.NAME,
                                     st.COUNTRY_ID
                                 }).ToList();

                var id_waers = db.MONEDAs.Where(m => m.ACTIVO == true).ToList();

                List<TAT001.Entities.CITy> id_city = new List<TAT001.Entities.CITy>();

                //ViewBag.BUKRS = id_bukrs;
                ViewBag.SOCIEDAD_ID = id_bukrs;
                dOCUMENTO.SOCIEDAD_ID = id_bukrs.BUKRS;
                ViewBag.PAIS_ID = id_pais;
                ViewBag.STATE_ID = new SelectList(id_states, "ID", dataTextField: "NAME", selectedValue: dOCUMENTO.ESTADO);
                ViewBag.CITY_ID = new SelectList(id_city, "ID", dataTextField: "NAME");
                ViewBag.MONEDA = new SelectList(id_waers, "WAERS", dataTextField: "WAERS", selectedValue: id_bukrs.WAERS);

                //Información del cliente
                var id_clientes = db.CLIENTEs.Where(c => c.LAND.Equals(p) && c.ACTIVO == true).ToList();
                //Obtener datos del payer
                CLIENTE payer = getCliente(dOCUMENTO.PAYER_ID);

                dOCUMENTO.VKORG = payer.VKORG;
                dOCUMENTO.VTWEG = payer.VTWEG;
                dOCUMENTO.SPART = payer.SPART;
                ViewBag.STCD1 = payer.STCD1;
                ViewBag.NAME1 = payer.NAME1;

                try
                {


                    //Obtener y dar formato a fecha
                    var fecha = dOCUMENTO.FECHAD.ToString();
                    string[] words = fecha.Split(' ');
                    //DateTime theTime = DateTime.ParseExact(fecha, //"06/04/2018 12:00:00 a.m."
                    //                        "dd/MM/yyyy hh:mm:ss t.t.",
                    //                        System.Globalization.CultureInfo.InvariantCulture,
                    //                        System.Globalization.DateTimeStyles.None);

                    DateTime theTime = DateTime.ParseExact(words[0], //"06/04/2018 12:00:00 a.m."
                                            "dd/MM/yyyy",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None);
                    ViewBag.FECHAD = theTime.ToString("yyyy-MM-dd");

                }
                catch (Exception e)
                {
                    ViewBag.FECHAD = "";
                }

                ViewBag.PERIODO = dOCUMENTO.PERIODO;
                ViewBag.EJERCICIO = dOCUMENTO.EJERCICIO;


                ViewBag.PAYER_ID = new SelectList(id_clientes, "KUNNR", dataTextField: "NAME1", selectedValue: dOCUMENTO.PAYER_ID);

                //Distribución
                //Información de categorías

                var id_cat = db.CATEGORIAs.Where(c => c.ACTIVO == true)
                                .Join(
                                db.CATEGORIATs.Where(ct => ct.SPRAS_ID == user.SPRAS_ID),
                                c => c.ID,
                                ct => ct.CATEGORIA_ID,
                                (c, ct) => new
                                {
                                    ct.CATEGORIA_ID,
                                    TEXT = ct.TXT50
                                }).ToList();



                ViewBag.CATEGORIA_ID = new SelectList(id_cat, "CATEGORIA_ID", "TEXT");
                List<TAT001.Entities.CITy> id_cityy = new List<TAT001.Entities.CITy>();
                ViewBag.BASE_ID = new SelectList(id_cityy, "CATEGORIA_ID", "TEXT");
            }

            ViewBag.tcambio = dOCUMENTO.TIPO_CAMBIO;
            ViewBag.MONTO_DOC_ML2 = dOCUMENTO.MONTO_DOC_ML2;
            if (notas_soporte == null || notas_soporte == "")
            {
                notas_soporte = "";
            }
            ViewBag.notas_soporte = notas_soporte;
            ViewBag.UNAFACTURA = unafact;


            //Relacionada
            string id_d = "" + dOCUMENTO.DOCUMENTO_REF;
            decimal rel = 0;
            try
            {
                //rel = Convert.ToDecimal(Session["rel"].ToString());
                if (id_d == null || id_d.Equals(""))
                {
                    throw new Exception();
                }
                rel = Convert.ToDecimal(id_d);
                ViewBag.relacionada = "prelacionada";
                ViewBag.relacionadan = rel + "";
                ViewBag.TSOL_ANT = dOCUMENTO.TSOL_ID;

            }
            catch
            {
                rel = 0;
                ViewBag.relacionada = "";
                ViewBag.relacionadan = "";
                ViewBag.TSOL_ANT = dOCUMENTO.TSOL_ID;
            }

            //Obtener los valores de tsols
            List<TSOL> tsols_val = new List<TSOL>();
            List<TSOLT_MODBD> tsols_valbd = new List<TSOLT_MODBD>();
            try
            {
                tsols_val = db.TSOLs.ToList();
                tsols_valbd = tsols_val.Select(tsv => new TSOLT_MODBD
                {
                    ID = tsv.ID,
                    FACTURA = tsv.FACTURA
                }).ToList();
            }
            catch (Exception e)
            {

            }
            var tsols_valbdjs = JsonConvert.SerializeObject(tsols_valbd, Formatting.Indented);
            ViewBag.TSOL_VALUES = tsols_valbdjs;

            ViewBag.SEL_NEG = select_neg;
            ViewBag.SEL_DIS = select_dis;
            ViewBag.BMONTO_APOYO = bmonto_apoyo;

            //----------------------------RSG 18.05.2018
            string spras = Session["spras"].ToString();
            ViewBag.PERIODOS = new SelectList(db.PERIODOTs.Where(a => a.SPRAS_ID == spras).ToList(), "PERIODO_ID", "TXT50", DateTime.Now.Month);
            List<string> anios = new List<string>();
            int mas = 10;
            for (int i = 0; i < mas; i++)
            {
                anios.Add((DateTime.Now.Year + i).ToString());
            }
            ViewBag.ANIOS = new SelectList(anios, DateTime.Now.Year.ToString());
            dOCUMENTO.SOCIEDAD = db.SOCIEDADs.Find(dOCUMENTO.SOCIEDAD_ID);
            //----------------------------RSG 18.05.2018

            return View(dOCUMENTO);
        }

        [HttpPost]
        public FileResult Descargar(string archivo)
        {
            Models.PresupuestoModels carga = new Models.PresupuestoModels();
            string nombre = "", contentyp = "";
            carga.contDescarga(archivo, ref contentyp, ref nombre);
            return File(archivo, contentyp, nombre);
        }
        public decimal getValSoc(string waers, string moneda_id, decimal monto_doc_md, out string errorString)
        {
            decimal val = 0;

            //Siempre la conversión va a la sociedad    

            var UKURS = getUkurs(waers, moneda_id, out errorString);

            if (errorString.Equals(""))
            {

                decimal uk = Convert.ToDecimal(UKURS);

                if (UKURS > 0)
                {
                    val = uk * Convert.ToDecimal(monto_doc_md);
                }
            }

            return val;
        }

        public decimal getUkurs(string waers, string moneda_id, out string errorString)
        {
            decimal ukurs = 0;
            errorString = string.Empty;
            using (TAT001Entities db = new TAT001Entities())
            {
                try
                {
                    //Siempre la conversión va a la sociedad    
                    var date = DateTime.Now.Date;
                    var tcambio = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(waers) && t.GDATU.Equals(date)).FirstOrDefault();
                    if (tcambio == null)
                    {
                        var max = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(waers)).Max(a => a.GDATU);
                        tcambio = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(waers) && t.GDATU.Equals(max)).FirstOrDefault();
                    }

                    ukurs = Convert.ToDecimal(tcambio.UKURS);

                }
                catch (Exception e)
                {
                    errorString = "detail: conversion " + moneda_id + " to " + waers + " in date " + DateTime.Now.Date;
                    return 0;
                }
            }

            return ukurs;
        }

        public decimal getUkursUSD(string waers, string waersusd, out string errorString)
        {
            decimal ukurs = 0;
            errorString = string.Empty;
            using (TAT001Entities db = new TAT001Entities())
            {
                try
                {
                    var date = DateTime.Now.Date;
                    var tcambio = db.TCAMBIOs.Where(t => t.FCURR.Equals(waers) && t.TCURR.Equals(waersusd) && t.GDATU.Equals(date)).FirstOrDefault();
                    if (tcambio == null)
                    {
                        var max = db.TCAMBIOs.Where(t => t.FCURR.Equals(waers) && t.TCURR.Equals(waersusd)).Max(a => a.GDATU);
                        tcambio = db.TCAMBIOs.Where(t => t.FCURR.Equals(waers) && t.TCURR.Equals(waersusd) && t.GDATU.Equals(max)).FirstOrDefault();
                    }

                    ukurs = Convert.ToDecimal(tcambio.UKURS);
                }
                catch (Exception e)
                {
                    errorString = "detail: conversion " + waers + " to " + waersusd + " in date " + DateTime.Now.Date;
                    return 0;
                }

            }

            return ukurs;
        }

        public RANGO getRango(string TSOL_ID)
        {
            RANGO rango = new RANGO();
            using (TAT001Entities db = new TAT001Entities())
            {

                rango = (from r in db.RANGOes
                         join s in db.TSOLs
                         on r.ID equals s.RANGO_ID
                         where s.ID == TSOL_ID && r.ACTIVO == true
                         select r).FirstOrDefault();

            }

            return rango;

        }

        public decimal getSolID(string TSOL_ID)
        {

            decimal id = 0;

            RANGO rango = getRango(TSOL_ID);

            if (rango.ACTUAL > rango.INICIO && rango.ACTUAL < rango.FIN)
            {
                rango.ACTUAL++;
                id = (decimal)rango.ACTUAL;
            }

            return id;
        }

        public STATE getEstado(int id)
        {
            STATE state = new STATE();

            using (TAT001Entities db = new TAT001Entities())
            {

                state = (from c in db.CITIES
                         join s in db.STATES
                         on c.STATE_ID equals s.ID
                         where s.ID == id
                         select s).FirstOrDefault();

            }

            return state;
        }


        public CLIENTE getCliente(string PAYER_ID)
        {
            CLIENTE payer = new CLIENTE();

            using (TAT001Entities db = new TAT001Entities())
            {

                payer = db.CLIENTEs.Where(c => c.KUNNR.Equals(PAYER_ID)).FirstOrDefault();

            }
            return payer;

        }


        public void updateRango(string TSOL_ID, decimal actual)
        {

            RANGO rango = getRango(TSOL_ID);

            if (rango.ACTUAL > rango.INICIO && rango.ACTUAL < rango.FIN)
            {
                rango.ACTUAL = actual;
            }

            db.Entry(rango).State = EntityState.Modified;
            db.SaveChanges();

        }

        // GET: Solicitudes/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCUMENTO dOCUMENTO = db.DOCUMENTOes.Find(id);
            if (dOCUMENTO == null)
            {
                return HttpNotFound();
            }
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION", dOCUMENTO.TALL_ID);
            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", dOCUMENTO.TSOL_ID);
            ViewBag.USUARIOC_ID = new SelectList(db.USUARIOs, "ID", "PASS", dOCUMENTO.USUARIOC_ID);
            ViewBag.VKORG = new SelectList(db.CLIENTEs, "VKORG", "NAME1", dOCUMENTO.VKORG);
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS", dOCUMENTO.PAIS_ID);
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", dOCUMENTO.SOCIEDAD_ID);
            return View(dOCUMENTO);
        }

        // POST: Solicitudes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NUM_DOC,TSOL_ID,TALL_ID,SOCIEDAD_ID,PAIS_ID,PERIODO,EJERCICIO,TIPO_TECNICO,TIPO_RECURRENTE,CANTIDAD_EV,USUARIOC_ID,FECHAD,FECHAC,ESTATUS,ESTATUS_C,ESTATUS_SAP,ESTATUS_WF,DOCUMENTO_REF,NOTAS,MONTO_DOC_MD,MONTO_FIJO_MD,MONTO_BASE_GS_PCT_MD,MONTO_BASE_NS_PCT_MD,MONTO_DOC_ML,MONTO_FIJO_ML,MONTO_BASE_GS_PCT_ML,MONTO_BASE_NS_PCT_ML,MONTO_DOC_ML2,MONTO_FIJO_ML2,MONTO_BASE_GS_PCT_ML2,MONTO_BASE_NS_PCT_ML2,IMPUESTO,FECHAI_VIG,FECHAF_VIG,ESTATUS_EXT,SOLD_TO_ID,PAYER_ID,GRUPO_CTE_ID,CANAL_ID,MONEDA_ID,TIPO_CAMBIO,NO_FACTURA,FECHAD_SOPORTE,METODO_PAGO,NO_PROVEEDOR,PASO_ACTUAL,AGENTE_ACTUAL,FECHA_PASO_ACTUAL,VKORG,VTWEG,SPART,HORAC,FECHAC_PLAN,FECHAC_USER,HORAC_USER,CONCEPTO,PORC_ADICIONAL,PAYER_NOMBRE,PAYER_EMAIL,MONEDAL_ID,MONEDAL2_ID,TIPO_CAMBIOL,TIPO_CAMBIOL2")] DOCUMENTO dOCUMENTO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dOCUMENTO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TALL_ID = new SelectList(db.TALLs, "ID", "DESCRIPCION", dOCUMENTO.TALL_ID);
            ViewBag.TSOL_ID = new SelectList(db.TSOLs, "ID", "DESCRIPCION", dOCUMENTO.TSOL_ID);
            ViewBag.USUARIOC_ID = new SelectList(db.USUARIOs, "ID", "PASS", dOCUMENTO.USUARIOC_ID);
            ViewBag.VKORG = new SelectList(db.CLIENTEs, "VKORG", "NAME1", dOCUMENTO.VKORG);
            ViewBag.PAIS_ID = new SelectList(db.PAIS, "LAND", "SPRAS", dOCUMENTO.PAIS_ID);
            ViewBag.SOCIEDAD_ID = new SelectList(db.SOCIEDADs, "BUKRS", "BUTXT", dOCUMENTO.SOCIEDAD_ID);
            return View(dOCUMENTO);
        }

        // GET: Solicitudes/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOCUMENTO dOCUMENTO = db.DOCUMENTOes.Find(id);
            if (dOCUMENTO == null)
            {
                return HttpNotFound();
            }
            return View(dOCUMENTO);
        }

        // POST: Solicitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            DOCUMENTO dOCUMENTO = db.DOCUMENTOes.Find(id);
            db.DOCUMENTOes.Remove(dOCUMENTO);
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

        public ActionResult CreateNew()
        {

            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.nombre = user.NOMBRE + " " + user.APELLIDO_P + " " + user.APELLIDO_M;
                ViewBag.email = user.EMAIL;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.returnUrl = Request.UrlReferrer;
                ViewBag.usuario = user; ViewBag.returnUrl = Request.Url.PathAndQuery;;
                try
                {
                    string p = Session["pais"].ToString();
                    ViewBag.pais = p + ".png";
                }
                catch
                {
                    return RedirectToAction("Pais", "Home");
                }

            }

            return View();
        }

        public ActionResult Sol_Tipo()
        {

            var btnRadioe = Request.Form["radio_tiposol"];

            try
            {
                string p = Session["pais"].ToString();
                ViewBag.pais = p + ".png";
            }
            catch
            {
                return RedirectToAction("Pais", "Home");
            }

            if (btnRadioe != "" || btnRadioe != null)
            {
                Session["sol_tipo"] = null;
                Session["sol_tipo"] = btnRadioe;

                //return RedirectToAction("Informacion", "Solicitud", new { tipo = btnRadioe });
                return RedirectToAction("Create", "Solicitudes");
            }
            else
            {
                return RedirectToAction("Index", "Solicitudes");
            }
        }

        public ActionResult Cancelar()
        {
            Session["sol_tipo"] = null;

            return RedirectToAction("Index", "Solicitudes");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SelectCity(int id)
        {

            TAT001Entities db = new TAT001Entities();

            var id_cl = db.CITIES.Where(city => city.STATE_ID.Equals(id)).Select(c => new { ID = c.ID.ToString(), NAME = c.NAME.ToString() }).ToList();

            JsonResult jc = Json(id_cl, JsonRequestBehavior.AllowGet);
            return jc;
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult SelectTall(string id)
        {

            TAT001Entities db = new TAT001Entities();

            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

            var id_clas = db.TALLs.Where(t => t.ACTIVO == true && t.GALL_ID.Equals(id))
                                .Join(
                                db.TALLTs.Where(tallt => tallt.SPRAS_ID == user.SPRAS_ID),
                                tall => tall.ID,
                                tallt => tallt.TALL_ID,
                                (tall, tallt) => new
                                {
                                    ID = tallt.TALL_ID.ToString(),
                                    TEXT = tallt.TXT50.ToString()
                                })
                            .ToList();

            JsonResult jc = Json(id_clas, JsonRequestBehavior.AllowGet);
            return jc;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult SelectCliente(string kunnr)
        {

            TAT001Entities db = new TAT001Entities();

            CLIENTE_MOD id_cl = (from c in db.CLIENTEs
                                 join co in db.CONTACTOCs
                                 on new { c.VKORG, c.VTWEG, c.SPART, c.KUNNR } equals new { co.VKORG, co.VTWEG, co.SPART, co.KUNNR } into jjcont
                                 from co in jjcont.DefaultIfEmpty()
                                 where (c.KUNNR == kunnr & co.DEFECTO == true)
                                 select new CLIENTE_MOD
                                 {
                                     VKORG = c.VKORG,
                                     VTWEG = c.VTWEG,
                                     SPART = c.SPART,//RSG 28.05.2018-------------------
                                     NAME1 = c.NAME1,
                                     KUNNR = c.KUNNR,
                                     STCD1 = c.STCD1,
                                     PARVW = c.PARVW,
                                     BANNER = c.BANNER,
                                     CANAL = c.CANAL,
                                     PAYER_NOMBRE = co == null ? String.Empty : co.NOMBRE,
                                     PAYER_EMAIL = co == null ? String.Empty : co.EMAIL,
                                 }).FirstOrDefault();

            if (id_cl == null)
            {
                id_cl = (from c in db.CLIENTEs
                         where (c.KUNNR == kunnr)
                         select new CLIENTE_MOD
                         {
                             VKORG = c.VKORG,
                             VTWEG = c.VTWEG,
                             SPART = c.SPART,//RSG 28.05.2018-------------------
                             NAME1 = c.NAME1,
                             KUNNR = c.KUNNR,
                             STCD1 = c.STCD1,
                             PARVW = c.PARVW,
                             BANNER = c.BANNER,
                             CANAL = c.CANAL,
                             PAYER_NOMBRE = String.Empty,
                             PAYER_EMAIL = String.Empty,
                         }).FirstOrDefault();
            }

            if (id_cl != null)
            {
                //Obtener el cliente
                //CANAL canal = db.CANALs.Where(ca => ca.BANNER == id_cl.BANNER && ca.KUNNR == kunnr).FirstOrDefault();
                CANAL canal = db.CANALs.Where(ca => ca.CANAL1 == id_cl.CANAL).FirstOrDefault();
                id_cl.VTWEG = "";
                //if (canal == null)
                //{
                //    string kunnrwz = kunnr.TrimStart('0');
                //    string bannerwz = id_cl.BANNER.TrimStart('0');
                //    canal = db.CANALs.Where(ca => ca.BANNER == bannerwz && ca.KUNNR == kunnrwz).FirstOrDefault();
                //}

                if (canal != null)
                {
                    id_cl.VTWEG = canal.CANAL1 + " - " + canal.CDESCRIPCION;
                }

                //Obtener el tipo de cliente
                var clientei = (from c in db.TCLIENTEs
                                join ct in db.TCLIENTETs
                                on c.ID equals ct.PARVW_ID
                                where c.ID == id_cl.PARVW && c.ACTIVO == true
                                select ct).FirstOrDefault();
                id_cl.PARVW = "";
                if (clientei != null)
                {
                    id_cl.PARVW = clientei.TXT50;
                }

            }

            JsonResult jc = Json(id_cl, JsonRequestBehavior.AllowGet);
            return jc;
        }

        [HttpPost]
        [AllowAnonymous]
        public string SelectTcambio(string fcurr)
        {
            string p = "";
            string errorString = "";
            decimal tcambio = 0;
            string tcurr = "USD";
            SOCIEDAD id_bukrs = new SOCIEDAD();
            try
            {
                p = Session["pais"].ToString();
            }
            catch
            {
            }

            TAT001Entities db = new TAT001Entities();
            try
            {
                id_bukrs = db.SOCIEDADs.Where(soc => soc.LAND.Equals(p)).FirstOrDefault();
                var date = DateTime.Now.Date;
                //var tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(date)).FirstOrDefault().UKURS;
                var tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(date)).FirstOrDefault();
                if (tc == null)
                {
                    var max = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr)).Max(a => a.GDATU);
                    tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(max)).FirstOrDefault();
                }

                tcambio = Convert.ToDecimal(tc.UKURS);

            }
            catch (Exception e)
            {
                errorString = e.Message + "detail: conversion " + fcurr + " to " + tcurr + " in date " + DateTime.Now.Date;
                //throw new System.Exception(errorString);
                return errorString;
            }

            return Convert.ToString(tcambio);
        }

        [HttpPost]
        [AllowAnonymous]
        public string SelectVcambio(string moneda_id, decimal monto_doc_md)
        {
            string p = "";
            string tcurr = "USD";
            string errorString = "";
            decimal monto = 0;
            try
            {
                p = Session["pais"].ToString();
            }
            catch
            {
            }

            TAT001Entities db = new TAT001Entities();

            var id_bukrs = db.SOCIEDADs.Where(soc => soc.LAND.Equals(p)).FirstOrDefault();
            try
            {
                var date = DateTime.Now.Date;
                //var UKURS = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(date)).FirstOrDefault().UKURS;
                var tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(date)).FirstOrDefault();
                if (tc == null)
                {
                    var max = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(tcurr)).Max(a => a.GDATU);
                    tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(max)).FirstOrDefault();
                }

                decimal uk = Convert.ToDecimal(tc.UKURS);

                if (tc.UKURS > 0)
                {
                    monto = Convert.ToDecimal(monto_doc_md) / uk;
                }
            }
            catch (Exception e)
            {
                errorString = e.Message + "detail: conversion " + moneda_id + " to " + tcurr + " in date " + DateTime.Now.Date;
                //throw new System.Exception(errorString);
                return errorString;
            }
            return Convert.ToString(monto);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadExcel()
        {
            List<DOCUMENTOP_MOD> ld = new List<DOCUMENTOP_MOD>();


            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                //using (var stream2 = System.IO.File.Open(url, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                //{
                string extension = System.IO.Path.GetExtension(file.FileName);
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                //using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream))
                //{
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream);
                // 2. Use the AsDataSet extension method
                DataSet result = reader.AsDataSet();

                // The result of each spreadsheet is in result.Tables
                // 3.DataSet - Create column names from first row
                DataTable dt = result.Tables[0];

                //Rows
                var rowsc = dt.Rows.Count;
                //columns
                var columnsc = dt.Columns.Count;

                //Columnd and row to start
                var rows = 1; // 2
                              //var cols = 0; // A
                var pos = 1;

                for (int i = rows; i < rowsc; i++)
                {
                    //for (var j = 0; j < columnsc; j++)
                    //{
                    //    var data = dt.Rows[i][j];
                    //}
                    if (i >= 4)
                    {
                        var v = dt.Rows[i][1];
                        if (Convert.ToString(v) == "")
                        {
                            break;
                        }
                    }
                    DOCUMENTOP_MOD doc = new DOCUMENTOP_MOD();

                    //Rows variables
                    double monto = 0;
                    double porc_apoyo = 0;
                    //double monto_apoyo = 0;
                    //double montoc_apoyo = 0;
                    double precio_sug = 0;
                    double volumen_est = 0;
                    //double porc_apoyoest = 0;


                    //var poss = dt.Rows[i][1];
                    string a = Convert.ToString(pos);


                    doc.POS = Convert.ToDecimal(a);
                    try
                    {
                        doc.VIGENCIA_DE = Convert.ToDateTime(dt.Rows[i][0]); //DEL
                    }
                    catch (Exception e)
                    {
                        doc.VIGENCIA_DE = null;
                    }
                    try
                    {
                        doc.VIGENCIA_AL = Convert.ToDateTime(dt.Rows[i][1]); //AL
                    }
                    catch (Exception e)
                    {
                        doc.VIGENCIA_AL = null;
                    }
                    try
                    {
                        doc.MATNR = dt.Rows[i][2].ToString(); //Material
                        MATERIAL mat = material(doc.MATNR);
                        if (mat != null)//Validar si el material existe
                        {
                            //doc.MATKL = (string)dt.Rows[i][4]; //Categoría se toma de la bd
                            CATEGORIAT cat = getCategoriaS(material: doc.MATNR); //Categoría
                            try
                            {
                                doc.MATKL = cat.TXT50.ToString();
                                doc.MATKL_ID = cat.CATEGORIA_ID.ToString();
                            }
                            catch (Exception e)
                            {
                                doc.MATKL = "";
                                doc.MATKL_ID = "";
                            }
                            //doc.DESC = (string)dt.Rows[i][5]; //Descripción se toma de la bd
                            doc.DESC = mat.MAKTX.ToString(); //Descripción
                            doc.ACTIVO = true;
                        }
                        else
                        {
                            doc.ACTIVO = false;
                        }
                    }
                    catch (Exception e)
                    {
                        doc.ACTIVO = false;
                    }
                    try
                    {
                        monto = (double)dt.Rows[i][3]; //Costo unitario    
                    }
                    catch (Exception e)
                    {
                        monto = 0;
                    }
                    doc.MONTO = Convert.ToDecimal(monto);
                    //doc.MONTO = Math.Round(doc.MONTO, 2);
                    try
                    {
                        porc_apoyo = (double)dt.Rows[i][4]; //% apoyo
                        porc_apoyo = porc_apoyo * 100;
                    }
                    catch (Exception e)
                    {
                        porc_apoyo = 0;
                    }
                    doc.PORC_APOYO = Convert.ToDecimal(porc_apoyo);
                    //doc.PORC_APOYO = Math.Round(doc.PORC_APOYO, 2);
                    //try
                    //{
                    //    monto_apoyo = (double)dt.Rows[i][8]; //Apoyo por pieza
                    //}
                    //catch (Exception e)
                    //{
                    //    monto_apoyo = 0;
                    //}
                    //    doc.MONTO_APOYO = Convert.ToDecimal(monto_apoyo);
                    //try
                    //{
                    //    montoc_apoyo = (double)dt.Rows[i][9]; //Costo con apoyo
                    //}
                    //catch (Exception e)
                    //{
                    //    montoc_apoyo = 0;
                    //}
                    //    doc.MONTOC_APOYO = Convert.ToDecimal(montoc_apoyo);
                    try
                    {
                        precio_sug = (double)dt.Rows[i][5]; //Precio sugerido
                    }
                    catch (Exception e)
                    {
                        precio_sug = 0;
                    }
                    doc.PRECIO_SUG = Convert.ToDecimal(precio_sug);
                    //doc.PRECIO_SUG = Math.Round(doc.PRECIO_SUG, 2);
                    try
                    {
                        volumen_est = (double)dt.Rows[i][6]; //Volumen estimado
                    }
                    catch (Exception e)
                    {
                        volumen_est = 0;
                    }
                    doc.VOLUMEN_EST = Convert.ToDecimal(volumen_est);
                    //doc.VOLUMEN_EST = Math.Round(doc.VOLUMEN_EST, 2);
                    //try
                    //{
                    //    //porc_apoyoest = (double)dt.Rows[i][12]; //Estimado $ apoyo
                    //}catch(Exception e)
                    //{
                    //    porc_apoyoest = 0;
                    //}
                    //doc.PORC_APOYOEST = Convert.ToDecimal(porc_apoyoest);

                    //RSG 24.05.2018--------------------------------- 
                    try
                    {
                        string apoyo = dt.Rows[i][7].ToString();
                        doc.APOYO_EST = (decimal.Parse(apoyo));//Apoyo
                    }
                    catch
                    {
                        doc.APOYO_EST = 0;
                    }
                    //RSG 24.05.2018----------------------------------
                    ld.Add(doc);
                    pos++;
                }

                reader.Close();

            }
            JsonResult jl = Json(ld, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadExcelSop()
        {
            List<DOCUMENTOF_MOD> ld = new List<DOCUMENTOF_MOD>();


            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                //using (var stream2 = System.IO.File.Open(url, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                //{
                string extension = System.IO.Path.GetExtension(file.FileName);
                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                //using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream))
                //{
                IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream);
                // 2. Use the AsDataSet extension method
                DataSet result = reader.AsDataSet();

                // The result of each spreadsheet is in result.Tables
                // 3.DataSet - Create column names from first row
                DataTable dt = result.Tables[0];

                //Rows
                var rowsc = dt.Rows.Count;
                //columns
                var columnsc = dt.Columns.Count;

                //Columnd and row to start
                var rows = 1; // 2
                              //var cols = 0; // A
                int pos = 1;

                for (int i = rows; i < rowsc; i++)
                {

                    DOCUMENTOF_MOD doc = new DOCUMENTOF_MOD();

                    doc.POS = pos;
                    try
                    {
                        doc.FACTURA = (string)dt.Rows[i][0]; //Factura
                    }
                    catch (Exception e)
                    {
                        doc.FACTURA = null;
                    }
                    try
                    {
                        doc.FECHA = Convert.ToDateTime(dt.Rows[i][1]); //Fecha
                    }
                    catch (Exception e)
                    {
                        doc.FECHA = null;
                    }
                    try
                    {
                        var provs = dt.Rows[i][2];
                        doc.PROVEEDOR = provs + ""; //Proveedor
                        PROVEEDOR prov = proveedor(doc.PROVEEDOR);
                        if (prov != null)//Validar si el proveedor existe
                        {

                            doc.PROVEEDOR_TXT = prov.NOMBRE.ToString(); //Descripción
                            doc.PROVEEDOR_ACTIVO = true;
                        }
                        else
                        {
                            doc.PROVEEDOR_ACTIVO = false;
                        }
                    }
                    catch (Exception e)
                    {
                        doc.PROVEEDOR_ACTIVO = false;
                    }
                    try
                    {
                        doc.CONTROL = (string)dt.Rows[i][3]; //Control   
                    }
                    catch (Exception e)
                    {
                        doc.CONTROL = "";
                    }
                    try
                    {
                        doc.AUTORIZACION = (string)dt.Rows[i][4]; //Autorización                        
                    }
                    catch (Exception e)
                    {
                        doc.AUTORIZACION = "";
                    }
                    try
                    {
                        doc.VENCIMIENTO = Convert.ToDateTime(dt.Rows[i][5]); //Vencimiento
                    }
                    catch (Exception e)
                    {
                        doc.VENCIMIENTO = null;
                    }
                    try
                    {
                        doc.FACTURAK = (string)dt.Rows[i][6]; //Facturak
                    }
                    catch (Exception e)
                    {
                        doc.FACTURAK = null;
                    }
                    try
                    {
                        int ej = Convert.ToInt32(dt.Rows[i][7]);
                        doc.EJERCICIOK = Convert.ToString(ej); //Ejerciciok
                    }
                    catch (Exception e)
                    {
                        doc.EJERCICIOK = null;
                    }
                    try
                    {
                        doc.BILL_DOC = (string)dt.Rows[i][8]; //Bill_doc
                    }
                    catch (Exception e)
                    {
                        doc.BILL_DOC = null;
                    }
                    try
                    {
                        doc.BELNR = (string)dt.Rows[i][9]; //Belnr
                    }
                    catch (Exception e)
                    {
                        doc.BELNR = null;
                    }

                    ld.Add(doc);
                    pos++;
                }

                reader.Close();

            }
            JsonResult jl = Json(ld, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult LoadConfigSoporte(string sociedad, string pais, string tsol, string nulos)
        {
            if (nulos == null)
            {
                nulos = "";
            }
            FACTURASCONF_MOD fc = new FACTURASCONF_MOD();

            try
            {
                FACTURASCONF f = db.FACTURASCONFs.Where(fi => fi.SOCIEDAD_ID.Equals(sociedad) && fi.PAIS_ID.Equals(pais) && fi.TSOL.Equals(tsol)).FirstOrDefault();

                if (f != null)
                {
                    fc.NUM_DOC = null;
                    fc.POS = true;
                    fc.SOCIEDAD_ID = f.SOCIEDAD_ID;
                    fc.PAIS_ID = f.PAIS_ID;
                    fc.TSOL = f.TSOL;
                    fc.FACTURA = f.FACTURA;
                    fc.FECHA = f.FECHA;
                    fc.PROVEEDOR = f.PROVEEDOR;
                    fc.PROVEEDOR_TXT = f.PROVEEDOR;
                    fc.CONTROL = f.CONTROL;
                    fc.AUTORIZACION = f.AUTORIZACION;
                    fc.VENCIMIENTO = f.VENCIMIENTO;
                    fc.FACTURAK = f.FACTURAK;
                    fc.EJERCICIOK = f.EJERCICIOK;
                    fc.BILL_DOC = f.BILL_DOC;
                    fc.BELNR = f.BELNR;

                    //fc.FACTURA = true; 
                    //fc.FECHA = true; 
                    //fc.PROVEEDOR = true;
                    //fc.PROVEEDOR_TXT = true; 
                    //fc.CONTROL = true;
                    //fc.AUTORIZACION = true;
                    //fc.VENCIMIENTO = true;
                    //fc.FACTURAK = true;
                    //fc.EJERCICIOK = true;
                    //fc.BILL_DOC = true;
                    //fc.BELNR = true;
                }
            }
            catch
            {

            }

            if (nulos.Equals("X"))
            {

                fc.PROVEEDOR_TXT = null;
                fc.NUM_DOC = 0;
                fc.SOCIEDAD_ID = null;
                fc.PAIS_ID = null;
                fc.TSOL = null;

            }

            JsonResult jl = Json(fc, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult categoriaMateriales(string kunnr, string catid, string soc_id)
        {
            if (kunnr == null)
            {
                kunnr = "";
            }

            if (catid == null)
            {
                catid = "";
            }

            List<PRESUPSAPP> jdl = new List<PRESUPSAPP>();

            //Obtener los materiales
            IEnumerable<MATERIAL> matl = Enumerable.Empty<MATERIAL>();
            try
            {
                if (catid != "000")//RSG 05.06.2018
                    matl = db.MATERIALs.Where(m => m.MATERIALGP_ID == catid && m.ACTIVO == true);//.Select(m => m.ID).ToList();
                else
                    matl = db.MATERIALs.Where(m => m.ACTIVO == true);//.Select(m => m.ID).ToList();
            }
            catch (Exception)
            {

            }

            //Validar si hay materiales
            string campoconf = "";
            if (matl != null)
            {

                CLIENTE cli = new CLIENTE();
                List<CLIENTE> clil = new List<CLIENTE>();

                try
                {
                    cli = db.CLIENTEs.Where(c => c.KUNNR == kunnr).FirstOrDefault();

                    //Saber si el cliente es sold to, payer o un grupo
                    if (cli != null)
                    {
                        //Es un soldto
                        if (cli.KUNNR != cli.PAYER && cli.KUNNR != cli.BANNER)
                        {
                            //cli.VKORG = cli.VKORG+" ";
                            clil.Add(cli);
                        }
                    }
                }
                catch (Exception)
                {

                }

                var cie = clil.Cast<CLIENTE>();
                //    IEnumerable<CLIENTE> cie = clil as IEnumerable<CLIENTE>;
                //Obtener el numero de periodos para obtener el historial
                int nummonths = 0;
                int imonths = 0;

                try
                {
                    CONFDIST_CAT conf = getCatConf(soc_id);
                    nummonths = (int)conf.PERIODOS;
                    campoconf = conf.CAMPO.ToString();

                }
                catch (Exception)
                {

                }
                if (nummonths > 0)
                {
                    imonths = nummonths * -1;
                }
                //Obtener el rango de los periodos incluyendo el año
                DateTime ff = DateTime.Today;
                DateTime fi = ff.AddMonths(imonths);

                string mi = fi.Month.ToString();//.ToString("MM");
                string ai = fi.Year.ToString();//.ToString("yyyy");

                string mf = ff.Month.ToString();// ("MM");
                string af = ff.Year.ToString();// "yyyy");

                int aii = 0;
                try
                {
                    aii = Convert.ToInt32(ai);
                }
                catch (Exception)
                {

                }

                int mii = 0;
                try
                {
                    mii = Convert.ToInt32(mi);
                }
                catch (Exception)
                {

                }

                int aff = 0;
                try
                {
                    aff = Convert.ToInt32(af);
                }
                catch (Exception)
                {

                }

                int mff = 0;
                try
                {
                    mff = Convert.ToInt32(mf);
                }
                catch (Exception)
                {

                }

                if (cie != null)
                {
                    //Obtener el historial de compras de los clientesd
                    var matt = matl.ToList();
                    //var pres = db.PRESUPSAPPs.ToList();
                    ////kunnr = kunnr.TrimStart('0').Trim();
                    var pres = db.PRESUPSAPPs.Where(a => a.VKORG.Equals(cli.VKORG) & a.SPART.Equals(cli.SPART) & a.KUNNR == kunnr).ToList();
                    ////foreach (var c in cie)
                    ////{
                    ////    c.KUNNR = c.KUNNR.TrimStart('0').Trim();
                    ////}

                    //jd = (from ps in db.PRESUPSAPPs.ToList()
                    jdl = (from ps in pres
                           join cl in cie
                           on ps.KUNNR equals cl.KUNNR
                           join m in matt
                           on ps.MATNR equals m.ID
                           where (ps.ANIO >= aii && ps.PERIOD >= mii) && (ps.ANIO <= aff && ps.PERIOD <= mff) &&
                           (ps.VKORG == cl.VKORG && ps.VTWEG == cl.VTWEG && ps.SPART == cl.SPART //&& ps.VKBUR == cl.VKBUR &&
                                                                                                 //ps.VKGRP == cl.VKGRP && ps.BZIRK == cl.BZIRK
                           ) && ps.BUKRS == soc_id
                           select new PRESUPSAPP
                           {
                               ID = ps.ID,
                               ANIO = ps.ANIO,
                               POS = ps.POS,
                               PERIOD = ps.PERIOD,
                               MATNR = ps.MATNR,
                               VVX17 = ps.VVX17,
                               CSHDC = ps.CSHDC,
                               RECUN = ps.RECUN,
                               DSTRB = ps.DSTRB,
                               OTHTA = ps.OTHTA,
                               ADVER = ps.ADVER,
                               CORPM = ps.CORPM,
                               POP = ps.POP,
                               OTHER = ps.OTHER,
                               CONPR = ps.CONPR,
                               OHV = ps.OHV,
                               FREEG = ps.FREEG,
                               RSRDV = ps.RSRDV,
                               SPA = ps.SPA,
                               PMVAR = ps.PMVAR,
                               GRSLS = ps.GRSLS,
                               NETLB = ps.NETLB
                           }).ToList();
                }
            }

            //var jll = db.PRESUPSAPPs.Select(psl => new { MATNR = psl.MATNR.ToString() }).Take(7).ToList();

            //List<PRESUPSAPP> lps = jd;

            List<PRESUPSAPP> jdlret = new List<PRESUPSAPP>();

            foreach (PRESUPSAPP p in jdl)
            {
                var pd = p.GetType().GetProperties();

                var v = pd.Where(x => x.Name == campoconf).Single().GetValue(p);

                decimal val = Convert.ToDecimal(v);

                if (val > 0)
                {
                    PRESUPSAPP pp = jdlret.Where(a => a.MATNR == p.MATNR).FirstOrDefault();
                    if (pp == null)
                    {
                        jdlret.Add(p);
                    }
                }
            }



            JsonResult jl = Json(jdlret, JsonRequestBehavior.AllowGet);
            return jl;
        }


        public List<DOCUMENTOM> addCatItems(string kunnr, string catid, string soc_id, decimal numdoc, int posid, DateTime? vig_de, DateTime? vig_a)
        {
            if (kunnr == null)
            {
                kunnr = "";
            }

            if (catid == null)
            {
                catid = "";
            }

            //var jd = (dynamic)null;

            //List<DOCUMENTOM> jd = new List<DOCUMENTOM>();
            List<PRESUPSAPP> jdl = new List<PRESUPSAPP>();
            //Obtener los materiales
            IEnumerable<MATERIAL> matl = Enumerable.Empty<MATERIAL>();
            try
            {
                if (catid != "000")//RSG 05.06.2018
                    matl = db.MATERIALs.Where(m => m.MATERIALGP_ID == catid && m.ACTIVO == true);//.Select(m => m.ID).ToList();
                else
                    matl = db.MATERIALs.Where(m => m.ACTIVO == true);//.Select(m => m.ID).ToList();
            }
            catch (Exception)
            {

            }

            //Validar si hay materiales
            string campoconf = "";
            if (matl != null)
            {

                CLIENTE cli = new CLIENTE();
                List<CLIENTE> clil = new List<CLIENTE>();

                try
                {
                    cli = db.CLIENTEs.Where(c => c.KUNNR == kunnr).FirstOrDefault();

                    //Saber si el cliente es sold to, payer o un grupo
                    if (cli != null)
                    {
                        //Es un soldto
                        if (cli.KUNNR != cli.PAYER && cli.KUNNR != cli.BANNER)
                        {
                            clil.Add(cli);
                        }
                    }
                }
                catch (Exception)
                {

                }

                var cie = clil.Cast<CLIENTE>();
                //    IEnumerable<CLIENTE> cie = clil as IEnumerable<CLIENTE>;
                //Obtener el numero de periodos para obtener el historial
                int nummonths = 0;
                int imonths = 0;
                try
                {
                    CONFDIST_CAT conf = getCatConf(soc_id);
                    nummonths = (int)conf.PERIODOS;
                    campoconf = conf.CAMPO.ToString();
                }
                catch (Exception)
                {

                }
                if (nummonths > 0)
                {
                    imonths = nummonths * -1;
                }
                //Obtener el rango de los periodos incluyendo el año
                DateTime ff = DateTime.Today;
                DateTime fi = ff.AddMonths(imonths);

                string mi = fi.Month.ToString();//.ToString("MM");
                string ai = fi.Year.ToString();//.ToString("yyyy");

                string mf = ff.Month.ToString();// ("MM");
                string af = ff.Year.ToString();// "yyyy");

                int aii = 0;
                try
                {
                    aii = Convert.ToInt32(ai);
                }
                catch (Exception)
                {

                }

                int mii = 0;
                try
                {
                    mii = Convert.ToInt32(mi);
                }
                catch (Exception)
                {

                }

                int aff = 0;
                try
                {
                    aff = Convert.ToInt32(af);
                }
                catch (Exception)
                {

                }

                int mff = 0;
                try
                {
                    mff = Convert.ToInt32(mf);
                }
                catch (Exception)
                {

                }

                if (cie != null)
                {
                    //Obtener el historial de compras de los clientesd
                    var matt = matl.ToList();
                    //var pres = db.PRESUPSAPPs.ToList();
                    ////kunnr = kunnr.TrimStart('0').Trim();
                    var pres = db.PRESUPSAPPs.Where(a => a.VKORG.Equals(cli.VKORG) & a.SPART.Equals(cli.SPART) & a.KUNNR == kunnr).ToList();
                    List<CLIENTE> ciee = new List<CLIENTE>();
                    foreach (var c in cie)
                    {
                        CLIENTE pa = new CLIENTE();
                        pa.VKORG = c.VKORG;
                        pa.VTWEG = c.VTWEG;
                        pa.SPART = c.SPART;
                        //pa.KUNNR = c.KUNNR.TrimStart('0').Trim();
                        pa.KUNNR = c.KUNNR;
                        ciee.Add(pa);
                    }

                    jdl = (from ps in pres
                           join cl in ciee
                           on ps.KUNNR equals cl.KUNNR
                           join m in matt
                           on ps.MATNR equals m.ID
                           where (ps.ANIO >= aii && ps.PERIOD >= mii) && (ps.ANIO <= aff && ps.PERIOD <= mff) &&
                           (ps.VKORG == cl.VKORG && ps.VTWEG == cl.VTWEG && ps.SPART == cl.SPART //&& ps.VKBUR == cl.VKBUR &&
                                                                                                 //ps.VKGRP == cl.VKGRP && ps.BZIRK == cl.BZIRK
                           ) && ps.BUKRS == soc_id
                           select new PRESUPSAPP
                           {
                               ID = ps.ID,
                               ANIO = ps.ANIO,
                               POS = ps.POS,
                               PERIOD = ps.PERIOD,
                               MATNR = ps.MATNR,
                               VVX17 = ps.VVX17,
                               CSHDC = ps.CSHDC,
                               RECUN = ps.RECUN,
                               DSTRB = ps.DSTRB,
                               OTHTA = ps.OTHTA,
                               ADVER = ps.ADVER,
                               CORPM = ps.CORPM,
                               POP = ps.POP,
                               OTHER = ps.OTHER,
                               CONPR = ps.CONPR,
                               OHV = ps.OHV,
                               FREEG = ps.FREEG,
                               RSRDV = ps.RSRDV,
                               SPA = ps.SPA,
                               PMVAR = ps.PMVAR,
                               GRSLS = ps.GRSLS,
                               NETLB = ps.NETLB
                           }).ToList();

                }
            }

            //var jll = db.PRESUPSAPPs.Select(psl => new { MATNR = psl.MATNR.ToString() }).Take(7).ToList();

            List<DOCUMENTOM> jdlret = new List<DOCUMENTOM>();

            foreach (PRESUPSAPP p in jdl)
            {
                var pd = p.GetType().GetProperties();

                var v = pd.Where(x => x.Name == campoconf).Single().GetValue(p);

                decimal val = Convert.ToDecimal(v);

                if (val > 0)
                {
                    DOCUMENTOM dm = new DOCUMENTOM();
                    dm = jdlret.Where(a => a.MATNR == p.MATNR).FirstOrDefault();
                    if (dm == null)
                    {
                        dm = new DOCUMENTOM();
                        dm.NUM_DOC = numdoc;
                        dm.POS_ID = posid;
                        dm.MATNR = p.MATNR;
                        dm.VIGENCIA_DE = vig_de;
                        dm.VIGENCIA_A = vig_a;

                        jdlret.Add(dm);
                    }
                }
            }



            return jdlret;
        }

        public CONFDIST_CAT getCatConf(string soc)
        {
            CONFDIST_CAT conf = new CONFDIST_CAT();

            try
            {
                conf = db.CONFDIST_CAT.Where(c => c.SOCIEDAD_ID == soc && c.ACTIVO == true).FirstOrDefault();
            }
            catch (Exception)
            {

            }

            return conf;
        }



        [HttpPost]
        [AllowAnonymous]
        public string saveFiles()
        {
            var res = "";
            string error = "";
            string path = "";
            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    string url = ConfigurationManager.AppSettings["URL_SAVE"];
                    HttpPostedFileBase file = Request.Files[i];
                    string filename = file.FileName;
                    res = SaveFile(file, url, "100", out error, out path);
                }
            }

            return res;
        }

        public string createDir(string path, string documento)
        {

            string ex = "";

            // Specify the path to save the uploaded file to.
            string savePath = path + documento + "\\";

            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath;

            try
            {
                //using (Impersonation.LogonUser("192.168.1.77", "EQUIPO", "0906", LogonType.NewCredentials))//RSG 05.06.2018
                //{
                if (!System.IO.File.Exists(pathToCheck))
                {
                    //No existe, se necesita crear
                    DirectoryInfo dir = new DirectoryInfo(pathToCheck);

                    dir.Create();

                }
                //}

                //file.SaveAs(Server.MapPath(savePath)); //Guardarlo el cualquier parte dentro del proyecto <add key="URL_SAVE" value="\Archivos\" />
                //System.IO.File.Create(savePath,100,FileOptions.DeleteOnClose, )
                //System.IO.File.Copy(copyFrom, savePath);
                //f.CopyTo(savePath,true);
            }
            catch (Exception e)
            {
                ex = "No se puede crear el directorio para guardar los archivos";
            }

            return ex;
        }

        public string SaveFile(HttpPostedFileBase file, string path, string documento, out string exception, out string pathsaved)
        {
            string ex = "";
            //string exdir = "";
            // Get the name of the file to upload.
            string fileName = file.FileName;//System.IO.Path.GetExtension(file.FileName);    // must be declared in the class above

            // Specify the path to save the uploaded file to.
            string savePath = path + documento + "\\";

            // Create the path and file name to check for duplicates.
            string pathToCheck = savePath;

            // Append the name of the file to upload to the path.
            savePath += fileName;

            // Call the SaveAs method to save the uploaded
            // file to the specified directory.
            //file.SaveAs(Server.MapPath(savePath));

            //file to domain
            //Parte para guardar archivo en el servidor
            //using (Impersonation.LogonUser("192.168.1.77", "EQUIPO", "0906", LogonType.NewCredentials))//RSG 05.06.2018
            //{
            //fileName = file.SaveAs(file, Server.MapPath("~/Nueva carpeta/") + file.FileName);
            try
            {


                //Guardar el archivo
                file.SaveAs(savePath);


            }
            catch (Exception e)
            {
                ex = "";
                ex = fileName;
            }
            //}

            //Guardarlo en la base de datos
            if (ex == "")
            {

            }
            pathsaved = savePath;
            exception = ex;
            return fileName;
        }

        [HttpPost]
        [AllowAnonymous]
        public string selectMatCat(string catid)
        {

            return "dfdf";
        }

        [HttpPost]
        [AllowAnonymous]
        public string cambioCurr(string fcurr, string tcurr, string monto)
        {
            string p = "";
            string errorString = "";
            decimal montoret = 0;
            try
            {
                p = Session["pais"].ToString();
            }
            catch
            {
            }

            TAT001Entities db = new TAT001Entities();

            var id_bukrs = db.SOCIEDADs.Where(soc => soc.LAND.Equals(p)).FirstOrDefault();
            try
            {
                var date = DateTime.Now.Date;
                //var UKURS = db.TCAMBIOs.Where(t => t.FCURR.Equals(moneda_id) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(date)).FirstOrDefault().UKURS;
                var tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(date)).FirstOrDefault();
                if (tc == null)
                {
                    var max = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr)).Max(a => a.GDATU);
                    tc = db.TCAMBIOs.Where(t => t.FCURR.Equals(fcurr) && t.TCURR.Equals(tcurr) && t.GDATU.Equals(max)).FirstOrDefault();
                }

                decimal uk = Convert.ToDecimal(tc.UKURS);

                if (tc.UKURS > 0)
                {
                    montoret = Convert.ToDecimal(monto) / uk;
                }
            }
            catch (Exception e)
            {
                errorString = e.Message + "detail: conversion " + fcurr + " to " + tcurr + " in date " + DateTime.Now.Date;
                //throw new System.Exception(errorString);
                return errorString;
            }
            return Convert.ToString(montoret);
        }

        [HttpPost]
        public JsonResult materiales(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from m in db.MATERIALs
                     where m.ID.Contains(Prefix) && m.ACTIVO == true
                     select new { m.ID, m.MAKTX }).ToList();
            if (c.Count == 0)
            {
                var c2 = (from m in db.MATERIALs
                          where m.MAKTX.Contains(Prefix) && m.ACTIVO == true
                          select new { m.ID, m.MAKTX }).ToList();
                c.AddRange(c2);
            }
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpPost]
        public JsonResult proveedores(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from m in db.PROVEEDORs
                     where m.ID.Contains(Prefix)
                     select new { m.ID, m.NOMBRE }).ToList();
            if (c.Count == 0)
            {
                var c2 = (from m in db.PROVEEDORs
                          where m.NOMBRE.Contains(Prefix)
                          select new { m.ID, m.NOMBRE }).ToList();
                c.AddRange(c2);
            }
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        public MATERIAL material(string material)
        {
            if (material == null)
                material = "";

            TAT001Entities db = new TAT001Entities();

            MATERIAL mat = db.MATERIALs.Where(m => m.ID == material).FirstOrDefault();

            return mat;
        }

        public PROVEEDOR proveedor(string proveedor)
        {
            if (proveedor == null)
                proveedor = "";

            PROVEEDOR pro = db.PROVEEDORs.Where(p => p.ID == proveedor).FirstOrDefault();

            return pro;
        }


        [HttpPost]
        public JsonResult getMaterial(string mat)
        {
            if (mat == null)
                mat = "";
            //RSG 07.06.2018---------------------------------------------
            mat = completaMaterial(mat);
            //RSG 07.06.2018---------------------------------------------

            MaterialVal matt = db.MATERIALs.Where(m => m.ID == mat && m.ACTIVO == true).Select(m => new MaterialVal { ID = m.ID.ToString(), MATKL_ID = m.MATKL_ID.ToString(), MAKTX = m.MAKTX.ToString() }).FirstOrDefault();

            JsonResult cc = Json(matt, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpPost]
        public JsonResult getProveedor(string prov)
        {
            if (prov == null)
                prov = "";

            ProveedorVal provv = db.PROVEEDORs.Where(m => m.ID == prov).Select(m => new ProveedorVal { ID = m.ID.ToString(), NOMBRE = m.NOMBRE.ToString() }).FirstOrDefault();

            JsonResult cc = Json(provv, JsonRequestBehavior.AllowGet);
            return cc;
        }

        //[HttpPost]
        //public JsonResult getPresupuesto(string kunnr)
        //{
        //    if (kunnr == null)
        //        kunnr = "";

        //    PRESUPUESTO_MOD pm = new PRESUPUESTO_MOD();

        //    //Obtener presupuesto
        //    try
        //    {
        //        var presupuesto = db.CSP_PRESU_CLIENT(cLIENTE: kunnr, pERIODO: "1").Select(p => new { DESC = p.DESCRIPCION.ToString(), VAL = p.VALOR.ToString() }).ToList();

        //        if (presupuesto != null)
        //        {
        //            pm.P_CANAL = presupuesto[0].VAL;
        //            pm.P_BANNER = presupuesto[1].VAL;
        //            pm.PC_C = presupuesto[4].VAL;
        //            pm.PC_A = presupuesto[5].VAL;
        //            pm.PC_P = presupuesto[6].VAL;
        //            pm.PC_T = presupuesto[7].VAL;
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    JsonResult cc = Json(pm, JsonRequestBehavior.AllowGet);
        //    return cc;
        //}



        [HttpPost]
        [AllowAnonymous]
        public JsonResult getCategoria(string material)
        {
            if (material == null)
                material = "";

            //RSG 07.06.2018---------------------------------------------
            material = completaMaterial(material);
            //RSG 07.06.2018---------------------------------------------

            TAT001Entities db = new TAT001Entities();

            MATERIAL m = db.MATERIALs.Where(mat => mat.ID.Equals(material)).FirstOrDefault();
            //var cat = new CATEGORIAT();
            var cat = (dynamic)null;
            if (m != null && m.MATKL_ID != "")
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                cat = db.CATEGORIAs.Where(c => c.ID == m.MATKL_ID && c.ACTIVO == true)
                            .Join(
                            db.CATEGORIATs.Where(ct => ct.SPRAS_ID == user.SPRAS_ID),
                            c => c.ID,
                            ct => ct.CATEGORIA_ID,
                            (c, ct) => new
                            {
                                SPRAS_ID = ct.SPRAS_ID.ToString(),
                                CATEGORIA_ID = ct.CATEGORIA_ID.ToString(),
                                TXT50 = ct.TXT50.ToString()
                            })
                        .FirstOrDefault();
            }

            //var catv = cat;
            JsonResult cc = Json(cat, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult getCategoriaDesc(string cate)
        {
            if (cate == null)
                cate = "";

            TAT001Entities db = new TAT001Entities();

            var cat = (dynamic)null;

            try
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                cat = db.MATERIALGPs.Where(c => c.ID == cate && c.ACTIVO == true)
                            .Join(
                            db.MATERIALGPTs.Where(ct => ct.SPRAS_ID == user.SPRAS_ID),
                            c => c.ID,
                            ct => ct.MATERIALGP_ID,
                            (c, ct) => new
                            {
                                SPRAS_ID = ct.SPRAS_ID.ToString(),
                                CATEGORIA_ID = ct.MATERIALGP_ID.ToString(),
                                TXT50 = ct.TXT50.ToString()
                            })
                        .FirstOrDefault();
            }
            catch (Exception)
            {

            }

            //var catv = cat;
            JsonResult cc = Json(cat, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpPost]
        public CATEGORIAT getCategoriaS(string material)
        {
            if (material == null)
                material = "";

            TAT001Entities db = new TAT001Entities();

            MATERIAL m = db.MATERIALs.Where(mat => mat.ID.Equals(material)).FirstOrDefault();
            CATEGORIAT cat = new CATEGORIAT();

            if (m != null && m.MATKL_ID != "")
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                cat = db.CATEGORIAs.Where(c => c.ID == m.MATKL_ID && c.ACTIVO == true)
                            .Join(
                            db.CATEGORIATs.Where(ct => ct.SPRAS_ID == user.SPRAS_ID),
                            c => c.ID,
                            ct => ct.CATEGORIA_ID,
                            (c, ct) => ct)
                        .FirstOrDefault();
            }
            return cat;
        }

        [HttpPost]
        public ActionResult getPartialDis(List<TAT001.Models.DOCUMENTOP_MOD> docs)
        {
            DOCUMENTO doc = new DOCUMENTO();

            doc.DOCUMENTOP = docs;
            return PartialView("~/Views/Solicitudes/_PartialDisTr.cshtml", doc);
        }

        [HttpPost]
        public ActionResult getPartialSop(List<DOCUMENTOF> docs)
        {
            DOCUMENTO doc = new DOCUMENTO();

            doc.DOCUMENTOF = docs;
            return PartialView("~/Views/Solicitudes/_PartialSopTr.cshtml", doc);
        }

        [HttpPost]
        public ActionResult getPartialRec(List<DOCUMENTOREC> docs)
        {
            DOCUMENTO doc = new DOCUMENTO();
            foreach (DOCUMENTOREC r in docs)
            {
                r.NUM_DOC = 0;
                r.DOC_REF = 0;
                r.EJERCICIO = 1;
                r.ESTATUS = " ";
                r.MONTO_FIJO = 0;
                r.MONTO_GRS = 0;
                r.MONTO_NET = 0;
                r.PERIODO = 1;
            }

            doc.DOCUMENTOREC = docs;
            return PartialView("~/Views/Solicitudes/_PartialRecTr.cshtml", doc);
        }
        private string completaMaterial(string material)//RSG 07.06.2018---------------------------------------------
        {
            string m = material;
            try
            {
                long matnr1 = long.Parse(m);
                int l = 18 - m.Length;
                for (int i = 0; i < l; i++)
                {
                    m = "0" + m;
                }
            }
            catch
            {

            }
            return m;
        }
    }
}