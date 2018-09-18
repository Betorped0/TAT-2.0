using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAT001.Models.Masiva
{
    public class Encabezado
    {
        public string NUM_DOC { get; set; }
        public string TSOL_ID { get; set; }
        public string GALL_ID { get; set; }
        public string SOCIEDAD_ID { get; set; }
        public string PAIS_ID { get; set; }
        public string ESTADO { get; set; }
        public string CIUDAD { get; set; }
        public string CONCEPTO { get; set; }
        public string NOTAS { get; set; }
        public string PAYER_ID { get; set; }
        public string PAYER_NOMBRE { get; set; }
        public string VKORG { get; set; }
        public string VTWEG { get; set; }
        public string PAYER_EMAIL { get; set; }
        public string FECHAI_VIG { get; set; }
        public string FECHAF_VIG { get; set; }
        public string MONEDA_ID { get; set; }
    }
}