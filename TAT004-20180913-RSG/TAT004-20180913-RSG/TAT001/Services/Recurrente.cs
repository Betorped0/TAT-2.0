using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Services
{
    public class Recurrente
    {
        public int creaRecurrente(string id_d, string tsol)
        {
            string dates = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime theTime = DateTime.ParseExact(dates, //"06/04/2018 12:00:00 a.m."
                                        "dd/MM/yyyy",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None);

            var relacionada_neg = "";
            var relacionada_dis = "";

            DOCUMENTO dOCUMENTO = new DOCUMENTO();
            DOCUMENTO dOCpADRE = new DOCUMENTO();
            string errorString = "";
            TAT001Entities db = new TAT001Entities();


            string p = "";
            List<TREVERSAT> ldocr = new List<TREVERSAT>();
            decimal rel = 0;
            try
            {
                if (id_d == null || id_d.Equals(""))
                {
                    throw new Exception();
                }
                rel = Convert.ToDecimal(id_d);

            }
            catch
            {
                rel = 0;
            }

            //Obtener los documentos relacionados
            List<DOCUMENTO> docsrel = new List<DOCUMENTO>();

            SOCIEDAD id_bukrs = new SOCIEDAD();
            var id_pais = new PAI();
            var id_waers = db.MONEDAs.Where(m => m.ACTIVO == true).ToList();

            if (rel > 0)
            {
                dOCpADRE = db.DOCUMENTOes.Where(doc => doc.NUM_DOC == rel).FirstOrDefault();
                if (dOCpADRE.TIPO_RECURRENTE == null)//RSG 28.05.2018----------------------------------------------
                    return 0;
                if (!((dOCpADRE.TIPO_RECURRENTE.Equals("M") | dOCpADRE.TIPO_RECURRENTE.Equals("P")) & dOCpADRE.ESTATUS.Equals("A") & dOCpADRE.ESTATUS_WF.Equals("A")))//RSG 28.05.2018
                {
                    return 0;
                }
                //List<DOCUMENTOREC> ddrec = new List<DOCUMENTOREC>();
                DOCUMENTOREC drec = dOCpADRE.DOCUMENTORECs.Where(a => a.ESTATUS == "A").FirstOrDefault();
                if (drec == null)
                    return 0;
                else
                {
                    DateTime hoy = (DateTime)drec.FECHAF;
                    var primer = new DateTime(hoy.Year, hoy.Month, 1);
                    var ultimo = primer.AddMonths(1).AddDays(-1);
                    dOCUMENTO.FECHAI_VIG = primer;
                    dOCUMENTO.FECHAF_VIG = ultimo;
                    dOCUMENTO.MONTO_DOC_MD = (decimal)drec.MONTO_BASE;
                }
                if (tsol != dOCpADRE.TSOL_ID)
                    return 0;
                //RSG 28.05.2018----------------------------------------------
                docsrel = db.DOCUMENTOes.Where(docr => docr.DOCUMENTO_REF == rel).ToList();
                id_bukrs = db.SOCIEDADs.Where(soc => soc.BUKRS == dOCpADRE.SOCIEDAD_ID && soc.ACTIVO == true).FirstOrDefault();
                id_pais = db.PAIS.Where(pais => pais.LAND.Equals(dOCpADRE.PAIS_ID)).FirstOrDefault();//RSG 15.05.2018
                dOCUMENTO.DOCUMENTO_REF = rel;
                relacionada_neg = dOCpADRE.TIPO_TECNICO;
                ////ViewBag.TSOL_ANT = dOCUMENTO.TSOL_ID;

                if (dOCUMENTO != null)
                {

                    dOCUMENTO.TSOL_ID = tsol;
                    dOCUMENTO.NUM_DOC = 0;
                    foreach (DOCUMENTOP pos in dOCpADRE.DOCUMENTOPs)
                    {

                    }
                }
            }
            else
            {
            }

            dOCUMENTO.SOCIEDAD_ID = id_bukrs.BUKRS;
            dOCUMENTO.PAIS_ID = id_pais.LAND;//RSG 18.05.2018
            dOCUMENTO.MONEDA_ID = id_bukrs.WAERS;
            dOCUMENTO.PERIODO = Convert.ToInt32(DateTime.Now.ToString("MM"));
            dOCUMENTO.EJERCICIO = Convert.ToString(DateTime.Now.Year);

            dOCUMENTO.FECHAD = theTime;

            //----------------------------RSG 18.05.2018

            //dOCUMENTO.SOCIEDAD = db.SOCIEDADs.Find(dOCUMENTO.SOCIEDAD_ID);
            //----------------------------RSG 18.05.2018



            ////HTTPPOST
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
                dOCUMENTO.TALL_ID = d.TALL_ID;
                //Obtener el país
                dOCUMENTO.PAIS_ID = d.PAIS_ID;//RSG 15.05.2018
                dOCUMENTO.TSOL_ID = d.TSOL_ID;
            }

            //Tipo técnico
            dOCUMENTO.TIPO_TECNICO = "M";

            USUARIO u = db.USUARIOs.Find(d.USUARIOC_ID);//RSG 02/05/2018
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
            dOCUMENTO.PUESTO_ID = u.PUESTO_ID;//RSG 02/05/2018
            dOCUMENTO.USUARIOC_ID = u.ID;

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

            dOCUMENTO.DOCUMENTO_REF = null;

            //Guardar el documento
            db.DOCUMENTOes.Add(dOCUMENTO);
            db.SaveChanges();

            //Actualizar el rango
            updateRango(dOCUMENTO.TSOL_ID, dOCUMENTO.NUM_DOC);

            //RSG 28.05.2018----------------------------------------------
            string recurrente = "";
            List<DOCUMENTOREC> ddrec = new List<DOCUMENTOREC>();
            DOCUMENTOREC drecc = d.DOCUMENTORECs.Where(a => a.ESTATUS == "A").FirstOrDefault();
            if (drecc == null)
                return 0;
            else
            {
                DateTime hoy = drecc.FECHAF.Value;
                var primer = new DateTime(hoy.Year, hoy.Month, 1);
                var ultimo = primer.AddMonths(1).AddDays(-1);
                dOCUMENTO.FECHAI_VIG = primer;
                dOCUMENTO.FECHAF_VIG = ultimo;
                dOCUMENTO.MONTO_DOC_MD = drecc.MONTO_BASE;
                dOCUMENTO.FECHAD = DateTime.Now;
                recurrente = "X";
            }
            drecc.DOC_REF = dOCUMENTO.NUM_DOC;
            //RSG 28.05.2018----------------------------------------------

            //RSG 28.05.2018----------------------------------------------
            drecc.DOC_REF = dOCUMENTO.NUM_DOC;
            drecc.ESTATUS = "P";
            db.SaveChanges();
            //RSG 28.05.2018----------------------------------------------

            //Guardar los documentos p para el documento guardado
            try
            {
                //Agregar materiales existentes para evitar que en la vista se hayan agregado o quitado
                List<DOCUMENTOP> docpl = new List<DOCUMENTOP>();

                docpl = db.DOCUMENTOPs.Where(docp => docp.NUM_DOC == dOCpADRE.NUM_DOC).ToList();

                for (int j = 0; j < docpl.Count; j++)
                {
                    try
                    {
                        //DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();
                        var cat = "";

                        if (docpl[j].MATNR != null && docpl[j].MATNR != "")
                        {
                            //docmod = dOCUMENTO.DOCUMENTOP.Where(docp => docp.MATNR == docpl[j].MATNR).FirstOrDefault();
                        }
                        else
                        {
                            //docmod = dOCUMENTO.DOCUMENTOP.Where(docp => docp.MATKL_ID == docpl[j].MATKL).FirstOrDefault();
                            cat = "C";
                        }
                        //Si lo encuentra meter valores de la base de datos y vista
                        if (docpl[j] != null)
                        {
                            if (cat != "C")
                            {
                                DOCUMENTOP docP = new DOCUMENTOP();
                                docP.NUM_DOC = dOCUMENTO.NUM_DOC;
                                docP.POS = docpl[j].POS;
                                if (docpl[j].MATNR == null || docpl[j].MATNR == "")
                                {
                                    docpl[j].MATNR = "";
                                }
                                docP.MATNR = docpl[j].MATNR;
                                docP.MATKL = docpl[j].MATKL;
                                docP.CANTIDAD = 1;
                                docP.MONTO = docpl[j].MONTO;
                                docP.PORC_APOYO = docpl[j].PORC_APOYO;
                                //docP.MONTO_APOYO = docmod.MONTO_APOYO;
                                docP.MONTO_APOYO = docP.MONTO * (docP.PORC_APOYO / 100);
                                docP.MONTO_APOYO = Math.Round(docP.MONTO_APOYO, 2);//RSG 16.05.2018
                                docP.PRECIO_SUG = docpl[j].PRECIO_SUG;
                                docP.VOLUMEN_EST = docpl[j].VOLUMEN_EST;
                                docP.VOLUMEN_REAL = docpl[j].VOLUMEN_REAL;
                                docP.VIGENCIA_DE = docpl[j].VIGENCIA_DE;
                                docP.VIGENCIA_AL = docpl[j].VIGENCIA_AL;
                                docP.APOYO_EST = docpl[j].APOYO_EST;
                                docP.APOYO_REAL = docpl[j].APOYO_REAL;
                                dOCUMENTO.DOCUMENTOPs.Add(docP);//RSG 28.05.2018
                                db.SaveChanges();//RSG
                            }
                            else
                            {
                                foreach (DOCUMENTOM docmmm in docpl[j].DOCUMENTOMs)
                                {
                                    DOCUMENTOP docP = new DOCUMENTOP();
                                    docP.NUM_DOC = dOCUMENTO.NUM_DOC;
                                    docP.POS = (docpl[j].POS * 10) + docmmm.POS;

                                    docP.MATNR = docmmm.MATNR;
                                    docP.MATKL = null;
                                    //docP.MONTO = (decimal)docmmm.APOYO_EST;
                                    docP.VIGENCIA_DE = docpl[j].VIGENCIA_DE;
                                    docP.VIGENCIA_AL = docpl[j].VIGENCIA_AL;
                                    docP.CANTIDAD = 1;
                                    docP.APOYO_EST = (decimal)docmmm.APOYO_EST;
                                    docP.APOYO_REAL = (decimal)docmmm.APOYO_REAL;
                                    docP.VOLUMEN_EST = 0;
                                    docP.VOLUMEN_REAL = 0;

                                    dOCUMENTO.DOCUMENTOPs.Add(docP);//RSG 28.05.2018
                                    db.SaveChanges();//RSG
                                }
                            }

                        }
                        else
                        {
                            DOCUMENTOP docP = new DOCUMENTOP();
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
                            dOCUMENTO.DOCUMENTOPs.Add(docP);//RSG 28.05.2018
                            db.SaveChanges();//RSG
                        }

                        //Agregarlo a la bd
                        //db.DOCUMENTOPs.Add(docP);
                    }
                    catch (Exception e)
                    {

                    }

                }


            }
            catch (Exception e)
            {

            }
            decimal total = 0;
            //RSG 28.05.2018-----------------------------------------------------
            foreach (DOCUMENTOP dp in dOCUMENTO.DOCUMENTOPs)
            {
                dp.VIGENCIA_DE = dOCUMENTO.FECHAI_VIG;
                dp.VIGENCIA_AL = dOCUMENTO.FECHAF_VIG;
                if (dOCpADRE.TIPO_TECNICO == "P")
                {
                    ////if (!dOCpADRE.TSOL.FACTURA)
                    ////{
                    ////    try
                    ////    {
                    ////        total += (decimal)dp.APOYO_EST;
                    ////    }
                    ////    catch { }
                    ////}
                    ////else
                    ////{
                    ////    try
                    ////    {
                    ////        total += (decimal)dp.APOYO_REAL;
                    ////    }
                    ////    catch { }
                    ////}
                    dp.MONTO = 0;
                    dp.CANTIDAD = 0;
                    dp.MONTO_APOYO = 0;
                    dp.PORC_APOYO = 0;
                    dp.PRECIO_SUG = 0;
                    dp.VOLUMEN_EST = 0;
                    dp.VOLUMEN_REAL = 0;
                    try
                    {
                        decimal val = (decimal)(from P in db.PRESUPSAPPs
                                                where P.VKORG == dOCpADRE.VKORG
                                                & P.VTWEG == dOCpADRE.VTWEG
                                                & P.SPART == dOCpADRE.SPART
                                                & P.KUNNR == dOCpADRE.PAYER_ID
                                                & P.MATNR == dp.MATNR
                                                & P.PERIOD == DateTime.Now.Month
                                                select new { P.GRSLS }).Sum(a => a.GRSLS);
                        total += val;
                        dp.MONTO = val;
                    }
                    catch { }
                }
                
                db.Entry(dOCUMENTO).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (dOCpADRE.TIPO_TECNICO == "P")
            {
                foreach (DOCUMENTOP dp in dOCUMENTO.DOCUMENTOPs)
                {
                    //total = 100%   200 = 100%
                    //dp.MONTo = ?      50 = 25%
                    decimal porcentaje = dp.MONTO / total * 100;
                    decimal nuevo_total = (decimal)drecc.MONTO_BASE;

                    if (!dOCpADRE.TSOL.FACTURA)
                    {
                        try
                        {
                            dp.APOYO_EST = nuevo_total * porcentaje / 100;
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            dp.APOYO_REAL = nuevo_total * porcentaje / 100; ;
                        }
                        catch { }
                        ////}
                    }
                    dp.MONTO = 0;

                    db.Entry(dOCUMENTO).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //RSG 28.05.2018-----------------------------------------------------

            ProcesaFlujo2 pf = new ProcesaFlujo2();
            //db.DOCUMENTOes.Add(dOCUMENTO);
            //db.SaveChanges();

            USUARIO user = db.USUARIOs.Where(a => a.ID.Equals(d.USUARIOC_ID)).FirstOrDefault();
            int rol = user.MIEMBROS.FirstOrDefault().ROL_ID;
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
                    string c = pf.procesa(f, recurrente);
                    //RSG 28.05.2018 -----------------------------------
                    //if (c == "1")
                    //{
                    //    Email em = new Email();
                    //    em.enviaMail(f.NUM_DOC, true);
                    //}
                    FLUJO conta = db.FLUJOes.Where(a => a.NUM_DOC.Equals(f.NUM_DOC)).OrderByDescending(a => a.POS).FirstOrDefault();
                    conta.USUARIOA_ID = user.ID;
                    conta.ESTATUS = "A";
                    conta.FECHAM = DateTime.Now;
                    pf.procesa(conta, "");
                    //RSG 28.05.2018 -----------------------------------
                }
            }
            catch (Exception ee)
            {
                if (errorString == "")
                {
                    errorString = ee.Message.ToString();
                    return 0;
                }
                //ViewBag.error = errorString;
            }

            return 1;
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
            TAT001Entities db = new TAT001Entities();
            RANGO rango = getRango(TSOL_ID);

            if (rango.ACTUAL > rango.INICIO && rango.ACTUAL < rango.FIN)
            {
                rango.ACTUAL = actual;
            }

            db.Entry(rango).State = EntityState.Modified;
            db.SaveChanges();

        }
    }
}