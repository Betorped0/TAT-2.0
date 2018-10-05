using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Xml;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;
using TAT001.Controllers;
using ClosedXML.Excel;
using ExcelDataReader;
using TAT001.Models.Masiva;

namespace TAT001.Controllers
{
    public class MasivaController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Masiva
        public ActionResult Index()
        {
            int pagina = 221; //ID EN BASE DE DATOS
            string u = User.Identity.Name;
            var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            //ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
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
            Session["sociedadUser"] = user.BUNIT;

            return View(db.DOCUMENTOes.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string a)
        {
            return (null);
        }

        public ActionResult Archivo(string varArch)//METODO PARA DESCARGAR EL ARCHIVO DEL SERVER
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            if (varArch == "X")
            {
                Response.AddHeader("Content-Disposition", "attachment; filename = LAYOUT_CARGA_MASIVA.xlsx");
                Response.TransmitFile(Server.MapPath("~/files/masiva.xlsx"));
            }
            else if (varArch == "L")
            {
                Models.CargaMasivaModels carga = new Models.CargaMasivaModels();
                carga.GenerarListaCliPro(Server.MapPath("~/pdfTemp/"));
                Response.AddHeader("Content-Disposition", "attachment; filename = LISTA_CLIENTE_PROVEEDORES.xlsx");
                Response.TransmitFile(Server.MapPath("~/pdfTemp/ListaClienteProveedores.xlsx"));
            }
            Response.End();

            return RedirectToAction("Index");
        }

        public string validaFech(string date, string tipo)
        {
            Calendario445 cale = new Calendario445();
            DateTime fecha;
            int anio, mes;

            if (date.Length == 7)
            {
                mes = Convert.ToInt32(date.Substring(0, 2));
                anio = Convert.ToInt32(date.Substring(3, 4));

                if (tipo != "")
                {
                    fecha = cale.getPrimerDia(anio, mes);
                }
                else
                {
                    fecha = cale.getUltimoDia(anio, mes);
                }
            }
            else
            {
                fecha = Convert.ToDateTime(date);
            }

            return Convert.ToString(fecha);
        }

        //COSULTA DE AJAX PARA LA CLASIFICACION GALL_ID
        public JsonResult clasificacion(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from t in db.TALLs
                     where t.ID.Contains(Prefix) && t.ACTIVO == true
                     select new { t.ID, t.DESCRIPCION, t.GALL_ID }).ToList();

            if (c.Count == 0)
            {
                var c2 = (from t in db.TALLs
                          where t.DESCRIPCION.Contains(Prefix) && t.ACTIVO == true
                          select new { t.ID, t.DESCRIPCION, t.GALL_ID }).ToList();
                c.AddRange(c2);
            }
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        //COSULTA DE AJAX PARA EL TIPO DE MONEDA 
        public JsonResult moneda(string Prefix)
        {
            string sociedadUser = Session["sociedadUser"].ToString();

            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from m in db.MONEDAs
                     join so in db.SOCIEDADs on m.WAERS equals so.WAERS
                     where (so.BUKRS == sociedadUser && m.KTEXT.Contains(Prefix) && m.ACTIVO == true) || (m.WAERS == "USD" && m.ACTIVO == true)
                     group m by m.WAERS into g
                     select new { WAERS = g.Key }).ToList();

            if (c.Count <= 1)
            {
                 c = (from m in db.MONEDAs
                         join so in db.SOCIEDADs on m.WAERS equals so.WAERS
                          where (so.BUKRS == sociedadUser && m.WAERS.Contains(Prefix) && m.ACTIVO == true) || (m.WAERS == "USD" && m.ACTIVO == true)
                          group m by m.WAERS into g
                          select new { WAERS = g.Key }).ToList();
                //c.AddRange(c2);
            }
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        //COSULTA DE AJAX PARA EL TIPO DE MONEDA 
        public JsonResult pais(string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from p in db.PAIS
                     where p.LANDX.Contains(Prefix) && p.ACTIVO == true
                     select new { p.LANDX }).ToList();

