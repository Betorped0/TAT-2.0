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
