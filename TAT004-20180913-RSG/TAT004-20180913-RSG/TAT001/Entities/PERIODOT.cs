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
    
    public partial class PERIODOT
    {
        public string SPRAS_ID { get; set; }
        public int PERIODO_ID { get; set; }
        public string TXT50 { get; set; }
        public string TXT03 { get; set; }
        public string TXT01 { get; set; }
    
        public virtual PERIODO PERIODO { get; set; }
        public virtual SPRA SPRA { get; set; }
    }
}
