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
    
    public partial class CUENTA
    {
        public string SOCIEDAD_ID { get; set; }
        public string PAIS_ID { get; set; }
        public string TALL_ID { get; set; }
        public decimal EJERCICIO { get; set; }
        public Nullable<decimal> ABONO { get; set; }
        public Nullable<decimal> CARGO { get; set; }
        public Nullable<decimal> CLEARING { get; set; }
        public Nullable<decimal> LIMITE { get; set; }
        public string IMPUESTO { get; set; }
    
        public virtual CUENTAGL CUENTAGL { get; set; }
        public virtual CUENTAGL CUENTAGL1 { get; set; }
        public virtual CUENTAGL CUENTAGL2 { get; set; }
        public virtual PAI PAI { get; set; }
        public virtual SOCIEDAD SOCIEDAD { get; set; }
        public virtual TALL TALL { get; set; }
    }
}
