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
    
    public partial class DOCUMENTON
    {
        public decimal NUM_DOC { get; set; }
        public int POS { get; set; }
        public Nullable<int> STEP { get; set; }
        public string USUARIO_ID { get; set; }
        public string TEXTO { get; set; }
    
        public virtual DOCUMENTO DOCUMENTO { get; set; }
    }
}
