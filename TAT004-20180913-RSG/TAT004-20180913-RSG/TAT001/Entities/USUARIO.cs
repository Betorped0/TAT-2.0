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
    
    public partial class USUARIO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USUARIO()
        {
            this.CALENDARIO_EX = new HashSet<CALENDARIO_EX>();
            this.CARTAs = new HashSet<CARTA>();
            this.DELEGARs = new HashSet<DELEGAR>();
            this.DELEGARs1 = new HashSet<DELEGAR>();
            this.DET_AGENTEC = new HashSet<DET_AGENTEC>();
            this.DET_AGENTEC1 = new HashSet<DET_AGENTEC>();
            this.DET_AGENTEH = new HashSet<DET_AGENTEH>();
            this.DET_AGENTEP = new HashSet<DET_AGENTEP>();
            this.DET_TAXEO = new HashSet<DET_TAXEO>();
            this.DOCUMENTOes = new HashSet<DOCUMENTO>();
            this.FLUJOes = new HashSet<FLUJO>();
            this.FLUJOes1 = new HashSet<FLUJO>();
            this.MIEMBROS = new HashSet<MIEMBRO>();
            this.NOTICIAs = new HashSet<NOTICIA>();
            this.PRESUPSAPHs = new HashSet<PRESUPSAPH>();
            this.PRESUPUESTOHs = new HashSet<PRESUPUESTOH>();
            this.USUARIOFs = new HashSet<USUARIOF>();
            this.GAUTORIZACIONs = new HashSet<GAUTORIZACION>();
        }
    
        public string ID { get; set; }
        public string PASS { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO_P { get; set; }
        public string APELLIDO_M { get; set; }
        public string EMAIL { get; set; }
        public string SPRAS_ID { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
        public Nullable<int> PUESTO_ID { get; set; }
        public string MANAGER { get; set; }
        public string BACKUP_ID { get; set; }
        public string BUNIT { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CALENDARIO_EX> CALENDARIO_EX { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARTA> CARTAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DELEGAR> DELEGARs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DELEGAR> DELEGARs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_AGENTEC> DET_AGENTEC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_AGENTEC> DET_AGENTEC1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_AGENTEH> DET_AGENTEH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_AGENTEP> DET_AGENTEP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_TAXEO> DET_TAXEO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO> DOCUMENTOes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FLUJO> FLUJOes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FLUJO> FLUJOes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MIEMBRO> MIEMBROS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NOTICIA> NOTICIAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRESUPSAPH> PRESUPSAPHs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRESUPUESTOH> PRESUPUESTOHs { get; set; }
        public virtual PUESTO PUESTO { get; set; }
        public virtual SOCIEDAD SOCIEDAD { get; set; }
        public virtual SPRA SPRA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIOF> USUARIOFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GAUTORIZACION> GAUTORIZACIONs { get; set; }
        
        
    }
}
