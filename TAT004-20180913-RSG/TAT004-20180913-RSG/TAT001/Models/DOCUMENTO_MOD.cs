﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAT001.Models
{
    public class DOCUMENTO_MOD
    {

        public decimal NUM_DOC { get; set; }
        public string TSOL_ID { get; set; }
        public string TALL_ID { get; set; }
        public string SOCIEDAD_ID { get; set; }
        public string PAIS_ID { get; set; }
        public string ESTADO { get; set; }
        public string CIUDAD { get; set; }
        public Nullable<int> PERIODO { get; set; }
        public string EJERCICIO { get; set; }
        public string TIPO_TECNICO { get; set; }
        public string TIPO_RECURRENTE { get; set; }
        public Nullable<decimal> CANTIDAD_EV { get; set; }
        public string USUARIOC_ID { get; set; }
        public string USUARIOD_ID { get; set; }
        public Nullable<System.DateTime> FECHAD { get; set; }
        public Nullable<System.DateTime> FECHAC { get; set; }
        public Nullable<System.TimeSpan> HORAC { get; set; }
        public Nullable<System.DateTime> FECHAC_PLAN { get; set; }
        public Nullable<System.DateTime> FECHAC_USER { get; set; }
        public Nullable<System.TimeSpan> HORAC_USER { get; set; }
        public string ESTATUS { get; set; }
        public string ESTATUS_C { get; set; }
        public string ESTATUS_SAP { get; set; }
        public string ESTATUS_WF { get; set; }
        public Nullable<decimal> DOCUMENTO_REF { get; set; }
        public string CONCEPTO { get; set; }
        public string NOTAS { get; set; }
        public Nullable<decimal> MONTO_DOC_MD { get; set; }
        public Nullable<decimal> MONTO_FIJO_MD { get; set; }
        public Nullable<decimal> MONTO_BASE_GS_PCT_MD { get; set; }
        public Nullable<decimal> MONTO_BASE_NS_PCT_MD { get; set; }
        public Nullable<decimal> MONTO_DOC_ML { get; set; }
        public Nullable<decimal> MONTO_FIJO_ML { get; set; }
        public Nullable<decimal> MONTO_BASE_GS_PCT_ML { get; set; }
        public Nullable<decimal> MONTO_BASE_NS_PCT_ML { get; set; }
        public Nullable<decimal> MONTO_DOC_ML2 { get; set; }
        public Nullable<decimal> MONTO_FIJO_ML2 { get; set; }
        public Nullable<decimal> MONTO_BASE_GS_PCT_ML2 { get; set; }
        public Nullable<decimal> MONTO_BASE_NS_PCT_ML2 { get; set; }
        public Nullable<decimal> PORC_ADICIONAL { get; set; }
        public string IMPUESTO { get; set; }
        public Nullable<System.DateTime> FECHAI_VIG { get; set; }
        public Nullable<System.DateTime> FECHAF_VIG { get; set; }
        public string ESTATUS_EXT { get; set; }
        public string SOLD_TO_ID { get; set; }
        public string PAYER_ID { get; set; }
        public string PAYER_NOMBRE { get; set; }
        public string PAYER_EMAIL { get; set; }
        public string GRUPO_CTE_ID { get; set; }
        public string CANAL_ID { get; set; }
        public string MONEDA_ID { get; set; }
        public string MONEDAL_ID { get; set; }
        public string MONEDAL2_ID { get; set; }
        public Nullable<decimal> TIPO_CAMBIO { get; set; }
        public Nullable<decimal> TIPO_CAMBIOL { get; set; }
        public Nullable<decimal> TIPO_CAMBIOL2 { get; set; }
        public string NO_FACTURA { get; set; }
        public Nullable<System.DateTime> FECHAD_SOPORTE { get; set; }
        public string METODO_PAGO { get; set; }
        public string NO_PROVEEDOR { get; set; }
        public Nullable<int> PASO_ACTUAL { get; set; }
        public string AGENTE_ACTUAL { get; set; }
        public Nullable<System.DateTime> FECHA_PASO_ACTUAL { get; set; }
        public string VKORG { get; set; }
        public string VTWEG { get; set; }
        public string SPART { get; set; }
        public Nullable<int> PUESTO_ID { get; set; }
        public string GALL_ID { get; set; }
        public Nullable<int> CONCEPTO_ID { get; set; }
        public string DOCUMENTO_SAP { get; set; }
        public Nullable<decimal> PORC_APOYO { get; set; }
        public Nullable<bool> LIGADA { get; set; }
        public Nullable<bool> OBJETIVOQ { get; set; }
        public Nullable<int> FRECUENCIA_LIQ { get; set; }
        public Nullable<decimal> OBJQ_PORC { get; set; }
        public Nullable<decimal> CUENTAP { get; set; }
        public Nullable<decimal> CUENTAPL { get; set; }
    }
}