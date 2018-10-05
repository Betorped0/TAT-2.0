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
            alertaCondiciones = new List<WARNING_COND>();

            sociedades = new List<SelectListItem>();
            tabs = new List<SelectListItem>();
            tiposSolicitud = new List<SelectListItem>();
            campos = new List<SelectListItem>();
            tipos = new List<SelectListItem>();
            condCampos = new List<SelectListItem>();
            condValores = new List<SelectListItem>();
            condCampos1 = new List<SelectListItem>();
            condValores1 = new List<SelectListItem>();

        }
        public List<WARNINGP> alertas { get; set; }
        public WARNINGP alerta { get; set; }
        public List<WARNINGPT> alertaMensajes { get; set; }
        public List<WARNING_COND> alertaCondiciones { get; set; }

        public List<SelectListItem> sociedades { get; set; }
        public List<SelectListItem> tiposSolicitud { get; set; }
        public List<SelectListItem> tabs { get; set; }
        public List<SelectListItem> campos { get; set; }
        public List<SelectListItem> tipos { get; set; }
        public List<SelectListItem> condCampos { get; set; }
        public List<SelectListItem> condValores { get; set; }
        public List<SelectListItem> condCampos1 { get; set; }
        public List<SelectListItem> condValores1 { get; set; }
    }
}