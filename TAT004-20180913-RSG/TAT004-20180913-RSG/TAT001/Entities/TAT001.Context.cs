﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAT001.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TAT001Entities : DbContext
    {
        public TAT001Entities()
            : base("name=TAT001Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ACCION> ACCIONs { get; set; }
        public virtual DbSet<ACCIONT> ACCIONTs { get; set; }
        public virtual DbSet<APPSETTING> APPSETTINGs { get; set; }
        public virtual DbSet<CALENDARIO_AC> CALENDARIO_AC { get; set; }
        public virtual DbSet<CALENDARIO_EX> CALENDARIO_EX { get; set; }
        public virtual DbSet<CAMPOS> CAMPOS { get; set; }
        public virtual DbSet<CAMPOZKE24> CAMPOZKE24 { get; set; }
        public virtual DbSet<CAMPOZKE24T> CAMPOZKE24T { get; set; }
        public virtual DbSet<CANAL> CANALs { get; set; }
        public virtual DbSet<CARPETA> CARPETAs { get; set; }
        public virtual DbSet<CARPETAT> CARPETATs { get; set; }
        public virtual DbSet<CARTA> CARTAs { get; set; }
        public virtual DbSet<CARTAP> CARTAPs { get; set; }
        public virtual DbSet<CATEGORIA> CATEGORIAs { get; set; }
        public virtual DbSet<CATEGORIAT> CATEGORIATs { get; set; }
        public virtual DbSet<CITy> CITIES { get; set; }
        public virtual DbSet<CLIENTE> CLIENTEs { get; set; }
        public virtual DbSet<CLIENTEF> CLIENTEFs { get; set; }
        public virtual DbSet<CLIENTEI> CLIENTEIs { get; set; }
        public virtual DbSet<CONDICION> CONDICIONs { get; set; }
        public virtual DbSet<CONFDIST_CAT> CONFDIST_CAT { get; set; }
        public virtual DbSet<CONMAIL> CONMAILs { get; set; }
        public virtual DbSet<CONPOSAPH> CONPOSAPHs { get; set; }
        public virtual DbSet<CONPOSAPP> CONPOSAPPs { get; set; }
        public virtual DbSet<CONSOPORTE> CONSOPORTEs { get; set; }
        public virtual DbSet<CONTACTOC> CONTACTOCs { get; set; }
        public virtual DbSet<COUNTRy> COUNTRIES { get; set; }
        public virtual DbSet<CUENTA> CUENTAs { get; set; }
        public virtual DbSet<CUENTAGL> CUENTAGLs { get; set; }
        public virtual DbSet<DELEGAR> DELEGARs { get; set; }
        public virtual DbSet<DET_AGENTE> DET_AGENTE { get; set; }
        public virtual DbSet<DET_AGENTEC> DET_AGENTEC { get; set; }
        public virtual DbSet<DET_AGENTEH> DET_AGENTEH { get; set; }
        public virtual DbSet<DET_AGENTEP> DET_AGENTEP { get; set; }
        public virtual DbSet<DET_APROB> DET_APROB { get; set; }
        public virtual DbSet<DET_APROBH> DET_APROBH { get; set; }
        public virtual DbSet<DET_APROBP> DET_APROBP { get; set; }
        public virtual DbSet<DET_TAX> DET_TAX { get; set; }
        public virtual DbSet<DET_TAXEO> DET_TAXEO { get; set; }
        public virtual DbSet<DET_TAXEOC> DET_TAXEOC { get; set; }
        public virtual DbSet<DOCUMENTBORR> DOCUMENTBORRs { get; set; }
        public virtual DbSet<DOCUMENTO> DOCUMENTOes { get; set; }
        public virtual DbSet<DOCUMENTOA> DOCUMENTOAs { get; set; }
        public virtual DbSet<DOCUMENTOBORRF> DOCUMENTOBORRFs { get; set; }
        public virtual DbSet<DOCUMENTOBORRM> DOCUMENTOBORRMs { get; set; }
        public virtual DbSet<DOCUMENTOBORRN> DOCUMENTOBORRNs { get; set; }
        public virtual DbSet<DOCUMENTOBORRP> DOCUMENTOBORRPs { get; set; }
        public virtual DbSet<DOCUMENTOBORRREC> DOCUMENTOBORRRECs { get; set; }
        public virtual DbSet<DOCUMENTOF> DOCUMENTOFs { get; set; }
        public virtual DbSet<DOCUMENTOL> DOCUMENTOLs { get; set; }
        public virtual DbSet<DOCUMENTOM> DOCUMENTOMs { get; set; }
        public virtual DbSet<DOCUMENTON> DOCUMENTONs { get; set; }
        public virtual DbSet<DOCUMENTOP> DOCUMENTOPs { get; set; }
        public virtual DbSet<DOCUMENTOR> DOCUMENTORs { get; set; }
        public virtual DbSet<DOCUMENTORAN> DOCUMENTORANs { get; set; }
        public virtual DbSet<DOCUMENTOREC> DOCUMENTORECs { get; set; }
        public virtual DbSet<DOCUMENTOSAP> DOCUMENTOSAPs { get; set; }
        public virtual DbSet<DOCUMENTOT> DOCUMENTOTS { get; set; }
        public virtual DbSet<FACTURASCONF> FACTURASCONFs { get; set; }
        public virtual DbSet<FLUJNEGO> FLUJNEGOes { get; set; }
        public virtual DbSet<FLUJO> FLUJOes { get; set; }
        public virtual DbSet<GALL> GALLs { get; set; }
        public virtual DbSet<GALLT> GALLTs { get; set; }
        public virtual DbSet<GAUTORIZACION> GAUTORIZACIONs { get; set; }
        public virtual DbSet<IIMPUESTO> IIMPUESTOes { get; set; }
        public virtual DbSet<IMPUESTO> IMPUESTOes { get; set; }
        public virtual DbSet<LEYENDA> LEYENDAs { get; set; }
        public virtual DbSet<MATERIAL> MATERIALs { get; set; }
        public virtual DbSet<MATERIALGP> MATERIALGPs { get; set; }
        public virtual DbSet<MATERIALGPT> MATERIALGPTs { get; set; }
        public virtual DbSet<MATERIALT> MATERIALTs { get; set; }
        public virtual DbSet<MATERIALVKE> MATERIALVKEs { get; set; }
        public virtual DbSet<MENSAJE> MENSAJES { get; set; }
        public virtual DbSet<MIEMBRO> MIEMBROS { get; set; }
        public virtual DbSet<MONEDA> MONEDAs { get; set; }
        public virtual DbSet<NEGOCIACION> NEGOCIACIONs { get; set; }
        public virtual DbSet<NOTICIA> NOTICIAs { get; set; }
        public virtual DbSet<PAGINA> PAGINAs { get; set; }
        public virtual DbSet<PAGINAT> PAGINATs { get; set; }
        public virtual DbSet<PAI> PAIS { get; set; }
        public virtual DbSet<PERIODO> PERIODOes { get; set; }
        public virtual DbSet<PERIODO445> PERIODO445 { get; set; }
        public virtual DbSet<PERIODOT> PERIODOTs { get; set; }
        public virtual DbSet<PERMISO_PAGINA> PERMISO_PAGINA { get; set; }
        public virtual DbSet<POSICION> POSICIONs { get; set; }
        public virtual DbSet<PRESUPSAPH> PRESUPSAPHs { get; set; }
        public virtual DbSet<PRESUPSAPP> PRESUPSAPPs { get; set; }
        public virtual DbSet<PRESUPUESTOH> PRESUPUESTOHs { get; set; }
        public virtual DbSet<PRESUPUESTOP> PRESUPUESTOPs { get; set; }
        public virtual DbSet<PROVEEDOR> PROVEEDORs { get; set; }
        public virtual DbSet<PUESTO> PUESTOes { get; set; }
        public virtual DbSet<PUESTOT> PUESTOTs { get; set; }
        public virtual DbSet<RANGO> RANGOes { get; set; }
        public virtual DbSet<REGION> REGIONs { get; set; }
        public virtual DbSet<RETENCION> RETENCIONs { get; set; }
        public virtual DbSet<RETENCIONT> RETENCIONTs { get; set; }
        public virtual DbSet<ROL> ROLs { get; set; }
        public virtual DbSet<ROLT> ROLTs { get; set; }
        public virtual DbSet<SOCIEDAD> SOCIEDADs { get; set; }
        public virtual DbSet<SPRA> SPRAS { get; set; }
        public virtual DbSet<STATE> STATES { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TAB> TABs { get; set; }
        public virtual DbSet<TALL> TALLs { get; set; }
        public virtual DbSet<TALLT> TALLTs { get; set; }
        public virtual DbSet<TAX_LAND> TAX_LAND { get; set; }
        public virtual DbSet<TAXEOH> TAXEOHs { get; set; }
        public virtual DbSet<TAXEOP> TAXEOPs { get; set; }
        public virtual DbSet<TCAMBIO> TCAMBIOs { get; set; }
        public virtual DbSet<TCLIENTE> TCLIENTEs { get; set; }
        public virtual DbSet<TCLIENTET> TCLIENTETs { get; set; }
        public virtual DbSet<TEXTO> TEXTOes { get; set; }
        public virtual DbSet<TEXTOCV> TEXTOCVs { get; set; }
        public virtual DbSet<TRETENCION> TRETENCIONs { get; set; }
        public virtual DbSet<TRETENCIONT> TRETENCIONTs { get; set; }
        public virtual DbSet<TREVERSA> TREVERSAs { get; set; }
        public virtual DbSet<TREVERSAT> TREVERSATs { get; set; }
        public virtual DbSet<TS_CAMPO> TS_CAMPO { get; set; }
        public virtual DbSet<TS_FORM> TS_FORM { get; set; }
        public virtual DbSet<TS_FORMT> TS_FORMT { get; set; }
        public virtual DbSet<TSOL> TSOLs { get; set; }
        public virtual DbSet<TSOLT> TSOLTs { get; set; }
        public virtual DbSet<TSOPORTE> TSOPORTEs { get; set; }
        public virtual DbSet<TSOPORTET> TSOPORTETs { get; set; }
        public virtual DbSet<TX_CONCEPTO> TX_CONCEPTO { get; set; }
        public virtual DbSet<TX_CONCEPTOT> TX_CONCEPTOT { get; set; }
        public virtual DbSet<TX_NOTAT> TX_NOTAT { get; set; }
        public virtual DbSet<TX_TNOTA> TX_TNOTA { get; set; }
        public virtual DbSet<UMEDIDA> UMEDIDAs { get; set; }
        public virtual DbSet<UMEDIDAT> UMEDIDATs { get; set; }
        public virtual DbSet<USUARIO> USUARIOs { get; set; }
        public virtual DbSet<USUARIOF> USUARIOFs { get; set; }
        public virtual DbSet<USUARIOSAP> USUARIOSAPs { get; set; }
        public virtual DbSet<WARNING> WARNINGs { get; set; }
        public virtual DbSet<WARNING_COND> WARNING_COND { get; set; }
        public virtual DbSet<WARNINGP> WARNINGPs { get; set; }
        public virtual DbSet<WARNINGPT> WARNINGPTs { get; set; }
        public virtual DbSet<WORKFH> WORKFHs { get; set; }
        public virtual DbSet<WORKFP> WORKFPs { get; set; }
        public virtual DbSet<WORKFT> WORKFTs { get; set; }
        public virtual DbSet<WORKFV> WORKFVs { get; set; }
        public virtual DbSet<ZBRAND> ZBRANDs { get; set; }
        public virtual DbSet<ZCTGR> ZCTGRs { get; set; }
        public virtual DbSet<CARPETAV> CARPETAVs { get; set; }
        public virtual DbSet<CREADOR> CREADORs { get; set; }
        public virtual DbSet<CREADOR2> CREADOR2 { get; set; }
        public virtual DbSet<DET_APROBV> DET_APROBV { get; set; }
        public virtual DbSet<DOCUMENTOV> DOCUMENTOVs { get; set; }
        public virtual DbSet<PAGINAV> PAGINAVs { get; set; }
        public virtual DbSet<WARNINGV> WARNINGVs { get; set; }
    
        [DbFunction("TAT001Entities", "split")]
        public virtual IQueryable<split_Result> split(string delimited, string delimiter)
        {
            var delimitedParameter = delimited != null ?
                new ObjectParameter("delimited", delimited) :
                new ObjectParameter("delimited", typeof(string));
    
            var delimiterParameter = delimiter != null ?
                new ObjectParameter("delimiter", delimiter) :
                new ObjectParameter("delimiter", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<split_Result>("[TAT001Entities].[split](@delimited, @delimiter)", delimitedParameter, delimiterParameter);
        }
    
        public virtual ObjectResult<CPS_LISTA_CLI_PRO_Result> CPS_LISTA_CLI_PRO()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CPS_LISTA_CLI_PRO_Result>("CPS_LISTA_CLI_PRO");
        }
    
        public virtual ObjectResult<string> CSP_BANNERSINCANAL()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("CSP_BANNERSINCANAL");
        }
    
        public virtual ObjectResult<CSP_CAMBIO_Result> CSP_CAMBIO(string sociedad)
        {
            var sociedadParameter = sociedad != null ?
                new ObjectParameter("sociedad", sociedad) :
                new ObjectParameter("sociedad", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_CAMBIO_Result>("CSP_CAMBIO", sociedadParameter);
        }
    
        public virtual ObjectResult<CSP_CARPETA_Result> CSP_CARPETA(string iD, Nullable<int> aCCION)
        {
            var iDParameter = iD != null ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(string));
    
            var aCCIONParameter = aCCION.HasValue ?
                new ObjectParameter("ACCION", aCCION) :
                new ObjectParameter("ACCION", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_CARPETA_Result>("CSP_CARPETA", iDParameter, aCCIONParameter);
        }
    
        public virtual ObjectResult<CSP_CONSULTARPRESUPUESTO_Result> CSP_CONSULTARPRESUPUESTO(string sOCIEDAD, string aNIOC, string aNIOS, string pERIODOC, string pERIODOS, string mONEDAD, string mONEDAA, string cPT)
        {
            var sOCIEDADParameter = sOCIEDAD != null ?
                new ObjectParameter("SOCIEDAD", sOCIEDAD) :
                new ObjectParameter("SOCIEDAD", typeof(string));
    
            var aNIOCParameter = aNIOC != null ?
                new ObjectParameter("ANIOC", aNIOC) :
                new ObjectParameter("ANIOC", typeof(string));
    
            var aNIOSParameter = aNIOS != null ?
                new ObjectParameter("ANIOS", aNIOS) :
                new ObjectParameter("ANIOS", typeof(string));
    
            var pERIODOCParameter = pERIODOC != null ?
                new ObjectParameter("PERIODOC", pERIODOC) :
                new ObjectParameter("PERIODOC", typeof(string));
    
            var pERIODOSParameter = pERIODOS != null ?
                new ObjectParameter("PERIODOS", pERIODOS) :
                new ObjectParameter("PERIODOS", typeof(string));
    
            var mONEDADParameter = mONEDAD != null ?
                new ObjectParameter("MONEDAD", mONEDAD) :
                new ObjectParameter("MONEDAD", typeof(string));
    
            var mONEDAAParameter = mONEDAA != null ?
                new ObjectParameter("MONEDAA", mONEDAA) :
                new ObjectParameter("MONEDAA", typeof(string));
    
            var cPTParameter = cPT != null ?
                new ObjectParameter("CPT", cPT) :
                new ObjectParameter("CPT", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_CONSULTARPRESUPUESTO_Result>("CSP_CONSULTARPRESUPUESTO", sOCIEDADParameter, aNIOCParameter, aNIOSParameter, pERIODOCParameter, pERIODOSParameter, mONEDADParameter, mONEDAAParameter, cPTParameter);
        }
    
        public virtual ObjectResult<CSP_DOCUMENTOSXUSER_Result> CSP_DOCUMENTOSXUSER(string uSUARIO, string sPRAS)
        {
            var uSUARIOParameter = uSUARIO != null ?
                new ObjectParameter("USUARIO", uSUARIO) :
                new ObjectParameter("USUARIO", typeof(string));
    
            var sPRASParameter = sPRAS != null ?
                new ObjectParameter("SPRAS", sPRAS) :
                new ObjectParameter("SPRAS", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_DOCUMENTOSXUSER_Result>("CSP_DOCUMENTOSXUSER", uSUARIOParameter, sPRASParameter);
        }
    
        public virtual ObjectResult<CSP_PERMISO_Result> CSP_PERMISO(string iD, Nullable<int> aCCION)
        {
            var iDParameter = iD != null ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(string));
    
            var aCCIONParameter = aCCION.HasValue ?
                new ObjectParameter("ACCION", aCCION) :
                new ObjectParameter("ACCION", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_PERMISO_Result>("CSP_PERMISO", iDParameter, aCCIONParameter);
        }
    
        public virtual ObjectResult<CSP_PRESU_CLIENT_Result> CSP_PRESU_CLIENT(string cLIENTE, string pERIODO)
        {
            var cLIENTEParameter = cLIENTE != null ?
                new ObjectParameter("CLIENTE", cLIENTE) :
                new ObjectParameter("CLIENTE", typeof(string));
    
            var pERIODOParameter = pERIODO != null ?
                new ObjectParameter("PERIODO", pERIODO) :
                new ObjectParameter("PERIODO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_PRESU_CLIENT_Result>("CSP_PRESU_CLIENT", cLIENTEParameter, pERIODOParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> CSP_PRESUPUESTO_ADD(Nullable<int> anio, string sociedad, string periodo, string usuario_id, string auto, Nullable<int> caso)
        {
            var anioParameter = anio.HasValue ?
                new ObjectParameter("anio", anio) :
                new ObjectParameter("anio", typeof(int));
    
            var sociedadParameter = sociedad != null ?
                new ObjectParameter("sociedad", sociedad) :
                new ObjectParameter("sociedad", typeof(string));
    
            var periodoParameter = periodo != null ?
                new ObjectParameter("periodo", periodo) :
                new ObjectParameter("periodo", typeof(string));
    
            var usuario_idParameter = usuario_id != null ?
                new ObjectParameter("usuario_id", usuario_id) :
                new ObjectParameter("usuario_id", typeof(string));
    
            var autoParameter = auto != null ?
                new ObjectParameter("auto", auto) :
                new ObjectParameter("auto", typeof(string));
    
            var casoParameter = caso.HasValue ?
                new ObjectParameter("caso", caso) :
                new ObjectParameter("caso", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("CSP_PRESUPUESTO_ADD", anioParameter, sociedadParameter, periodoParameter, usuario_idParameter, autoParameter, casoParameter);
        }
    
        public virtual ObjectResult<CSP_PRESUPUESTO_ADDP_Result> CSP_PRESUPUESTO_ADDP(string path)
        {
            var pathParameter = path != null ?
                new ObjectParameter("path", path) :
                new ObjectParameter("path", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_PRESUPUESTO_ADDP_Result>("CSP_PRESUPUESTO_ADDP", pathParameter);
        }
    
        public virtual ObjectResult<CSP_USUARIO_Result> CSP_USUARIO(string iD, string pASS, string nOMBRE, string aPELLIDO_P, string aPELLIDO_M, string eMAIL, string sPRAS_ID, Nullable<bool> aCTIVO, Nullable<int> aCCION)
        {
            var iDParameter = iD != null ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(string));
    
            var pASSParameter = pASS != null ?
                new ObjectParameter("PASS", pASS) :
                new ObjectParameter("PASS", typeof(string));
    
            var nOMBREParameter = nOMBRE != null ?
                new ObjectParameter("NOMBRE", nOMBRE) :
                new ObjectParameter("NOMBRE", typeof(string));
    
            var aPELLIDO_PParameter = aPELLIDO_P != null ?
                new ObjectParameter("APELLIDO_P", aPELLIDO_P) :
                new ObjectParameter("APELLIDO_P", typeof(string));
    
            var aPELLIDO_MParameter = aPELLIDO_M != null ?
                new ObjectParameter("APELLIDO_M", aPELLIDO_M) :
                new ObjectParameter("APELLIDO_M", typeof(string));
    
            var eMAILParameter = eMAIL != null ?
                new ObjectParameter("EMAIL", eMAIL) :
                new ObjectParameter("EMAIL", typeof(string));
    
            var sPRAS_IDParameter = sPRAS_ID != null ?
                new ObjectParameter("SPRAS_ID", sPRAS_ID) :
                new ObjectParameter("SPRAS_ID", typeof(string));
    
            var aCTIVOParameter = aCTIVO.HasValue ?
                new ObjectParameter("ACTIVO", aCTIVO) :
                new ObjectParameter("ACTIVO", typeof(bool));
    
            var aCCIONParameter = aCCION.HasValue ?
                new ObjectParameter("ACCION", aCCION) :
                new ObjectParameter("ACCION", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CSP_USUARIO_Result>("CSP_USUARIO", iDParameter, pASSParameter, nOMBREParameter, aPELLIDO_PParameter, aPELLIDO_MParameter, eMAILParameter, sPRAS_IDParameter, aCTIVOParameter, aCCIONParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
