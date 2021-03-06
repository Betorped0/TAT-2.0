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
    
    public partial class PAI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PAI()
        {
            this.CLIENTEs = new HashSet<CLIENTE>();
            this.CUENTAs = new HashSet<CUENTA>();
            this.DET_AGENTEC = new HashSet<DET_AGENTEC>();
            this.DOCUMENTOes = new HashSet<DOCUMENTO>();
            this.IIMPUESTOes = new HashSet<IIMPUESTO>();
            this.LEYENDAs = new HashSet<LEYENDA>();
            this.TAX_LAND = new HashSet<TAX_LAND>();
            this.TAXEOHs = new HashSet<TAXEOH>();
            this.TS_FORM = new HashSet<TS_FORM>();
        }
    
        public string LAND { get; set; }
        public string SPRAS { get; set; }
        public string LANDX { get; set; }
        public string IMAGE { get; set; }
        public bool ACTIVO { get; set; }
        public string SOCIEDAD_ID { get; set; }
        public string DECIMAL { get; set; }
        public string MILES { get; set; }
        public bool BACKORDER { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CLIENTE> CLIENTEs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CUENTA> CUENTAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DET_AGENTEC> DET_AGENTEC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO> DOCUMENTOes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IIMPUESTO> IIMPUESTOes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LEYENDA> LEYENDAs { get; set; }
        public virtual SOCIEDAD SOCIEDAD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAX_LAND> TAX_LAND { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TAXEOH> TAXEOHs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TS_FORM> TS_FORM { get; set; }
    }
}
