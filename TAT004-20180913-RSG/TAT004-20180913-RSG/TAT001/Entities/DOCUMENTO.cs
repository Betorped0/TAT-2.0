//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DOCUMENTO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCUMENTO()
        {
            this.CARTAs = new HashSet<CARTA>();
            this.DOCUMENTOAs = new HashSet<DOCUMENTOA>();
            this.DOCUMENTOFs = new HashSet<DOCUMENTOF>();
            this.DOCUMENTOLs = new HashSet<DOCUMENTOL>();
            this.DOCUMENTONs = new HashSet<DOCUMENTON>();
            this.DOCUMENTOPs = new HashSet<DOCUMENTOP>();
            this.DOCUMENTORECs = new HashSet<DOCUMENTOREC>();
            this.DOCUMENTOTS = new HashSet<DOCUMENTOT>();
            this.FLUJOes = new HashSet<FLUJO>();
        }

        public List<TAT001.Models.DOCUMENTOP_MOD> DOCUMENTOP { get; set; }
        public List<DOCUMENTOF> DOCUMENTOF { get; set; }
        public List<DOCUMENTOREC> DOCUMENTOREC { get; set; }
        public List<DOCUMENTORAN> DOCUMENTORAN { get; set; }
        public decimal NUM_DOC { get; set; }
        public string TSOL_ID { get; set; }
        public string TALL_ID { get; set; }
        public string SOCIEDAD_ID { get; set; }
        public string PAIS_ID { get; set; }
        public string ESTADO { get; set; }
        public string CIUDAD { get; set; }
        public Nullable<int> PERIODO { get; set; }
        public string EJERCICIO { get; set; }
        public string TIPO_TECNICO { get; set; }
        public string TIPO_RECURRENTE { get; set; }
        public Nullable<decimal> CANTIDAD_EV { get; set; }
        public string USUARIOC_ID { get; set; }
        public string USUARIOD_ID { get; set; }
        public Nullable<System.DateTime> FECHAD { get; set; }
        public Nullable<System.DateTime> FECHAC { get; set; }
        public Nullable<System.TimeSpan> HORAC { get; set; }
        public Nullable<System.DateTime> FECHAC_PLAN { get; set; }
        public Nullable<System.DateTime> FECHAC_USER { get; set; }
        public Nullable<System.TimeSpan> HORAC_USER { get; set; }
        public string ESTATUS { get; set; }
        public string ESTATUS_C { get; set; }
        public string ESTATUS_SAP { get; set; }
        public string ESTATUS_WF { get; set; }
        public Nullable<decimal> DOCUMENTO_REF { get; set; }
        public string CONCEPTO { get; set; }
        public string NOTAS { get; set; }
        public Nullable<decimal> MONTO_DOC_MD { get; set; }
        public Nullable<decimal> MONTO_FIJO_MD { get; set; }
        public Nullable<decimal> MONTO_BASE_GS_PCT_MD { get; set; }
        public Nullable<decimal> MONTO_BASE_NS_PCT_MD { get; set; }
        public Nullable<decimal> MONTO_DOC_ML { get; set; }
        public Nullable<decimal> MONTO_FIJO_ML { get; set; }
        public Nullable<decimal> MONTO_BASE_GS_PCT_ML { get; set; }
        public Nullable<decimal> MONTO_BASE_NS_PCT_ML { get; set; }
        public Nullable<decimal> MONTO_DOC_ML2 { get; set; }
        public Nullable<decimal> MONTO_FIJO_ML2 { get; set; }
        public Nullable<decimal> MONTO_BASE_GS_PCT_ML2 { get; set; }
        public Nullable<decimal> MONTO_BASE_NS_PCT_ML2 { get; set; }
        public Nullable<decimal> PORC_ADICIONAL { get; set; }
        public string IMPUESTO { get; set; }
        public Nullable<System.DateTime> FECHAI_VIG { get; set; }
        public Nullable<System.DateTime> FECHAF_VIG { get; set; }
        public string ESTATUS_EXT { get; set; }
        public string SOLD_TO_ID { get; set; }
        public string PAYER_ID { get; set; }
        public string PAYER_NOMBRE { get; set; }
        public string PAYER_EMAIL { get; set; }
        public string GRUPO_CTE_ID { get; set; }
        public string CANAL_ID { get; set; }
        public string MONEDA_ID { get; set; }
        public string MONEDAL_ID { get; set; }
        public string MONEDAL2_ID { get; set; }
        public Nullable<decimal> TIPO_CAMBIO { get; set; }
        public Nullable<decimal> TIPO_CAMBIOL { get; set; }
        public Nullable<decimal> TIPO_CAMBIOL2 { get; set; }
        public string NO_FACTURA { get; set; }
        public Nullable<System.DateTime> FECHAD_SOPORTE { get; set; }
        public string METODO_PAGO { get; set; }
        public string NO_PROVEEDOR { get; set; }
        public Nullable<int> PASO_ACTUAL { get; set; }
        public string AGENTE_ACTUAL { get; set; }
        public Nullable<System.DateTime> FECHA_PASO_ACTUAL { get; set; }
        public string VKORG { get; set; }
        public string VTWEG { get; set; }
        public string SPART { get; set; }
        public Nullable<int> PUESTO_ID { get; set; }
        public string GALL_ID { get; set; }
        public Nullable<int> CONCEPTO_ID { get; set; }
        public string DOCUMENTO_SAP { get; set; }
        public Nullable<decimal> PORC_APOYO { get; set; }
        public Nullable<bool> LIGADA { get; set; }
        public Nullable<bool> OBJETIVOQ { get; set; }
        public Nullable<int> FRECUENCIA_LIQ { get; set; }
        public Nullable<decimal> OBJQ_PORC { get; set; }
        public Nullable<decimal> CUENTAP { get; set; }
        public Nullable<decimal> CUENTAPL { get; set; }
        public string EXCEDE_PRES { get; set; }
        public Nullable<decimal> CUENTACL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARTA> CARTAs { get; set; }
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual CUENTAGL CUENTAGL { get; set; }
        public virtual CUENTAGL CUENTAGL1 { get; set; }
        public virtual GALL GALL { get; set; }
        public virtual PAI PAI { get; set; }
        public virtual SOCIEDAD SOCIEDAD { get; set; }
        public virtual TALL TALL { get; set; }
        public virtual TSOL TSOL { get; set; }
        public virtual USUARIO USUARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOA> DOCUMENTOAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOF> DOCUMENTOFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOL> DOCUMENTOLs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTON> DOCUMENTONs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOP> DOCUMENTOPs { get; set; }
        public virtual DOCUMENTOR DOCUMENTOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOREC> DOCUMENTORECs { get; set; }
        public virtual DOCUMENTOSAP DOCUMENTOSAP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTOT> DOCUMENTOTS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FLUJO> FLUJOes { get; set; }
    }
}
