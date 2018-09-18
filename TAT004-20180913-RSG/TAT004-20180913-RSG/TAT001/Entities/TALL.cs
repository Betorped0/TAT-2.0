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
    
    public partial class TALL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TALL()
        {
            this.CUENTAs = new HashSet<CUENTA>();
            this.DOCUMENTOes = new HashSet<DOCUMENTO>();
            this.TALLTs = new HashSet<TALLT>();
        }
    
        public string ID { get; set; }
        public string DESCRIPCION { get; set; }
        public Nullable<System.DateTime> FECHAI { get; set; }
        public Nullable<System.DateTime> FECHAF { get; set; }
        public string GALL_ID { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CUENTA> CUENTAs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTO> DOCUMENTOes { get; set; }
        public virtual GALL GALL { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TALLT> TALLTs { get; set; }
    }
}
