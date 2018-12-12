using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;

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
                    new SelectListItem{Text=(anio).ToString(),Value=(anio).ToString(), Selected=true},
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
        public static MATERIALGP ObtenerMaterialGroup(TAT001Entities db,string materialgp_id)
        {
            return db.MATERIALGPs.Where(x => x.ID == materialgp_id).FirstOrDefault();
        }
        public static MATERIALGPT ObtenerTotalProducts(TAT001Entities db)
        {
            return db.MATERIALGPTs.Where(x => x.MATERIALGP_ID == "000" && x.SPRAS_ID == "EN").FirstOrDefault();
        }

       
       

        public static List<CSP_PRESU_CLIENT_Result> ObtenerPresupuestoCliente(TAT001Entities db, string kunnr,  string periodo)
        {
            List<CSP_PRESU_CLIENT_Result> presupuesto = db.Database.SqlQuery<CSP_PRESU_CLIENT_Result>("CSP_PRESU_CLIENT @CLIENTE, @PERIODO",
            new SqlParameter("@CLIENTE", kunnr),
            new SqlParameter("@PERIODO", periodo)).ToList();
            return presupuesto;
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


        public static List<SelectListItem> ObtenerCmbNivelesA()
        {
            return new List<SelectListItem> {
                    new SelectListItem{Text="Nivel 1",Value="1"},
                    new SelectListItem{Text="Nivel 2",Value="2"},
                    new SelectListItem{Text="Nivel 3",Value="3"},
                    new SelectListItem{Text="Nivel 4",Value="4"},
                    new SelectListItem{Text="Nivel 5",Value="5"}
            };
        }
        public static List<SelectListItem> ObtenerCmbNivel()
        {
            return new List<SelectListItem> {
                    //new SelectListItem{Text="1",Value="1"},
                    new SelectListItem{Text="2",Value="2"},
                    new SelectListItem{Text="3",Value="3"},
                    new SelectListItem{Text="4",Value="4"},
                    new SelectListItem{Text="5",Value="5"}
            };
        }
        public static decimal ObtenerImpuesto(TAT001Entities db, DOCUMENTO D, ref bool esNC, string[] categorias=null)
        {
            decimal impuesto = 0.0M;
            string[] tsolImp = new string[] { "NC", "NCA", "NCAS", "NCAM", "NCASM", "NCS", "NCI", "NCIA", "NCIAS", "NCIS" };
            string pais_id = db.SOCIEDADs.Find(D.SOCIEDAD_ID).LAND;
            if (tsolImp.Contains(D.TSOL_ID))
            {
                decimal KBETR;
                esNC = true;

                if (categorias != null)
                {
                    // Se obtiene Id de la categoria
                    Cadena cad = new Cadena();
                    string[] auxCategorias = categorias;
                    int i = 0;
                    bool esMaterial = false;
                    foreach (string cat in auxCategorias)
                    {
                        string catAux = cad.completaMaterial(cat);
                        if (!esMaterial)
                        {
                            esMaterial = db.MATERIALs.Any(x => x.ID == catAux);
                        }
                        if (esMaterial)
                        {
                            categorias[i] = db.MATERIALs.First(x => x.ID == catAux).MATERIALGP_ID;
                        }
                        
                        i++;
                    }

                }
                if ((categorias!=null && categorias.Any() && (categorias.Contains("605") || categorias.Contains("207"))) || db.DOCUMENTOPs.Any(x => (x.MATKL == "605" || x.MATKL == "207") && x.NUM_DOC == D.NUM_DOC))
                {
                    KBETR = db.IIMPUESTOes.First(x => x.MWSKZ == "A0").KBETR.Value;
                }
                else
                {
                    decimal concecutivo = db.CONPOSAPHs.First(x => x.TIPO_SOL == "NC" && x.SOCIEDAD == D.SOCIEDAD_ID && (x.TIPO_DOC == "YG" || x.TIPO_DOC == "DG")).CONSECUTIVO;
                    string tax_code = db.CONPOSAPPs.First(x => x.CONSECUTIVO == concecutivo).TAX_CODE;
                    KBETR = db.IIMPUESTOes.Any(x => x.MWSKZ == tax_code && x.LAND==pais_id) ? db.IIMPUESTOes.First(x => x.MWSKZ == tax_code && x.LAND == pais_id).KBETR.Value : 0.0M;
                }
                impuesto = (D.MONTO_DOC_MD.Value * KBETR);
            }
            return impuesto;
        }
    }
}