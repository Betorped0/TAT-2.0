using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services; //B20180730 MGC 2018.07.30 Formatos

namespace TAT001.Controllers
{
    [Authorize]
    public class CartaDController : Controller
    {
        // GET: CartaD
        public ActionResult Index(string ruta, decimal ids)
        {
            int pagina = 230; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user;
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
            }
            ViewBag.url = Request.Url.OriginalString.Replace(Request.Url.PathAndQuery, "") + HostingEnvironment.ApplicationVirtualPath + "/" + ruta;
            ViewBag.miNum = ids;

            return View();
        }

        // GET: CartaD/Details/5
        public ActionResult Details(string ruta)
        {
            int pagina = 230; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user;
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
            }
            ViewBag.url = ruta;
            return View();
        }

        // GET: CartaD/Details/5
        public ActionResult Create(decimal id)
        {
            int pagina = 232; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                ViewBag.usuario = user;
                ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                ViewBag.de = db.TEXTOCVs.Where(t => t.SPRAS_ID.Equals(user.SPRAS_ID) & t.CAMPO == "de").Select(t => t.TEXTO).FirstOrDefault();
                ViewBag.al = db.TEXTOCVs.Where(t => t.SPRAS_ID.Equals(user.SPRAS_ID) & t.CAMPO == "a").Select(t => t.TEXTO).FirstOrDefault();
                ViewBag.mon = db.TEXTOCVs.Where(t => t.SPRAS_ID.Equals(user.SPRAS_ID) & t.CAMPO == "monto").Select(t => t.TEXTO).FirstOrDefault();

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

                DOCUMENTO d = new DOCUMENTO();
                PUESTOT pp = new PUESTOT();
                d = db.DOCUMENTOes.Include("SOCIEDAD").Include("USUARIO").Where(a => a.NUM_DOC.Equals(id)).First();

                //Formatos para numeros
                d.PAI = db.PAIS.Where(a => a.LAND.Equals(d.PAIS_ID)).FirstOrDefault();
                string miles = d.PAI.MILES; //B20180730 MGC 2018.07.30 Formatos
                string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos

                FormatosC format = new FormatosC(); //B20180730 MGC 2018.07.30 Formatos

                //B20180726 MGC 2018.07.26
                bool fact = false;
                try
                {
                    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                }
                catch (Exception)
                {

                }

                List<string> lista = new List<string>();
                List<string> armadoCuerpoTab = new List<string>();
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTabla = new List<int>();
                int contadorTabla = 0;
                HeaderFooter hfc = new HeaderFooter();
                hfc.eliminaArchivos();
                CartaD cv = new CartaD();

                if (d != null)
                {
                    d.CLIENTE = db.CLIENTEs.Where(a => a.VKORG.Equals(d.VKORG)
                                                              & a.VTWEG.Equals(d.VTWEG)
                                                            & a.SPART.Equals(d.SPART)
                                                            & a.KUNNR.Equals(d.PAYER_ID)).First();
                    string sp = Session["spras"].ToString();
                    pp = db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(sp) && a.PUESTO_ID == d.USUARIO.PUESTO_ID).FirstOrDefault();
                }
                ViewBag.legal = db.LEYENDAs.Where(a => a.PAIS_ID.Equals(d.PAIS_ID) && a.ACTIVO == true).FirstOrDefault();


                /////////////////////////////////////////////DATOS PARA LA TABLA 1 MATERIALES EN LA VISTA///////////////////////////////////////
                //B20180720P MGC 2018.07.25
                var cabeza = new List<string>();
                bool varligada = Convert.ToBoolean(d.LIGADA);
                if (varligada != true)
                {
                    var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(id)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();

                    foreach (var item in con)
                    {
                        lista.Add(item.Key.VIGENCIA_DE.ToString() + item.Key.VIGENCIA_AL.ToString());
                    }

                    for (int i = 0; i < lista.Count; i++)
                    {
                        contadorTabla = 0;

                        DateTime a1 = DateTime.Parse(lista[i].Remove(lista[i].Length / 2));
                        DateTime a2 = DateTime.Parse(lista[i].Remove(0, lista[i].Length / 2));

                        var con2 = db.DOCUMENTOPs
                                              .Where(x => x.NUM_DOC.Equals(id) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                              .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new { x.NUM_DOC, x.MATNR, x.MATKL, y.MAKTX, x.MONTO, y.PUNIT, x.PORC_APOYO, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.VOLUMEN_EST, x.VOLUMEN_REAL, x.APOYO_EST, x.APOYO_REAL })//B20180726 MGC 2018.07.26
                                              .ToList();

                        if (con2.Count > 0)
                        {
                            foreach (var item2 in con2)
                            {
                                armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                                armadoCuerpoTab.Add(item2.MATKL);
                                armadoCuerpoTab.Add(item2.MAKTX);
                                //armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.resta, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //B20180726 MGC 2018.07.26
                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString());
                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString());
                                //Volumen y apoyo
                                if (fact)
                                    {
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                    else
                                    {
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                contadorTabla++;
                            }
                        }
                        else
                        {
                            var con3 = db.DOCUMENTOPs
                                                .Where(x => x.NUM_DOC.Equals(id) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                                .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new { x.NUM_DOC, x.MATNR, x.MATKL, y.ID, x.MONTO, x.PORC_APOYO, y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG,x.VOLUMEN_EST, x.VOLUMEN_REAL, x.APOYO_EST, x.APOYO_REAL })//B20180726 MGC 2018.07.26
                                                .ToList();

                            foreach (var item2 in con3)
                            {
                                armadoCuerpoTab.Add("");
                                armadoCuerpoTab.Add(item2.MATKL);
                                armadoCuerpoTab.Add(item2.TXT50);
                                //armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.resta, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //B20180726 MGC 2018.07.26
                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString());
                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString());
                                if (fact)
                                {
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                contadorTabla++;
                            }
                        }
                        numfilasTabla.Add(contadorTabla);
                    }


                    //var cabeza = new List<string>();//B20180720P MGC 2018.07.25
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "materialC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "categoriaC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "descripcionC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costouC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopoC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopiC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costoaC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "preciosC").Select(x => x.TEXTO).FirstOrDefault());
                    //B20180726 MGC 2018.07.26
                    //cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyoeC").Select(x => x.TEXTO).FirstOrDefault());
                    //cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyorC").Select(x => x.TEXTO).FirstOrDefault());
                    //Volumen                   
                    if (fact)
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "volumenrC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                        else
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "volumeneC").Select(x => x.TEXTO).FirstOrDefault());
                        }                   
                    //Apoyo
                    //B20180726 MGC 2018.07.26                  
                        if (fact)
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyorC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                        else
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyoeC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                    
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                /////////////////////////////////////////////DATOS PARA LA TABLA 2 RECURRENCIAS EN LA VISTA///////////////////////////////////////
                var cabeza2 = new List<string>();
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "posC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "tipoC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "fechaC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "montoC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "porcentajeC2").Select(x => x.TEXTO).FirstOrDefault());

                var con4 = db.DOCUMENTORECs
                                            .Where(x => x.NUM_DOC.Equals(id))
                                            .Join(db.DOCUMENTOes, x => x.NUM_DOC, y => y.NUM_DOC, (x, y) => new { x.POS, y.TSOL_ID, x.FECHAF, x.MONTO_BASE, x.PORC })
                                            .ToList();

                foreach (var item in con4)
                {
                    DateTime a = Convert.ToDateTime(item.FECHAF);

                    armadoCuerpoTab2.Add(item.POS.ToString());
                    armadoCuerpoTab2.Add(db.TSOLs.Where(x => x.ID == item.TSOL_ID).Select(x => x.DESCRIPCION).First());
                    armadoCuerpoTab2.Add(a.ToShortDateString());
                    armadoCuerpoTab2.Add(item.MONTO_BASE.ToString());
                    armadoCuerpoTab2.Add(item.PORC.ToString());
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //TABLA 1 MATERIALES
                cv.listaFechas = lista;//////////////RANGO DE FECHAS QUE DETERMINAN EL NUMERO DE TABLAS
                cv.numfilasTabla = numfilasTabla;////NUMERO FILAS POR TABLA CALCULADA
                cv.listaCuerpo = armadoCuerpoTab;////NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE QUE POSTERIORMENTE ES DISTRIBUIDA EN LAS TABLAS
                cv.numColEncabezado = cabeza;////////NUMERO DE COLUMNAS PARA LAS TABLAS
                cv.secondTab_x = true;
                cv.costoun_x = true;
                cv.apoyo_x = true;
                cv.apoyop_x = true;
                cv.costoap_x = true;
                cv.precio_x = true;
                //cv.apoyoEst_x = true;
                //cv.apoyoRea_x = true;
                //B20180726 MGC 2018.07.26
                cv.volumen_x = true;
                cv.apoyototal_x = true;
                /////////////////////////////////

                //TABLA 2 RECURRENCIAS
                cv.numColEncabezado2 = cabeza2;////////NUMERO DE COLUMNAS PARA LAS TABLAS
                cv.numfilasTabla2 = con4.Count();//////NUMERO FILAS TOTAL PARA LA TABLA
                cv.listaCuerpoRec = armadoCuerpoTab2;//NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE
                ///////////////////////////////

                cv.num_doc = id;
                cv.company = d.SOCIEDAD.BUTXT;
                cv.company_x = true;
                cv.taxid = d.SOCIEDAD.LAND;
                cv.taxid_x = true;
                cv.concepto = d.CONCEPTO;
                cv.concepto_x = true;
                cv.cliente = d.PAYER_NOMBRE;
                cv.cliente_x = true;
                cv.puesto = " ";
                cv.puesto_x = false;
                cv.direccion = d.CLIENTE.STRAS_GP;
                cv.direccion_x = true;
                cv.folio = d.NUM_DOC.ToString();
                cv.folio_x = true;
                cv.lugar = d.CIUDAD.Trim() + ", " + d.ESTADO.Trim();
                cv.lugar_x = true;
                cv.lugarFech = DateTime.Now.ToShortDateString();
                cv.lugarFech_x = true;
                cv.payerId = d.CLIENTE.PAYER;
                cv.payerId_x = true;
                cv.payerNom = d.CLIENTE.NAME1;
                cv.payerNom_x = true;
                cv.estimado = d.PAYER_NOMBRE;
                cv.estimado_x = true;
                cv.mecanica = d.NOTAS;
                cv.mecanica_x = true;
                cv.nombreE = d.USUARIO.NOMBRE + " " + d.USUARIO.APELLIDO_P + " " + d.USUARIO.APELLIDO_M;
                cv.nombreE_x = true;
                if (pp != null)
                    cv.puestoE = pp.TXT50;
                cv.puestoE_x = true;
                cv.companyC = cv.company;
                cv.companyC_x = true;
                cv.nombreC = d.PAYER_NOMBRE;
                cv.nombreC_x = true;
                cv.puestoC = " ";
                cv.puestoC_x = false;
                cv.companyCC = d.CLIENTE.NAME1;
                cv.companyCC_x = true;
                if (ViewBag.legal != null)
                    cv.legal = ViewBag.legal.LEYENDA1;
                cv.legal_x = true;
                cv.mail = db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "correo").Select(x => x.TEXTO).FirstOrDefault() + " " + d.PAYER_EMAIL;
                cv.mail_x = true;
                cv.comentarios = "";
                cv.comentarios_x = true;
                cv.compromisoK = "";
                cv.compromisoK_x = true;
                cv.compromisoC = "";
                cv.compromisoC_x = true;
                cv.monto_x = true;
                cv.monto = d.MONTO_DOC_MD.ToString();
                cv.moneda = d.MONEDA_ID;

                //B20180720P MGC 2018.07.25
                ViewBag.varligada = varligada;

                //B20180801 MGC Formato
                //v.monto = monto_enviar;
                decimal montod = 0;
                try
                {
                    montod = Convert.ToDecimal(cv.monto);
                }
                catch (Exception)
                {

                }

                @ViewBag.montoformat = format.toShow(montod, decimales);

                return View(cv);
            }
        }

        // POST: CartaD/Details/5
        [HttpPost]
        public ActionResult Create(CartaD v)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                List<string> encabezadoFech = new List<string>();
                List<string> armadoCuerpoTab = new List<string>();
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTab = new List<int>();

                int contadorTabla = 0;
                DOCUMENTO d = db.DOCUMENTOes.Find(v.num_doc);

                //B20180726 MGC 2018.07.26
                bool fact = false;
                    try
                    {
                        d = db.DOCUMENTOes.Find(v.num_doc);
                        fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                    }
                    catch (Exception)
                    {

                    }

                //Formatos para numeros
                d.PAI = db.PAIS.Where(a => a.LAND.Equals(d.PAIS_ID)).FirstOrDefault();
                string miles = d.PAI.MILES; //B20180730 MGC 2018.07.30 Formatos
                string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos

                FormatosC format = new FormatosC(); //B20180730 MGC 2018.07.30 Formatos

                /////////////////////////////////////////////DATOS PARA LA TABLA 1 MATERIALES EN EL PDF///////////////////////////////////////
                //B20180720P MGC 2018.07.25
                var cabeza = new List<string>();
                bool varligada = Convert.ToBoolean(d.LIGADA);
                if (varligada != true)
                {
                    var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(v.num_doc)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();

                    foreach (var item in con)
                    {
                        encabezadoFech.Add(item.Key.VIGENCIA_DE.ToString() + item.Key.VIGENCIA_AL.ToString());
                    }

                    for (int i = 0; i < encabezadoFech.Count; i++)
                    {
                        contadorTabla = 0;
                        DateTime a1 = DateTime.Parse(encabezadoFech[i].Remove(encabezadoFech[i].Length / 2));
                        DateTime a2 = DateTime.Parse(encabezadoFech[i].Remove(0, encabezadoFech[i].Length / 2));

                        var con2 = db.DOCUMENTOPs
                                          .Where(x => x.NUM_DOC.Equals(v.num_doc) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                          .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new { x.MATNR, x.MATKL, y.MAKTX, x.MONTO, y.PUNIT, x.PORC_APOYO, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.VOLUMEN_EST, x.VOLUMEN_REAL, x.APOYO_EST, x.APOYO_REAL }) //B20180726 MGC 2018.07.26
                                          .ToList();


                        if (con2.Count > 0)
                        {
                            foreach (var item2 in con2)
                            {
                                armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                                armadoCuerpoTab.Add(item2.MATKL);
                                armadoCuerpoTab.Add(item2.MAKTX);
                                if (v.costoun_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.apoyo_x == true)
                                {
                                    //armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.apoyop_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.costoap_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.resta, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.precio_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                //B20180726 MGC 2018.07.26
                                //if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString()); }
                                //if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString()); }
                                //Volumen
                                //B20180726 MGC 2018.07.26
                                if (v.volumen_x == true)
                                {
                                    if (fact)
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        //carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                    }
                                    else
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        //carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                    }
                                }

                                //Apoyo
                                //B20180726 MGC 2018.07.26
                                if (v.apoyototal_x == true)
                                {
                                    if (fact)
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    }
                                    else
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    }
                                }
                                contadorTabla++;
                            }
                        }
                        else
                        {
                            var con3 = db.DOCUMENTOPs
                                                .Where(x => x.NUM_DOC.Equals(v.num_doc) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                                .Join(db.CATEGORIAs, x => x.MATKL, y => y.ID, (x, y) => new { x.NUM_DOC, x.MATNR, x.MATKL, y.ID, x.MONTO, x.PORC_APOYO, y.CATEGORIATs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.VOLUMEN_REAL, x.VOLUMEN_EST, x.APOYO_EST, x.APOYO_REAL })//B20180726 MGC 2018.07.26
                                                .ToList();

                            foreach (var item2 in con3)
                            {
                                armadoCuerpoTab.Add("");
                                armadoCuerpoTab.Add(item2.MATKL);
                                armadoCuerpoTab.Add(item2.TXT50);
                                if (v.costoun_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.apoyo_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos                                    
                                    armadoCuerpoTab.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.apoyop_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                //if (v.costoap_x == true) { armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                if (v.costoap_x == true) {
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.resta, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.precio_x == true) {
                                    //armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    }
                                //B20180726 MGC 2018.07.26
                                //if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString()); }
                                //if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString()); }
                                //Volumen
                                //B20180726 MGC 2018.07.26
                                if (v.volumen_x == true)
                                {
                                    if (fact)
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        //carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                    }
                                    else
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        //carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                        }
                                }

                                //Apoyo
                                //B20180726 MGC 2018.07.26
                                if (v.apoyototal_x == true)
                                {
                                    if (fact)
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                    else
                                    {
                                        //armadoCuerpoTab.Add(Math.Round(Convert.(item2.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                        armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                }
                                contadorTabla++;
                            }
                        }
                        numfilasTab.Add(contadorTabla);
                    }

                    //var cabeza = new List<string>();//B20180720P MGC 2018.07.25
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "materialC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "categoriaC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "descripcionC").Select(x => x.TEXTO).FirstOrDefault());
                    if (v.costoun_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costouC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.apoyo_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopoC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.apoyop_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopiC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.costoap_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costoaC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.precio_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "preciosC").Select(x => x.TEXTO).FirstOrDefault()); }
                    //B20180726 MGC 2018.07.26
                    //if (v.apoyoEst_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyoeC").Select(x => x.TEXTO).FirstOrDefault()); }
                    //if (v.apoyoRea_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyorC").Select(x => x.TEXTO).FirstOrDefault()); }
                    //Volumen
                    //B20180726 MGC 2018.07.26
                    if (v.volumen_x == true)
                    {
                        if (fact)
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "volumenrC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                        else
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "volumeneC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                    }
                    //Apoyo
                    //B20180726 MGC 2018.07.26
                    if (v.apoyototal_x == true)
                    {
                        if (fact)
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyorC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                        else
                        {
                            cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyoeC").Select(x => x.TEXTO).FirstOrDefault());
                        }
                    }
                }
                else
                {
                    v.monto_x = false;//B20180720P MGC 2018.07.25
                }
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                /////////////////////////////////////////////DATOS PARA LA TABLA 2 RECURRENCIAS EN PDF///////////////////////////////////////
                var cabeza2 = new List<string>();
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "posC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "tipoC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "fechaC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "montoC2").Select(x => x.TEXTO).FirstOrDefault());
                cabeza2.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "porcentajeC2").Select(x => x.TEXTO).FirstOrDefault());

                var con4 = db.DOCUMENTORECs
                                            .Where(x => x.NUM_DOC.Equals(v.num_doc))
                                            .Join(db.DOCUMENTOes, x => x.NUM_DOC, y => y.NUM_DOC, (x, y) => new { x.POS, y.TSOL_ID, x.FECHAF, x.MONTO_BASE, x.PORC })
                                            .ToList();

                foreach (var item in con4)
                {
                    DateTime a = Convert.ToDateTime(item.FECHAF);
                    armadoCuerpoTab2.Add(item.POS.ToString());
                    armadoCuerpoTab2.Add(db.TSOLs.Where(x => x.ID == item.TSOL_ID).Select(x => x.DESCRIPCION).First());
                    armadoCuerpoTab2.Add(a.ToShortDateString());
                    armadoCuerpoTab2.Add(item.MONTO_BASE.ToString());
                    armadoCuerpoTab2.Add(item.PORC.ToString());
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //MARCA DE AGUA
                bool aprob = false;
                aprob = (d.ESTATUS_WF.Equals("A") | d.ESTATUS_WF.Equals("S"));

                //PARA LA TABLA 1 MATERIALES
                v.numColEncabezado = cabeza;
                v.listaFechas = encabezadoFech;
                v.numfilasTabla = numfilasTab;
                v.listaCuerpo = armadoCuerpoTab;
                //PARA LA TABLA 2 RECURRENCIAS
                v.numColEncabezado2 = cabeza2;
                v.numfilasTabla2 = con4.Count();
                v.listaCuerpoRec = armadoCuerpoTab2;

                //B20180801 MGC Formato
                decimal montod = 0;
                try
                {
                    montod = Convert.ToDecimal(v.monto);
                }
                catch (Exception)
                {

                }

                v.monto = format.toShow(montod, decimales);

                CartaD carta = v;
                CartaDEsqueleto cve = new CartaDEsqueleto();
                cve.crearPDF(carta, user.SPRAS_ID, aprob);
                string recibeRuta = Convert.ToString(Session["rutaCompletaV"]);
                return RedirectToAction("Index", new { ruta = recibeRuta, ids = v.num_doc });
            }
        }

        // GET: CartaD/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartaD/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CartaD/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartaD/Delete/5
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