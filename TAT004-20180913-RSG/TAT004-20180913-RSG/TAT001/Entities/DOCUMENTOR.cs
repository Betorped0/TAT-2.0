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
    
    public partial class DOCUMENTOR
    {
        public decimal NUM_DOC { get; set; }
        public int TREVERSA_ID { get; set; }
        public string USUARIOC_ID { get; set; }
        public Nullable<System.DateTime> FECHAC { get; set; }
        public string COMENTARIO { get; set; }
    
        public virtual DOCUMENTO DOCUMENTO { get; set; }
        public virtual TREVERSA TREVERSA { get; set; }
    }
}
