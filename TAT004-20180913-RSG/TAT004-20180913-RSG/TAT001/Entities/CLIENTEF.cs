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
    
    public partial class CLIENTEF
    {
        public string VKORG { get; set; }
        public string VTWEG { get; set; }
        public string SPART { get; set; }
        public string KUNNR { get; set; }
        public int VERSION { get; set; }
        public string USUARIO0_ID { get; set; }
        public string USUARIO1_ID { get; set; }
        public string USUARIO2_ID { get; set; }
        public string USUARIO3_ID { get; set; }
        public string USUARIO4_ID { get; set; }
        public string USUARIO5_ID { get; set; }
        public string USUARIO6_ID { get; set; }
        public bool ACTIVO { get; set; }
        public Nullable<System.DateTime> FECHAC { get; set; }
        public Nullable<System.DateTime> FECHAM { get; set; }
        public string USUARIO7_ID { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
    }
}
