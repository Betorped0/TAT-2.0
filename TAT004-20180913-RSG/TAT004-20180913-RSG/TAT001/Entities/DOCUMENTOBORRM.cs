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
    
    public partial class DOCUMENTOBORRM
    {
        public string USUARIOC_ID { get; set; }
        public decimal POS_ID { get; set; }
        public int POS { get; set; }
        public string MATNR { get; set; }
        public Nullable<decimal> PORC_APOYO { get; set; }
        public Nullable<decimal> APOYO_EST { get; set; }
        public Nullable<decimal> APOYO_REAL { get; set; }
        public Nullable<System.DateTime> VIGENCIA_DE { get; set; }
        public Nullable<System.DateTime> VIGENCIA_AL { get; set; }
        public Nullable<decimal> VALORH { get; set; }
    
        public virtual DOCUMENTOBORRP DOCUMENTOBORRP { get; set; }
    }
}
