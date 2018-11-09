using System;

namespace TAT001.Models
{
    public class ModificacionesGlobalesViewModel
    {
        public ModificacionesGlobalesViewModel()
        {
            filtros = new Filtros();
        }
        public Filtros filtros { get; set; }

    }
    public class Filtros{
        public decimal? NUM_DOCI { get; set; }
        public decimal? NUM_DOCF { get; set; }
        public DateTime FECHAI { get; set; }
        public DateTime FECHAF { get; set; }
        public string KUNNR { get; set; }
        public string USUARIO_ID { get; set; }
    }
}