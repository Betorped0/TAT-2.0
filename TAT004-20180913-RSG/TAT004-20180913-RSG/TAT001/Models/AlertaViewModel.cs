using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Models
{
    public class AlertaViewModel
    {
        public AlertaViewModel()
        {
            alertas = new List<WARNINGP>();
            alerta = new WARNINGP();
            alertaMensajes = new List<WARNINGPT>();

            sociedades = new List<SelectListItem>();
            tabs = new List<SelectListItem>();
            tiposSolicitud = new List<SelectListItem>();
            campos = new List<SelectListItem>();

        }
        public List<WARNINGP> alertas { get; set; }
        public WARNINGP alerta { get; set; }
        public List<WARNINGPT> alertaMensajes { get; set; }
        public List<SelectListItem> sociedades { get; set; }
        public List<SelectListItem> tiposSolicitud { get; set; }
        public List<SelectListItem> tabs { get; set; }
        public List<SelectListItem> campos { get; set; }
    }
}