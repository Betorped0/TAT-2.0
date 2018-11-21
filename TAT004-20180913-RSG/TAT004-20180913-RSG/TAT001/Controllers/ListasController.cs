﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;

namespace TAT001.Controllers
{
    public class ListasController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();
        // GET: Listas
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ReportesFiltroCliente(string Prefix, string cocodes)
        {
            if (Prefix == null)
                Prefix = "";
            string[] comcodessplit = { };
            string cocode = cocodes.ToString();
            if (!string.IsNullOrEmpty(cocode))
            {
                comcodessplit = cocode.Split(',');
            }

            if (comcodessplit.Count() > 0)
            {
                var c = (from cl in db.CLIENTEs
                         join p in db.PAIS on cl.LAND equals p.LAND
                         where cl.KUNNR.Contains(Prefix) && comcodessplit.Contains(p.SOCIEDAD_ID) && cl.ACTIVO && p.ACTIVO
                         orderby cl.NAME1
                         select new { cl.KUNNR, cl.NAME1 }).ToList();
                if (c.Count == 0)
                {
                    var c2 = (from cl in db.CLIENTEs
                              join p in db.PAIS on cl.LAND equals p.LAND
                              where cl.NAME1.Contains(Prefix) && comcodessplit.Contains(p.SOCIEDAD_ID) && cl.ACTIVO && p.ACTIVO
                              orderby cl.NAME1
                              select new { cl.KUNNR, cl.NAME1 }).ToList();
                    c.AddRange(c2);
                }
                JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
                return cc;
            }
            else
            {
                var c = (from cl in db.CLIENTEs
                         join p in db.PAIS on cl.LAND equals p.LAND
                         where cl.KUNNR.Contains(Prefix) && cl.ACTIVO && p.ACTIVO
                         orderby cl.NAME1
                         select new { cl.KUNNR, cl.NAME1 }).ToList();
                if (c.Count == 0)
                {
                    var c2 = (from cl in db.CLIENTEs
                              join p in db.PAIS on cl.LAND equals p.LAND
                              where cl.NAME1.Contains(Prefix) && cl.ACTIVO && p.ACTIVO
                              orderby cl.NAME1
                              select new { cl.KUNNR, cl.NAME1 }).ToList();
                    c.AddRange(c2);
                }
                JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
                return cc;
            }
        }
        [HttpGet]
        public JsonResult ReportesFiltroPaises(string cocodes)
        {
            string[] comcodessplit = { };
            string cocode = cocodes.ToString();
            if (!string.IsNullOrEmpty(cocode))
            {
                comcodessplit = cocode.Split(',');
            }
            

            var c = (from D in db.PAIS
                     where comcodessplit.Contains(D.SOCIEDAD_ID) && D.ACTIVO
                     orderby D.LANDX
                     select new { D.LAND, D.LANDX });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult ReportesFiltroPayer(string Prefix, string cocodes)
        {
            string[] comcodessplit = { };
            string cocode = cocodes.ToString();
            if (!string.IsNullOrEmpty(cocode))
            {
                comcodessplit = cocode.Split(',');
            }
            

            var c = (from cl in db.CLIENTEs
                     join p in db.PAIS on cl.LAND equals p.LAND
                     where comcodessplit.Contains(p.SOCIEDAD_ID) && cl.NAME1.Contains(Prefix) && cl.ACTIVO && p.ACTIVO
                     orderby cl.NAME1
                     select new { cl.KUNNR, cl.NAME1 });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }

        [HttpGet]
        public JsonResult Clientes(string Prefix, string usuario, string pais)
        {
            if (usuario==""){ usuario = null;}
            if (pais == ""){  pais = null;}
            var clientes = FnCommon.ObtenerClientes(db,Prefix,usuario,pais);
            JsonResult cc = Json(clientes, JsonRequestBehavior.AllowGet);
            return cc;
           

        }
        [HttpGet]
        public JsonResult Solicitudes(string Prefix,decimal? num_doci,decimal? num_docf)
        {
            var solicitudes = FnCommon.ObtenerSolicitudes(db, Prefix, num_doci, num_docf);
            JsonResult cc = Json(solicitudes, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult Usuarios(string Prefix,bool? autorizador)
        {
            var usuarios = FnCommon.ObtenerUsuarios(db, Prefix, autorizador);
            JsonResult cc = Json(usuarios, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult Estados(string pais, string Prefix)
        {
            if (Prefix == null)
                Prefix = "";
            

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
            int p = Int16.Parse(puesto);
            DET_APROBH dh = db.DET_APROBH.Where(a => a.SOCIEDAD_ID.Equals(bukrs) & a.PUESTOC_ID == p).OrderByDescending(a => a.VERSION).FirstOrDefault();
            if (dh != null)
            {
                var c = (from N in db.DET_APROBP
                         join St in db.PUESTOTs
                         on N.PUESTOA_ID equals St.PUESTO_ID
                         where N.SOCIEDAD_ID.Equals(bukrs) & N.PUESTOC_ID.Equals(p) & St.SPRAS_ID.Equals(spras) & N.VERSION.Equals(dh.VERSION)
                         //where N.BUKRS.Equals(bukrs) 
                         select new { N.POS, N.PUESTOA_ID.Value, St.TXT50, N.MONTO, PRESUPUESTO = (bool)N.PRESUPUESTO.Value }).ToList();

                TAX_LAND tl = db.TAX_LAND.Where(a => a.SOCIEDAD_ID.Equals(bukrs) & a.ACTIVO == true).FirstOrDefault();
                if (tl != null)
                {
                    var col = (from St in db.PUESTOTs
                               where St.PUESTO_ID == 9 & St.SPRAS_ID.Equals(spras)
                               //where N.BUKRS.Equals(bukrs) 
                               select new { POS = 98, Value = St.PUESTO_ID, St.TXT50, PRESUPUESTO = false });
                    foreach (var coll in col)
                    {
                        var colll = new { POS = 98, Value = coll.Value, coll.TXT50, MONTO = (decimal?)decimal.Parse("-1"), PRESUPUESTO = false };
                        c.Add(colll);
                    }
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

        [HttpGet]
        public JsonResult Grupos(string pais, string user, string vkorg, string vtweg, string spart, string kunnr)
        {
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);
            
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
        [HttpPost]
        public JsonResult getPresupuesto(string kunnr, string periodo)
        {
            PRESUPUESTO_MOD pm = new PRESUPUESTO_MOD();
            Presupuesto pr = new Presupuesto();
            Cadena c = new Cadena();
            pm = pr.getPresupuesto(c.completaCliente(kunnr), periodo);
            

            JsonResult cc = Json(pm, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult Relacionados(string num_doc, string spras)
        {
            //var c = db.DOCUMENTOes.Where(a => a.DOCUMENTO_REF.Equals(num_doc));
            decimal num = (decimal.Parse(num_doc));
            DOCUMENTO d = db.DOCUMENTOes.Find(num);
            if (d.DOCUMENTORECs.Count > 0)
            {
                Estatus e = new Estatus();
                List<DOCUMENTO_MOD> c = (from DR in db.DOCUMENTORECs
                                         join D in db.DOCUMENTOes
                                         on DR.DOC_REF equals D.NUM_DOC
                                         join T in db.TSOLTs
                                         on D.TSOL_ID equals T.TSOL_ID
                                         join TA in db.TALLs
                                         on D.TALL_ID equals TA.ID
                                         join G in db.GALLTs
                                         on TA.GALL_ID equals G.GALL_ID
                                         where DR.NUM_DOC == num
                                         & T.SPRAS_ID == spras
                                         & G.SPRAS_ID == spras
                                         & D.NUM_DOC != 0
                                         select new DOCUMENTO_MOD
                                         {
                                             NUM_DOC = D.NUM_DOC,
                                             TSOL_ID = T.TXT020,
                                             GALL_ID = G.TXT50,
                                             ESTADO = D.FECHAD.Value.Year + "/" + D.FECHAD.Value.Month + "/" + D.FECHAD.Value.Day,
                                             CIUDAD = D.HORAC.Value.ToString(),
                                             ESTATUS = D.ESTATUS,
                                             CONCEPTO = D.CONCEPTO,
                                             MONTO_DOC_ML = D.MONTO_DOC_ML
                                         }).ToList();
                foreach (DOCUMENTO_MOD ddd in c)
                {
                    ddd.ESTATUS = e.getHtml(ddd.NUM_DOC);
                }
                JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
                return cc;
            }
            else
            {
                Estatus e = new Estatus();
                List<DOCUMENTO_MOD> c = (from D in db.DOCUMENTOes
                                         join T in db.TSOLTs
                                         on D.TSOL_ID equals T.TSOL_ID
                                         join TA in db.TALLs
                                         on D.TALL_ID equals TA.ID
                                         join G in db.GALLTs
                                         on TA.GALL_ID equals G.GALL_ID
                                         where D.DOCUMENTO_REF == num
                                         & T.SPRAS_ID == spras
                                         & G.SPRAS_ID == spras
                                         select new DOCUMENTO_MOD
                                         {
                                             NUM_DOC = D.NUM_DOC,
                                             TSOL_ID = T.TXT020,
                                             GALL_ID = G.TXT50,
                                             ESTADO = D.FECHAD.Value.Year + "/" + D.FECHAD.Value.Month + "/" + D.FECHAD.Value.Day,
                                             CIUDAD = D.HORAC.Value.ToString(),
                                             ESTATUS = D.ESTATUS,
                                             CONCEPTO = D.CONCEPTO,
                                             MONTO_DOC_ML = D.MONTO_DOC_ML
                                         }).ToList();
                foreach (DOCUMENTO_MOD ddd in c)
                {
                    ddd.ESTATUS = e.getHtml(ddd.NUM_DOC);
                }
                JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
                return cc;
            }
        }
        [HttpGet]
        public JsonResult Paises(string bukrs)
        {
            var c = (from D in db.PAIS
                     where D.SOCIEDAD_ID == bukrs
                     select new { D.LAND, D.LANDX });
            JsonResult cc = Json(c, JsonRequestBehavior.AllowGet);
            return cc;
        }
        [HttpGet]
        public JsonResult selectTaxeo(string bukrs, string pais, string vkorg, string vtweg, string spart, string kunnr, string spras)
        {
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);

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
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);
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
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);
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
            decimal ejer = decimal.Parse(ejercicio);

            var c = (from C in db.CUENTAs
                     where C.SOCIEDAD_ID == bukrs
                     & C.PAIS_ID == land
                     & C.TALL_ID == gall
                     & C.EJERCICIO == ejer
                     //-----DRS 1.10.2018-----
                     select new { C.ABONO, NOMBREA = C.CUENTAGL.NOMBRE, C.CARGO, NOMBREC = C.CUENTAGL1.NOMBRE, C.CLEARING, C.LIMITE }).FirstOrDefault();

            JsonResult cc = Json("", JsonRequestBehavior.AllowGet);
            if (c != null)
            {
                cc = Json(c, JsonRequestBehavior.AllowGet);
            }
            return cc;
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult categoriasCliente(string vkorg, string spart, string kunnr, string soc_id)
        {
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);
            if (kunnr == null)
            {
                kunnr = "";
            }
            List<MATERIALGPT> jd = new List<MATERIALGPT>();

           

            //Validar si hay materiales
            if (db.MATERIALs.Any(x => x.MATERIALGP_ID != null && x.ACTIVO.Value))
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
                    jd = FnCommon.ObtenerMaterialGroupsCliente(db,vkorg,spart,kunnr,soc_id,aii,mii,aff,mff);
                }
            }

            var list = new List<MATERIALGPT>();
            if (jd.Count > 0)
            {
                MATERIALGPT c = FnCommon.ObtenerTotalProducts(db);
                list.Add(new MATERIALGPT
                {
                    MATERIALGP_ID = c.MATERIALGP_ID,
                    TXT50 = c.TXT50
                });
                list.AddRange(jd);
            }
            JsonResult jl = Json(list, JsonRequestBehavior.AllowGet);
            return jl;
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult grupoMateriales(string vkorg, string spart, string kunnr, string soc_id)
        {
         
            if (kunnr == null)
            {
                kunnr = "";
            }

            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);

            List<DOCUMENTOM_MOD> jd = new List<DOCUMENTOM_MOD>();


            //Validar si hay materiales
            if (db.MATERIALs.Any(x => x.MATERIALGP_ID != null && x.ACTIVO.Value))
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
                   jd = FnCommon.ObtenerMaterialGroupsMateriales(db, vkorg, spart, kunnr, soc_id, aii, mii, aff, mff,User.Identity.Name);
                }
            }

            //Obtener las categorías
            var categorias = jd.GroupBy(c => c.ID_CAT, c => new { ID = c.ID_CAT.ToString(), DESC = c.DESC }).ToList();

            List<CategoriaMaterial> lcatmat = new List<CategoriaMaterial>();

            foreach (var item in categorias)
            {
                CategoriaMaterial cm = new CategoriaMaterial();
                cm.ID = item.Key;
                cm.EXCLUIR = jd.Where(x => x.ID_CAT.Equals(item.Key)).FirstOrDefault().EXCLUIR; //RSG 09.07.2018 ID167

                //Obtener los materiales de la categoría
                List<DOCUMENTOM_MOD> dl = new List<DOCUMENTOM_MOD>();
                List<DOCUMENTOM_MOD> dm = new List<DOCUMENTOM_MOD>();
                dl = jd.Where(c => c.ID_CAT == item.Key).Select(c => new DOCUMENTOM_MOD { ID_CAT = c.ID_CAT, MATNR = c.MATNR, VAL = c.VAL, DESC = c.DESC }).ToList();//Falta obtener el groupby

                //Obtener la descripción de los materiales
                foreach (DOCUMENTOM_MOD d in dl)
                {
                    DOCUMENTOM_MOD dcl = new DOCUMENTOM_MOD();
                    dcl = dm.Where(z => z.MATNR == d.MATNR).Select(c => new DOCUMENTOM_MOD { ID_CAT = c.ID_CAT, MATNR = c.MATNR, VAL = c.VAL, DESC = c.DESC }).FirstOrDefault();

                    if (dcl == null)
                    {
                        DOCUMENTOM_MOD dcll = new DOCUMENTOM_MOD();
                        //No se ha agregado
                        decimal val = dl.Where(y => y.MATNR == d.MATNR).Sum(x => x.VAL);
                        dcll.ID_CAT = item.Key;
                        dcll.MATNR = d.MATNR;
                        
                        dcll.DESC = d.DESC;
                        dcll.VAL = val;

                        dm.Add(dcll);
                    }
                }

                cm.MATERIALES = dm;
                //LEJ 18.07.2018-----------------------------------------------------------
                MATERIALGP vv = FnCommon.ObtenerMaterialGroup(db, cm.ID);
                cm.UNICA = vv.UNICA;
                cm.DESCRIPCION = vv.DESCRIPCION;
                lcatmat.Add(cm);
            }

            if (lcatmat.Count > 0)
            {
                CategoriaMaterial nnn = new CategoriaMaterial();
                nnn.ID = "000";
                nnn.DESCRIPCION = FnCommon.ObtenerTotalProducts(db).TXT50;
                nnn.MATERIALES = new List<DOCUMENTOM_MOD>();
                //foreach (var item in lcatmat)//RSG 09.07.2018 ID167
                foreach (var item in lcatmat.Where(x => x.EXCLUIR == false).ToList())
                {
                    foreach (var ii in item.MATERIALES)
                    {
                        DOCUMENTOM_MOD dm = new DOCUMENTOM_MOD();
                        dm.ID_CAT = "000";
                        dm.DESC = ii.DESC;
                        dm.MATNR = ii.MATNR;
                        dm.POR = ii.POR;
                        dm.VAL = ii.VAL;
                        nnn.MATERIALES.Add(dm);
                    }
                }
                //LEJ 18.07.2018-----------------------------------------------------------
                nnn.UNICA = FnCommon.ObtenerMaterialGroup(db, nnn.ID).UNICA;
                lcatmat.Add(nnn);
            }



            JsonResult jl = Json(lcatmat, JsonRequestBehavior.AllowGet);
            return jl;
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
        public JsonResult getPrimerViernes(string ejercicio, string periodo)
        {
            int e = int.Parse(ejercicio);
            int p = int.Parse(periodo);
            Calendario445 c4 = new Calendario445();
            DateTime f = c4.getPrimerDia(e, p);
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)f.DayOfWeek + 7) % 7;
            DateTime nextFridat = f.AddDays(daysUntilFriday);

            JsonResult jl = Json(nextFridat.ToShortDateString(), JsonRequestBehavior.AllowGet);
            return jl;
        }
        [HttpPost]
        public JsonResult getPrimerLunes(string ejercicio, string periodo)
        {
            int e = int.Parse(ejercicio);
            int p = int.Parse(periodo);
            Calendario445 c4 = new Calendario445();
            DateTime f = c4.getPrimerDia(e, p);
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)f.DayOfWeek + 7) % 7;

            DateTime nextMonday = f.AddDays(daysUntilMonday);

            JsonResult jl = Json(nextMonday.ToShortDateString(), JsonRequestBehavior.AllowGet);
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
        public JsonResult SelectCliente(string kunnr,bool? esBorrador)
        {
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);

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
                CANAL canal = db.CANALs.Where(ca => ca.CANAL1 == id_cl.CANAL).FirstOrDefault();
                id_cl.VTWEG = "";

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
            //Si es borrador asignar datos de contacto a cliente
            if (id_cl != null && esBorrador != null && esBorrador.Value)
            {
                DOCUMENTBORR doc = db.DOCUMENTBORRs.Where(x => x.USUARIOC_ID== User.Identity.Name && x.PAYER_ID== kunnr).FirstOrDefault();
                if (doc != null)
                {
                    id_cl.PAYER_EMAIL = doc.PAYER_EMAIL;
                    id_cl.PAYER_NOMBRE = doc.PAYER_NOMBRE;
                }
            }
            //Asignar Manager
            if (id_cl != null && db.CLIENTEFs.Any(x => x.KUNNR == id_cl.KUNNR && x.ACTIVO))
            {
                id_cl.MANAGER = db.CLIENTEFs.Where(x => x.KUNNR == id_cl.KUNNR &&  x.ACTIVO).First().USUARIO1_ID;
            }
            JsonResult jc = Json(id_cl, JsonRequestBehavior.AllowGet);
            return jc;
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult SelectClienteDup(string kunnr, bool? esBorrador, string num_doc)
        {
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);

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
                CANAL canal = db.CANALs.Where(ca => ca.CANAL1 == id_cl.CANAL).FirstOrDefault();
                id_cl.VTWEG = "";

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
            //Si es borrador asignar datos de contacto a cliente
            if (id_cl != null && esBorrador != null && esBorrador.Value)
            {
                DOCUMENTBORR doc = db.DOCUMENTBORRs.Where(x => x.USUARIOC_ID == User.Identity.Name && x.PAYER_ID == kunnr).FirstOrDefault();
                if (doc != null)
                {
                    id_cl.PAYER_EMAIL = doc.PAYER_EMAIL;
                    id_cl.PAYER_NOMBRE = doc.PAYER_NOMBRE;
                }
                else//ADD RSG 01.11.2018
                {
                    DOCUMENTO dp = db.DOCUMENTOes.Find(decimal.Parse(num_doc));
                    id_cl.PAYER_EMAIL = dp.PAYER_EMAIL;
                    id_cl.PAYER_NOMBRE = dp.PAYER_NOMBRE;
                }
            }
            //Asignar Manager
            if (id_cl != null && db.CLIENTEFs.Any(x => x.KUNNR == id_cl.KUNNR && x.ACTIVO))
            {
                id_cl.MANAGER = db.CLIENTEFs.Where(x => x.KUNNR == id_cl.KUNNR && x.ACTIVO).First().USUARIO1_ID;
            }
            JsonResult jc = Json(id_cl, JsonRequestBehavior.AllowGet);
            return jc;
        }

