﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAT001.Entities;

namespace TAT001.Services
{
    public class UsuarioLogin
    {
        private TAT001Entities db = new TAT001Entities();

        public bool validaUsuario(string usuario)
        {
            var existeUsuario = db.USUARIOLOGs.SingleOrDefault(x => x.USUARIO_ID == usuario);

            if (System.Web.HttpContext.Current.Session.SessionID != existeUsuario.SESION)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}