//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TAT001.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class DOCUMENTOREC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCUMENTOREC()
        {
            this.DOCUMENTORANs = new HashSet<DOCUMENTORAN>();
        }
    
        public decimal NUM_DOC { get; set; }
        public int POS { get; set; }
        public Nullable<System.DateTime> FECHAF { get; set; }
        public Nullable<int> PERIODO { get; set; }
        public Nullable<int> EJERCICIO { get; set; }
        public Nullable<decimal> MONTO_BASE { get; set; }
        public Nullable<decimal> MONTO_FIJO { get; set; }
        public Nullable<decimal> MONTO_GRS { get; set; }
        public Nullable<decimal> MONTO_NET { get; set; }
        public string ESTATUS { get; set; }
        public Nullable<decimal> PORC { get; set; }
        public Nullable<decimal> DOC_REF { get; set; }
        public Nullable<System.DateTime> FECHAV { get; set; }
        public Nullable<decimal> NUM_DOC_Q { get; set; }
        public string ESTATUS_Q { get; set; }
    
        public virtual DOCUMENTO DOCUMENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENTORAN> DOCUMENTORANs { get; set; }
    }
}
