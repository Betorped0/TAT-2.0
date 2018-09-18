using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAT001.Models.Masiva
{
    public class Distribucion
    {
        public string NUM_DOC { get; set; }
        public string VIGENCIA_DE { get; set; }
        public string VIGENCIA_AL { get; set; }
        public string MATNR { get; set; }//MATERIAL
        public string MATKL { get; set; }//CATEGORIA
        public string MONTO { get; set; }
        public string PORC_APOYO { get; set; }
        public string PRECIO_SUG { get; set; }
        public string VOLUMEN_REAL { get; set; }
        public string APOYO { get; set; }
    }
}