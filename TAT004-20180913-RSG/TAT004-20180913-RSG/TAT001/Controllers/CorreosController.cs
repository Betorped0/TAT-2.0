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

namespace TAT001.Controllers
{
    [AllowAnonymous]
    public class CorreosController : Controller
    {
        private TAT001Entities db = new TAT001Entities();

        // GET: Correos
        public ActionResult Index(decimal id, bool? mail) //B20180803 MGC Correos
        {
            var dOCUMENTO = db.DOCUMENTOes.Where(x => x.NUM_DOC == id).FirstOrDefault();
            var flujo = db.FLUJOes.Where(x => x.NUM_DOC == id).OrderByDescending(o => o.POS).Select(s => s.POS).ToList();
            ViewBag.Pos = flujo[0];
            ViewBag.url = "http://localhost:64497";
            ViewBag.url = "http://192.168.1.77";
            ViewBag.url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            //ViewBag.miles = dOCUMENTOes.PAI.MILES;//LEJGG 090718
            //ViewBag.dec = dOCUMENTOes.PAI.DECIMAL;//LEJGG 090718
            FormatosC fc = new FormatosC();
            ViewBag.monto = fc.toShow((decimal)dOCUMENTO.MONTO_DOC_MD, dOCUMENTO.PAI.DECIMAL) + " " + dOCUMENTO.MONEDA_ID;
            if (mail == null)
                mail = true;
            //B20180803 MGC Correos............
            string mailv = "";
            if (mail != null)
            {
                if (mail == true)
                {
                    mailv = "X";
                }
            }

            ViewBag.mail = mailv;
            //B20180803 MGC Correos............

            //B20180803 MGC Presupuesto............
            Models.PresupuestoModels carga = new Models.PresupuestoModels();
            ViewBag.ultMod = carga.consultarUCarga();

            dOCUMENTO.PAI = db.PAIS.Where(a => a.LAND.Equals(dOCUMENTO.PAIS_ID)).FirstOrDefault();
            if (dOCUMENTO.PAI != null)
            {
                ViewBag.miles = dOCUMENTO.PAI.MILES;//LEJGG 090718
                ViewBag.dec = dOCUMENTO.PAI.DECIMAL;//LEJGG 090718
            }

            CLIENTE_MOD cli = new CLIENTE_MOD();

            cli = SelectCliente(dOCUMENTO.PAYER_ID);

            ViewBag.kunnr = cli.KUNNR + "-" + cli.NAME1;
            ViewBag.vtweg = cli.VTWEG;

            Services.FormatosC format = new FormatosC();

            PRESUPUESTO_MOD presu = new PRESUPUESTO_MOD();
            presu = getPresupuesto(dOCUMENTO.PAYER_ID);

            decimal pcanal = 0;
            try
            {
                pcanal = Convert.ToDecimal(presu.P_CANAL) / 1;
            }
            catch (Exception)
            {

            }
            decimal pbanner = 0;
            try
            {
                pbanner = Convert.ToDecimal(presu.P_BANNER) / 1;
            }
            catch (Exception)
            {

            }
            decimal pcc = 0;
            try
            {
                pcc = Convert.ToDecimal(presu.PC_C) / 1 * -1;
            }
            catch (Exception)
            {

            }
            decimal pca = 0;
            try
            {
                pca = Convert.ToDecimal(presu.PC_A) / 1 * -1;
            }
            catch (Exception)
            {

            }
            decimal pcp = 0;
            try
            {
                pcp = Convert.ToDecimal(presu.PC_P) / 1 * -1;
            }
            catch (Exception)
            {

            }
            decimal pct = 0;
            try
            {
                pct = Convert.ToDecimal(presu.PC_T) / 1 * -1;
            }
            catch (Exception)
            {

            }
            decimal consu = 0;
            try
            {
                consu = Convert.ToDecimal(presu.CONSU) / 1;
            }
            catch (Exception)
            {

            }
            ViewBag.pcan = format.toShowG(pcanal, dOCUMENTO.PAI.DECIMAL);
            ViewBag.pban = format.toShowG(pbanner, dOCUMENTO.PAI.DECIMAL);
            ViewBag.pcc = format.toShowG(pcc, dOCUMENTO.PAI.DECIMAL);
            ViewBag.pca = format.toShowG(pca, dOCUMENTO.PAI.DECIMAL);
            ViewBag.pcp = format.toShowG(pcp, dOCUMENTO.PAI.DECIMAL);
            ViewBag.pct = format.toShowG(pct, dOCUMENTO.PAI.DECIMAL);
            ViewBag.consu = format.toShowG(consu, dOCUMENTO.PAI.DECIMAL);

            SOLICITUD_MOD sol = new SOLICITUD_MOD();
            if (dOCUMENTO.DOCUMENTO_REF == null)
                sol = getSolicitud("0.00", dOCUMENTO.MONTO_DOC_MD + "", dOCUMENTO.PAI.DECIMAL);
            else
                sol = getSolicitud(dOCUMENTO.DOCUMENTO_REF+"", dOCUMENTO.MONTO_DOC_MD + "", dOCUMENTO.PAI.DECIMAL);

            ViewBag.S_IMPA = sol.S_IMPA;
            ViewBag.S_IMPB = sol.S_IMPB;
            ViewBag.S_IMPC = sol.S_IMPC;
            ViewBag.S_MONTOA = sol.S_MONTOA;
            ViewBag.S_MONTOB = sol.S_MONTOB;
            ViewBag.S_MONTOP = sol.S_MONTOP;
            ViewBag.S_NUM = sol.S_NUM;
            ViewBag.S_REMA = sol.S_REMA;
            ViewBag.rema_color = "";
            if(format.toNum(sol.S_REMA, dOCUMENTO.PAI.MILES, dOCUMENTO.PAI.DECIMAL) <0)
                ViewBag.rema_color = "#F44336 !important";

            ViewBag.S_RET = sol.S_RET;
            ViewBag.S_TOTAL = sol.S_TOTAL;

            //B20180803 MGC Presupuesto............

            return View(dOCUMENTO);
        }