        [HttpPost]
        public JsonResult materiales(string Prefix, string vkorg, string vtweg)
        {
            List<MATERIAL> materiales = FnCommon.ObtenerMateriales(db, Prefix, vkorg, vtweg, User.Identity.Name);
            
            JsonResult cc = new JsonResult()
            {
                Data = materiales,
                MaxJsonLength = Int32.MaxValue
            };
            return cc;
        }

        public string tipoRecurrencia(string tsol)
        {
            string tipo = "";
            var aa = db.TSOLs.Where(a => a.ID.Equals(tsol)).FirstOrDefault();
            if (aa != null)
            {
                tipo = aa.TRECU;
            }
            return tipo;
        }

        //B20180710 MGC 2018.07.13 Modificaciones para editar los campos de distribución se agrego los objetos
        [HttpPost]
        [AllowAnonymous]
        public ActionResult getPartialMat(List<DOCUMENTOP_MOD> docs)
        {

            CartaV doc = new CartaV();

            doc.DOCUMENTOP = docs;
            return PartialView("~/Views/CartaV/_PartialMatTr.cshtml", doc);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult contactos(string Prefix, string vkorg, string vtweg, string kunnr)
        {
            
            Cadena cad = new Cadena();
            kunnr = cad.completaCliente(kunnr);

           var contactos=FnCommon.ObtenerContactos(db,Prefix,vkorg,vtweg,kunnr);
            JsonResult cc = Json(contactos, JsonRequestBehavior.AllowGet);
            return cc;
        }


        [HttpGet]
        public JsonResult tiposSolicitud(string Prefix)
        {
            string spras_id = FnCommon.ObtenerSprasId(db,User.Identity.Name);
            
            var tsr = (from ts in db.TSOLTs
                        where spras_id == ts.SPRAS_ID && (ts.TSOL_ID.Contains(Prefix)|| ts.TXT50.Contains(Prefix))
                        select new { TSOL_ID=ts.TSOL_ID, TXT50=(ts.TSOL_ID+" - "+ts.TXT50) }).ToList();
          
            return Json(tsr, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult soportes(string Prefix)
        {
            string spras_id = FnCommon.ObtenerSprasId(db, User.Identity.Name);

            var c = (from st in db.TSOPORTETs
                     where st.SPRAS_ID == spras_id && (st.TXT50.Contains(Prefix) || st.TSOPORTE_ID.Contains(Prefix))
                     select new { TSOPORTE_ID=st.TSOPORTE_ID, TXT50=(st.TSOPORTE_ID + " - "+st.TXT50) });

            return Json(c, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string cierre(string sociedad_id, string tsol_id, string periodo_id, string usuario_id)
        {
            int periodo = int.Parse(periodo_id);
            bool a = FnCommon.ValidarPeriodoEnCalendario445(db, sociedad_id, tsol_id, periodo, "PRE", usuario_id) ||
                FnCommon.ValidarPeriodoEnCalendario445(db, sociedad_id, tsol_id, periodo, "CI");
            if (a)
                return "X";
            else
                return "";
        }
        [HttpPost]
        public int periodo(string sociedad_id, string tsol_id, string usuario_id)
        {
            return FnCommon.ObtenerPeriodoCalendario445(db,sociedad_id,tsol_id,usuario_id);
        }


        [HttpGet]
        public JsonResult sociedades(string Prefix)
        {
            var c = (from st in db.SOCIEDADs
                     where (st.BUKRS.Contains(Prefix) || st.BUTXT.Contains(Prefix))
                     select new { BUKRS = st.BUKRS, TXT50 = (st.BUKRS + " - " + st.BUTXT) });

            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult paises_(string Prefix)
        {
            var c = (from sp in db.PAIS
                     where (sp.LAND.Contains(Prefix)|| sp.LANDX.Contains(Prefix))
                     select new { LAND = sp.LAND, TXT50 = (sp.LANDX) });

            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult tipoAllowance(string Prefix)
        {
            var c = (from st in db.TALLs
                     where (st.ID.Contains(Prefix)|| st.DESCRIPCION.Contains(Prefix))
                     select new { ID = st.ID, TXT50 = (st.DESCRIPCION) });

            return Json(c, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult cuentas(string Prefix)
        {
            var c = (from st in db.CUENTAGLs
                     where st.ACTIVO==true && (st.ID.ToString().Contains(Prefix)||st.NOMBRE.Contains(Prefix))
                     select new { ID = st.ID, TXT50 = (st.ID + " - " + st.NOMBRE) });

            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult impuestos(string Prefix)
        {
            var c = (from st in db.IMPUESTOes
                     where st.ACTIVO == true && (st.MWSKZ.Contains(Prefix))
                     select new { ID = st.MWSKZ, TXT50 = (st.MWSKZ) });

            return Json(c, JsonRequestBehavior.AllowGet);
        }
    }
}