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
    
    public partial class LEYENDA
    {
        public string ID { get; set; }
        public string PAIS_ID { get; set; }
        public string LEYENDA1 { get; set; }
        public Nullable<bool> ACTIVO { get; set; }
        public bool EDITABLE { get; set; }
        public bool OBLIGATORIA { get; set; }
    
        public virtual PAI PAI { get; set; }
    }
}
