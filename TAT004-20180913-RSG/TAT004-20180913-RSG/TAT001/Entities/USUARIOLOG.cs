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
    
    public partial class USUARIOLOG
    {
        public string USUARIO_ID { get; set; }
        public Nullable<int> POS { get; set; }
        public string SESION { get; set; }
        public string NAVEGADOR { get; set; }
        public string UBICACION { get; set; }
        public Nullable<System.DateTime> FECHA { get; set; }
        public Nullable<bool> LOGIN { get; set; }
    
        public virtual USUARIO USUARIO { get; set; }
    }
}