            if (c.Count == 0)
            {
                var c2 = (from p in db.PAIS
                         where p.LAND.Contains(Prefix) && p.ACTIVO == true
                         select new { p.LANDX }).ToList();
                c.AddRange(c2);
            }
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }










        public ActionResult loadExcelMasiva()
        {

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];

                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        string fileExtension = System.IO.Path.GetExtension(file.FileName);
                        string extension = System.IO.Path.GetExtension(file.FileName);
                        IExcelDataReader reader = ExcelReaderFactory.CreateReader(file.InputStream);
                        DataSet result = reader.AsDataSet();

                        //////CABECERA
                        DataSet dt = new DataSet();
                        DataTable dtt = result.Tables[1].Copy();
                        dt.Tables.Add(dtt);

                        /////DOCUMENTOS RELACIONADOS
                        DataSet dt2 = new DataSet();
                        DataTable dtt2 = result.Tables[2].Copy();
                        dt2.Tables.Add(dtt2);

                        /////DOCUMENTOS RELACIONADOS MULTIPLES
                        DataSet dt3 = new DataSet();
                        DataTable dtt3 = result.Tables[3].Copy();
                        dt3.Tables.Add(dtt3);

                        /////MATERIALES
                        DataSet dt4 = new DataSet();
                        DataTable dtt4 = result.Tables[4].Copy();
                        dt4.Tables.Add(dtt4);

                        Session["ds1"] = dt;//////CABECERA
                        Session["ds2"] = dt2;/////DOCUMENTOS RELACIONADOS
                        Session["ds3"] = dt3;/////DOCUMENTOS RELACIONADOS MULTIPLES
                        Session["ds4"] = dt4;/////MATERIALES
                        reader.Close();
                    }
                }
            }
            return (null);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult validaHoja1()
        {
            DataSet ds1 = (DataSet)Session["ds1"];
            List<object> lp = new List<object>();

            for (int i = 1; i < ds1.Tables[0].Rows.Count; i++)
            {
                Encabezado doc = new Encabezado();

                string num_doc = ds1.Tables[0].Rows[i][0].ToString().Trim();
                string t_sol = ds1.Tables[0].Rows[i][1].ToString().Trim();
                string gall_id = ds1.Tables[0].Rows[i][2].ToString().Trim();
                //gall_id = db.TALLs.Where(x => x.DESCRIPCION == gall_id & x.ACTIVO == true).Select(x => x.GALL_ID).FirstOrDefault();
                string bukrs = ds1.Tables[0].Rows[i][3].ToString().Trim();
                string land = ds1.Tables[0].Rows[i][4].ToString().Trim();
                //land = db.PAIS.Where(x => x.LANDX == land).Select(x => x.LAND).FirstOrDefault();
                string estado = ds1.Tables[0].Rows[i][5].ToString().Trim();
                string ciudad = ds1.Tables[0].Rows[i][6].ToString().Trim();
                string concepto = ds1.Tables[0].Rows[i][7].ToString().Trim();
                string notas = ds1.Tables[0].Rows[i][8].ToString().Trim();
                string num_cliente = ds1.Tables[0].Rows[i][9].ToString().Trim();
                string vkorg = db.CLIENTEs.Where(x => x.KUNNR == num_cliente).Select(x => x.VKORG).First();
                string vtweg = db.CLIENTEs.Where(x => x.KUNNR == num_cliente).Select(x => x.VTWEG).First();
                string payer_nombre = ds1.Tables[0].Rows[i][10].ToString().Trim();
                string payer_email = ds1.Tables[0].Rows[i][11].ToString().Trim();
                string fechai_vig = ds1.Tables[0].Rows[i][12].ToString().Trim();
                fechai_vig = validaFech(fechai_vig, "x");
                string fechaf_vig = ds1.Tables[0].Rows[i][13].ToString().Trim();
                fechaf_vig = validaFech(fechaf_vig, "");
                string moneda_id = ds1.Tables[0].Rows[i][14].ToString().Trim();

                doc.NUM_DOC = num_doc;
                doc.TSOL_ID = t_sol;
                doc.GALL_ID = gall_id;
                doc.SOCIEDAD_ID = bukrs;
                doc.PAIS_ID = land;
                doc.ESTADO = estado;
                doc.CIUDAD = ciudad;
                doc.CONCEPTO = concepto;
                doc.NOTAS = notas;
                doc.PAYER_ID = num_cliente;
                doc.VKORG = vkorg;
                doc.VTWEG = vtweg;
                doc.PAYER_NOMBRE = payer_nombre;
                doc.PAYER_EMAIL = payer_email;
                doc.FECHAI_VIG = fechai_vig;
                doc.FECHAF_VIG = fechaf_vig;
                doc.MONEDA_ID = moneda_id;

                lp.Add(doc);
            }

            lp.Add(db.TALLs.Where(x => x.ACTIVO == true).Select(x => x.DESCRIPCION).ToList());
            lp.Add(db.MONEDAs.Select(x => x.WAERS).ToList());

            JsonResult jl = Json(lp, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult validaHoja2()
        {
            DataSet ds2 = (DataSet)Session["ds2"];
            List<object> lp = new List<object>();

            for (int i = 1; i < ds2.Tables[0].Rows.Count; i++)
            {
                Relacionada doc = new Relacionada();

                string num_doc = ds2.Tables[0].Rows[i][0].ToString().Trim();
                string factura = ds2.Tables[0].Rows[i][1].ToString().Trim();
                string fecha = ds2.Tables[0].Rows[i][2].ToString().Trim();
                string proveedor = ds2.Tables[0].Rows[i][3].ToString().Trim();
                string proveedor_nombre = ds2.Tables[0].Rows[i][4].ToString().Trim();
                string autorizacion = ds2.Tables[0].Rows[i][5].ToString().Trim();
                string vencimiento = ds2.Tables[0].Rows[i][6].ToString().Trim();
                string facturak = ds2.Tables[0].Rows[i][7].ToString().Trim();
                string ejerciciok = ds2.Tables[0].Rows[i][8].ToString().Trim();

                doc.NUM_DOC = num_doc;
                doc.FACTURA = factura;
                doc.FECHA = fecha;
                doc.PROVEEDOR = proveedor;
                doc.PROVEEDOR_NOMBRE = proveedor_nombre;
                doc.AUTORIZACION = autorizacion;
                doc.VENCIMIENTO = vencimiento;
                doc.FACTURAK = facturak;
                doc.EJERCICIOK = ejerciciok;

                lp.Add(doc);
            }

            lp.Add(db.PROVEEDORs.Select(x => x.ID).ToList());

            JsonResult jl = Json(lp, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult validaHoja3()
        {
            DataSet ds3 = (DataSet)Session["ds3"];
            List<Multiple> lm = new List<Multiple>();

            for (int i = 1; i < ds3.Tables[0].Rows.Count; i++)
            {
                Multiple doc = new Multiple();

                string num_doc = ds3.Tables[0].Rows[i][0].ToString().Trim();
                string factura = ds3.Tables[0].Rows[i][1].ToString().Trim();
                string bill_doc = ds3.Tables[0].Rows[i][2].ToString().Trim();
                string ejerciciok = ds3.Tables[0].Rows[i][3].ToString().Trim();
                string payer = ds3.Tables[0].Rows[i][4].ToString().Trim();
                string importe_fac = ds3.Tables[0].Rows[i][5].ToString().Trim();
                string belnr = ds3.Tables[0].Rows[i][6].ToString().Trim();

                doc.NUM_DOC = num_doc;
                doc.FACTURA = factura;
                doc.BILL_DOC = bill_doc;
                doc.EJERCICIOK = ejerciciok;
                doc.PAYER = payer;
                doc.IMPORTE_FAC = importe_fac;
                doc.BELNR = belnr;

                lm.Add(doc);
            }

            JsonResult jl = Json(lm, JsonRequestBehavior.AllowGet);
            return jl;
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult validaHoja2()
        //{
        //    DataSet ds2 = (DataSet)Session["ds2"];
        //    List<Distribucion> ld = new List<Distribucion>();

        //    for (int i = 1; i < ds2.Tables[0].Rows.Count; i++)
        //    {
        //        Distribucion doc = new Distribucion();

        //        string num_doc = ds2.Tables[0].Rows[i][0].ToString().Trim();
        //        string vigencia_de = ds2.Tables[0].Rows[i][1].ToString().Trim();
        //        string vigencia_al = ds2.Tables[0].Rows[i][2].ToString().Trim();
        //        string matnr = ds2.Tables[0].Rows[i][3].ToString().Trim();
        //        string matkl = ds2.Tables[0].Rows[i][4].ToString().Trim();
        //        string monto = ds2.Tables[0].Rows[i][5].ToString().Trim();
        //        string porc_apoyo = ds2.Tables[0].Rows[i][6].ToString().Trim();
        //        string precio_sug = ds2.Tables[0].Rows[i][7].ToString().Trim();
        //        string volumen_real = ds2.Tables[0].Rows[i][8].ToString().Trim();
        //        string apoyo = ds2.Tables[0].Rows[i][9].ToString().Trim();

        //        doc.NUM_DOC = num_doc;
        //        doc.VIGENCIA_DE = vigencia_de;
        //        doc.VIGENCIA_AL = vigencia_al;
        //        doc.MATNR = matnr;
        //        doc.MATKL = matkl;
        //        doc.MONTO = monto;
        //        doc.PORC_APOYO = porc_apoyo;
        //        doc.PRECIO_SUG = precio_sug;
        //        doc.VOLUMEN_REAL = volumen_real;
        //        doc.APOYO = apoyo;

        //        ld.Add(doc);
        //    }

        //    JsonResult jl = Json(ld, JsonRequestBehavior.AllowGet);
        //    return jl;
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public JsonResult validaHoja4()
        //{
        //    DataSet ds4 = (DataSet)Session["ds1"];
        //    List<object> la = new List<object>();
        //    List<string> textos = new List<string>();

        //    for (int i = 1; i < ds4.Tables[0].Rows.Count; i++)
        //    {
        //        string num_doc = ds4.Tables[0].Rows[i][0].ToString().Trim();
        //        la.Add(num_doc);
        //    }

        //    textos = db.TSOPORTEs.Where(x => x.ACTIVO == true).Select(x => x.DESCRIPCION).ToList();
        //    la.Add(textos);

        //    JsonResult jl = Json(la, JsonRequestBehavior.AllowGet);
        //    return jl;
        //}
    }
}


//public ActionResult validaHoja2()
//{
//    DataSet ds2 = (DataSet)Session["ds2"];

//    var docu = db.DOCUMENTOes.ToList();
//    return PartialView("_Hoja2", docu);
//}

//public MATERIAL material(string material)
//{
//    if (material == null)
//        material = "";
//    //RSG 07.06.2018---------------------------------------------
//    material = new Cadena().completaMaterial(material);
//    //RSG 07.06.2018---------------------------------------------

//    TAT001Entities db = new TAT001Entities();

//    MATERIAL mat = db.MATERIALs.Where(m => m.ID == material).FirstOrDefault();

//    return mat;
//}
//[HttpPost]
////public CATEGORIAT getCategoriaS(string material)
//public MATERIALGPT getCategoriaS(string material)
//{
//    if (material == null)
//        material = "";
//    material = new Cadena().completaMaterial(material);
//    TAT001Entities db = new TAT001Entities();

//    MATERIAL m = db.MATERIALs.Where(mat => mat.ID.Equals(material)).FirstOrDefault();
//    //CATEGORIAT cat = new CATEGORIAT();
//    MATERIALGPT cat = new MATERIALGPT();//RSG 01.08.2018

//    //if (m != null && m.MATKL_ID != "")
//    if (m != null && m.MATERIALGP_ID != "")//RSG 01.08.2018
//    {
//        string u = User.Identity.Name;
//        var user = db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();

//        //cat = db.CATEGORIAs.Where(c => c.ID == m.MATKL_ID && c.ACTIVO == true)
//        cat = db.MATERIALGPs.Where(c => c.ID == m.MATERIALGP_ID && c.ACTIVO == true)//RSG 01.08.2018
//                    .Join(
//                    db.MATERIALGPTs.Where(ct => ct.SPRAS_ID == user.SPRAS_ID),
//                    c => c.ID,
//                    //ct => ct.CATEGORIA_ID,
//                    ct => ct.MATERIALGP_ID,//RSG 01.08.2018
//                    (c, ct) => ct)
//                .FirstOrDefault();
//    }
//    return cat;
//}