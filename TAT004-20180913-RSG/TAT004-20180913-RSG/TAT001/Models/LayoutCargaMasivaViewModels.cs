using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Models
{
    public class LayoutCargaMasivaViewModels
    {
        public LayoutCargaMasivaViewModels()
        {
            Layouts = new List<LAYOUT_CARGA>();
            Layout = new LAYOUT_CARGA();
            paises = new List<SelectListItem>();
            sociedades = new List<SelectListItem>();
        }
        
        public List<LAYOUT_CARGA> Layouts { get; set; }
        public LAYOUT_CARGA Layout { get; set; }
        public List<SelectListItem> paises { get; set; }
        public List<SelectListItem> sociedades { get; set; }
    }
}