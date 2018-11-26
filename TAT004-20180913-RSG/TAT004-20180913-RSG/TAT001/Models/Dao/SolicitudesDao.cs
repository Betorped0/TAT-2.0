﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TAT001.Entities;

namespace TAT001.Models.Dao
{
    public class SolicitudesDao
    {
        readonly TAT001Entities db = new TAT001Entities();

        public  List<DOCUMENTO> ObtenerSolicitudes(int accion,string prefix, decimal? num_doci, decimal? num_docf,
           string usuario_id = null,
           DateTime? fechai = null,
           DateTime? fechaf = null,
           string kunnr = null)
        {
            if (prefix == null) { prefix = ""; }
            List<object> paramsCSP = new List<object>();

            paramsCSP.Add(new SqlParameter("@ACCION", accion));
            paramsCSP.Add(new SqlParameter("@PREFIX", prefix));

            if (num_doci != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCI", num_doci)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCI", DBNull.Value)); }

            if (num_docf != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCF", num_docf)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCF", DBNull.Value)); }

            if (fechai != null) { paramsCSP.Add(new SqlParameter("@FECHAI", fechai)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAI", DBNull.Value)); }

            if (fechaf != null) { paramsCSP.Add(new SqlParameter("@FECHAF", fechaf)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAF", DBNull.Value)); }

            if (kunnr != null) { paramsCSP.Add(new SqlParameter("@KUNNR", kunnr)); }
            else { paramsCSP.Add(new SqlParameter("@KUNNR", DBNull.Value)); }

            if (usuario_id != null) { paramsCSP.Add(new SqlParameter("@USUARIO_ID", usuario_id)); }
            else { paramsCSP.Add(new SqlParameter("@USUARIO_ID", DBNull.Value)); }




            List<DOCUMENTO> solicitudes = db.Database.SqlQuery<DOCUMENTO>("CPS_LISTA_SOLICITUDES @ACCION,@PREFIX,@NUM_DOCI,@NUM_DOCF,@FECHAI,@FECHAF,@KUNNR,@USUARIO_ID",
            paramsCSP.ToArray()).ToList();
            return solicitudes;

        }
        public  List<SolicitudPorAprobar> ObtenerSolicitudesPorAprobar(decimal? num_doci, decimal? num_docf, DateTime? fechai, DateTime? fechaf,          string kunnr, string usuarioa_id,
          decimal? num_doc = null)
        {
            List<object> paramsCSP = new List<object>();

            if (num_doci != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCI", num_doci)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCI", DBNull.Value)); }

            if (num_docf != null) { paramsCSP.Add(new SqlParameter("@NUM_DOCF", num_docf)); }
            else { paramsCSP.Add(new SqlParameter("@NUM_DOCF", DBNull.Value)); }

            if (fechai != null) { paramsCSP.Add(new SqlParameter("@FECHAI", fechai)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAI", DBNull.Value)); }

            if (fechaf != null) { paramsCSP.Add(new SqlParameter("@FECHAF", fechaf)); }
            else { paramsCSP.Add(new SqlParameter("@FECHAF", DBNull.Value)); }

            if (kunnr != null) { paramsCSP.Add(new SqlParameter("@KUNNR", kunnr)); }
            else { paramsCSP.Add(new SqlParameter("@KUNNR", DBNull.Value)); }

            if (usuarioa_id != null) { paramsCSP.Add(new SqlParameter("@USUARIOA_ID", usuarioa_id)); }
            else { paramsCSP.Add(new SqlParameter("@USUARIOA_ID", DBNull.Value)); }

            List<SolicitudPorAprobar> solicitudes = db.Database.SqlQuery<SolicitudPorAprobar>("CPS_LISTA_SOLICITUDES_POR_APROBAR @NUM_DOCI,@NUM_DOCF,@FECHAI,@FECHAF,@KUNNR,@USUARIOA_ID",
            paramsCSP.ToArray()).ToList();
            return solicitudes;

        }
    }
}