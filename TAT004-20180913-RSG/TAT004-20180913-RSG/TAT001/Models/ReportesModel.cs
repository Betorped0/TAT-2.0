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
            public string TIPO_SOLICITUD_ID { get; internal set; }
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
            public string PERIODO { get; set; }
            public string YEAR { get; set; }
            public string BU { get; set; }
            public string PAYER { get; set; }
            public string CLIENTE { get; set; }
            public string CANAL { get; set; }
            public string CATEGORIA { get; set; }
            public int CD_EN_PROCESO { get; set; }
            public int CD_ALLOWANCE_TAT { get; set; }
            public int CD_ALLOWANCE_FACT { get; set; }
            public int CD_AJUSTES { get; set; }
            public int CD_TOTALES { get { return CD_EN_PROCESO + CD_ALLOWANCE_TAT + CD_ALLOWANCE_FACT + CD_AJUSTES; } }
            public int C_EN_PROCESO { get; set; }
            public int C_ALLOWANCE_TAT { get; set; }
            public int C_ALLOWANCE_FACT { get; set; }
            public int C_AJUSTES { get; set; }
            public int C_TOTALES { get { return C_EN_PROCESO + C_ALLOWANCE_TAT + C_ALLOWANCE_FACT + C_AJUSTES; } }
            public int COD_EN_PROCESO { get; set; }
            public int COD_ALLOWANCE_TAT { get; set; }
            public int COD_ALLOWANCE_FACT { get; set; }
            public int COD_AJUSTES { get; set; }
            public int COD_TOTALES { get { return COD_EN_PROCESO + COD_ALLOWANCE_TAT + COD_ALLOWANCE_FACT + COD_AJUSTES; } }
            public int DPS_EN_PROCESO { get; set; }
            public int DPS_ALLOWANCE_TAT { get; set; }
            public int DPS_ALLOWANCE_FACT { get; set; }
            public int DPS_AJUSTES { get; set; }
            public int DPS_TOTALES { get { return DPS_EN_PROCESO + DPS_ALLOWANCE_TAT + DPS_ALLOWANCE_FACT + DPS_AJUSTES; } }
            public int DC_EN_PROCESO { get; set; }
            public int DC_ALLOWANCE_TAT { get; set; }
            public int DC_ALLOWANCE_FACT { get; set; }
            public int DC_AJUSTES { get; set; }
            public int DC_TOTALES { get { return DC_EN_PROCESO + DC_ALLOWANCE_TAT + DC_ALLOWANCE_FACT + DC_AJUSTES; } }
            public int ELP_EN_PROCESO { get; set; }
            public int ELP_ALLOWANCE_TAT { get; set; }
            public int ELP_ALLOWANCE_FACT { get; set; }
            public int ELP_AJUSTES { get; set; }
            public int ELP_TOTALES { get { return ELP_EN_PROCESO + ELP_ALLOWANCE_TAT + ELP_ALLOWANCE_FACT + ELP_AJUSTES; } }
            public int FG_EN_PROCESO { get; set; }
            public int FG_ALLOWANCE_TAT { get; set; }
            public int FG_ALLOWANCE_FACT { get; set; }
            public int FG_AJUSTES { get; set; }
            public int FG_TOTALES { get { return FG_EN_PROCESO + FG_ALLOWANCE_TAT + FG_ALLOWANCE_FACT + FG_AJUSTES; } }
            public int GD_EN_PROCESO { get; set; }
            public int GD_ALLOWANCE_TAT { get; set; }
            public int GD_ALLOWANCE_FACT { get; set; }
            public int GD_AJUSTES { get; set; }
            public int GD_TOTALES { get { return GD_EN_PROCESO + GD_ALLOWANCE_TAT + GD_ALLOWANCE_FACT + GD_AJUSTES; } }
            public int GP_EN_PROCESO { get; set; }
            public int GP_ALLOWANCE_TAT { get; set; }
            public int GP_ALLOWANCE_FACT { get; set; }
            public int GP_AJUSTES { get; set; }
            public int GP_TOTALES { get { return GP_EN_PROCESO + GP_ALLOWANCE_TAT + GP_ALLOWANCE_FACT + GP_AJUSTES; } }
            public int LD_EN_PROCESO { get; set; }
            public int LD_ALLOWANCE_TAT { get; set; }
            public int LD_ALLOWANCE_FACT { get; set; }
            public int LD_AJUSTES { get; set; }
            public int LD_TOTALES { get { return LD_EN_PROCESO + LD_ALLOWANCE_TAT + LD_ALLOWANCE_FACT + LD_AJUSTES; } }
            public int MS_EN_PROCESO { get; set; }
            public int MS_ALLOWANCE_TAT { get; set; }
            public int MS_ALLOWANCE_FACT { get; set; }
            public int MS_AJUSTES { get; set; }
            public int MS_TOTALES { get { return MS_EN_PROCESO + MS_ALLOWANCE_TAT + MS_ALLOWANCE_FACT + MS_AJUSTES; } }
            public int R_EN_PROCESO { get; set; }
            public int R_ALLOWANCE_TAT { get; set; }
            public int R_ALLOWANCE_FACT { get; set; }
            public int R_AJUSTES { get; set; }
            public int R_TOTALES { get { return R_EN_PROCESO + R_ALLOWANCE_TAT + R_ALLOWANCE_FACT + R_AJUSTES; } }
            public int TP_EN_PROCESO { get; set; }
            public int TP_ALLOWANCE_TAT { get; set; }
            public int TP_ALLOWANCE_FACT { get; set; }
            public int TP_AJUSTES { get; set; }
            public int TP_TOTALES { get { return TP_EN_PROCESO + TP_ALLOWANCE_TAT + TP_ALLOWANCE_FACT + TP_AJUSTES; } }
            public int SIS_EN_PROCESO { get; set; }
            public int SIS_ALLOWANCE_TAT { get; set; }
            public int SIS_ALLOWANCE_FACT { get; set; }
            public int SIS_AJUSTES { get; set; }
            public int SIS_TOTALES { get { return SIS_EN_PROCESO + SIS_ALLOWANCE_TAT + SIS_ALLOWANCE_FACT + SIS_AJUSTES; } }
            public int SO_EN_PROCESO { get; set; }
            public int SO_ALLOWANCE_TAT { get; set; }
            public int SO_ALLOWANCE_FACT { get; set; }
            public int SO_AJUSTES { get; set; }
            public int SO_TOTALES { get { return SO_EN_PROCESO + SO_ALLOWANCE_TAT + SO_ALLOWANCE_FACT + SO_AJUSTES; } }
            public int TPIS_EN_PROCESO { get; set; }
            public int TPIS_ALLOWANCE_TAT { get; set; }
            public int TPIS_ALLOWANCE_FACT { get; set; }
            public int TPIS_AJUSTES { get; set; }
            public int TPIS_TOTALES { get { return TPIS_EN_PROCESO + TPIS_ALLOWANCE_TAT + TPIS_ALLOWANCE_FACT + TPIS_AJUSTES; } }
            public int TPO_EN_PROCESO { get; set; }
            public int TPO_ALLOWANCE_TAT { get; set; }
            public int TPO_ALLOWANCE_FACT { get; set; }
            public int TPO_AJUSTES { get; set; }
            public int TPO_TOTALES { get { return TPO_EN_PROCESO + TPO_ALLOWANCE_TAT + TPO_ALLOWANCE_FACT + TPO_AJUSTES; } }
            public int U_EN_PROCESO { get; set; }
            public int U_ALLOWANCE_TAT { get; set; }
            public int U_ALLOWANCE_FACT { get; set; }
            public int U_AJUSTES { get; set; }
            public int U_TOTALES { get { return U_EN_PROCESO + U_ALLOWANCE_TAT + U_ALLOWANCE_FACT + U_AJUSTES; } }
            public int WA_EN_PROCESO { get; set; }
            public int WA_ALLOWANCE_TAT { get; set; }
            public int WA_ALLOWANCE_FACT { get; set; }
            public int WA_AJUSTES { get; set; }
            public int WA_TOTALES { get { return WA_EN_PROCESO + WA_ALLOWANCE_TAT + WA_ALLOWANCE_FACT + WA_AJUSTES; } }
            public int PROCESO_TAT_TOTAL { get { return CD_EN_PROCESO + C_EN_PROCESO + COD_EN_PROCESO + DPS_EN_PROCESO + DC_EN_PROCESO + ELP_EN_PROCESO + FG_EN_PROCESO + GD_EN_PROCESO + GP_EN_PROCESO + LD_EN_PROCESO + MS_EN_PROCESO + R_EN_PROCESO + TP_EN_PROCESO + SIS_EN_PROCESO + SO_EN_PROCESO + TPIS_EN_PROCESO + TPO_EN_PROCESO + U_EN_PROCESO; } }
            public int ALLOWANCE_TAT_TOTAL { get { return CD_ALLOWANCE_TAT + C_ALLOWANCE_TAT + COD_ALLOWANCE_TAT + DPS_ALLOWANCE_TAT + DC_ALLOWANCE_TAT + ELP_ALLOWANCE_TAT + FG_ALLOWANCE_TAT + GD_ALLOWANCE_TAT + GP_ALLOWANCE_TAT + LD_ALLOWANCE_TAT + MS_ALLOWANCE_TAT + R_ALLOWANCE_TAT + TP_ALLOWANCE_TAT + SIS_ALLOWANCE_TAT + SO_ALLOWANCE_TAT + TPIS_ALLOWANCE_TAT + TPO_ALLOWANCE_TAT + U_ALLOWANCE_TAT; } }
            public int ALLOWANCE_FACT_TOTAL { get { return CD_ALLOWANCE_FACT + C_ALLOWANCE_FACT + COD_ALLOWANCE_FACT + DPS_ALLOWANCE_FACT + DC_ALLOWANCE_FACT + ELP_ALLOWANCE_FACT + FG_ALLOWANCE_FACT + GD_ALLOWANCE_FACT + GP_ALLOWANCE_FACT + LD_ALLOWANCE_FACT + MS_ALLOWANCE_FACT + R_ALLOWANCE_FACT + TP_ALLOWANCE_FACT + SIS_ALLOWANCE_FACT + SO_ALLOWANCE_FACT + TPIS_ALLOWANCE_FACT + TPO_ALLOWANCE_FACT + U_ALLOWANCE_FACT; } }
            public int AJUSTES { get { return CD_AJUSTES + C_AJUSTES + COD_AJUSTES + DPS_AJUSTES + DC_AJUSTES + ELP_AJUSTES + FG_AJUSTES + GD_AJUSTES + GP_AJUSTES + LD_AJUSTES + MS_AJUSTES + R_AJUSTES + TP_AJUSTES + SIS_AJUSTES + SO_AJUSTES + TPIS_AJUSTES + TPO_AJUSTES + U_AJUSTES; } }
            public int ALLOWANCE_TOTALES { get { return PROCESO_TAT_TOTAL + ALLOWANCE_TAT_TOTAL + ALLOWANCE_FACT_TOTAL + AJUSTES; } }
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