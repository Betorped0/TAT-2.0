using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TAT001.Models
{
    public class ModificacionesGlobalesViewModel
    {
        public ModificacionesGlobalesViewModel()
        {
            filtros = new Filtros();
            pageSizes = new List<SelectListItem>();
            solicitudPorAprobar = new List<SolicitudPorAprobar>();
        }
        public Filtros filtros { get; set; }
        public List<SelectListItem>  pageSizes  { get; set; }
        public List<SolicitudPorAprobar> solicitudPorAprobar { get; set; }

    }
    public class Filtros {
        public decimal? NUM_DOCI { get; set; }
        public decimal? NUM_DOCF { get; set; }
        public DateTime FECHAI { get; set; }
        public DateTime FECHAF { get; set; }
        public string KUNNR { get; set; }
        public string USUARIO_ID { get; set; }
    }

    public class SolicitudPorAprobar
    {
        public decimal NUM_DOC { get; set; }
        public string USUARIOA_ID { get; set; }
        public string USUARIOA_NOMBRE { get; set; }
        public string USUARIOA_ID_NUEVO { get; set; }

    }
}