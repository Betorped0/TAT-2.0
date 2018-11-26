using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TAT001.Entities;

namespace TAT001.Models.Dao
{
    public class UsuariosDao
    {
        readonly TAT001Entities db = new TAT001Entities();
        public List<USUARIO> ListaUsuarios(string prefix, int accion)
        {
            if (prefix == null) { prefix = ""; }

            List<object> paramsCSP = new List<object>();

            paramsCSP.Add(new SqlParameter("@ACCION", accion));
            paramsCSP.Add(new SqlParameter("@PREFIX", prefix));

            List<USUARIO> usuarios = db.Database.SqlQuery<USUARIO>("CPS_LISTA_USUARIOS @ACCION,@PREFIX",
                paramsCSP.ToArray()).ToList();
            return usuarios;
        }
    }
}