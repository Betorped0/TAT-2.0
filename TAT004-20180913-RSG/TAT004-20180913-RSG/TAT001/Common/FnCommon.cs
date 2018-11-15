using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Common
{
    public static class FnCommon
    {
        public static USUARIO ObtenerUsuario(TAT001Entities db, string user_id)
        {
            return db.USUARIOs.Where(a => a.ID.Equals(user_id)).FirstOrDefault();
        }
        public static string ObtenerSprasId(TAT001Entities db, string user_id)
        {
            return db.USUARIOs.Where(a => a.ID.Equals(user_id)).FirstOrDefault().SPRAS_ID;
        }
        public static string ObtenerTextoMnj(TAT001Entities db, int pagina_id, string campo_id, string user_id)
        {
            string spras_id = ObtenerSprasId(db, user_id);
            string texto = "";
            if (db.TEXTOes.Any(a => (a.PAGINA_ID.Equals(pagina_id) && a.SPRAS_ID.Equals(spras_id) && a.CAMPO_ID.Equals(campo_id))))
            {
                texto = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina_id) && a.SPRAS_ID.Equals(spras_id) && a.CAMPO_ID.Equals(campo_id))).FirstOrDefault().TEXTOS;
            }
            return texto;
        }
        public static void ObtenerTextos(TAT001Entities db, int pagina_id_textos, string user_id, ControllerBase controller)
        {
            var user = ObtenerUsuario(db, user_id);
            controller.ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina_id_textos) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

        }
        public static void ObtenerConfPage(TAT001Entities db, int pagina_id, string user_id, ControllerBase controller,int? pagina_id_textos=null)
        {
            var user = ObtenerUsuario(db, user_id);
            controller.ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            controller.ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            controller.ViewBag.usuario = user;
            controller.ViewBag.rol = user.PUESTO.PUESTOTs.FirstOrDefault(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).TXT50;
            controller.ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina_id)).FirstOrDefault().PAGINATs.FirstOrDefault(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).TXT50;
            if (pagina_id_textos != null)
            {
                pagina_id = pagina_id_textos.Value;
            }
            controller.ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina_id) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            controller.ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina_id) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            controller.ViewBag.spras_id = user.SPRAS_ID;
        }
        public static List<SelectListItem> ObtenerCmbSociedades(TAT001Entities db, string id)
        {
            return db.SOCIEDADs
                .Where(x => (x.BUKRS == id || id == null) && x.ACTIVO)
                .Select(x => new SelectListItem
                {
                    Value = x.BUKRS,
                    Text = x.BUKRS
                }).ToList();
        }
        public static List<SelectListItem> ObtenerCmbPeriodos(TAT001Entities db, string spras_id, int? id)
        {
            return db.PERIODOes
                .Join(db.PERIODOTs, p => p.ID, pt => pt.PERIODO_ID, (p, pt) => pt)
                .Where(x => x.SPRAS_ID == spras_id && (x.PERIODO_ID == id || id == null))
                .Select(x => new SelectListItem
                {
                    Value = x.PERIODO_ID.ToString(),
                    Text = (x.PERIODO_ID.ToString() + " - " + x.TXT50)
                }).ToList();
        }
        public static List<SelectListItem> ObtenerCmbUsuario(TAT001Entities db, string id)
        {
            return db.USUARIOs
                .Where(x => (x.ID == id || id == null) && (x.ACTIVO != null && x.ACTIVO.Value))
                .Select(x => new SelectListItem
                {
                    Value = x.ID,
                    Text = (x.ID +" - "+x.NOMBRE + " " + x.APELLIDO_P + " " + (x.APELLIDO_M == null ? "" : x.APELLIDO_M))
                }).ToList();
        }
        public static List<SelectListItem> ObtenerCmbTabs(TAT001Entities db, string spras_id,bool? activo, string id)
        {
            return db.TABs.Where(x=>x.TAB_CAMPO.Any(y=>y.ACTIVO == activo.Value || activo == null))
                .Where(x=>(x.ACTIVO==activo.Value || activo==null))
                .Join(db.TEXTOes, ta => ta.ID, te => te.CAMPO_ID, (ta, te) => te)
                .Where(x => x.SPRAS_ID == spras_id && x.PAGINA_ID == 202 && (x.CAMPO_ID == id || id == null))
                .Select(x => new SelectListItem
                {
                    Value = x.CAMPO_ID,
                    Text = x.TEXTOS
                }).ToList();
        }
        public static List<SelectListItem> ObtenerCmbCamposPoTabId(TAT001Entities db, string spras_id, string tab_id,bool? activo, string id)
        {
            return db.TAB_CAMPO
                    .Where(x => x.TAB_ID == tab_id && (x.ACTIVO == activo.Value || activo == null))
                    .Join(db.TEXTOes, tc => tc.CAMPO_ID, te => te.CAMPO_ID, (ta, te) => te)
                    .Where(x => x.SPRAS_ID == spras_id && x.PAGINA_ID == 202 && (x.CAMPO_ID == id || id == null))
                    .Select(x => new SelectListItem
                    {
                        Value = x.CAMPO_ID,
                        Text = x.TEXTOS
                    }).ToList();
        }
        public static List<SelectListItem> ObtenerCmbTiposSolicitud(TAT001Entities db, string spras_id, string id,bool? esReversa = false)
        {
            return  db.TSOLs
                .Where(x => ((esReversa.Value && x.TSOLR == null) || !esReversa.Value) && (id == null || x.ID == id)&& (x.ACTIVO!=null && x.ACTIVO.Value))
                .Join(db.TSOLTs, s => s.ID, st => st.TSOL_ID, (s, st) => st)
                .Where(x => x.SPRAS_ID == spras_id)
                .Select(x => new SelectListItem
                {
                    Value = x.TSOL_ID,
                    Text = (x.TSOL_ID + " - " + x.TXT50)
                }).ToList();
        }
        public static List<SelectTreeItem> ObtenerTreeTiposSolicitud(TAT001Entities db,string sociedad_id, string spras_id, string tipo = null,bool? esReversa=false)
        {
            // tipo
            // SD = Solicitud directa
            // SR = Solicitud relacionada

            List<SelectTreeItem> tree = new List<SelectTreeItem>();
            if (esReversa.Value)
            {
                tree.Add(new SelectTreeItem
                {
                    text = "-",
                    expanded = false,
                    items = ObtenerItemsTSOLT(db, "", "", spras_id, esReversa)
                });
            }
            else
            {
                db.TSOL_GROUP
                    .Where(x => 
                    x.ID_PADRE == null && x.TIPO_PADRE == null 
                    && (tipo == x.TIPO || tipo == null) 
                    && (((sociedad_id=="KCMX" || sociedad_id == "KLCO") && x.ID!= "1_5_OP" && x.ID != "2_5_OP")|| (sociedad_id != "KCMX" && sociedad_id != "KLCO")))
                    .Join(db.TSOL_GROUPT, tg => new { ID = tg.ID, TIPO = tg.TIPO }, tgt => new { ID = tgt.TSOL_GROUP_ID, TIPO = tgt.TSOL_GROUP_TIPO }, (tg, tgt) => tgt)
                    .Where(x => x.SPRAS_ID == spras_id).OrderBy(x => x.TSOL_GROUP_ID)
                    .ToList().ForEach(x =>
                    {
                        SelectTreeItem item = new SelectTreeItem
                        {
                            text = x.TXT50,
                            expanded = false,
                            items = ObtenerItemsSelectTree(db, x.TSOL_GROUP_ID, x.TSOL_GROUP_TIPO, spras_id)
                        };
                        if (item.items.Any())
                        {
                            tree.Add(item);
                        }
                    });
            }
            return tree;
        }
        
        static List<SelectTreeItem> ObtenerItemsSelectTree(TAT001Entities db, string id_padre, string tipo_padre, string spras_id)
        {
            List<SelectTreeItem> items = new List<SelectTreeItem>();
            db.TSOL_GROUP
                .Where(x => x.ID_PADRE == id_padre && x.TIPO_PADRE == tipo_padre)
                .ToList().ForEach(x =>
                {
                    SelectTreeItem item = new SelectTreeItem();
                    item.text = x.DESCRIPCION;
                    item.expanded = false;
                    item.items = ObtenerItemsSelectTree(db, x.ID, x.TIPO, spras_id);
                    if (!item.items.Any())
                    {
                        item.items=ObtenerItemsTSOLT(db, x.ID, x.TIPO, spras_id);
                    }
                    items.Add(item);
                });
            if (!items.Any())
            {
               items = ObtenerItemsTSOLT(db, id_padre, tipo_padre, spras_id);
            }
            return items;
        }
        static List<SelectTreeItem> ObtenerItemsTSOLT(TAT001Entities db, string id_padre, string tipo_padre, string spras_id, bool? esReversa = false)
        {
            List<SelectTreeItem> items = new List<SelectTreeItem>();
            if (esReversa.Value)
            {
                items = db.TSOLs
                    .Where(x=>x.TSOLR==null && (x.ACTIVO != null && x.ACTIVO.Value))
                    .Join(db.TSOLTs, s => s.ID, st => st.TSOL_ID, (s, st) => st)
                    .Where(x=>x.SPRAS_ID == spras_id)
                    .Select(x=> new SelectTreeItem
                    {
                        value = x.TSOL_ID,
                        text = (x.TSOL_ID + " - " + x.TXT50),
                        expanded = true
                    })
                    .ToList();
            }
            else
            {
                db.TSOL_TREE
                           .Where(y => y.TSOL_GROUP_ID == id_padre && y.TSOL_GROUP_TIPO == tipo_padre)
                           .Join(db.TSOLTs, tst => tst.TSOL_ID, st => st.TSOL_ID, (tst, st) => st)
                           .Where(y => y.SPRAS_ID == spras_id && (y.TSOL.ACTIVO != null && y.TSOL.ACTIVO.Value))
                           .ToList().ForEach(y =>
                           {
                               items.Add(new SelectTreeItem
                               {
                                   value = y.TSOL_ID,
                                   text = (y.TSOL_ID + " - " + y.TXT50),
                                   expanded = true
                               });
                           });
            }
            return items;
        }
        public static int ObtenerPeriodoCalendario445(TAT001Entities db, string sociedad_id, string tsol_id, string usuario_id = null)
        {
            //tipo
            //PRE = PreCierre
            //CI =  Cierre
            DateTime fechaActual = DateTime.Now;
            short ejercicio = short.Parse(fechaActual.Year.ToString());
            CALENDARIO_AC calendarioAc = null;
            CALENDARIO_EX calendarioEx = null;

            calendarioAc = db.CALENDARIO_AC.FirstOrDefault(x =>
            x.ACTIVO &&
            x.SOCIEDAD_ID == sociedad_id &&
            x.TSOL_ID == tsol_id &&
            x.EJERCICIO == ejercicio &&
            (fechaActual >= DbFunctions.CreateDateTime(x.PRE_FROMF.Year, x.PRE_FROMF.Month, x.PRE_FROMF.Day, x.PRE_FROMH.Hours, x.PRE_FROMH.Minutes, x.PRE_FROMH.Seconds) &&
             fechaActual <= DbFunctions.CreateDateTime(x.PRE_TOF.Year, x.PRE_TOF.Month, x.PRE_TOF.Day, x.PRE_TOH.Hours, x.PRE_TOH.Minutes, x.PRE_TOH.Seconds)));
            if (calendarioAc!=null)
            {
                return calendarioAc.PERIODO;
            }
            if (usuario_id != null)
            {
                calendarioEx = db.CALENDARIO_EX.FirstOrDefault(x =>
                    x.ACTIVO &&
                    x.SOCIEDAD_ID == sociedad_id &&
                    x.TSOL_ID == tsol_id &&
                    x.USUARIO_ID == usuario_id &&
                    x.EJERCICIO == ejercicio &&
                    (fechaActual >= DbFunctions.CreateDateTime(x.EX_FROMF.Year, x.EX_FROMF.Month, x.EX_FROMF.Day, x.EX_FROMH.Hours, x.EX_FROMH.Minutes, x.EX_FROMH.Seconds) &&
                    fechaActual <= DbFunctions.CreateDateTime(x.EX_TOF.Year, x.EX_TOF.Month, x.EX_TOF.Day, x.EX_TOH.Hours, x.EX_TOH.Minutes, x.EX_TOH.Seconds)));
                if (calendarioEx != null)
                {
                    return calendarioEx.PERIODO;
                }
            }
             
                calendarioAc = db.CALENDARIO_AC.FirstOrDefault(x =>
                x.ACTIVO &&
                x.SOCIEDAD_ID == sociedad_id &&
                x.TSOL_ID == tsol_id &&
                x.EJERCICIO == ejercicio &&
                (fechaActual >= DbFunctions.CreateDateTime(x.CIE_FROMF.Year, x.CIE_FROMF.Month, x.CIE_FROMF.Day, x.CIE_FROMH.Hours, x.CIE_FROMH.Minutes, x.CIE_FROMH.Seconds) &&
                fechaActual <= DbFunctions.CreateDateTime(x.CIE_TOF.Year, x.CIE_TOF.Month, x.CIE_TOF.Day, x.CIE_TOH.Hours, x.CIE_TOH.Minutes, x.CIE_TOH.Seconds)));
            if (calendarioAc != null)
            {
                return calendarioAc.PERIODO;
            }

            return 0;
        }
        public static bool  ValidarPeriodoEnCalendario445(TAT001Entities db,string sociedad_id, string tsol_id,int periodo_id,string tipo, string usuario_id=null)
        {
            //tipo
            //PRE = PreCierre
            //CI =  Cierre
            bool esPeriodoAbierto=false;
            DateTime fechaActual = DateTime.Now;
            short ejercicio = short.Parse(fechaActual.Year.ToString());

            switch (tipo)
            {
                case "PRE":
                    esPeriodoAbierto = db.CALENDARIO_AC.Any(x =>
                    x.ACTIVO &&
                    x.SOCIEDAD_ID == sociedad_id && 
                    x.TSOL_ID == tsol_id && 
                    x.PERIODO == periodo_id &&
                    x.EJERCICIO==ejercicio &&
                    (fechaActual>= DbFunctions.CreateDateTime(x.PRE_FROMF.Year, x.PRE_FROMF.Month, x.PRE_FROMF.Day, x.PRE_FROMH.Hours, x.PRE_FROMH.Minutes, x.PRE_FROMH.Seconds) && 
                     fechaActual<= DbFunctions.CreateDateTime(x.PRE_TOF.Year, x.PRE_TOF.Month, x.PRE_TOF.Day, x.PRE_TOH.Hours, x.PRE_TOH.Minutes, x.PRE_TOH.Seconds)));
                    if (!esPeriodoAbierto && usuario_id != null)
                    {
                        esPeriodoAbierto = db.CALENDARIO_EX.Any(x =>
                          x.ACTIVO &&
                          x.SOCIEDAD_ID == sociedad_id &&
                          x.TSOL_ID == tsol_id &&
                          x.PERIODO == periodo_id &&
                          x.USUARIO_ID==usuario_id &&
                          x.EJERCICIO == ejercicio &&
                          (fechaActual >= DbFunctions.CreateDateTime(x.EX_FROMF.Year, x.EX_FROMF.Month, x.EX_FROMF.Day, x.EX_FROMH.Hours, x.EX_FROMH.Minutes,x.EX_FROMH.Seconds) &&
                          fechaActual <= DbFunctions.CreateDateTime(x.EX_TOF.Year, x.EX_TOF.Month, x.EX_TOF.Day, x.EX_TOH.Hours, x.EX_TOH.Minutes, x.EX_TOH.Seconds)));
                    }
                    break;
                case "CI":
                    esPeriodoAbierto = db.CALENDARIO_AC.Any(x=>
                        x.ACTIVO &&
                        x.SOCIEDAD_ID == sociedad_id &&
                        x.TSOL_ID == tsol_id &&
                        x.PERIODO == periodo_id  &&
                        x.EJERCICIO == ejercicio &&
                        (fechaActual >= DbFunctions.CreateDateTime(x.CIE_FROMF.Year, x.CIE_FROMF.Month, x.CIE_FROMF.Day, x.CIE_FROMH.Hours, x.CIE_FROMH.Minutes, x.CIE_FROMH.Seconds) && 
                        fechaActual <= DbFunctions.CreateDateTime(x.CIE_TOF.Year, x.CIE_TOF.Month, x.CIE_TOF.Day, x.CIE_TOH.Hours, x.CIE_TOH.Minutes, x.CIE_TOH.Seconds)));
                 
                    break;
                default:
                    break;
            }
            return esPeriodoAbierto;
        }
        public static List<SelectListItem> ObtenerCmbEjercicio()
        {
            DateTime fechaActual = DateTime.Now;
            int anio = fechaActual.Year;
            return new List<SelectListItem> {
                    new SelectListItem{Text=(anio).ToString(),Value=(anio).ToString()},
                    new SelectListItem{Text=(anio+1).ToString(),Value=(anio+1).ToString()},
            };
        }
        public static List<SelectListItem> ObtenerCmbPageSize()
        {
            return new List<SelectListItem> {
                    new SelectListItem{Text="10",Value="10"},
                    new SelectListItem{Text="25",Value="25"},
                    new SelectListItem{Text="50",Value="50"},
                    new SelectListItem{Text="100",Value="100"}
            };
        }

        public static List<MATERIAL> ObtenerMateriales(TAT001Entities db,string prefix, string vkorg, string vtweg, string user_id)
        {
            string spras_id = ObtenerSprasId(db, user_id);
            if (prefix == null) { prefix = ""; }

            List<MATERIAL> materiales =  db.Database.SqlQuery<MATERIAL>("CPS_LISTA_MATERIALES @SPRAS_ID,@VKORG,@VTWEG,@PREFIX",
                new SqlParameter("@SPRAS_ID", spras_id),
                new SqlParameter("@VKORG", vkorg),
                new SqlParameter("@VTWEG", vtweg),
                new SqlParameter("@PREFIX", prefix)).ToList();
            
            return materiales;
        }
        public static MATERIAL ObtenerMaterial(TAT001Entities db, string user_id, string material_id)
        {
            string spras_id = ObtenerSprasId(db, user_id);
            MATERIAL material= db.MATERIALs.Where(x => x.ID == material_id).FirstOrDefault();

            if (material.MATERIALTs.Any(x => x.SPRAS == spras_id))
            {
                MATERIALT mt = material.MATERIALTs.First(x => x.SPRAS == spras_id);
                material.MAKTX = mt.MAKTX;
                material.MAKTG = mt.MAKTG;
            }
            return material;


        }
        public static List<MATERIALGP> ObtenerMaterialGroups(TAT001Entities db)
        {
            return db.MATERIALGPs.Where(a => a.ACTIVO).ToList();
        }
        public static List<MATERIALGPT> ObtenerMaterialGroupsCliente(TAT001Entities db, string vkorg, string spart, string kunnr, string soc_id,int aii, int mii, int aff, int mff)
        {
            List<MATERIALGPT> materialgp = db.Database.SqlQuery<MATERIALGPT>("CPS_LISTA_MATERIALGP_CLIENTE @SOCIEDAD_ID,@VKORG,@SPART,@KUNNR,@aii,@mii,@aff,@mff",
               new SqlParameter("@SOCIEDAD_ID", soc_id),
              new SqlParameter("@VKORG", vkorg),
              new SqlParameter("@SPART", spart),
              new SqlParameter("@KUNNR", kunnr),
              new SqlParameter("@aii", aii),
              new SqlParameter("@mii",mii ),
              new SqlParameter("@aff",aff ),
              new SqlParameter("@mff", mff)).ToList();
            return materialgp;
        }
        public static List<DOCUMENTOM_MOD> ObtenerMaterialGroupsMateriales(TAT001Entities db, string vkorg, string spart, string kunnr, string soc_id, int aii, int mii, int aff, int mff,string user_id)
        {
            string spras_id = ObtenerSprasId(db, user_id);
            List<DOCUMENTOM_MOD> materialgp = db.Database.SqlQuery<DOCUMENTOM_MOD>("CPS_LISTA_MATERIALGP_MATERIALES @SOCIEDAD_ID,@VKORG,@SPART,@KUNNR,@SPRAS_ID,@aii,@mii,@aff,@mff",
              new SqlParameter("@SOCIEDAD_ID", soc_id),
              new SqlParameter("@VKORG", vkorg),
              new SqlParameter("@SPART", spart),
              new SqlParameter("@KUNNR", kunnr),
              new SqlParameter("@SPRAS_ID", spras_id),
              new SqlParameter("@aii", aii),
              new SqlParameter("@mii", mii),
              new SqlParameter("@aff", aff),
              new SqlParameter("@mff", mff)).ToList();
            return materialgp;
        }
        public static MATERIALGP ObtenerMaterialGroup(TAT001Entities db,string materialgp_id)
        {
            return db.MATERIALGPs.Where(x => x.ID == materialgp_id).FirstOrDefault();
        }
        public static MATERIALGPT ObtenerTotalProducts(TAT001Entities db)
        {
            return db.MATERIALGPTs.Where(x => x.MATERIALGP_ID == "000" && x.SPRAS_ID == "EN").FirstOrDefault();
        }

        public static List<CLIENTE> ObtenerClientes(TAT001Entities db, string prefix, string usuario_id, string pais)
        {
            if (prefix==null) { prefix = ""; }

            List<object> paramsCSP = new List<object>();

            if (usuario_id != null){ paramsCSP.Add(new SqlParameter("@USUARIO_ID", usuario_id));}
            else{ paramsCSP.Add(new SqlParameter("@USUARIO_ID", DBNull.Value));}

            if (pais != null){ paramsCSP.Add(new SqlParameter("@PAIS", pais));}
            else {paramsCSP.Add(new SqlParameter("@PAIS", DBNull.Value));}

            paramsCSP.Add(new SqlParameter("@PREFIX", prefix));
           
            List<CLIENTE> clientes = db.Database.SqlQuery<CLIENTE>("CPS_LISTA_CLIENTES @USUARIO_ID,@PAIS,@PREFIX",
            paramsCSP.ToArray()).ToList();
            return clientes;
        }

        public static List<CONTACTOC> ObtenerContactos(TAT001Entities db, string prefix, string vkorg, string vtweg, string kunnr)
        {
            if (prefix == null) { prefix = ""; }

            List<CONTACTOC> contactos = db.Database.SqlQuery<CONTACTOC>("CPS_LISTA_CONTACTOS @KUNNR,@VKORG,@VTWEG,@PREFIX",
            new SqlParameter("@KUNNR", kunnr),
            new SqlParameter("@VKORG", vkorg),
            new SqlParameter("@VTWEG", vtweg),
            new SqlParameter("@PREFIX", prefix)).ToList();
            return contactos;
        }

        public static List<CSP_PRESU_CLIENT_Result> ObtenerPresupuestoCliente(TAT001Entities db, string kunnr,  string periodo)
        {
            List<CSP_PRESU_CLIENT_Result> presupuesto = db.Database.SqlQuery<CSP_PRESU_CLIENT_Result>("CSP_PRESU_CLIENT @CLIENTE, @PERIODO",
            new SqlParameter("@CLIENTE", kunnr),
            new SqlParameter("@PERIODO", periodo)).ToList();
            return presupuesto;
        }

        public static List<DOCUMENTO> ObtenerSolicitudes(TAT001Entities db, 
            string prefix, 
            decimal? num_doci,decimal? num_docf,
            DateTime? fechai,DateTime? fechaf,
            string kunnr,string usuario_id,
            decimal? num_doc=null)
        {
            if (prefix == null) { prefix = ""; }
            List<object> paramsCSP = new List<object>();

            if (num_doci != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCI", num_doci)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCI", DBNull.Value)); }

            if (num_docf != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCF", num_docf)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCF", DBNull.Value)); }

            if (num_doci != null) { paramsCSP.Add(new SqlParameter("@FECHAI", fechai)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAI", DBNull.Value)); }

            if (num_docf != null) { paramsCSP.Add(new SqlParameter("@FECHAF", fechaf)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAF", DBNull.Value)); }

            if (num_doci != null) { paramsCSP.Add(new SqlParameter("@KUNNR", kunnr)); }
            else { paramsCSP.Add(new SqlParameter("@KUNNR", DBNull.Value)); }

            if (num_docf != null) { paramsCSP.Add(new SqlParameter("@USUARIO_ID", usuario_id)); }
            else { paramsCSP.Add(new SqlParameter("@USUARIO_ID", DBNull.Value)); }

            if (num_doc != null) { paramsCSP.Add(new SqlParameter("@NUM_DOC", num_doc)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOC", DBNull.Value)); }

            paramsCSP.Add(new SqlParameter("@PREFIX", prefix));


            List<DOCUMENTO> solicitudes = db.Database.SqlQuery<DOCUMENTO>("CPS_LISTA_SOLICITUDES @PREFIX,@NUM_DOCI,@NUM_DOCF,@NUM_DOC",
            paramsCSP.ToArray()).ToList();
            return solicitudes;

        }
        public static List<USUARIO> ObtenerUsuarios(TAT001Entities db, string prefix)
        {
            if (prefix == null) { prefix = ""; }

            List<USUARIO> usuarios = db.Database.SqlQuery<USUARIO>("CPS_LISTA_USUARIOS @PREFIX",
                 new SqlParameter("@PREFIX", prefix)).ToList();
            return usuarios;
        }

        public static List<DOCUMENTOP_SP> ObtenerDocumentoP(TAT001Entities db, string spras_id, decimal num_doc, DateTime vigencia_de, DateTime vigencia_al)
        {
            List<DOCUMENTOP_SP> documentop = db.Database.SqlQuery<DOCUMENTOP_SP>("CPS_LISTA_DOCUMENTOP @NUM_DOC,@SPRAS_ID,@VIGENCIA_DE,@VIGENCIA_AL",
                new SqlParameter("@NUM_DOC", num_doc),
                new SqlParameter("@SPRAS_ID", spras_id),
                new SqlParameter("@VIGENCIA_DE", vigencia_de),
                new SqlParameter("@VIGENCIA_AL", vigencia_al)).ToList();

            return documentop;
        }

        public static List<SolicitudPorAprobar> ObtenerSolicitudesPorAprobar(TAT001Entities db,
           decimal? num_doci, decimal? num_docf,
           DateTime? fechai, DateTime? fechaf,
           string kunnr, string usuarioa_id,
           decimal? num_doc = null)
        {
            List<object> paramsCSP = new List<object>();

            if (num_doci != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCI", num_doci)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCI", DBNull.Value)); }

            if (num_docf != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCF", num_docf)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCF", DBNull.Value)); }

            if (fechai != null) { paramsCSP.Add(new SqlParameter("@FECHAI", fechai)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAI", DBNull.Value)); }

            if (fechaf != null) { paramsCSP.Add(new SqlParameter("@FECHAF", fechaf)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAF", DBNull.Value)); }

            if (kunnr != null) { paramsCSP.Add(new SqlParameter("@KUNNR", kunnr)); }
            else { paramsCSP.Add(new SqlParameter("@KUNNR", DBNull.Value)); }

            if (usuarioa_id != null) { paramsCSP.Add(new SqlParameter("@USUARIOA_ID", usuarioa_id)); }
            else { paramsCSP.Add(new SqlParameter("@USUARIOA_ID", DBNull.Value)); }
            
            List<SolicitudPorAprobar> solicitudes = db.Database.SqlQuery<SolicitudPorAprobar>("CPS_LISTA_SOLICITUDES_POR_APROBAR @NUM_DOCI,@NUM_DOCF@FECHAI,@FECHAF,@KUNNR,@USUARIOA_ID",
            paramsCSP.ToArray()).ToList();
            return solicitudes;

        }
    }
}