        // GET: Correos/Details/5
        public ActionResult Details(decimal id, bool? mail)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dOCUMENTO = db.DOCUMENTOes.Where(x => x.NUM_DOC == id).FirstOrDefault();
            ViewBag.workflow = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id)).OrderBy(a => a.POS).ToList();
            ViewBag.acciones = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id) & a.ESTATUS.Equals("P") & a.USUARIOA_ID.Equals(User.Identity.Name)).FirstOrDefault();
            ViewBag.url = "http://localhost:64497";
            ViewBag.url = "http://192.168.1.77";
            ViewBag.url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            if (dOCUMENTO == null)
            {
                return HttpNotFound();
            }
            //ViewBag.miles = dOCUMENTO.PAI.MILES;//LEJGG 090718
            //ViewBag.dec = dOCUMENTO.PAI.DECIMAL;//LEJGG 090718
            FormatosC fc = new FormatosC();
            //ViewBag.monto = fc.toShow((decimal)dOCUMENTO.MONTO_DOC_MD, dOCUMENTO.PAI.DECIMAL);
            ViewBag.monto = fc.toShow((decimal)dOCUMENTO.MONTO_DOC_MD, dOCUMENTO.PAI.DECIMAL) + " " + dOCUMENTO.MONEDA_ID;
            if (mail == null)
                mail = true;
            //B20180803 MGC Correos............
            string mailv = "";
            if (mail != null)
            {
                if (mail == true)
                {
                    mailv = "X";
                }
            }

            ViewBag.mail = mailv;
            //B20180803 MGC Correos............
            return View(dOCUMENTO);
        }

        // GET: Correos
        public ActionResult Recurrente(decimal id, bool? mail)
        {
            var dOCUMENTO = db.DOCUMENTOes.Where(x => x.NUM_DOC == id).FirstOrDefault();
            var flujo = db.FLUJOes.Where(x => x.NUM_DOC == id).OrderByDescending(o => o.POS).Select(s => s.POS).ToList();
            ViewBag.Pos = flujo[0];
            ViewBag.url = "http://localhost:64497";
            ViewBag.url = "http://192.168.1.77";
            ViewBag.url = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");

            DOCUMENTOL dl = dOCUMENTO.DOCUMENTOLs.OrderByDescending(x => x.POS).FirstOrDefault();
            FormatosC fc = new FormatosC();
            ViewBag.monto = fc.toShow((decimal)dOCUMENTO.MONTO_DOC_MD, dOCUMENTO.PAI.DECIMAL);
            ViewBag.mes = dl.FECHAF.Value.Month;
            ViewBag.venta = fc.toShow((decimal)dl.MONTO_VENTA, dOCUMENTO.PAI.DECIMAL);
            DOCUMENTOREC dr = db.DOCUMENTORECs.Where(x => x.DOC_REF == dOCUMENTO.NUM_DOC).FirstOrDefault();
            ViewBag.objetivo = fc.toShow((decimal)dr.MONTO_BASE, dOCUMENTO.PAI.DECIMAL);
            ViewBag.porc = fc.toShowPorc((decimal)dr.PORC, dOCUMENTO.PAI.DECIMAL);
            if (dl.MONTO_VENTA < dr.MONTO_BASE)
            {
                ViewBag.tsol = dOCUMENTO.TSOL.TSOLR;
                ViewBag.nota = false;
            }
            else
            {
                ViewBag.tsol = "";
                ViewBag.nota = true;
            }
            if (mail == null)
                mail = true;
            //B20180803 MGC Correos............
            string mailv = "";
            if (mail != null)
            {
                if (mail == true)
                {
                    mailv = "X";
                }
            }

            ViewBag.mail = mailv;
            //B20180803 MGC Correos............
            return View(dOCUMENTO);
        }

        // GET: Correos
        public ActionResult Backorder(decimal id, bool? mail)
        {
            var dOCUMENTO = db.DOCUMENTOes.Where(x => x.NUM_DOC == id).FirstOrDefault();
            var flujo = db.FLUJOes.Where(x => x.NUM_DOC == id).OrderByDescending(o => o.POS).Select(s => s.POS).ToList();
            ViewBag.Pos = flujo[0];
            ViewBag.url = "http://localhost:64497";
            ViewBag.url = "http://192.168.1.77";
            ViewBag.url = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, "");

            DOCUMENTOL dl = dOCUMENTO.DOCUMENTOLs.OrderByDescending(x => x.POS).FirstOrDefault();
            FormatosC fc = new FormatosC();
            ViewBag.monto = fc.toShow((decimal)dOCUMENTO.MONTO_DOC_MD, dOCUMENTO.PAI.DECIMAL);
            ViewBag.mes = dl.FECHAF.Value.Month;
            ViewBag.venta = fc.toShow((decimal)dl.MONTO_VENTA, dOCUMENTO.PAI.DECIMAL);
            DOCUMENTOREC dr = db.DOCUMENTORECs.Where(x => x.DOC_REF == dOCUMENTO.NUM_DOC).FirstOrDefault();
            ViewBag.objetivo = fc.toShow((decimal)dr.MONTO_BASE, dOCUMENTO.PAI.DECIMAL);
            ViewBag.porc = fc.toShowPorc((decimal)dr.PORC, dOCUMENTO.PAI.DECIMAL);
            if (dl.MONTO_VENTA < dr.MONTO_BASE)
            {
                ViewBag.tsol = dOCUMENTO.TSOL.TSOLR;
                ViewBag.nota = false;
            }
            else
            {
                ViewBag.tsol = "";
                ViewBag.nota = true;
            }
            if (mail == null)
                mail = true;
            //B20180803 MGC Correos............
            string mailv = "";
            if (mail != null)
            {
                if (mail == true)
                {
                    mailv = "X";
                }
            }

            ViewBag.mail = mailv;
            //B20180803 MGC Correos............
            return View(dOCUMENTO);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public CLIENTE_MOD SelectCliente(string kunnr)
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
                                     VTWEG2 = c.VTWEG,//RSG 05.07.2018
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
                             VTWEG2 = c.VTWEG,//RSG 05.07.2018
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
                    //id_cl.VTWEG = canal.CANAL1 + " - " + canal.CDESCRIPCION;
                    id_cl.VTWEG = canal.CDESCRIPCION;
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

            return id_cl;
        }

        public PRESUPUESTO_MOD getPresupuesto(string kunnr)
        {
            PRESUPUESTO_MOD pm = new PRESUPUESTO_MOD();
            Presupuesto pr = new Presupuesto();
            pm = pr.getPresupuesto(kunnr);
            //try
            //{
            //    if (kunnr == null)
            //        kunnr = "";

            //    //Obtener presupuesto
            //    string mes = DateTime.Now.Month.ToString();
            //    var presupuesto = db.CSP_PRESU_CLIENT(cLIENTE: kunnr, pERIODO: mes).Select(p => new { DESC = p.DESCRIPCION.ToString(), VAL = p.VALOR.ToString() }).ToList();
            //    string clien = db.CLIENTEs.Where(x => x.KUNNR == kunnr).Select(x => x.BANNERG).First();
            //    if (presupuesto != null)
            //    {
            //        if (String.IsNullOrEmpty(clien))
            //        {
            //            pm.P_CANAL = presupuesto[0].VAL;
            //            pm.P_BANNER = presupuesto[1].VAL;
            //            pm.PC_C = (float.Parse(presupuesto[4].VAL) + float.Parse(presupuesto[5].VAL) + float.Parse(presupuesto[6].VAL)).ToString();
            //            pm.PC_A = presupuesto[8].VAL;
            //            pm.PC_P = presupuesto[9].VAL;
            //            pm.PC_T = presupuesto[10].VAL;
            //            pm.CONSU = (float.Parse(presupuesto[1].VAL) - float.Parse(presupuesto[10].VAL)).ToString();
            //        }
            //        else
            //        {
            //            pm.P_CANAL = presupuesto[0].VAL;
            //            pm.P_BANNER = presupuesto[0].VAL;
            //            pm.PC_C = (float.Parse(presupuesto[4].VAL) + float.Parse(presupuesto[5].VAL) + float.Parse(presupuesto[6].VAL)).ToString();
            //            pm.PC_A = presupuesto[8].VAL;
            //            pm.PC_P = presupuesto[9].VAL;
            //            pm.PC_T = presupuesto[10].VAL;
            //            pm.CONSU = (float.Parse(presupuesto[0].VAL) - float.Parse(presupuesto[10].VAL)).ToString();
            //        }
            //    }
            //}
            //catch (Exception e)
            //{

            //}

            return pm;
        }

        public SOLICITUD_MOD getSolicitud(string num, string num2,  string d)//RSG 07.06.2018---------------------------------------------
        {
            TAT001.Models.SOLICITUD_MOD sm = new SOLICITUD_MOD();
            Services.FormatosC format = new FormatosC();

            //Obtener info solicitud
            if (num == null | num == "" | num == "0.00")
            {
                sm.S_NUM = num = "";
                sm.S_MONTOB = format.toShow(Convert.ToDecimal(num2), d);
                sm.S_MONTOP = sm.S_MONTOB;
                sm.S_MONTOA = "-";
                sm.S_REMA = "-";
                sm.S_IMPA = "-";
                sm.S_IMPB = "-";
                sm.S_IMPC = "-";
                sm.S_RET = "-";
                sm.S_TOTAL = format.toShow(Convert.ToDecimal(num2), d); ;
            }
            else
            {
                decimal hola = Convert.ToDecimal(num);
                var rev = db.DOCUMENTOes.Where(x => x.DOCUMENTO_REF == hola).ToList();
                ;

                if (rev.Count() == 0)
                {
                    //CON UN RELACIONADO 
                    var rev2 = db.DOCUMENTOes.Where(x => x.NUM_DOC == hola).FirstOrDefault();
                    decimal? rem2 = (rev2.MONTO_DOC_MD - Convert.ToDecimal(num2));

                    sm.S_MONTOB = format.toShow(Convert.ToDecimal(num2), d);
                    sm.S_MONTOP = format.toShow(0,d);
                    sm.S_MONTOA = "-";
                    sm.S_REMA = format.toShow((decimal)rem2,d);
                    sm.S_IMPA = "-";
                    sm.S_IMPB = "-";
                    sm.S_IMPC = "-";
                    sm.S_RET = "-";
                    sm.S_TOTAL = format.toShow(Convert.ToDecimal(num2), d); ;
                }
                else if (rev.Count() == 1)
                {
                    //CON DOS RELACIONADOS
                    var rev3 = db.DOCUMENTOes.Where(x => x.NUM_DOC == hola).FirstOrDefault();
                    var rev33 = db.DOCUMENTOes.Where(x => x.DOCUMENTO_REF == hola).FirstOrDefault();
                    decimal? rem3 = ((rev3.MONTO_DOC_MD - rev33.MONTO_DOC_MD) - (Convert.ToDecimal(num2)));

                    sm.S_MONTOB = format.toShow(Convert.ToDecimal(num2), d);
                    sm.S_MONTOP = format.toShow((decimal)rev3.MONTO_DOC_MD, d);
                    sm.S_MONTOA = format.toShow((decimal)rev33.MONTO_DOC_MD- Convert.ToDecimal(num2), d);
                    sm.S_REMA = format.toShow((decimal)rem3 + Convert.ToDecimal(num2), d);
                    sm.S_IMPA = "-";
                    sm.S_IMPB = "-";
                    sm.S_IMPC = "-";
                    sm.S_RET = "-";
                    sm.S_TOTAL = format.toShow(Convert.ToDecimal(num2), d); ;
                }
                else if (rev.Count() > 1)
                {
                    var rev4 = db.DOCUMENTOes.Where(x => x.NUM_DOC == hola).FirstOrDefault();
                    var rev44 = db.DOCUMENTOes.Where(x => x.DOCUMENTO_REF == hola).Select(x => x.MONTO_DOC_MD);
                    decimal sum = 0;

                    foreach (var k in rev44)
                    {
                        sum = sum + k.Value;
                    }
                    decimal? rem4 = ((rev4.MONTO_DOC_MD - sum) - (Convert.ToDecimal(num2)));

                    sm.S_MONTOB = format.toShow(Convert.ToDecimal(num2), d); 
                    sm.S_MONTOP = format.toShow((decimal)rev4.MONTO_DOC_MD, d);
                    sm.S_MONTOA = format.toShow(sum - Convert.ToDecimal(num2), d);
                    sm.S_REMA = format.toShow((-sum + (decimal)rev4.MONTO_DOC_MD + Convert.ToDecimal(num2)) , d);
                    sm.S_IMPA = "-";
                    sm.S_IMPB = "-";
                    sm.S_IMPC = "-";
                    sm.S_RET = "-";
                    sm.S_TOTAL = format.toShow(Convert.ToDecimal(num2), d); 
                }
            }

            //JsonResult cc = Json(sm, JsonRequestBehavior.AllowGet);
            return sm;
        }
    }
}
