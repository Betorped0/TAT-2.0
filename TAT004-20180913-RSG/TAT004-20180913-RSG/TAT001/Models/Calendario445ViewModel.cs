using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Models
{
    public class Calendario445ViewModel
    {
        public Calendario445ViewModel()
        {
            calendario445 = new CALENDARIO_AC();
            calendarios445 = new List<CALENDARIO_AC>();
            calendarioEx445 = new CALENDARIO_EX();
            calendariosEx445 = new List<CALENDARIO_EX>();
            sociedades = new List<SelectListItem>();
            periodos = new List<SelectListItem>();
            tipoSolicitudes = new List<SelectListItem>();
            usuarios = new List<SelectListItem>();
        }
        public CALENDARIO_AC calendario445 { get; set; }
        public CALENDARIO_EX calendarioEx445 { get; set; }
        public List<CALENDARIO_AC> calendarios445 { get; set; }
        public List<CALENDARIO_EX> calendariosEx445 { get; set; }
        public List<SelectListItem> sociedades { get; set; }
        public List<SelectListItem> periodos { get; set; }
        public List<SelectListItem> tipoSolicitudes { get; set; }
        public List<SelectListItem> usuarios { get; set; }
        
        

    }
}
