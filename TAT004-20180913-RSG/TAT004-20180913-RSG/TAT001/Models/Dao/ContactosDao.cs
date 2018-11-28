using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TAT001.Entities;

namespace TAT001.Models.Dao
{
    public class ContactosDao
    {
        readonly TAT001Entities db = new TAT001Entities();
        public List<CONTACTOC> ListaContactos(string prefix, string vkorg, string vtweg, string kunnr)
        {
            if (prefix == null) { prefix = ""; }

            List<object> paramsCSP = new List<object>
            {
                new SqlParameter("@KUNNR", kunnr)
            };

            if (!string.IsNullOrEmpty(vkorg)) { paramsCSP.Add(new SqlParameter("@VKORG", vkorg)); }
            else { paramsCSP.Add(new SqlParameter("@VKORG", DBNull.Value)); }

            if (!string.IsNullOrEmpty(vtweg)) { paramsCSP.Add(new SqlParameter("@VTWEG", vtweg)); }
            else { paramsCSP.Add(new SqlParameter("@VTWEG", DBNull.Value)); }

            paramsCSP.Add(new SqlParameter("@PREFIX", prefix));

            List<CONTACTOC> contactos = db.Database.SqlQuery<CONTACTOC>("CPS_LISTA_CONTACTOS @KUNNR,@VKORG,@VTWEG,@PREFIX",
           paramsCSP).ToList();
            return contactos;
        }
    }
}