using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;

namespace TAT001.Controllers
{
    public class ListasController2 : Controller
    {
        // GET: Listas
        public ActionResult Index()
        {
            return View();
        }

        ////[HttpGet]
        ////public JsonResult Clientes(string Prefix)
        ////{
        ////    if (Prefix == null)
        ////        Prefix = "";

        ////    TAT001Entities db = new TAT001Entities();

        ////    var c = (from N in db.CLIENTEs
        ////             where N.KUNNR.Contains(Prefix)
        ////             select new { N.KUNNR, N.NAME1 }).ToList();
        ////    if (c.Count == 0)
        ////    {
        ////        var c2 = (from N in db.CLIENTEs
        ////                  where N.NAME1.Contains(Prefix)
        ////                  select new { N.KUNNR, N.NAME1 }).ToList();
        ////        c.AddRange(c2);
        ////    }
        ////    JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
        ////    return cc;
        ////}

        [HttpGet]
        public JsonResult Clientes(string Prefix, string usuario, string pais)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();
            if (usuario == "" & pais == "")
            {
                var c = (from N in db.CLIENTEs
                         where N.KUNNR.Contains(Prefix)
                         select new { N.KUNNR, N.NAME1 }).ToList();
                if (c.Count == 0)
                {
                    var c2 = (from N in db.CLIENTEs
                              where N.NAME1.Contains(Prefix)
                              select new { N.KUNNR, N.NAME1 }).ToList();
                    c.AddRange(c2);
                }
                JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
                return cc;
            }
            else
            {

                var det = db.DET_AGENTEC.Where(a => a.USUARIOC_ID.Equals(usuario) & a.POS == 1 & a.PAIS_ID.Equals(pais) & a.ACTIVO == true).ToList();

                var c = (from N in db.CLIENTEs.ToList()
                         join D in det
                         on new { N.VKORG, N.VTWEG, N.SPART, N.KUNNR } equals new { D.VKORG, D.VTWEG, D.SPART, D.KUNNR }
                         where N.KUNNR.Contains(Prefix)
                         select new { N.KUNNR, N.NAME1 }).ToList();

                if (c.Count == 0)
                {
                    var c2 = (from N in db.CLIENTEs.ToList()
                              join D in det
                              on new { N.VKORG, N.VTWEG, N.SPART, N.KUNNR } equals new { D.VKORG, D.VTWEG, D.SPART, D.KUNNR }
                              where CultureInfo.CurrentCulture.CompareInfo.IndexOf(N.NAME1, Prefix, CompareOptions.IgnoreCase) >= 0
                              select new { N.KUNNR, N.NAME1 }).ToList();
                    c.AddRange(c2);
                }
                JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
                return cc;
            }

        }
        [HttpGet]
        public JsonResult Estados(string pais, string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            string p = pais.Split('.')[0].ToUpper();
            var c = (from N in db.STATES
                     where N.NAME.Contains(Prefix) & N.COUNTRy.SORTNAME.Equals(p)
                     select new { N.NAME });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpGet]
        public JsonResult Ciudades(string estado, string Prefix)
        {
            if (Prefix == null)
                Prefix = "";

            TAT001Entities db = new TAT001Entities();

            var c = (from N in db.CITIES
                     join St in db.STATES
                     on N.STATE_ID equals St.ID
                     where N.NAME.Contains(Prefix) & St.NAME.Equals(estado)
                     select new { N.NAME });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpGet]
        public JsonResult Det_Aprob(string bukrs, string puesto, string spras)
        {
            TAT001Entities db = new TAT001Entities();
            int p = Int16.Parse(puesto);
            var c = (from N in db.DET_APROB
                     join St in db.PUESTOTs
                     on N.PUESTOA_ID equals St.PUESTO_ID
                     where N.BUKRS.Equals(bukrs) & N.PUESTOC_ID.Equals(p) & St.SPRAS_ID.Equals(spras)
                     //where N.BUKRS.Equals(bukrs) 
                     select new { N.PUESTOA_ID, St.TXT50 });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpGet]
        public JsonResult Det_Aprob2(string bukrs, string puesto, string spras)
        {
            TAT001Entities db = new TAT001Entities();
            int p = Int16.Parse(puesto);
            DET_APROBH dh = db.DET_APROBH.Where(a => a.SOCIEDAD_ID.Equals(bukrs) & a.PUESTOC_ID == p).OrderByDescending(a => a.VERSION).FirstOrDefault();
            if (dh != null)
            {
                var c = (from N in db.DET_APROBP
                         join St in db.PUESTOTs
                         on N.PUESTOA_ID equals St.PUESTO_ID
                         where N.SOCIEDAD_ID.Equals(bukrs) & N.PUESTOC_ID.Equals(p) & St.SPRAS_ID.Equals(spras) & N.VERSION.Equals(dh.VERSION)
                         //where N.BUKRS.Equals(bukrs) 
                         select new { N.POS,  N.PUESTOA_ID.Value, St.TXT50, N.MONTO, PRESUPUESTO = (bool)N.PRESUPUESTO.Value }).ToList();

                TAX_LAND tl = db.TAX_LAND.Where(a => a.SOCIEDAD_ID.Equals(bukrs) & a.ACTIVO == true).FirstOrDefault();
                if (tl != null)
                {
                    var col = (from St in db.PUESTOTs
                               where St.PUESTO_ID == 9 & St.SPRAS_ID.Equals(spras)
                               //where N.BUKRS.Equals(bukrs) 
                               select new { POS = 98, Value = St.PUESTO_ID, St.TXT50, MONTO = (decimal?)decimal.Parse("-1"), PRESUPUESTO = false });
                    c.AddRange(col);
                }
                var ca = c.OrderBy(a => a.POS);
                JsonResult cc = Json(ca, JsonRequestBehavior.AllowGet);
                return cc;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public JsonResult UsuariosPuesto(string puesto, string Prefix)
        {
            TAT001Entities db = new TAT001Entities();
            int p = Int16.Parse(puesto);
            var c = (from N in db.USUARIOs
                     where //N.PUESTO_ID == p & 
                     N.ID.Contains(Prefix)
                     //where N.BUKRS.Equals(bukrs) 
                     select new { N.ID, NOMBRE = N.ID + " - " + N.NOMBRE + " " + N.APELLIDO_P + " " + N.APELLIDO_M });
            if (c.Count() == 0)
            {
                c = (from N in db.USUARIOs
                     where //N.PUESTO_ID == p & 
                     N.NOMBRE.Contains(Prefix) | N.APELLIDO_P.Contains(Prefix) | N.APELLIDO_M.Contains(Prefix)
                     //where N.BUKRS.Equals(bukrs) 
                     select new { N.ID, NOMBRE = N.ID + " - " + N.NOMBRE + " " + N.APELLIDO_P + " " + N.APELLIDO_M });
            }
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        //[HttpGet]
        //public JsonResult Grupos(string bukrs, string pais, string user)
        //{
        //    TAT001Entities db = new TAT001Entities();
        //    var c = (from N in db.CREADOR2
        //             where N.BUKRS == bukrs & N.LAND == pais & N.ID.Equals(user)
        //             //where N.BUKRS.Equals(bukrs) 
        //             select new { N.AGROUP_ID });
        //    JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
        //    return cc;
        //}
        [HttpGet]
        public JsonResult Grupos(string pais, string user, string vkorg, string vtweg, string spart, string kunnr)
        {
            TAT001Entities db = new TAT001Entities();
            var c = (from N in db.DET_AGENTEC
                     where N.PAIS_ID == pais
                     & N.USUARIOC_ID.Equals(user)
                     & N.VKORG.Equals(vkorg)
                     & N.VTWEG.Equals(vtweg)
                     & N.SPART.Equals(spart)
                     & N.KUNNR.Equals(kunnr)
                     select new { N.POS });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        //[HttpPost]
        //public JsonResult getPresupuesto(string kunnr)
        //{
        //    TAT001Entities db = new TAT001Entities();
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
        //    db.Dispose();

        //    JsonResult cc = Json(pm, JsonRequestBehavior.AllowGet);
        //    return cc;
        //}
        [HttpGet]
        public JsonResult Relacionados(string num_doc, string spras)
        {
            TAT001Entities db = new TAT001Entities();
            //var c = db.DOCUMENTOes.Where(a => a.DOCUMENTO_REF.Equals(num_doc));
            decimal num = (decimal.Parse(num_doc));
            var c = (from D in db.DOCUMENTOes
                     join T in db.TSOLTs
                     on D.TSOL_ID equals T.TSOL_ID
                     join TA in db.TALLs
                     on D.TALL_ID equals TA.ID
                     join G in db.GALLTs
                     on TA.GALL_ID equals G.GALL_ID
                     where D.DOCUMENTO_REF == num
                     & T.SPRAS_ID == spras
                     & G.SPRAS_ID == spras
                     select new { D.NUM_DOC, T.TXT020, TXT500 = G.TXT50, FECHAD = D.FECHAD.Value.Year + "/" + D.FECHAD.Value.Month + "/" + D.FECHAD.Value.Day, HORAC = D.HORAC.Value.ToString(), D.ESTATUS_WF, D.ESTATUS, D.CONCEPTO, D.MONTO_DOC_ML });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult Paises(string bukrs)
        {
            TAT001Entities db = new TAT001Entities();

            var c = (from D in db.PAIS
                     where D.SOCIEDAD_ID == bukrs
                     select new { D.LAND, D.LANDX });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult selectTaxeo(string bukrs, string pais, string vkorg, string vtweg, string spart, string kunnr, string spras)
        {
            TAT001Entities db = new TAT001Entities();

            var c = (from T in db.TAXEOHs
                     join TX in db.TX_CONCEPTOT
                     on T.CONCEPTO_ID equals TX.CONCEPTO_ID
                     where T.SOCIEDAD_ID == bukrs
                     & T.PAIS_ID == pais
                     & T.VKORG == vkorg
                     & T.VTWEG == vtweg
                     & T.SPART == spart
                     & T.KUNNR == kunnr
                     & TX.SPRAS_ID == spras
                     select new { T.CONCEPTO_ID, TX.TXT50 });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpPost]
        public JsonResult selectConcepto(string bukrs, string pais, string vkorg, string vtweg, string spart, string kunnr, string concepto, string spras)
        {
            TAT001Entities db = new TAT001Entities();
            int co = int.Parse(concepto);
            var c = (from T in db.TAXEOHs
                     join TX in db.TX_NOTAT
                     on T.TNOTA_ID equals TX.TNOTA_ID
                     where T.SOCIEDAD_ID == bukrs
                     & T.PAIS_ID == pais
                     & T.VKORG == vkorg
                     & T.VTWEG == vtweg
                     & T.SPART == spart
                     & T.KUNNR == kunnr
                     & T.CONCEPTO_ID == co
                     & TX.SPRAS_ID == spras
                     select new { T.TNOTA_ID, TX.TXT50 });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpPost]
        public JsonResult selectImpuesto(string bukrs, string pais, string vkorg, string vtweg, string spart, string kunnr, string concepto, string spras)
        {
            TAT001Entities db = new TAT001Entities();
            int co = int.Parse(concepto);
            var c = (from T in db.TAXEOHs
                     where T.SOCIEDAD_ID == bukrs
                     & T.PAIS_ID == pais
                     & T.VKORG == vkorg
                     & T.VTWEG == vtweg
                     & T.SPART == spart
                     & T.KUNNR == kunnr
                     & T.CONCEPTO_ID == co
                     select new { T.IMPUESTO_ID, T.PORC, TXT50 = "IVA" }).ToList();

            var c2 = (from T in db.TAXEOPs
                      join R in db.TRETENCIONTs
                      on T.TRETENCION_ID equals R.TRETENCION_ID
                      where T.SOCIEDAD_ID == bukrs
                      & T.PAIS_ID == pais
                      & T.VKORG == vkorg
                      & T.VTWEG == vtweg
                      & T.SPART == spart
                      & T.KUNNR == kunnr
                      & T.CONCEPTO_ID == co
                      & R.SPRAS_ID == spras
                      select new { IMPUESTO_ID = T.RETENCION_ID.ToString(), T.PORC, R.TXT50 }).ToList();

            c.AddRange(c2);

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpPost]
        public JsonResult soportes(string tsol, string spras)
        {
            TAT001Entities db = new TAT001Entities();

            var c = (from C in db.CONSOPORTEs
                     join T in db.TSOPORTETs
                     on C.TSOPORTE_ID equals T.TSOPORTE_ID
                     where C.TSOL_ID == tsol
                     & T.SPRAS_ID == spras
                     select new { C.TSOPORTE_ID, C.OBLIGATORIO, T.TXT50 });

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpPost]
        public JsonResult clearing(string bukrs, string land, string gall, string ejercicio)
        {
            TAT001Entities db = new TAT001Entities();
            decimal ejer = decimal.Parse(ejercicio);

            var c = (from C in db.CUENTAs
                     where C.SOCIEDAD_ID == bukrs
                     & C.PAIS_ID == land
                     & C.TALL_ID == gall
                     & C.EJERCICIO == ejer
                     select new { C.ABONO, C.CARGO, C.CLEARING, C.LIMITE }).FirstOrDefault();

            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult categoriasCliente(string vkorg, string spart, string kunnr, string soc_id)
        {
            TAT001Entities db = new TAT001Entities();
            if (kunnr == null)
            {
                kunnr = "";
            }

            //if (catid == null)
            //{
            //    catid = "";
            //}

            var jd = (dynamic)null;

            //Obtener los materiales
            IEnumerable<MATERIAL> matl = Enumerable.Empty<MATERIAL>();
            try
            {
                matl = db.MATERIALs.Where(m => m.ACTIVO == true);//.Select(m => m.ID).ToList();
            }
            catch (Exception e)
            {

            }

            var spras = Session["spras"].ToString();
            //Validar si hay materiales
            if (matl != null)
            {

                CLIENTE cli = new CLIENTE();
                List<CLIENTE> clil = new List<CLIENTE>();

                try
                {
                    cli = db.CLIENTEs.Where(c => c.KUNNR == kunnr & c.VKORG == vkorg & c.SPART == spart).FirstOrDefault();

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
                catch (Exception e)
                {

                }

                var cie = clil.Cast<CLIENTE>();
                //    IEnumerable<CLIENTE> cie = clil as IEnumerable<CLIENTE>;
                //Obtener el numero de periodos para obtener el historial
                int nummonths = 3;
                int imonths = nummonths * -1;
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
                catch (Exception e)
                {

                }

                int mii = 0;
                try
                {
                    mii = Convert.ToInt32(mi);
                }
                catch (Exception e)
                {

                }

                int aff = 0;
                try
                {
                    aff = Convert.ToInt32(af);
                }
                catch (Exception e)
                {

                }

                int mff = 0;
                try
                {
                    mff = Convert.ToInt32(mf);
                }
                catch (Exception e)
                {

                }

                if (cie != null)
                {
                    //Obtener el historial de compras de los clientesd
                    var matt = matl.ToList();
                    //kunnr = kunnr.TrimStart('0').Trim();
                    var pres = db.PRESUPSAPPs.Where(a => a.VKORG.Equals(vkorg) & a.SPART.Equals(spart) & a.KUNNR == kunnr & (a.GRSLS != null | a.NETLB != null)).ToList();
                    var cat = db.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(spras)).ToList();
                    //foreach (var c in cie)
                    //{
                    //    c.KUNNR = c.KUNNR.TrimStart('0').Trim();
                    //}

                    CONFDIST_CAT conf = getCatConf(soc_id);
                    if (conf.CAMPO == "GRSLS")
                    {
                        jd = (from ps in pres
                              join cl in cie
                              on ps.KUNNR equals cl.KUNNR
                              join m in matt
                              on ps.MATNR equals m.ID
                              join mk in cat
                              on m.MATERIALGP_ID equals mk.MATERIALGP_ID
                              where (ps.ANIO >= aii && ps.PERIOD >= mii) && (ps.ANIO <= aff && ps.PERIOD <= mff) &&
                              (ps.VKORG == cl.VKORG && ps.VTWEG == cl.VTWEG && ps.SPART == cl.SPART //&& ps.VKBUR == cl.VKBUR &&
                                                                                                    //ps.VKGRP == cl.VKGRP && ps.BZIRK == cl.BZIRK
                              ) && ps.BUKRS == soc_id
                              && ps.GRSLS > 0
                              select new
                              {
                                  m.MATERIALGP_ID,
                                  mk.TXT50
                              }).ToList();
                    }
                    else
                    {
                        jd = (from ps in pres
                              join cl in cie
                              on ps.KUNNR equals cl.KUNNR
                              join m in matt
                              on ps.MATNR equals m.ID
                              join mk in cat
                              on m.MATERIALGP_ID equals mk.MATERIALGP_ID
                              where (ps.ANIO >= aii && ps.PERIOD >= mii) && (ps.ANIO <= aff && ps.PERIOD <= mff) &&
                              (ps.VKORG == cl.VKORG && ps.VTWEG == cl.VTWEG && ps.SPART == cl.SPART //&& ps.VKBUR == cl.VKBUR &&
                                                                                                    //ps.VKGRP == cl.VKGRP && ps.BZIRK == cl.BZIRK
                              ) && ps.BUKRS == soc_id
                              && ps.NETLB > 0
                              select new
                              {
                                  m.MATERIALGP_ID,
                                  mk.TXT50
                              }).ToList();
                    }
                }
            }

            //var jll = db.PRESUPSAPPs.Select(psl => new { MATNR = psl.MATNR.ToString() }).Take(7).ToList();
            var list = new List<MATERIALGPT>();
            if (jd.Count > 0)
            {
                //MATERIALGPT c = db.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(spras) & a.MATERIALGP_ID.Equals("000")).FirstOrDefault();
                //MATERIALGPT cc = new MATERIALGPT();
                //cc.SPRAS_ID = c.SPRAS_ID;
                //cc.MATERIALGP_ID = c.MATERIALGP_ID;
                //cc.TXT50 = c.TXT50;
                //list.Add(cc);
                MATERIALGPT c = db.MATERIALGPTs.Where(a => a.SPRAS_ID.Equals(spras) & a.MATERIALGP_ID.Equals("000")).FirstOrDefault();
                MATERIALGPT cc = new MATERIALGPT();//B20180625 MGC 2018.07.02
                if (c != null)//B20180625 MGC 2018.07.02
                {
                    cc.SPRAS_ID = c.SPRAS_ID;
                    cc.TXT50 = c.TXT50;

                }
                else
                {
                    cc.MATERIALGP_ID = "000";
                }
                list.Add(cc);
            }

            foreach (var line in jd)
            {
                bool ban = true;
                foreach (var line2 in list)
                {
                    if (line.MATERIALGP_ID == line2.MATERIALGP_ID)
                        ban = false;

                }
                if (ban)
                {

                    MATERIALGPT c = new MATERIALGPT();
                    c.MATERIALGP_ID = line.MATERIALGP_ID;
                    c.TXT50 = line.TXT50;
                    list.Add(c);
                }
            }

            JsonResult jl = Json(list, JsonRequestBehavior.AllowGet);
            return jl;
        }

        public CONFDIST_CAT getCatConf(string soc)
        {
            TAT001Entities db = new TAT001Entities();
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
        public JsonResult getPeriodo(string fecha)
        {
            string f = "";
            Calendario445 c4 = new Calendario445();
            string[] ff = fecha.Split('/');
            f = c4.getPeriodo(new DateTime(int.Parse(ff[2]), int.Parse(ff[1]), int.Parse(ff[0]))).ToString();

            JsonResult jl = Json(f, JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult getPrimerDia(string ejercicio, string periodo)
        {
            int e = int.Parse(ejercicio);
            int p = int.Parse(periodo);
            Calendario445 c4 = new Calendario445();
            DateTime f = c4.getPrimerDia(e, p);

            JsonResult jl = Json(f.ToShortDateString(), JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult getUltimoDia(string ejercicio, string periodo)
        {
            int e = int.Parse(ejercicio);
            int p = int.Parse(periodo);
            Calendario445 c4 = new Calendario445();
            DateTime f = c4.getUltimoDia(e, p);

            JsonResult jl = Json(f.ToShortDateString(), JsonRequestBehavior.AllowGet);
            return jl;
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


            JsonResult jc = Json(id_cl, JsonRequestBehavior.AllowGet);
            return jc;
        }

    }
}