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
    public class CartaVController : Controller
    {
        // GET: CartaV
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

        // GET: CartaV/Details/5
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

        // GET: CartaV/Details/5
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

                string miles = d.PAI.MILES; //B20180730 MGC 2018.07.30 Formatos
                string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos

                ViewBag.miles = d.PAI.MILES;//LEJGG 090718
                ViewBag.dec = d.PAI.DECIMAL;//LEJGG 090718

                List<string> lista = new List<string>();
                List<listacuerpoc> armadoCuerpoTab = new List<listacuerpoc>(); //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTabla = new List<int>();
                int contadorTabla = 0;
                HeaderFooter hfc = new HeaderFooter();
                hfc.eliminaArchivos();
                CartaV cv = new CartaV();

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
                //B20180710 MGC 2018.07.12 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                bool fact = false;
                try
                {
                    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                }
                catch (Exception)
                {

                }

                //B20180730 MGC 2018.07.30 Formatos
                //Referencia a formatos
                FormatosC format = new FormatosC();

                //B20180720P MGC 2018.07.25
                string trclass = "";
                bool editmonto = false; //B20180710 MGC 2018.07.18 editar el monto en porcentaje categoría
                var cabeza = new List<string>();
                bool varligada = Convert.ToBoolean(d.LIGADA);
                if (varligada != true)
                {
                    var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(id)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();

                    ////B20180710 MGC 2018.07.12 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                    //bool fact = false;
                    //try
                    //{
                    //    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                    //}
                    //catch (Exception)
                    //{

                    //}
                    //B20180710 MGC 2018.07.12 Modificación 9 y 10 dependiendo del campo de factura en tsol..............

                    //B20180710 MGC 2018.07.18 total es input o text
                    //string trclass = "";//B20180710 MGC 2018.07.18 editar el monto en porcentaje categoría
                    //bool editmonto = false; //B20180710 MGC 2018.07.18 editar el monto en porcentaje categoría

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
                                              .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new
                                              {
                                                  x.NUM_DOC,
                                                  x.MATNR,
                                                  x.MATKL,
                                                  y.MAKTX,
                                                  x.MONTO,
                                                  y.PUNIT,
                                                  x.PORC_APOYO,
                                                  x.MONTO_APOYO,
                                                  resta = (x.MONTO - x.MONTO_APOYO),
                                                  x.PRECIO_SUG,
                                                  x.APOYO_EST,
                                                  x.APOYO_REAL
                                              ,
                                                  x.VOLUMEN_EST,
                                                  x.VOLUMEN_REAL
                                              }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL
                                              .ToList();

                        //Definición si la distribución es monto o porcentaje
                        string porclass = "";//B20180710 MGC 2018.07.18 total es input o text
                        string totalm = "";//B20180710 MGC 2018.07.18 total es input o text
                        if (d.TIPO_TECNICO == "M")
                        {
                            porclass = " tipom";
                            totalm = " total";
                            trclass = " total";
                        }
                        else if (d.TIPO_TECNICO == "P")
                        {
                            porclass = " tipop";
                            totalm = " ni";
                        }

                        if (con2.Count > 0)
                        {
                            foreach (var item2 in con2)
                            {
                                //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                                listacuerpoc lc1 = new listacuerpoc();
                                lc1.val = item2.MATNR.TrimStart('0');
                                lc1.clase = "ni";
                                armadoCuerpoTab.Add(lc1);

                                listacuerpoc lc2 = new listacuerpoc();
                                lc2.val = item2.MATKL;
                                lc2.clase = "ni";
                                armadoCuerpoTab.Add(lc2);

                                listacuerpoc lc3 = new listacuerpoc();
                                lc3.val = item2.MAKTX;
                                lc3.clase = "ni";
                                armadoCuerpoTab.Add(lc3);

                                //Costo unitario
                                listacuerpoc lc4 = new listacuerpoc();
                                //lc4.val = "$" + Math.Round(item2.MONTO, 2).ToString(); //B20180730 MGC 2018.07.30 Formatos
                                lc4.val = format.toShow(Math.Round(item2.MONTO, 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                                lc4.clase = "input_oper numberd input_dc mon" + porclass;
                                armadoCuerpoTab.Add(lc4);

                                //Porcentaje de apoyo
                                listacuerpoc lc5 = new listacuerpoc();
                                //lc5.val = Math.Round(item2.PORC_APOYO, 2).ToString() + "%"; //B20180730 MGC 2018.07.30 Formatos
                                lc5.val = format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                lc5.clase = "input_oper numberd porc input_dc" + porclass;
                                armadoCuerpoTab.Add(lc5);

                                //Apoyo por pieza
                                listacuerpoc lc6 = new listacuerpoc();
                                //lc6.val = "$" + Math.Round(item2.MONTO_APOYO, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc6.val = format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                lc6.clase = "input_oper numberd costoa input_dc mon" + porclass;
                                armadoCuerpoTab.Add(lc6);

                                //Costo con apoyo
                                listacuerpoc lc7 = new listacuerpoc();
                                //lc7.val = "$" + Math.Round(item2.resta, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc7.val = format.toShow(Math.Round(item2.resta, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                lc7.clase = "input_oper numberd costoa input_dc mon" + porclass;//Importante costoa para validación en vista
                                armadoCuerpoTab.Add(lc7);

                                //Precio Sugerido
                                listacuerpoc lc8 = new listacuerpoc();
                                //lc8.val = "$" + Math.Round(item2.PRECIO_SUG, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc8.val = format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                lc8.clase = "input_oper numberd input_dc mon" + porclass;
                                armadoCuerpoTab.Add(lc8);

                                //Modificación 9 y 10 dependiendo del campo de factura en tsol
                                //fact = true es real
                                //Volumen
                                listacuerpoc lc9 = new listacuerpoc();
                                if (fact)
                                {
                                    //lc9.val = Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                    lc9.val = format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                    //lc9.val = Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                    lc9.val = format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                }
                                lc9.clase = "input_oper numberd input_dc num" + porclass;
                                armadoCuerpoTab.Add(lc9);

                                //Apoyo
                                listacuerpoc lc10 = new listacuerpoc();
                                if (fact)
                                {
                                    //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                    lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                    //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                    lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                                }
                                lc10.clase = "input_oper numberd input_dc mon" + totalm + "" + porclass;
                                armadoCuerpoTab.Add(lc10);

                                contadorTabla++;
                            }
                        }
                        else
                        {
                            var con3 = db.DOCUMENTOPs
                                                .Where(x => x.NUM_DOC.Equals(id) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                                .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new
                                                {
                                                    x.NUM_DOC,
                                                    x.MATNR,
                                                    x.MATKL,
                                                    y.ID,
                                                    x.MONTO,
                                                    x.PORC_APOYO,
                                                    y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50,
                                                    x.MONTO_APOYO,
                                                    resta = (x.MONTO - x.MONTO_APOYO),
                                                    x.PRECIO_SUG,
                                                    x.APOYO_EST,
                                                    x.APOYO_REAL
                                                ,
                                                    x.VOLUMEN_EST,
                                                    x.VOLUMEN_REAL
                                                }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL})
                                                .ToList();
                            if (d.TIPO_TECNICO == "M")
                            {
                                trclass = "total";
                            }
                            else if (d.TIPO_TECNICO == "P")
                            {
                                editmonto = true;
                            }


                            foreach (var item2 in con3)
                            {
                                //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                                listacuerpoc lc1 = new listacuerpoc();
                                lc1.val = "";
                                lc1.clase = "ni";
                                armadoCuerpoTab.Add(lc1);

                                listacuerpoc lc2 = new listacuerpoc();
                                lc2.val = item2.MATKL;
                                lc2.clase = "ni";
                                armadoCuerpoTab.Add(lc2);

                                listacuerpoc lc3 = new listacuerpoc();
                                lc3.val = item2.TXT50;
                                lc3.clase = "ni";
                                armadoCuerpoTab.Add(lc3);

                                //Costo unitario
                                listacuerpoc lc4 = new listacuerpoc();
                                //lc4.val = Math.Round(item2.MONTO, 2).ToString();
                                //lc4.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc4.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                lc4.clase = "ni";
                                armadoCuerpoTab.Add(lc4);

                                //Porcentaje de apoyo
                                listacuerpoc lc5 = new listacuerpoc();
                                //lc5.val = Math.Round(item2.PORC_APOYO, 2).ToString();
                                //Definición si la distribución es monto o porcentaje
                                if (d.TIPO_TECNICO == "M")
                                {
                                    //lc5.val = ""; //B20180730 MGC 2018.07.30 Formatos
                                    lc5.val = format.toShowPorc(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                }
                                else if (d.TIPO_TECNICO == "P")
                                {
                                    //lc5.val = Math.Round(item2.PORC_APOYO, 2).ToString() + "%"; //B20180730 MGC 2018.07.30 Formatos
                                    lc5.val = format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                                }

                                //lc5.clase = "input_oper numberd input_dc";
                                lc5.clase = "ni";
                                armadoCuerpoTab.Add(lc5);

                                //Apoyo por pieza
                                listacuerpoc lc6 = new listacuerpoc();
                                //lc6.val = Math.Round(item2.MONTO_APOYO, 2).ToString();
                                //lc6.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc6.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                lc6.clase = "ni";
                                armadoCuerpoTab.Add(lc6);

                                //Costo con apoyo
                                listacuerpoc lc7 = new listacuerpoc();
                                //lc7.val = Math.Round(item2.resta, 2).ToString();
                                //lc7.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc7.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                lc7.clase = "ni";
                                armadoCuerpoTab.Add(lc7);

                                //Precio Sugerido
                                listacuerpoc lc8 = new listacuerpoc();
                                //lc8.val = Math.Round(item2.PRECIO_SUG, 2).ToString();
                                //lc8.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc8.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                lc8.clase = "ni";
                                armadoCuerpoTab.Add(lc8);
                                //Modificación 9 y 10 dependiendo del campo de factura en tsol
                                //fact = true es real

                                //Volumen
                                listacuerpoc lc9 = new listacuerpoc();
                                if (fact)
                                {
                                    //lc9.val = Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString();
                                    //lc9.val = "";//B20180730 MGC 2018.07.30 Formatos
                                    lc9.val = format.toShowNum(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                    //lc9.val = Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString();
                                    //lc9.val = "";//B20180730 MGC 2018.07.30 Formatos
                                    lc9.val = format.toShowNum(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                                }
                                lc9.clase = "ni";
                                armadoCuerpoTab.Add(lc9);

                                //Apoyo
                                listacuerpoc lc10 = new listacuerpoc();
                                if (fact)
                                {
                                    //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2).ToString(); //B20180730 MGC 2018.07.30 Formatos
                                    lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                    //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2).ToString(); //B20180730 MGC 2018.07.30 Formatos
                                    lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                                }
                                //Definición si la distribución es monto o porcentaje
                                if (d.TIPO_TECNICO == "M")
                                {
                                    lc10.clase = "input_oper numberd input_dc total cat mon";
                                }
                                else if (d.TIPO_TECNICO == "P")
                                {
                                    lc10.clase = "ni";
                                }

                                armadoCuerpoTab.Add(lc10);

                                contadorTabla++;
                            }
                        }
                        numfilasTabla.Add(contadorTabla);
                    }

                    //var cabeza = new List<string>(); //B20180720P MGC 2018.07.25
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "materialC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "categoriaC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "descripcionC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costouC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopoC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopiC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costoaC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "preciosC").Select(x => x.TEXTO).FirstOrDefault());
                    //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                    //fact = true es real
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
                    if (fact)
                    {
                        cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyorC").Select(x => x.TEXTO).FirstOrDefault());
                    }
                    else
                    {
                        cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyoeC").Select(x => x.TEXTO).FirstOrDefault());
                    }
                }
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                cv.listaCuerpom = armadoCuerpoTab;////NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE QUE POSTERIORMENTE ES DISTRIBUIDA EN LAS TABLAS //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                cv.numColEncabezado = cabeza;////////NUMERO DE COLUMNAS PARA LAS TABLAS
                cv.secondTab_x = true;
                cv.costoun_x = true;
                cv.apoyo_x = true;
                cv.apoyop_x = true;
                cv.costoap_x = true;
                cv.precio_x = true;
                //cv.apoyoEst_x = true; //Volumen //B20180726 MGC 2018.07.26
                //cv.apoyoRea_x = true; //Apoyo //B20180726 MGC 2018.07.26
                cv.volumen_x = true; //Volumen //B20180726 MGC 2018.07.26 
                cv.apoyototal_x = true; //Apoyo //B20180726 MGC 2018.07.26
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

                ViewBag.factura = fact;//B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                ViewBag.trclass = trclass;//B20180710 MGC 2018.07.18 total es input o text
                ViewBag.editmonto = editmonto;//B20180710 MGC 2018.07.18 total es input o text

                //B20180720P MGC 2018.07.25
                ViewBag.varligada = varligada;

                //B20180801 MGC Formato
                ViewBag.montoformat = format.toShow(Convert.ToDecimal(d.MONTO_DOC_MD), decimales);

                return View(cv);
            }
        }

        // POST: CartaV/Details/5
        [HttpPost]
        //[ValidateAntiForgeryToken] //B20180710 MGC 2018.07.16 Modificaciones para editar los campos de distribución se agrego los objetos
        //public ActionResult Create([Bind(Include = "num_doc, listaCuerpo, DOCUMENTOP")] CartaV v)
        public ActionResult Create(CartaV v, string monto_enviar, string guardar_param)
        {
            //v.monto = monto_enviar; //B20180720P MGC //B20180801 MGC Formato
            int pos = 0;//B20180720P MGC Guardar Carta

            //B20180726 MGC 2018.07.26
            bool fact = false;
            DOCUMENTO d = new DOCUMENTO();
            using (TAT001Entities db = new TAT001Entities())
            {
                try
                {
                    d = db.DOCUMENTOes.Find(v.num_doc);
                    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                    d.PAI = db.PAIS.Where(a => a.LAND.Equals(d.PAIS_ID)).FirstOrDefault();
                }
                catch (Exception)
                {

                }
            }

            //Formatos para numeros

            string miles = d.PAI.MILES; //B20180730 MGC 2018.07.30 Formatos
            string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos

            FormatosC format = new FormatosC(); //B20180730 MGC 2018.07.30 Formatos

            CARTA ca = new CARTA();
            ca.NUM_DOC = v.num_doc;
            ca.CLIENTE = v.cliente;
            ca.CLIENTEX = v.cliente_x;
            ca.COMPANY = v.company;
            ca.COMPANYC = v.companyC;
            ca.COMPANYCC = v.companyCC;
            ca.COMPANYCCX = v.companyCC_x;
            ca.COMPANYCX = v.companyC_x;
            ca.COMPANYX = v.company_x;
            ca.CONCEPTO = v.concepto;
            ca.CONCEPTOX = v.concepto_x;
            ca.DIRECCION = v.direccion;
            ca.DIRECCIONX = v.direccion_x;
            //ca.DOCUMENTO = v.DOCUMENTO;
            ca.ESTIMADO = v.estimado;
            ca.ESTIMADOX = v.estimado_x;
            //ca.FECHAC = v.FECHAC;
            ca.FOLIO = v.folio;
            ca.FOLIOX = v.folio_x;
            ca.LEGAL = v.legal;
            ca.LEGALX = v.legal_x;
            ca.LUGARFECH = v.lugarFech;
            ca.LUGARFECHX = v.lugarFech_x;
            ca.LUGAR = v.lugar;
            ca.LUGARX = v.lugar_x;
            ca.MAIL = v.mail;
            ca.MAILX = v.mail_x;
            ca.MECANICA = v.mecanica;
            ca.MECANICAX = v.mecanica_x;
            ca.NOMBREC = v.nombreC;
            ca.NOMBRECX = v.nombreC_x;
            ca.NOMBREE = v.nombreE;
            ca.NOMBREEX = v.nombreE_x;
            ca.NUM_DOC = v.num_doc;
            ca.PAYER = v.payerId;
            ca.PAYERX = v.payerId_x;
            ca.PUESTO = v.puesto;
            ca.PUESTOC = v.puestoC;
            ca.PUESTOCX = v.puestoC_x;
            ca.PUESTOE = v.puestoE;
            ca.PUESTOEX = v.puestoE_x;
            ca.PUESTOX = v.puestoE_x;
            ca.TAXID = v.taxid;
            ca.TAXIDX = v.taxid_x;
            ca.MONTO = v.monto;
            ca.MONEDA = v.moneda;
            //ca.TIPO = v.TIPO;
            //ca.USUARIO = v.USUARIO;
            //ca.USUARIO_ID = v.USUARIO_ID;
            ca.USUARIO_ID = User.Identity.Name;
            ca.FECHAC = DateTime.Now;

            //Agregadas  //B20180720P MGC Guardar Carta
            ca.PAYERNOM = v.payerNom;
            ca.PAYERNOMX = v.payerNom_x;
            ca.COMENTARIO = v.comentarios;
            ca.COMENTARIOX = v.comentarios_x;
            ca.COMPROMISOC = v.compromisoC;
            ca.COMPROMISOCX = v.compromisoC_x;
            ca.COMPROMISOK = v.compromisoK;
            ca.COMPROMISOKX = v.compromisoK_x;

            ca.SECOND_TABX = v.secondTab_x;
            ca.COSTO_UNX = v.costoun_x;
            ca.APOYOX = v.apoyo_x;
            ca.APOYOPX = v.apoyop_x;
            ca.COSTOAPX = v.costoap_x;
            ca.PRECIOX = v.precio_x;
            //B20180726 MGC 2018.07.26
            //ca.APOYO_ESTX = v.apoyoEst_x; //Volumen 
            //ca.APOYO_REAX = v.apoyoRea_x; //Apoyo
            if (fact)
            {
                ca.VOLUMEN_REAX = v.volumen_x; //Volumen 
                ca.APOYO_REAX = v.apoyototal_x; //Apoyo
            }
            else
            {
                ca.VOLUMEN_ESTX = v.volumen_x; //Volumen 
                ca.APOYO_ESTX = v.apoyototal_x; //Apoyo
            }

            //B20180801 MGC Formato
            //v.monto = monto_enviar;
            decimal montod = 0;
            try
            {
                montod = Convert.ToDecimal(monto_enviar);
            }
            catch (Exception)
            {

            }

            v.monto = format.toShow(montod, decimales);


            //CartaFEsqueleto cfe = new CartaFEsqueleto();//B20180720P MGC Guardar Carta
            //TEXTOCARTAF f = new TEXTOCARTAF();//B20180720P MGC Guardar Carta
            string u = User.Identity.Name;
            //string recibeRuta = ""; //B20180720P MGC Guardar Carta
            using (TAT001Entities db = new TAT001Entities())
            {
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                var cartas = db.CARTAs.Where(a => a.NUM_DOC.Equals(ca.NUM_DOC)).ToList();
                //int pos = 0;//B20180720P MGC Guardar Carta
                if (cartas.Count > 0)
                    pos = cartas.OrderByDescending(a => a.POS).First().POS;

                ca.POS = pos + 1;
                pos = ca.POS; //B20180720P MGC Guardar Carta
                if (guardar_param == "guardar_param")//B20180720P MGC Guardar Carta
                {
                    db.CARTAs.Add(ca);
                    db.SaveChanges();
                }
            }
            //bool aprob = false;//B20180720P MGC Guardar Carta
            //B20180720P MGC Guardar Carta
            //using (TAT001Entities db = new TAT001Entities())
            //{
            //    DOCUMENTO d = db.DOCUMENTOes.Find(c.num_doc);
            //    aprob = (d.ESTATUS_WF.Equals("A"));

            //    cfe.crearPDF(c, f, aprob);
            //    recibeRuta = Convert.ToString(Session["rutaCompletaf"]);
            //    return RedirectToAction("Details", new { ruta = recibeRuta });
            //}

            using (TAT001Entities db = new TAT001Entities())
            {
                d = db.DOCUMENTOes.Find(v.num_doc);
                //string u = User.Identity.Name; //B20180720P MGC Guardar Carta
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                List<string> encabezadoFech = new List<string>();
                List<string> armadoCuerpoTab = new List<string>(); //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTab = new List<int>();

                int contadorTabla = 0;
                //B20180726 MGC 2018.07.26
                //DOCUMENTO d = db.DOCUMENTOes.Find(v.num_doc);


                /////////////////////////////////////////////DATOS PARA LA TABLA 1 MATERIALES EN EL PDF///////////////////////////////////////

                //B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol..............
                var cabeza = new List<string>();
                bool varligada = Convert.ToBoolean(d.LIGADA);
                if (varligada != true)
                {
                    var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(v.num_doc)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();
                    //B20180710 MGC 2018.07.17 Modificación de monto
                    //v.monto = monto_enviar; //B20180720P MGC
                    //B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                    //bool fact = false;
                    //try
                    //{
                    //    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                    //}
                    //catch (Exception)
                    //{

                    //}
                    ////B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol..............

                    foreach (var item in con)
                    {
                        encabezadoFech.Add(item.Key.VIGENCIA_DE.ToString() + item.Key.VIGENCIA_AL.ToString());
                    }

                    //B20180710 MGC 2018.07.19 Provisional obtener la siguiente posición para carta......................
                    //B20180720P MGC Guardar Carta.......................................................................
                    //int pos = 0; //B20180720P MGC Guardar Carta

                    //try
                    //{
                    //    pos = db.CARTAs.Where(ca => ca.NUM_DOC == v.num_doc).Max(ca => ca.POS);
                    //    pos++;
                    //}
                    //catch (Exception)
                    //{

                    //}

                    ////Guardar carta
                    //if (guardar_param == "guardar_param")
                    //{
                    //    CARTA car = new CARTA();
                    //    car.NUM_DOC = v.num_doc;
                    //    car.POS = pos;

                    //    try
                    //    {
                    //        db.CARTAs.Add(car);
                    //        db.SaveChanges();
                    //    }
                    //    catch (Exception e)
                    //    {

                    //    }
                    //}
                    //B20180720P MGC Guardar Carta......................................................................

                    //B20180710 MGC 2018.07.19 Provisional obtener la siguiente posición para carta......................
                    int indexp = 1; //B20180710 MGC 2018.07.17
                    for (int i = 0; i < encabezadoFech.Count; i++)
                    {
                        contadorTabla = 0;
                        DateTime a1 = DateTime.Parse(encabezadoFech[i].Remove(encabezadoFech[i].Length / 2));
                        DateTime a2 = DateTime.Parse(encabezadoFech[i].Remove(0, encabezadoFech[i].Length / 2));

                        var con2 = db.DOCUMENTOPs
                                          .Where(x => x.NUM_DOC.Equals(v.num_doc) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                          .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new { x.MATNR, x.MATKL, y.MAKTX, x.MONTO, y.PUNIT, x.PORC_APOYO, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.APOYO_EST, x.APOYO_REAL, x.VIGENCIA_DE, x.VIGENCIA_AL }) //B20180710 MGC 2018.07.19
                                          .ToList();


                        if (con2.Count > 0)
                        {
                            foreach (var item2 in con2)
                            {
                                //B20180710 MGC 2018.07.17 Pasar los documentos almacenados pero con los nuevos valores editados
                                //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                                //armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                                //armadoCuerpoTab.Add(item2.MATKL);
                                //armadoCuerpoTab.Add(item2.MAKTX);                        
                                //if (v.costoun_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString()); }
                                //if (v.apoyo_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString()); }
                                //if (v.apoyop_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString()); }
                                //if (v.costoap_x == true) { armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString()); }
                                //if (v.precio_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString()); }
                                //if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString()); }
                                //if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString()); }
                                DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();

                                try
                                {
                                    docmod = v.DOCUMENTOP.Where(x => x.MATNR == item2.MATNR.TrimStart('0')).FirstOrDefault();

                                    if (docmod != null)
                                    {
                                        armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                                        armadoCuerpoTab.Add(item2.MATKL);
                                        armadoCuerpoTab.Add(item2.MAKTX);

                                        if (v.costoun_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyo_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShowPorc(Math.Round(docmod.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyop_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.costoap_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.precio_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        ////B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                                        ////fact = true es real
                                        ////Apoyo
                                        //if (fact)
                                        //{
                                        //    if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_REAL), 2).ToString()); }
                                        //}
                                        //else
                                        //{
                                        //    if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_EST), 2).ToString()); }
                                        //}
                                        //Volumen
                                        //Volumen
                                        //B20180726 MGC 2018.07.26
                                        if (v.volumen_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_REAL), 2).ToString()); //B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_REAL), 2), decimales)); //B20180730 MGC 2018.07.30 Formatos
                                                //carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_EST), 2).ToString()); //B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_EST), 2), decimales)); //B20180730 MGC 2018.07.30 Formatos
                                                //carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                            }
                                        }

                                        //Apoyo
                                        //B20180726 MGC 2018.07.26
                                        if (v.apoyototal_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                        }

                                        //Guardar carta
                                        if (guardar_param == "guardar_param")
                                        {
                                            CARTAP carp = new CARTAP();
                                            //Armado para registro en bd
                                            carp.NUM_DOC = v.num_doc;
                                            carp.POS_ID = pos;
                                            carp.POS = indexp;
                                            //carp.MATNR = item2.MATNR.TrimStart('0');
                                            carp.MATNR = item2.MATNR;
                                            carp.MATKL = item2.MATKL;
                                            carp.CANTIDAD = 1;
                                            //B20180726 MGC 2018.07.26
                                            //if (v.costoun_x == true) { carp.MONTO = docmod.MONTO; }
                                            //if (v.apoyo_x == true) { carp.PORC_APOYO = docmod.PORC_APOYO; }
                                            //if (v.apoyop_x == true) { carp.MONTO_APOYO = docmod.MONTO_APOYO; }
                                            //if (v.precio_x == true) { carp.PRECIO_SUG = docmod.PRECIO_SUG; }
                                            carp.MONTO = docmod.MONTO;
                                            carp.PORC_APOYO = docmod.PORC_APOYO;
                                            carp.MONTO_APOYO = docmod.MONTO_APOYO;
                                            carp.PRECIO_SUG = docmod.PRECIO_SUG;

                                            //Volumen
                                            //B20180726 MGC 2018.07.26
                                            if (v.volumen_x == true)
                                            {
                                                if (fact)
                                                {
                                                    carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                                    carp.VOLUMEN_EST = 0;
                                                }
                                                else
                                                {
                                                    carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                                    carp.VOLUMEN_REAL = 0;
                                                }
                                            }

                                            //Apoyo
                                            //B20180726 MGC 2018.07.26
                                            if (v.apoyototal_x == true)
                                            {
                                                if (fact)
                                                {
                                                    carp.APOYO_REAL = docmod.APOYO_REAL;
                                                    carp.APOYO_EST = 0;
                                                }
                                                else
                                                {
                                                    carp.APOYO_EST = docmod.APOYO_EST;
                                                    carp.APOYO_REAL = 0;
                                                }
                                            }

                                            //Fechas
                                            carp.VIGENCIA_DE = item2.VIGENCIA_DE;
                                            carp.VIGENCIA_AL = item2.VIGENCIA_AL;

                                            try
                                            {
                                                //Guardar en CARPETAP
                                                db.CARTAPs.Add(carp);
                                                db.SaveChanges();
                                                indexp++;
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
                                contadorTabla++;
                            }
                        }
                        else
                        {
                            var con3 = db.DOCUMENTOPs.Where(x => x.NUM_DOC.Equals(v.num_doc) & x.VIGENCIA_DE == a1 & x.VIGENCIA_AL == a2).ToList();
                            //.Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new { x.NUM_DOC, x.MATNR, x.MATKL, y.ID, x.MONTO, x.PORC_APOYO, y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.APOYO_EST, x.APOYO_REAL, x.VIGENCIA_DE, x.VIGENCIA_AL }) //B20180710 MGC 2018.07.19
                            //.ToList();

                            foreach (var item2 in con3)
                            {
                                //B20180710 MGC 2018.07.17 Pasar los documentos almacenados pero con los nuevos valores editados
                                //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                                //armadoCuerpoTab.Add("");
                                //armadoCuerpoTab.Add(item2.MATKL);
                                //armadoCuerpoTab.Add(item2.TXT50);
                                //if (v.costoun_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString()); }
                                //if (v.apoyo_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString()); }
                                //if (v.apoyop_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString()); }
                                //if (v.costoap_x == true) { armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString()); }
                                //if (v.precio_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString()); }
                                //if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString()); }
                                //if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString()); }
                                DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();

                                try
                                {
                                    docmod = v.DOCUMENTOP.Where(x => x.MATKL_ID == item2.MATKL).FirstOrDefault();

                                    if (docmod != null)
                                    {
                                        armadoCuerpoTab.Add("");
                                        armadoCuerpoTab.Add(item2.MATKL);
                                        //armadoCuerpoTab.Add(item2.TXT50);
                                        MATERIALGPT mt = db.MATERIALGPTs.Where(x => x.MATERIALGP_ID == item2.MATKL & x.SPRAS_ID == d.CLIENTE.SPRAS).FirstOrDefault();
                                        if (mt != null)
                                            armadoCuerpoTab.Add(mt.TXT50);
                                        else
                                            armadoCuerpoTab.Add("");

                                        if (v.costoun_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyo_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShowPorc(Math.Round(docmod.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyop_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }

                                        if (v.costoap_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.precio_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                                        //fact = true es real
                                        //Apoyo
                                        //if (fact)
                                        //{
                                        //    if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_REAL), 2).ToString()); }
                                        //}
                                        //else
                                        //{
                                        //    if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_EST), 2).ToString()); }
                                        //}

                                        //Volumen
                                        //B20180726 MGC 2018.07.26
                                        if (v.volumen_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                //carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                //carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                            }
                                        }

                                        //Apoyo
                                        //B20180726 MGC 2018.07.26
                                        if (v.apoyototal_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                        }


                                        //Guardar carta
                                        if (guardar_param == "guardar_param")
                                        {
                                            CARTAP carp = new CARTAP();
                                            //Armado para registro en bd
                                            carp.NUM_DOC = v.num_doc;
                                            carp.POS_ID = pos;
                                            carp.POS = indexp;
                                            carp.MATNR = "";
                                            carp.MATKL = item2.MATKL;
                                            carp.CANTIDAD = 1;
                                            //B20180726 MGC 2018.07.26
                                            //if (v.costoun_x == true) { carp.MONTO = docmod.MONTO; }
                                            //if (v.apoyo_x == true) { carp.PORC_APOYO = docmod.PORC_APOYO; }
                                            //if (v.apoyop_x == true) { carp.MONTO_APOYO = docmod.MONTO_APOYO; }
                                            //if (v.precio_x == true) { carp.PRECIO_SUG = docmod.PRECIO_SUG; }
                                            carp.MONTO = docmod.MONTO;
                                            carp.PORC_APOYO = docmod.PORC_APOYO;
                                            carp.MONTO_APOYO = docmod.MONTO_APOYO;
                                            carp.PRECIO_SUG = docmod.PRECIO_SUG;

                                            //Volumen
                                            //B20180726 MGC 2018.07.26
                                            if (v.volumen_x == true)
                                            {
                                                if (fact)
                                                {
                                                    carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                                    carp.VOLUMEN_EST = 0;
                                                }
                                                else
                                                {
                                                    carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                                    carp.VOLUMEN_REAL = 0;
                                                }
                                            }

                                            //Apoyo
                                            //B20180726 MGC 2018.07.26
                                            if (v.apoyototal_x == true)
                                            {
                                                if (fact)
                                                {
                                                    carp.APOYO_REAL = docmod.APOYO_REAL;
                                                    carp.APOYO_EST = 0;
                                                }
                                                else
                                                {
                                                    carp.APOYO_REAL = 0;
                                                    carp.APOYO_EST = docmod.APOYO_EST;
                                                }
                                            }

                                            //Fechas
                                            carp.VIGENCIA_DE = item2.VIGENCIA_DE;
                                            carp.VIGENCIA_AL = item2.VIGENCIA_AL;

                                            try
                                            {
                                                //Guardar en CARPETAP
                                                db.CARTAPs.Add(carp);
                                                db.SaveChanges();
                                                indexp++;
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
                                contadorTabla++;
                            }
                        }
                        numfilasTab.Add(contadorTabla);
                    }

                    //var cabeza = new List<string>(); //B20180720P MGC 2018.07.25
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "materialC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "categoriaC").Select(x => x.TEXTO).FirstOrDefault());
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "descripcionC").Select(x => x.TEXTO).FirstOrDefault());
                    if (v.costoun_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costouC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.apoyo_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopoC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.apoyop_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopiC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.costoap_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costoaC").Select(x => x.TEXTO).FirstOrDefault()); }
                    if (v.precio_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "preciosC").Select(x => x.TEXTO).FirstOrDefault()); }
                    //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                    //fact = true es real
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

                CartaV carta = v;
                CartaVEsqueleto cve = new CartaVEsqueleto();
                cve.crearPDF(carta, user.SPRAS_ID, aprob);
                string recibeRuta = Convert.ToString(Session["rutaCompletaV"]);
                return RedirectToAction("Index", new { ruta = recibeRuta, ids = v.num_doc });
            }
        }

        // POST: CartaV/Details/5
        [HttpPost]
        //[ValidateAntiForgeryToken] //B20180710 MGC 2018.07.16 Modificaciones para editar los campos de distribución se agrego los objetos
        //public ActionResult Create([Bind(Include = "num_doc, listaCuerpo, DOCUMENTOP")] CartaV v)
        public ActionResult Visualizar(CartaV v, string monto_enviar)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                string u = User.Identity.Name;
                var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

                List<string> encabezadoFech = new List<string>();
                List<string> armadoCuerpoTab = new List<string>(); //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTab = new List<int>();

                int contadorTabla = 0;
                DOCUMENTO d = db.DOCUMENTOes.Find(v.num_doc);

                //Formatos para numeros
                d.PAI = db.PAIS.Where(a => a.LAND.Equals(d.PAIS_ID)).FirstOrDefault();
                string miles = d.PAI.MILES; //B20180730 MGC 2018.07.30 Formatos
                string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos

                FormatosC format = new FormatosC(); //B20180730 MGC 2018.07.30 Formatos

                /////////////////////////////////////////////DATOS PARA LA TABLA 1 MATERIALES EN EL PDF///////////////////////////////////////
                //B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                bool fact = false;
                try
                {
                    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                }
                catch (Exception)
                {

                }
                //B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol..............
                //B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol..............

                //B20180801 MGC Formato
                //v.monto = monto_enviar;
                decimal montod = 0;
                try
                {
                    montod = Convert.ToDecimal(monto_enviar);
                }
                catch (Exception)
                {

                }
                v.monto = format.toShow(montod, decimales);

                var cabeza = new List<string>();
                bool varligada = Convert.ToBoolean(d.LIGADA);
                if (varligada != true)
                {
                    var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(v.num_doc)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();
                    //B20180710 MGC 2018.07.17 Modificación de monto
                    //v.monto = monto_enviar;
                    ////B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                    //bool fact = false;
                    //try
                    //{
                    //    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                    //}
                    //catch (Exception)
                    //{

                    //}
                    ////B20180710 MGC 2018.07.17 Modificación 9 y 10 dependiendo del campo de factura en tsol..............

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
                                          .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new { x.MATNR, x.MATKL, y.MAKTX, x.MONTO, y.PUNIT, x.PORC_APOYO, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.APOYO_EST, x.APOYO_REAL })
                                          .ToList();


                        if (con2.Count > 0)
                        {
                            foreach (var item2 in con2)
                            {
                                //B20180710 MGC 2018.07.17 Pasar los documentos almacenados pero con los nuevos valores editados
                                //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                                //armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                                //armadoCuerpoTab.Add(item2.MATKL);
                                //armadoCuerpoTab.Add(item2.MAKTX);                        
                                //if (v.costoun_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString()); }
                                //if (v.apoyo_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString()); }
                                //if (v.apoyop_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString()); }
                                //if (v.costoap_x == true) { armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString()); }
                                //if (v.precio_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString()); }
                                //if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString()); }
                                //if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString()); }
                                DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();

                                try
                                {
                                    docmod = v.DOCUMENTOP.Where(x => x.MATNR == item2.MATNR.TrimStart('0')).FirstOrDefault();

                                    if (docmod != null)
                                    {
                                        armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                                        armadoCuerpoTab.Add(item2.MATKL);
                                        armadoCuerpoTab.Add(item2.MAKTX);

                                        if (v.costoun_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyo_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShowPorc(Math.Round(docmod.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyop_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }

                                        if (v.costoap_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.precio_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                                        //fact = true es real
                                        //Apoyo
                                        //B20180726 MGC 2018.07.26
                                        if (v.volumen_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                        }
                                        //Volumen
                                        //B20180726 MGC 2018.07.26
                                        if (v.apoyototal_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {

                                }
                                contadorTabla++;
                            }
                        }
                        else
                        {
                            var con3 = db.DOCUMENTOPs
                                                .Where(x => x.NUM_DOC.Equals(v.num_doc) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                                .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new { x.NUM_DOC, x.MATNR, x.MATKL, y.ID, x.MONTO, x.PORC_APOYO, y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50, x.MONTO_APOYO, resta = (x.MONTO - x.MONTO_APOYO), x.PRECIO_SUG, x.APOYO_EST, x.APOYO_REAL })
                                                .ToList();

                            foreach (var item2 in con3)
                            {
                                //B20180710 MGC 2018.07.17 Pasar los documentos almacenados pero con los nuevos valores editados
                                //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                                //armadoCuerpoTab.Add("");
                                //armadoCuerpoTab.Add(item2.MATKL);
                                //armadoCuerpoTab.Add(item2.TXT50);
                                //if (v.costoun_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString()); }
                                //if (v.apoyo_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString()); }
                                //if (v.apoyop_x == true) { armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString()); }
                                //if (v.costoap_x == true) { armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString()); }
                                //if (v.precio_x == true) { armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString()); }
                                //if (v.apoyoEst_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString()); }
                                //if (v.apoyoRea_x == true) { armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString()); }
                                DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();

                                try
                                {
                                    docmod = v.DOCUMENTOP.Where(x => x.MATKL_ID == item2.MATKL).FirstOrDefault();

                                    if (docmod != null)
                                    {
                                        armadoCuerpoTab.Add("");
                                        armadoCuerpoTab.Add(item2.MATKL);
                                        armadoCuerpoTab.Add(item2.TXT50);

                                        if (v.costoun_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyo_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.MONTO_APOYO, 2).ToString())//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShowPorc(Math.Round(docmod.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.apoyop_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PORC_APOYO, 2).ToString())//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }

                                        if (v.costoap_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2).ToString())//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        if (v.precio_x == true)
                                        {
                                            //armadoCuerpoTab.Add(Math.Round(docmod.PRECIO_SUG, 2).ToString())//B20180730 MGC 2018.07.30 Formatos
                                            armadoCuerpoTab.Add(format.toShow(Math.Round(docmod.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                        }
                                        //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                                        //fact = true es real
                                        //Apoyo
                                        //B20180726 MGC 2018.07.26
                                        if (v.volumen_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.VOLUMEN_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                        }
                                        //Volumen
                                        //B20180726 MGC 2018.07.26
                                        if (v.apoyototal_x == true)
                                        {
                                            if (fact)
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_REAL), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                            else
                                            {
                                                //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(docmod.APOYO_EST), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                                armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {

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
                    //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                    //fact = true es real
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

                CartaV carta = v;
                CartaVEsqueleto cve = new CartaVEsqueleto();
                cve.crearPDF(carta, user.SPRAS_ID, aprob);
                string recibeRuta = Convert.ToString(Session["rutaCompletaV"]);
                return RedirectToAction("Index", new { ruta = recibeRuta, ids = v.num_doc });
            }
        }
        // GET: CartaV/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartaV/Edit/5
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

        // GET: CartaV/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartaV/Delete/5
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

        //B20180710 MGC 2018.07.13 Modificaciones para editar los campos de distribución se agrego los objetos
        [HttpPost]
        public ActionResult getPartialMat(List<DOCUMENTOP_MOD> docs)
        {

            CartaV doc = new CartaV();

            doc.DOCUMENTOP = docs;
            return PartialView("~/Views/CartaV/_PartialMatTr.cshtml", doc);
        }

        //B20180720P MGC 2018.07.23
        // GET: Lista de cartas
        public ActionResult Lista(decimal id, string swf)
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
                    ViewBag.pais = p + ".svg";
                }
                catch
                {
                    //return RedirectToAction("Pais", "Home");
                }
                Session["spras"] = user.SPRAS_ID;
                TempData["id"] = id;

                //B20180720P MGC 2018.07.25 Obtener el statuswf para reedireccionar a d o a v
                TempData["ESTATUS_WF"] = swf;

                var lista = db.CARTAs.Where(a => a.NUM_DOC.Equals(id)).ToList();
                return View(lista);
            }
        }

        //B20180720P MGC 2018.07.23
        // GET: CartaF/Create
        public ActionResult DetailsPos(decimal id, int pos)
        {
            //int pagina = 231; //ID EN BASE DE DATOS
            int pagina = 232; //ID EN BASE DE DATOS
            using (TAT001Entities db = new TAT001Entities())
            {
                //    string u = User.Identity.Name;
                //    var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
                //    ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
                //    ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
                //    ViewBag.usuario = user;
                //    ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                //    ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
                //    ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                //    ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
                //    //ViewBag.mon = db.TEXTOCARTAVs.Where(t => t.SPRAS_ID.Equals(user.SPRAS_ID)).Select(t => t.MONTO).FirstOrDefault();

                //    try
                //    {
                //        string pa = Session["pais"].ToString();
                //        ViewBag.pais = pa + ".svg";
                //    }
                //    catch
                //    {
                //        ViewBag.pais = "mx.svg";
                //        ////return RedirectToAction("Pais", "Home");
                //    }
                //    Session["spras"] = user.SPRAS_ID;
                //}
                //CARTA c = new CARTA();
                //using (TAT001Entities db = new TAT001Entities())
                //{
                //    c = db.CARTAs.Where(a => a.NUM_DOC.Equals(id) & a.POS.Equals(pos)).First();
                //}
                ////B20180720P MGC 2018.07.23
                ////CartaF cf = new CartaF();
                //CartaV cf = new CartaV();
                //cf.num_doc = id;
                //cf.company = c.COMPANY;
                //cf.company_x = (bool)c.COMPANYX;
                //cf.taxid = c.TAXID;
                //cf.taxid_x = (bool)c.TAXIDX;
                //cf.concepto = c.CONCEPTO;
                //cf.concepto_x = (bool)c.CONCEPTOX;
                //cf.cliente = c.CLIENTE;
                //cf.cliente_x = (bool)c.CLIENTEX;
                //cf.puesto = c.PUESTO;
                //cf.puesto_x = (bool)c.PUESTOX;
                //cf.direccion = c.DIRECCION;
                //cf.direccion_x = (bool)c.DIRECCIONX;
                //cf.folio = c.FOLIO;
                //cf.folio_x = (bool)c.FOLIOX;
                //cf.lugar = c.LUGAR;
                //cf.lugar_x = (bool)c.LUGARX;
                //cf.lugarFech = c.LUGARFECH;
                //cf.lugarFech_x = (bool)c.LUGARFECHX;
                //cf.payerId = c.PAYER;
                //cf.payerId_x = (bool)c.PAYERX;
                //cf.payerNom = c.NOMBREC;
                //cf.payerNom_x = (bool)c.NOMBRECX;
                //cf.estimado = c.ESTIMADO;
                //cf.estimado_x = (bool)c.ESTIMADOX;
                //cf.mecanica = c.MECANICA;
                //cf.mecanica_x = (bool)c.MECANICAX;
                //cf.monto = c.MONEDA;
                //cf.monto = c.MONTO;
                //cf.nombreE = c.NOMBREE;
                //cf.nombreE_x = (bool)c.NOMBREEX;
                //cf.puestoE = c.PUESTOE;
                //cf.puestoE_x = (bool)c.PUESTOEX;
                //cf.companyC = c.COMPANYC;
                //cf.companyC_x = (bool)c.COMPANYCX;
                //cf.nombreC = c.NOMBREC;
                //cf.nombreC_x = (bool)c.NOMBRECX;
                //cf.puestoC = c.PUESTOC;
                //cf.puestoC_x = (bool)c.PUESTOCX;
                //cf.companyCC = c.COMPANYCC;
                //cf.companyCC_x = (bool)c.COMPANYCCX;
                //cf.legal = c.LEGAL;
                //cf.legal_x = (bool)c.LEGALX;
                //cf.mail = c.MAIL;
                //cf.mail_x = (bool)c.MAILX;

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

                //B20180720P MGC 2018.07.23
                //ViewBag.miles = d.PAI.MILES;//LEJGG 090718
                //ViewBag.dec = d.PAI.DECIMAL;//LEJGG 090718

                //Formatos para numeros
                d.PAI = db.PAIS.Where(a => a.LAND.Equals(d.PAIS_ID)).FirstOrDefault();
                string miles = d.PAI.MILES; //B20180730 MGC 2018.07.30 Formatos
                string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos

                FormatosC format = new FormatosC(); //B20180730 MGC 2018.07.30 Formatos

                List<string> lista = new List<string>();
                List<listacuerpoc> armadoCuerpoTab = new List<listacuerpoc>(); //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTabla = new List<int>();
                int contadorTabla = 0;
                HeaderFooter hfc = new HeaderFooter();
                hfc.eliminaArchivos();
                CartaV cv = new CartaV();

                //B20180720P MGC 2018.07.23
                CARTA cs = new CARTA();
                cs = db.CARTAs.Where(a => a.NUM_DOC.Equals(id) & a.POS.Equals(pos)).First();
                //B20180720P MGC 2018.07.23
                //if (d != null)
                //{
                //    d.CLIENTE = db.CLIENTEs.Where(a => a.VKORG.Equals(d.VKORG)
                //                                              & a.VTWEG.Equals(d.VTWEG)
                //                                            & a.SPART.Equals(d.SPART)
                //                                            & a.KUNNR.Equals(d.PAYER_ID)).First();
                //    string sp = Session["spras"].ToString();
                //    pp = db.PUESTOTs.Where(a => a.SPRAS_ID.Equals(sp) && a.PUESTO_ID == d.USUARIO.PUESTO_ID).FirstOrDefault();
                //}
                //B20180720P MGC 2018.07.23
                //ViewBag.legal = db.LEYENDAs.Where(a => a.PAIS_ID.Equals(d.PAIS_ID) && a.ACTIVO == true).FirstOrDefault();
                ViewBag.legal = db.CARTAs.Where(a => a.NUM_DOC.Equals(id) & a.POS.Equals(pos)).First().LEGAL;


                /////////////////////////////////////////////DATOS PARA LA TABLA 1 MATERIALES EN LA VISTA///////////////////////////////////////
                //B20180720P MGC 2018.07.23
                //var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(id)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();
                var con = db.CARTAPs.Select(x => new { x.NUM_DOC, x.POS_ID, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(id) & a.POS_ID.Equals(pos)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();

                //B20180710 MGC 2018.07.12 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                bool fact = false;
                try
                {
                    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                }
                catch (Exception)
                {

                }
                //B20180710 MGC 2018.07.12 Modificación 9 y 10 dependiendo del campo de factura en tsol..............
                //B20180720P MGC 2018.07.23
                //B20180710 MGC 2018.07.18 total es input o text
                //string trclass = "";
                //bool editmonto = false; //B20180710 MGC 2018.07.18 editar el monto en porcentaje categoría

                foreach (var item in con)
                {
                    lista.Add(item.Key.VIGENCIA_DE.ToString() + item.Key.VIGENCIA_AL.ToString());
                }

                for (int i = 0; i < lista.Count; i++)
                {
                    contadorTabla = 0;

                    DateTime a1 = DateTime.Parse(lista[i].Remove(lista[i].Length / 2));
                    DateTime a2 = DateTime.Parse(lista[i].Remove(0, lista[i].Length / 2));

                    //B20180720P MGC 2018.07.23
                    //var con2 = db.DOCUMENTOPs
                    //                      .Where(x => x.NUM_DOC.Equals(id) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                    //                      .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new {
                    //                          x.NUM_DOC,
                    //                          x.MATNR,
                    //                          x.MATKL,
                    //                          y.MAKTX,
                    //                          x.MONTO,
                    //                          y.PUNIT,
                    //                          x.PORC_APOYO,
                    //                          x.MONTO_APOYO,
                    //                          resta = (x.MONTO - x.MONTO_APOYO),
                    //                          x.PRECIO_SUG,
                    //                          x.APOYO_EST,
                    //                          x.APOYO_REAL
                    //                      ,
                    //                          x.VOLUMEN_EST,
                    //                          x.VOLUMEN_REAL
                    //                      }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL
                    //                      .ToList();

                    var con2 = db.CARTAPs
                                          .Where(x => x.NUM_DOC.Equals(id) & x.POS_ID.Equals(pos) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                          .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new
                                          {
                                              x.NUM_DOC,
                                              x.MATNR,
                                              x.MATKL,
                                              y.MAKTX,
                                              x.MONTO,
                                              y.PUNIT,
                                              x.PORC_APOYO,
                                              x.MONTO_APOYO,
                                              resta = (x.MONTO - x.MONTO_APOYO),
                                              x.PRECIO_SUG,
                                              x.APOYO_EST,
                                              x.APOYO_REAL
                                          ,
                                              x.VOLUMEN_EST,
                                              x.VOLUMEN_REAL
                                          }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL
                                          .ToList();

                    //B20180720P MGC 2018.07.23
                    //Definición si la distribución es monto o porcentaje
                    //string porclass = "";//B20180710 MGC 2018.07.18 total es input o text
                    //string totalm = "";//B20180710 MGC 2018.07.18 total es input o text
                    //if (d.TIPO_TECNICO == "M")
                    //{
                    //    porclass = " tipom";
                    //    totalm = " total";
                    //    trclass = " total";
                    //}
                    //else if (d.TIPO_TECNICO == "P")
                    //{
                    //    porclass = " tipop";
                    //    totalm = " ni";
                    //}

                    if (con2.Count > 0)
                    {
                        foreach (var item2 in con2)
                        {
                            //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                            listacuerpoc lc1 = new listacuerpoc();
                            lc1.val = item2.MATNR.TrimStart('0');
                            lc1.clase = "ni";
                            armadoCuerpoTab.Add(lc1);

                            listacuerpoc lc2 = new listacuerpoc();
                            lc2.val = item2.MATKL;
                            lc2.clase = "ni";
                            armadoCuerpoTab.Add(lc2);

                            listacuerpoc lc3 = new listacuerpoc();
                            lc3.val = item2.MAKTX;
                            lc3.clase = "ni";
                            armadoCuerpoTab.Add(lc3);

                            //Costo unitario
                            listacuerpoc lc4 = new listacuerpoc();
                            //lc4.val = "$" + Math.Round(item2.MONTO, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                            lc4.val = format.toShow(Math.Round(item2.MONTO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc4.clase = "ni";
                            armadoCuerpoTab.Add(lc4);

                            //Porcentaje de apoyo
                            listacuerpoc lc5 = new listacuerpoc();
                            //lc5.val = Math.Round(item2.PORC_APOYO, 2).ToString() + "%";//B20180730 MGC 2018.07.30 Formatos
                            lc5.val = format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc5.clase = "ni";
                            armadoCuerpoTab.Add(lc5);

                            //Apoyo por pieza
                            listacuerpoc lc6 = new listacuerpoc();
                            //lc6.val = "$" + Math.Round(item2.MONTO_APOYO, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                            lc6.val = format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc6.clase = "ni";
                            armadoCuerpoTab.Add(lc6);

                            //Costo con apoyo
                            listacuerpoc lc7 = new listacuerpoc();
                            //lc7.val = "$" + Math.Round(item2.resta, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                            lc7.val = format.toShow(Math.Round(item2.resta, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc7.clase = "ni";
                            armadoCuerpoTab.Add(lc7);

                            //Precio Sugerido
                            listacuerpoc lc8 = new listacuerpoc();
                            //lc8.val = "$" + Math.Round(item2.PRECIO_SUG, 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                            lc8.val = format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc8.clase = "ni";
                            armadoCuerpoTab.Add(lc8);

                            //Modificación 9 y 10 dependiendo del campo de factura en tsol
                            //fact = true es real
                            //Volumen
                            listacuerpoc lc9 = new listacuerpoc();
                            if (fact)
                            {
                                //lc9.val = Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc9.val = format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                //lc9.val = Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc9.val = format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            lc9.clase = "ni";
                            armadoCuerpoTab.Add(lc9);

                            //Apoyo estimado
                            listacuerpoc lc10 = new listacuerpoc();
                            if (fact)
                            {
                                //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            lc10.clase = "ni";
                            armadoCuerpoTab.Add(lc10);

                            contadorTabla++;
                        }
                    }
                    else
                    {
                        //B20180720P MGC 2018.07.23
                        //var con3 = db.DOCUMENTOPs
                        //                    .Where(x => x.NUM_DOC.Equals(id) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                        //                    .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new {
                        //                        x.NUM_DOC,
                        //                        x.MATNR,
                        //                        x.MATKL,
                        //                        y.ID,
                        //                        x.MONTO,
                        //                        x.PORC_APOYO,
                        //                        y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50,
                        //                        x.MONTO_APOYO,
                        //                        resta = (x.MONTO - x.MONTO_APOYO),
                        //                        x.PRECIO_SUG,
                        //                        x.APOYO_EST,
                        //                        x.APOYO_REAL
                        //                    ,
                        //                        x.VOLUMEN_EST,
                        //                        x.VOLUMEN_REAL
                        //                    }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL})
                        //                    .ToList();
                        var con3 = db.CARTAPs
                                            .Where(x => x.NUM_DOC.Equals(id) & x.POS_ID.Equals(pos) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                            .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new
                                            {
                                                x.NUM_DOC,
                                                x.MATNR,
                                                x.MATKL,
                                                y.ID,
                                                x.MONTO,
                                                x.PORC_APOYO,
                                                y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50,
                                                x.MONTO_APOYO,
                                                resta = (x.MONTO - x.MONTO_APOYO),
                                                x.PRECIO_SUG,
                                                x.APOYO_EST,
                                                x.APOYO_REAL
                                            ,
                                                x.VOLUMEN_EST,
                                                x.VOLUMEN_REAL
                                            }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL})
                                            .ToList();
                        //B20180720P MGC 2018.07.23
                        //if (d.TIPO_TECNICO == "M")
                        //{
                        //    trclass = "total";
                        //}
                        //else if (d.TIPO_TECNICO == "P")
                        //{
                        //    editmonto = true;
                        //}


                        foreach (var item2 in con3)
                        {
                            //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                            listacuerpoc lc1 = new listacuerpoc();
                            lc1.val = "";
                            lc1.clase = "ni";
                            armadoCuerpoTab.Add(lc1);

                            listacuerpoc lc2 = new listacuerpoc();
                            lc2.val = item2.MATKL;
                            lc2.clase = "ni";
                            armadoCuerpoTab.Add(lc2);

                            listacuerpoc lc3 = new listacuerpoc();
                            lc3.val = item2.TXT50;
                            lc3.clase = "ni";
                            armadoCuerpoTab.Add(lc3);

                            //Costo unitario
                            listacuerpoc lc4 = new listacuerpoc();
                            //lc4.val = Math.Round(item2.MONTO, 2).ToString();
                            //lc4.val = "";//B20180730 MGC 2018.07.30 Formatos
                            lc4.val = format.toShow(0, decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc4.clase = "ni";
                            armadoCuerpoTab.Add(lc4);

                            //Porcentaje de apoyo
                            listacuerpoc lc5 = new listacuerpoc();
                            //lc5.val = Math.Round(item2.PORC_APOYO, 2).ToString();
                            //Definición si la distribución es monto o porcentaje
                            if (d.TIPO_TECNICO == "M")
                            {
                                //lc5.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc5.val = format.toShowPorc(0, decimales); ;//B20180730 MGC 2018.07.30 Formatos
                            }
                            else if (d.TIPO_TECNICO == "P")
                            {
                                //lc5.val = Math.Round(item2.PORC_APOYO, 2).ToString() + "%";//B20180730 MGC 2018.07.30 Formatos
                                lc5.val = format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales); ;//B20180730 MGC 2018.07.30 Formatos
                            }

                            //lc5.clase = "input_oper numberd input_dc";
                            lc5.clase = "ni";
                            armadoCuerpoTab.Add(lc5);

                            //Apoyo por pieza
                            listacuerpoc lc6 = new listacuerpoc();
                            //lc6.val = Math.Round(item2.MONTO_APOYO, 2).ToString();
                            //lc6.val = "";//B20180730 MGC 2018.07.30 Formatos
                            lc6.val = format.toShow(0, decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc6.clase = "ni";
                            armadoCuerpoTab.Add(lc6);

                            //Costo con apoyo
                            listacuerpoc lc7 = new listacuerpoc();
                            //lc7.val = Math.Round(item2.resta, 2).ToString();
                            //lc7.val = "";//B20180730 MGC 2018.07.30 Formatos
                            lc7.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc7.clase = "ni";
                            armadoCuerpoTab.Add(lc7);

                            //Precio Sugerido
                            listacuerpoc lc8 = new listacuerpoc();
                            //lc8.val = Math.Round(item2.PRECIO_SUG, 2).ToString();
                            //lc8.val = "";//B20180730 MGC 2018.07.30 Formatos
                            lc8.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc8.clase = "ni";
                            armadoCuerpoTab.Add(lc8);
                            //Modificación 9 y 10 dependiendo del campo de factura en tsol
                            //fact = true es real

                            //Volumen
                            listacuerpoc lc9 = new listacuerpoc();
                            if (fact)
                            {
                                //lc9.val = Math.Round(Convert.ToDouble(item2.VOLUMEN_REAL), 2).ToString();
                                lc9.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc9.val = format.toShowNum(0, decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                //lc9.val = Math.Round(Convert.ToDouble(item2.VOLUMEN_EST), 2).ToString();
                                lc9.val = "";//B20180730 MGC 2018.07.30 Formatos
                                lc9.val = format.toShowNum(0, decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            lc9.clase = "ni";
                            armadoCuerpoTab.Add(lc9);

                            //Apoyo
                            listacuerpoc lc10 = new listacuerpoc();
                            if (fact)
                            {
                                //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                //lc10.val = "$" + Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2).ToString();//B20180730 MGC 2018.07.30 Formatos
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            //Definición si la distribución es monto o porcentaje
                            if (d.TIPO_TECNICO == "M")
                            {
                                lc10.clase = "ni";
                            }
                            else if (d.TIPO_TECNICO == "P")
                            {
                                lc10.clase = "ni";
                            }

                            armadoCuerpoTab.Add(lc10);

                            contadorTabla++;
                        }
                    }
                    numfilasTabla.Add(contadorTabla);
                }

                var cabeza = new List<string>();
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "materialC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "categoriaC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "descripcionC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costouC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopoC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopiC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costoaC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "preciosC").Select(x => x.TEXTO).FirstOrDefault());
                //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                //fact = true es real
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
                if (fact)
                {
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyorC").Select(x => x.TEXTO).FirstOrDefault());
                }
                else
                {
                    cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyoeC").Select(x => x.TEXTO).FirstOrDefault());
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                cv.listaCuerpom = armadoCuerpoTab;////NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE QUE POSTERIORMENTE ES DISTRIBUIDA EN LAS TABLAS //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                cv.numColEncabezado = cabeza;////////NUMERO DE COLUMNAS PARA LAS TABLAS

                //B20180720P MGC 2018.07.23
                //cv.secondTab_x = true;
                //cv.costoun_x = true;
                //cv.apoyo_x = true;
                //cv.apoyop_x = true;
                //cv.costoap_x = true;
                //cv.precio_x = true;
                //cv.apoyoEst_x = true; //Volumen
                //cv.apoyoRea_x = true; //Apoyo

                //B20180720P MGC 2018.07.23
                cv.secondTab_x = Convert.ToBoolean(cs.SECOND_TABX);
                cv.costoun_x = Convert.ToBoolean(cs.COSTO_UNX);
                cv.apoyo_x = Convert.ToBoolean(cs.APOYOX);
                cv.apoyop_x = Convert.ToBoolean(cs.APOYOPX);
                cv.costoap_x = Convert.ToBoolean(cs.COSTOAPX);
                cv.precio_x = Convert.ToBoolean(cs.PRECIOX);
                //B20180726 MGC 2018.07.26
                //cv.apoyoEst_x = Convert.ToBoolean(cs.APOYO_ESTX); //Volumen
                //cv.apoyoRea_x = Convert.ToBoolean(cs.APOYO_REAX); //Apoyo
                if (fact)
                {
                    cv.volumen_x = Convert.ToBoolean(cs.VOLUMEN_REAX); //Volumen
                    cv.apoyototal_x = Convert.ToBoolean(cs.APOYO_REAX); //Apoyo
                }
                else
                {
                    cv.volumen_x = Convert.ToBoolean(cs.VOLUMEN_ESTX); //Volumen
                    cv.apoyototal_x = Convert.ToBoolean(cs.APOYO_ESTX); //Apoyo
                }


                //TABLA 2 RECURRENCIAS
                cv.numColEncabezado2 = cabeza2;////////NUMERO DE COLUMNAS PARA LAS TABLAS
                cv.numfilasTabla2 = con4.Count();//////NUMERO FILAS TOTAL PARA LA TABLA
                cv.listaCuerpoRec = armadoCuerpoTab2;//NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE
                ///////////////////////////////

                //B20180720P MGC 2018.07.23
                //cv.num_doc = id;
                //cv.company = d.SOCIEDAD.BUTXT;
                //cv.company_x = true;
                //cv.taxid = d.SOCIEDAD.LAND;
                //cv.taxid_x = true;
                //cv.concepto = d.CONCEPTO;
                //cv.concepto_x = true;
                //cv.cliente = d.PAYER_NOMBRE;
                //cv.cliente_x = true;
                //cv.puesto = " ";
                //cv.puesto_x = false;
                //cv.direccion = d.CLIENTE.STRAS_GP;
                //cv.direccion_x = true;
                //cv.folio = d.NUM_DOC.ToString();
                //cv.folio_x = true;
                //cv.lugar = d.CIUDAD.Trim() + ", " + d.ESTADO.Trim();
                //cv.lugar_x = true;
                //cv.lugarFech = DateTime.Now.ToShortDateString();
                //cv.lugarFech_x = true;
                //cv.payerId = d.CLIENTE.PAYER;
                //cv.payerId_x = true;
                //cv.payerNom = d.CLIENTE.NAME1;
                //cv.payerNom_x = true;
                //cv.estimado = d.PAYER_NOMBRE;
                //cv.estimado_x = true;
                //cv.mecanica = d.NOTAS;
                //cv.mecanica_x = true;
                //cv.nombreE = d.USUARIO.NOMBRE + " " + d.USUARIO.APELLIDO_P + " " + d.USUARIO.APELLIDO_M;
                //cv.nombreE_x = true;
                //if (pp != null)
                //    cv.puestoE = pp.TXT50;
                //cv.puestoE_x = true;
                //cv.companyC = cv.company;
                //cv.companyC_x = true;
                //cv.nombreC = d.PAYER_NOMBRE;
                //cv.nombreC_x = true;
                //cv.puestoC = " ";
                //cv.puestoC_x = false;
                //cv.companyCC = d.CLIENTE.NAME1;
                //cv.companyCC_x = true;
                //if (ViewBag.legal != null)
                //    cv.legal = ViewBag.legal.LEYENDA1;
                //cv.legal_x = true;
                //cv.mail = db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "correo").Select(x => x.TEXTO).FirstOrDefault() + " " + d.PAYER_EMAIL;
                //cv.mail_x = true;
                //cv.comentarios = "";
                //cv.comentarios_x = true;
                //cv.compromisoK = "";
                //cv.compromisoK_x = true;
                //cv.compromisoC = "";
                //cv.compromisoC_x = true;
                //cv.monto_x = true;
                //cv.monto = d.MONTO_DOC_MD.ToString();
                //cv.moneda = d.MONEDA_ID;


                //B20180720P MGC 2018.07.23
                //CARTA ca = new CARTA();
                //ca.NUM_DOC = v.num_doc;
                //ca.CLIENTE = v.cliente;
                //ca.CLIENTEX = v.cliente_x;
                //ca.COMPANY = v.company;
                //ca.COMPANYC = v.companyC;
                //ca.COMPANYCC = v.companyCC;
                //ca.COMPANYCCX = v.companyCC_x;
                //ca.COMPANYCX = v.companyC_x;
                //ca.COMPANYX = v.company_x;
                //ca.CONCEPTO = v.concepto;
                //ca.CONCEPTOX = v.concepto_x;
                //ca.DIRECCION = v.direccion;
                //ca.DIRECCIONX = v.direccion_x;
                ////ca.DOCUMENTO = v.DOCUMENTO;
                //ca.ESTIMADO = v.estimado;
                //ca.ESTIMADOX = v.estimado_x;
                ////ca.FECHAC = v.FECHAC;
                //ca.FOLIO = v.folio;
                //ca.FOLIOX = v.folio_x;
                //ca.LEGAL = v.legal;
                //ca.LEGALX = v.legal_x;
                //ca.LUGARFECH = v.lugarFech;
                //ca.LUGARFECHX = v.lugarFech_x;
                //ca.LUGAR = v.lugar;
                //ca.LUGARX = v.lugar_x;
                //ca.MAIL = v.mail;
                //ca.MAILX = v.mail_x;
                //ca.MECANICA = v.mecanica;
                //ca.MECANICAX = v.mecanica_x;
                //ca.NOMBREC = v.nombreC;
                //ca.NOMBRECX = v.nombreC_x;
                //ca.NOMBREE = v.nombreE;
                //ca.NOMBREEX = v.nombreE_x;
                //ca.NUM_DOC = v.num_doc;
                //ca.PAYER = v.payerId;
                //ca.PAYERX = v.payerId_x;
                //ca.PUESTO = v.puesto;
                //ca.PUESTOC = v.puestoC;
                //ca.PUESTOCX = v.puestoC_x;
                //ca.PUESTOE = v.puestoE;
                //ca.PUESTOEX = v.puestoE_x;
                //ca.PUESTOX = v.puestoE_x;
                //ca.TAXID = v.taxid;
                //ca.TAXIDX = v.taxid_x;
                //ca.MONTO = v.monto;
                //ca.MONEDA = v.moneda;
                ////ca.TIPO = v.TIPO;
                ////ca.USUARIO = v.USUARIO;
                ////ca.USUARIO_ID = v.USUARIO_ID;
                //ca.USUARIO_ID = User.Identity.Name;
                //ca.FECHAC = DateTime.Now;

                cv.num_doc = cs.NUM_DOC;
                cv.pos = pos;
                cv.company = cs.COMPANY;
                cv.company_x = Convert.ToBoolean(cs.COMPANYX);
                cv.taxid = cs.TAXID;
                cv.taxid_x = Convert.ToBoolean(cs.TAXIDX);
                cv.concepto = cs.CONCEPTO;
                cv.concepto_x = Convert.ToBoolean(cs.CONCEPTOX);
                //cv.cliente = d.PAYER_NOMBRE;
                //cv.cliente = cs.PAYER;
                cv.cliente = cs.CLIENTE;
                //cv.cliente_x = Convert.ToBoolean(cs.PAYERX);
                cv.cliente_x = Convert.ToBoolean(cs.CLIENTEX);
                cv.puesto = cs.PUESTOC;
                cv.puesto_x = Convert.ToBoolean(cs.PUESTOCX);
                cv.direccion = cs.DIRECCION;
                cv.direccion_x = Convert.ToBoolean(cs.DIRECCIONX);
                cv.folio = cs.FOLIO;
                cv.folio_x = Convert.ToBoolean(cs.FOLIOX);
                cv.lugar = cs.LUGAR;
                cv.lugar_x = Convert.ToBoolean(cs.LUGARX);
                cv.lugarFech = cs.LUGARFECH;
                cv.lugarFech_x = Convert.ToBoolean(cs.LUGARFECHX);
                //cv.payerId = d.CLIENTE.PAYER;
                cv.payerId = cs.PAYER;
                cv.payerId_x = Convert.ToBoolean(cs.PAYERX);
                //cv.payerNom = d.CLIENTE.NAME1;
                //cv.estimado = d.PAYER_NOMBRE;
                cv.estimado = cs.ESTIMADO;
                cv.estimado_x = Convert.ToBoolean(cs.ESTIMADOX);
                cv.mecanica = cs.MECANICA;
                cv.mecanica_x = Convert.ToBoolean(cs.MECANICAX);

                cv.nombreE = cs.NOMBREE;
                cv.nombreE_x = Convert.ToBoolean(cs.NOMBREEX);
                if (cs.PUESTOE != null)
                    cv.puestoE = cs.PUESTOE;
                cv.puestoE_x = Convert.ToBoolean(cs.PUESTOEX);
                cv.companyC = cs.COMPANYC;
                cv.companyC_x = Convert.ToBoolean(cs.COMPANYCX);
                cv.nombreC = cs.NOMBREC;
                cv.nombreC_x = Convert.ToBoolean(cs.NOMBRECX);
                cv.puestoC = cs.PUESTOC;
                cv.puestoC_x = Convert.ToBoolean(cs.PUESTOCX);
                cv.companyCC = cs.COMPANYCC;
                cv.companyCC_x = Convert.ToBoolean(cs.COMPANYCCX);
                if (ViewBag.legal != null)
                    cv.legal = ViewBag.legal;
                cv.legal_x = Convert.ToBoolean(cs.LEGALX);
                cv.mail = cs.MAIL;
                cv.mail_x = Convert.ToBoolean(cs.MAILX);
                cv.monto_x = true;
                cv.monto = cs.MONTO.ToString();
                cv.moneda = cs.MONEDA;

                //B20180720P MGC Guardar Carta
                cv.payerNom = cs.PAYERNOM;
                cv.payerNom_x = Convert.ToBoolean(cs.PAYERNOMX);
                cv.comentarios = cs.COMENTARIO;
                cv.comentarios_x = Convert.ToBoolean(cs.COMENTARIOX);
                cv.compromisoC = cs.COMPROMISOC;
                cv.compromisoC_x = Convert.ToBoolean(cs.COMPROMISOCX);
                cv.compromisoK = cs.COMPROMISOK;
                cv.compromisoK_x = Convert.ToBoolean(cs.COMPROMISOKX);

                //B20180720P MGC 2018.07.23
                //ViewBag.factura = fact;//B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                //ViewBag.trclass = trclass;//B20180710 MGC 2018.07.18 total es input o text
                //ViewBag.editmonto = editmonto;//B20180710 MGC 2018.07.18 total es input o text

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

                cv.monto = format.toShow(montod, decimales);

                return View(cv);
            }
        }
        //B20180720P MGC 2018.07.24
        [HttpPost]
        public ActionResult DetailsPos(CartaV va)
        {
            //Obtener el id y la pos
            decimal id = va.num_doc;
            int pos = va.pos;


            //int pagina = 231; //ID EN BASE DE DATOS
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

                //B20180720P MGC 2018.07.23
                //ViewBag.miles = d.PAI.MILES;//LEJGG 090718
                //ViewBag.dec = d.PAI.DECIMAL;//LEJGG 090718
                //B20180726 MGC 2018.07.26
                bool fact = false;
                try
                {
                    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                }
                catch (Exception)
                {

                }

                List<string> encabezadoFech = new List<string>();
                List<string> armadoCuerpoTab = new List<string>(); //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                List<string> armadoCuerpoTab2 = new List<string>();
                List<int> numfilasTabla = new List<int>();
                int contadorTabla = 0;
                HeaderFooter hfc = new HeaderFooter();
                hfc.eliminaArchivos();
                CartaV v = new CartaV();

                //B20180720P MGC 2018.07.23
                CARTA cs = new CARTA();
                cs = db.CARTAs.Where(a => a.NUM_DOC.Equals(id) & a.POS.Equals(pos)).First();

                ViewBag.legal = db.CARTAs.Where(a => a.NUM_DOC.Equals(id) & a.POS.Equals(pos)).First().LEGAL;

                v.num_doc = cs.NUM_DOC;
                v.pos = pos;
                v.company = cs.COMPANY;
                v.company_x = Convert.ToBoolean(cs.COMPANYX);
                v.taxid = cs.TAXID;
                v.taxid_x = Convert.ToBoolean(cs.TAXIDX);
                v.concepto = cs.CONCEPTO;
                v.concepto_x = Convert.ToBoolean(cs.CONCEPTOX);
                //v.cliente = d.PAYER_NOMBRE;
                //v.cliente = cs.PAYER;
                v.cliente = cs.CLIENTE;
                //v.cliente_x = Convert.ToBoolean(cs.PAYERX);
                v.cliente_x = Convert.ToBoolean(cs.CLIENTEX);
                v.puesto = cs.PUESTOC;
                v.puesto_x = Convert.ToBoolean(cs.PUESTOCX);
                v.direccion = cs.DIRECCION;
                v.direccion_x = Convert.ToBoolean(cs.DIRECCIONX);
                v.folio = cs.FOLIO;
                v.folio_x = Convert.ToBoolean(cs.FOLIOX);
                v.lugar = cs.LUGAR;
                v.lugar_x = Convert.ToBoolean(cs.LUGARX);
                v.lugarFech = cs.LUGARFECH;
                v.lugarFech_x = Convert.ToBoolean(cs.LUGARFECHX);
                //v.payerId = d.CLIENTE.PAYER;
                //v.payerNom = d.CLIENTE.NAME1;
                v.payerNom = cs.NOMBREC;
                v.payerNom_x = Convert.ToBoolean(cs.NOMBRECX);
                //v.estimado = d.PAYER_NOMBRE;
                v.estimado = cs.ESTIMADO;
                v.estimado_x = Convert.ToBoolean(cs.ESTIMADOX);
                v.mecanica = cs.MECANICA;
                v.mecanica_x = Convert.ToBoolean(cs.MECANICAX);

                v.nombreE = cs.NOMBREE;
                v.nombreE_x = Convert.ToBoolean(cs.NOMBREEX);
                if (cs.PUESTOE != null)
                    v.puestoE = cs.PUESTOE;
                v.puestoE_x = Convert.ToBoolean(cs.PUESTOEX);
                v.companyC = cs.COMPANYC;
                v.companyC_x = Convert.ToBoolean(cs.COMPANYCX);
                v.nombreC = cs.NOMBREC;
                v.nombreC_x = Convert.ToBoolean(cs.NOMBRECX);
                v.puestoC = cs.PUESTOC;
                v.puestoC_x = Convert.ToBoolean(cs.PUESTOCX);
                v.companyCC = cs.COMPANYCC;
                v.companyCC_x = Convert.ToBoolean(cs.COMPANYCCX);
                if (ViewBag.legal != null)
                    v.legal = ViewBag.legal;
                v.legal_x = Convert.ToBoolean(cs.LEGALX);
                v.mail = cs.MAIL;
                v.mail_x = Convert.ToBoolean(cs.MAILX);
                v.monto_x = true;
                v.monto = cs.MONTO.ToString();
                v.moneda = cs.MONEDA;
                v.payerNom = cs.PAYERNOM;
                v.payerNom_x = Convert.ToBoolean(cs.PAYERNOMX);
                v.comentarios = cs.COMENTARIO;
                v.comentarios_x = Convert.ToBoolean(cs.COMENTARIOX);
                v.compromisoC = cs.COMPROMISOC;
                v.compromisoC_x = Convert.ToBoolean(cs.COMPROMISOCX);
                v.compromisoK = cs.COMPROMISOK;
                v.compromisoK_x = Convert.ToBoolean(cs.COMPROMISOKX);

                //B20180720P MGC 2018.07.23
                v.secondTab_x = Convert.ToBoolean(cs.SECOND_TABX);
                v.costoun_x = Convert.ToBoolean(cs.COSTO_UNX);
                v.apoyo_x = Convert.ToBoolean(cs.APOYOX);
                v.apoyop_x = Convert.ToBoolean(cs.APOYOPX);
                v.costoap_x = Convert.ToBoolean(cs.COSTOAPX);
                v.precio_x = Convert.ToBoolean(cs.PRECIOX);
                //B20180726 MGC 2018.07.26
                //v.apoyoEst_x = Convert.ToBoolean(cs.APOYO_ESTX); //Volumen
                //v.apoyoRea_x = Convert.ToBoolean(cs.APOYO_REAX); //Apoyo
                if (fact)
                {
                    v.volumen_x = Convert.ToBoolean(cs.VOLUMEN_REAX); //Volumen
                    v.apoyototal_x = Convert.ToBoolean(cs.APOYO_REAX); //Apoyo
                }
                else
                {
                    v.volumen_x = Convert.ToBoolean(cs.VOLUMEN_ESTX); //Volumen
                    v.apoyototal_x = Convert.ToBoolean(cs.APOYO_ESTX); //Apoyo
                }


                /////////////////////////////////////////////DATOS PARA LA TABLA 1 MATERIALES EN LA VISTA///////////////////////////////////////
                //B20180720P MGC 2018.07.23
                //var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(id)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();
                var con = db.CARTAPs.Select(x => new { x.NUM_DOC, x.POS_ID, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(id) & a.POS_ID.Equals(pos)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();

                //B20180710 MGC 2018.07.12 Modificación 9 y 10 dependiendo del campo de factura en tsol............
                //bool fact = false;
                //try
                //{
                //    fact = db.TSOLs.Where(ts => ts.ID == d.TSOL_ID).FirstOrDefault().FACTURA;
                //}
                //catch (Exception)
                //{

                //}

                foreach (var item in con)
                {
                    encabezadoFech.Add(item.Key.VIGENCIA_DE.ToString() + item.Key.VIGENCIA_AL.ToString());
                }

                for (int i = 0; i < encabezadoFech.Count; i++)
                {
                    contadorTabla = 0;

                    DateTime a1 = DateTime.Parse(encabezadoFech[i].Remove(encabezadoFech[i].Length / 2));
                    DateTime a2 = DateTime.Parse(encabezadoFech[i].Remove(0, encabezadoFech[i].Length / 2));

                    var con2 = db.CARTAPs
                                          .Where(x => x.NUM_DOC.Equals(id) & x.POS_ID.Equals(pos) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                          .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new
                                          {
                                              x.NUM_DOC,
                                              x.MATNR,
                                              x.MATKL,
                                              y.MAKTX,
                                              x.MONTO,
                                              y.PUNIT,
                                              x.PORC_APOYO,
                                              x.MONTO_APOYO,
                                              resta = (x.MONTO - x.MONTO_APOYO),
                                              x.PRECIO_SUG,
                                              x.APOYO_EST,
                                              x.APOYO_REAL
                                          ,
                                              x.VOLUMEN_EST,
                                              x.VOLUMEN_REAL
                                          }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL
                                          .ToList();

                    if (con2.Count > 0)
                    {
                        foreach (var item2 in con2)
                        {

                            armadoCuerpoTab.Add(item2.MATNR.TrimStart('0'));
                            armadoCuerpoTab.Add(item2.MATKL);
                            armadoCuerpoTab.Add(item2.MAKTX);

                            if (v.costoun_x == true)
                            {
                                //armadoCuerpoTab.Add(Math.Round(item2.MONTO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            if (v.apoyo_x == true)
                            {
                                //armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            if (v.apoyop_x == true)
                            {
                                //armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }

                            if (v.costoap_x == true)
                            {
                                //armadoCuerpoTab.Add(Math.Round((item2.MONTO - item2.MONTO_APOYO), 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round((item2.MONTO - item2.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            if (v.precio_x == true)
                            {
                                //armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }

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
                        //B20180720P MGC 2018.07.23

                        var con3 = db.CARTAPs
                                            .Where(x => x.NUM_DOC.Equals(id) & x.POS_ID.Equals(pos) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                            .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new
                                            {
                                                x.NUM_DOC,
                                                x.MATNR,
                                                x.MATKL,
                                                y.ID,
                                                x.MONTO,
                                                x.PORC_APOYO,
                                                y.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(d.CLIENTE.SPRAS)).FirstOrDefault().TXT50,
                                                x.MONTO_APOYO,
                                                resta = (x.MONTO - x.MONTO_APOYO),
                                                x.PRECIO_SUG,
                                                x.APOYO_EST,
                                                x.APOYO_REAL
                                            ,
                                                x.VOLUMEN_EST,
                                                x.VOLUMEN_REAL
                                            }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL})
                                            .ToList();

                        foreach (var item2 in con3)
                        {

                            armadoCuerpoTab.Add("");
                            armadoCuerpoTab.Add(item2.MATKL);
                            armadoCuerpoTab.Add(item2.TXT50);

                            if (v.costoun_x == true)
                            {
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            if (v.apoyo_x == true)
                            {
                                armadoCuerpoTab.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            if (v.apoyop_x == true)
                            {
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }

                            if (v.costoap_x == true)
                            {
                                armadoCuerpoTab.Add(format.toShow(Math.Round((item2.MONTO - item2.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            if (v.precio_x == true)
                            {
                                armadoCuerpoTab.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }

                            //Volumen
                            //B20180726 MGC 2018.07.26
                            if (v.volumen_x == true)
                            {
                                if (fact)
                                {
                                    armadoCuerpoTab.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    //carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                }
                                else
                                {
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
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                    armadoCuerpoTab.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                            }

                            contadorTabla++;
                        }
                    }
                    numfilasTabla.Add(contadorTabla);
                }

                var cabeza = new List<string>();
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "materialC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "categoriaC").Select(x => x.TEXTO).FirstOrDefault());
                cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "descripcionC").Select(x => x.TEXTO).FirstOrDefault());
                if (v.costoun_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costouC").Select(x => x.TEXTO).FirstOrDefault()); }
                if (v.apoyo_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopoC").Select(x => x.TEXTO).FirstOrDefault()); }
                if (v.apoyop_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "apoyopiC").Select(x => x.TEXTO).FirstOrDefault()); }
                if (v.costoap_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "costoaC").Select(x => x.TEXTO).FirstOrDefault()); }
                if (v.precio_x == true) { cabeza.Add(db.TEXTOCVs.Where(x => x.SPRAS_ID == user.SPRAS_ID & x.CAMPO == "preciosC").Select(x => x.TEXTO).FirstOrDefault()); }
                //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                //fact = true es real
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

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                v.listaFechas = encabezadoFech;//////////////RANGO DE FECHAS QUE DETERMINAN EL NUMERO DE TABLAS
                v.numfilasTabla = numfilasTabla;////NUMERO FILAS POR TABLA CALCULADA
                v.listaCuerpo = armadoCuerpoTab;////NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE QUE POSTERIORMENTE ES DISTRIBUIDA EN LAS TABLAS //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                v.numColEncabezado = cabeza;////////NUMERO DE COLUMNAS PARA LAS TABLAS

                //B20180720P MGC 2018.07.23
                //v.secondTab_x = true;
                //v.costoun_x = true;
                //v.apoyo_x = true;
                //v.apoyop_x = true;
                //v.costoap_x = true;
                //v.precio_x = true;
                //v.apoyoEst_x = true; //Volumen
                //v.apoyoRea_x = true; //Apoyo
                /////////////////////////////////

                //TABLA 2 RECURRENCIAS
                v.numColEncabezado2 = cabeza2;////////NUMERO DE COLUMNAS PARA LAS TABLAS
                v.numfilasTabla2 = con4.Count();//////NUMERO FILAS TOTAL PARA LA TABLA
                v.listaCuerpoRec = armadoCuerpoTab2;//NUMERO TOTAL DE FILAS CON LA INFO CORRESPONDIENTE
                                                    ///////////////////////////////


                //MARCA DE AGUA
                bool aprob = false;
                aprob = (d.ESTATUS_WF.Equals("A") | d.ESTATUS_WF.Equals("S"));

                //PARA LA TABLA 1 MATERIALES
                v.numColEncabezado = cabeza;
                v.listaFechas = encabezadoFech;
                v.numfilasTabla = numfilasTabla;
                v.listaCuerpo = armadoCuerpoTab;
                //PARA LA TABLA 2 RECURRENCIAS
                v.numColEncabezado2 = cabeza2;
                v.numfilasTabla2 = con4.Count();
                v.listaCuerpoRec = armadoCuerpoTab2;

                //B20180801 MGC Formato
                //v.monto = monto_enviar;
                decimal montod = 0;
                try
                {
                    montod = Convert.ToDecimal(v.monto);
                }
                catch (Exception)
                {

                }

                v.monto = format.toShow(montod, decimales);

                CartaV carta = v;
                CartaVEsqueleto cve = new CartaVEsqueleto();
                cve.crearPDF(carta, user.SPRAS_ID, aprob);
                string recibeRuta = Convert.ToString(Session["rutaCompletaV"]);
                return RedirectToAction("Index", new { ruta = recibeRuta, ids = v.num_doc });

            }
        }
    }
}
