using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;

namespace TAT001.Models.Dao
{
    public class SociedadesDao
    {
        readonly TAT001Entities db = new TAT001Entities();
        public List<SOCIEDAD> ListaSociedades(int accion,string bukrs=null, string usuario_id=null)
        {
            List<SOCIEDAD> sociedades = new List<SOCIEDAD>();
            if (accion== TATConstantes.ACCION_LISTA_SOCIEDADES) {
                sociedades= db.SOCIEDADs
                .Where(x => (x.BUKRS == bukrs || bukrs == null) && x.ACTIVO).ToList();
            }
            else if (accion== TATConstantes.ACCION_LISTA_SOCPORUSUARIO) {
                sociedades= db.USUARIOs.First(x => x.ID == usuario_id).SOCIEDADs.Where(x=> x.ACTIVO).ToList();
            }
            return sociedades;
        }
        public  List<SelectListItem> ComboSociedades(int accion, string bukrs = null, string usuario_id = null)
        {
            return ListaSociedades(accion,bukrs,usuario_id)
                .Select(x => new SelectListItem
                {
                    Value = x.BUKRS,
                    Text = (x.BUKRS+" - "+x.BUTXT)
                }).ToList();
        }
    }
}