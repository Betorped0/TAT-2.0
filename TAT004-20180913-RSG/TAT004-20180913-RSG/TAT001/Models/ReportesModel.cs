using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAT001.Models
{
    public class ReportesModel
    {
        public class TrackingTS
        {
            public int WF_POS { get; set; }
            public decimal NUMERO_SOLICITUD { get; set; }
            public string CO_CODE { get; set; }
            public string PAIS { get; set; }
            public string NUMERO_CLIENTE { get; set; }
            public string CLIENTE { get; set; }
            public string TIPO_SOLICITUD { get; set; }
            public string ESTATUS { get; set; }
            public string ESTATUS_C { get; set; }
            public string ESTATUS_SAP { get; set; }
            public string ESTATUS_WF { get; set; }
            public string TIPO { get; set; }
            public bool PADRE { get; set; }
            public DateTime FECHA { get; set; }
            public int PERIODO { get; set; }
            public string ANIO { get; set; }
            public string USUARIO { get; set; }
            public string COMENTARIO { get; set; }
            public string ROL { get; set; }
            public double TIEMPO_TRANSCURRIDO { get; set; }
            public int SEMANA { get; set; }
            public int NUMERO_CORRECCIONES { get; set; }
            public string STATUS_STRING {
                get
                {
                    string estatus = "";
                    
                    // FORMAR LA CADENA DE STATUS PARA IDENTIFICAR EL MENSAJE A DESPLEGAR
                    if (ESTATUS != null) { estatus += ESTATUS; } else { estatus += " "; }
                    if (ESTATUS_C != null) { estatus += ESTATUS_C; } else { estatus += " "; }
                    if (ESTATUS_SAP != null) { estatus += ESTATUS_SAP; } else { estatus += " "; }
                    if (ESTATUS_WF != null) { estatus += ESTATUS_WF; } else { estatus += " "; }
                    if (TIPO != null) { estatus += TIPO; } else { estatus += " "; }
                    if (PADRE) { estatus += "P"; } else { estatus += " "; }

                    // ESTABLECER EL MENSAJE DE STATUS QUE SE MOSTRARÁ
                    if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "^.[C]")) { return "Cancelada"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[P][R].")) { return "Pendiente validación TS"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[P][A].")) { return "Pendiente aprobador"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "[C]..[A]..")) { return "Por gen .txt"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "[P]..[A]..")) { return "Por contabilizar"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "[N]..[A]..")) { return "Por gen .txt"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "..[P][A]..")) { return "Por contabilizar"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "..[X][A]..")) { return "Error en contabilización"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[A].[P]")) { return TIPO_SOLICITUD + "Abierta"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[A]..")) { return "Registrada en SAP"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[R]..")) { return "Pendiente corrección usuario"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[R][S].")) { return "Pendiente corrección usuario TS"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[S]..")) { return "Pendiente firma"; }
                    else if (System.Text.RegularExpressions.Regex.IsMatch(estatus, "...[T]..")) { return "Pendiente tax"; }
                    else { return ""; }
                }
            }

            public string FECHA_STRING
            {
                get
                {
                    return FECHA.ToString("dd/MM/yyyy");
                }
            }
            public string HORA_STRING
            {
                get
                {
                    return FECHA.ToString("hh:mm tt");
                }
            }
            public int TIEMPO_TRANSCURRIDO_STRING
            {
                get
                {
                    return Convert.ToInt32(TIEMPO_TRANSCURRIDO);
                }
            }
        }
    }
}