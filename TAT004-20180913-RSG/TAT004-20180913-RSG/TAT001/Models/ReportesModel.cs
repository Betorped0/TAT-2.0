using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAT001.Entities;

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
            public string STATUS_STRING
            {
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

        public class MRLTS
        {
            public string CO_CODE { get; set; }
            public string PAIS { get; set; }
            public decimal NUMERO_SOLICITUD { get; set; }
            public DateTime FECHA_SOLICITUD { get; set; }
            public int PERIODO_CONTABLE { get; set; }
            public string NUMERO_DOCUMENTO_SAP { get; set; }
            public int NUMERO_REVERSO_SAP { get; set; }
            public DateTime FECHA_REVERSO { get; set; }
            public string PERIODO_CONTABLE_REVERSO { get; set; }
            public string COMENTARIOS_REVERSO_PROVISION { get; set; }
            public string TIPO_SOLICITUD { get; set; }
            public string STATUS { get; set; }
            public string CONCEPTO_SOLICITUD { get; set; }
            public DateTime DE { get; set; }
            public DateTime A { get; set; }
            public string CLASIFICACION { get; set; }
            public string NUMERO_CLIENTE { get; set; }
            public string CLIENTE { get; set; }
            public decimal MONTO { get; set; }
            public string MONEDA { get; set; }
            public decimal TIPO_CAMBIO { get; set; }
        }

        public class AllowancesB
        {
            public string CUENTA_DE_BALANCE { get; set; }
            public string DESCRIPCION { get; set; }
            public string FUENTE { get; set; }
            public int KCMX { get; set; }
            public int KLCA { get; set; }
            public int LCCR { get; set; }
            public int LPKP { get; set; }
            public int KLSV { get; set; }
            public int KCAR { get; set; }
            public int KPRS { get; set; }
            public int KLCO { get; set; }
            public int LEKE { get; set; }
            public int LAGA { get; set; }
            public int KLCH { get; set; }
            public bool IS_DOLAR { get; set; }
        }

        public class AllowancesPL
        {
            public string PERIDO { get; set; }
            public int YEAR { get; set; }
            public string BU { get; set; }
            public int PAYER { get; set; }
            public string CLIENTE { get; set; }
            public string CANAL { get; set; }
            public string CATEGORIA { get; set; }
            public string CD_EN_PROCESO { get; set; }
            public string CD_ALLOWANCE_TAT { get; set; }
            public string CD_ALLOWANCE_FACT { get; set; }
            public string CD_AJUSTES { get; set; }
            public string CD_TOTALES { get; set; }
            public string C_EN_PROCESO { get; set; }
            public string C_ALLOWANCE_TAT { get; set; }
            public string C_ALLOWANCE_FACT { get; set; }
            public string C_AJUSTES { get; set; }
            public string C_TOTALES { get; set; }
            public string COD_EN_PROCESO { get; set; }
            public string COD_ALLOWANCE_TAT { get; set; }
            public string COD_ALLOWANCE_FACT { get; set; }
            public string COD_AJUSTES { get; set; }
            public string COD_TOTALES { get; set; }
            public string DPS_EN_PROCESO { get; set; }
            public string DPS_ALLOWANCE_TAT { get; set; }
            public string DPS_ALLOWANCE_FACT { get; set; }
            public string DPS_AJUSTES { get; set; }
            public string DPS_TOTALES { get; set; }
            public string DC_EN_PROCESO { get; set; }
            public string DC_ALLOWANCE_TAT { get; set; }
            public string DC_ALLOWANCE_FACT { get; set; }
            public string DC_AJUSTES { get; set; }
            public string DC_TOTALES { get; set; }
            public string ELP_EN_PROCESO { get; set; }
            public string ELP_ALLOWANCE_TAT { get; set; }
            public string ELP_ALLOWANCE_FACT { get; set; }
            public string ELP_AJUSTES { get; set; }
            public string ELP_TOTALES { get; set; }
            public string FG_EN_PROCESO { get; set; }
            public string FG_ALLOWANCE_TAT { get; set; }
            public string FG_ALLOWANCE_FACT { get; set; }
            public string FG_AJUSTES { get; set; }
            public string FG_TOTALES { get; set; }
            public string GD_EN_PROCESO { get; set; }
            public string GD_ALLOWANCE_TAT { get; set; }
            public string GD_ALLOWANCE_FACT { get; set; }
            public string GD_AJUSTES { get; set; }
            public string GD_TOTALES { get; set; }
            public string GP_EN_PROCESO { get; set; }
            public string GP_ALLOWANCE_TAT { get; set; }
            public string GP_ALLOWANCE_FACT { get; set; }
            public string GP_AJUSTES { get; set; }
            public string GP_TOTALES { get; set; }
            public string LD_EN_PROCESO { get; set; }
            public string LD_ALLOWANCE_TAT { get; set; }
            public string LD_ALLOWANCE_FACT { get; set; }
            public string LD_AJUSTES { get; set; }
            public string LD_TOTALES { get; set; }
            public string MS_EN_PROCESO { get; set; }
            public string MS_ALLOWANCE_TAT { get; set; }
            public string MS_ALLOWANCE_FACT { get; set; }
            public string MS_AJUSTES { get; set; }
            public string MS_TOTALES { get; set; }
            public string R_EN_PROCESO { get; set; }
            public string R_ALLOWANCE_TAT { get; set; }
            public string R_ALLOWANCE_FACT { get; set; }
            public string R_AJUSTES { get; set; }
            public string R_TOTALES { get; set; }
            public string TP_EN_PROCESO { get; set; }
            public string TP_ALLOWANCE_TAT { get; set; }
            public string TP_ALLOWANCE_FACT { get; set; }
            public string TP_AJUSTES { get; set; }
            public string TP_TOTALES { get; set; }
            public string SIS_EN_PROCESO { get; set; }
            public string SIS_ALLOWANCE_TAT { get; set; }
            public string SIS_ALLOWANCE_FACT { get; set; }
            public string SIS_AJUSTES { get; set; }
            public string SIS_TOTALES { get; set; }
            public string SO_EN_PROCESO { get; set; }
            public string SO_ALLOWANCE_TAT { get; set; }
            public string SO_ALLOWANCE_FACT { get; set; }
            public string SO_AJUSTES { get; set; }
            public string SO_TOTALES { get; set; }
            public string TPIS_EN_PROCESO { get; set; }
            public string TPIS_ALLOWANCE_TAT { get; set; }
            public string TPIS_ALLOWANCE_FACT { get; set; }
            public string TPIS_AJUSTES { get; set; }
            public string TPIS_TOTALES { get; set; }
            public string TPO_EN_PROCESO { get; set; }
            public string TPO_ALLOWANCE_TAT { get; set; }
            public string TPO_ALLOWANCE_FACT { get; set; }
            public string TPO_AJUSTES { get; set; }
            public string TPO_TOTALES { get; set; }
            public string U_EN_PROCESO { get; set; }
            public string U_ALLOWANCE_TAT { get; set; }
            public string U_ALLOWANCE_FACT { get; set; }
            public string U_AJUSTES { get; set; }
            public string U_TOTALES { get; set; }
            public string WA_EN_PROCESO { get; set; }
            public string WA_ALLOWANCE_TAT { get; set; }
            public string WA_ALLOWANCE_FACT { get; set; }
            public string WA_AJUSTES { get; set; }
            public string WA_TOTALES { get; set; }
            public int PROCESO_TAT_TOTAL { get; set; }
            public int ALLOWANCE_TAT_TOTAL { get; set; }
            public int ALLOWANCE_FACT_TOTAL { get; set; }
            public int AJUSTES { get; set; }
            public int ALLOWANCE_TOTALES { get; set; }
        }

        public class Concentrado
        {
            internal List<FLUJO> workflow;
            internal FLUJO acciones;
            internal List<DOCUMENTOA> files;
            internal string pais;
            internal List<TS_FORM> ts;
            internal List<DOCUMENTOT> tts;
            internal List<CARTAP> cartap;
            internal string ultMod;
            internal string miles;
            internal string dec;

            public DOCUMENTO documento { set; get; }
            public FLUJO flujo { set; get; }
            public string Title { get; set; }
            public List<TSOL> TSOL_RELA { get; set; }
            public CANAL CANAL { get; set; }
            public decimal CUENTA_ABONO { get; set; }
            public decimal CUENTA_CARGO { get; set; }
            public decimal CUENTA_CLEARING { get; set; }
            public decimal CUENTA_LIMITE { get; set; }
            public string CUENTA_CARGO_NOMBRE { get; set; }
            public PRESUPUESTO_MOD PRESUPUESTO { get; set; }
            public string PROVEEDOR_NOMBRE { get; set; }
        }
    }
}