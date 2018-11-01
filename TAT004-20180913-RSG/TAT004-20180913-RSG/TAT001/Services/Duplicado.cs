﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAT001.Entities;

namespace TAT001.Services
{
    public class Duplicado
    {
        public DOCUMENTBORR llenaDuplicado(decimal num_doc, string user)
        {
            using (TAT001Entities db = new TAT001Entities())
            {
                DOCUMENTBORR docb = new DOCUMENTBORR();
                DOCUMENTO docPadre = db.DOCUMENTOes.Find(num_doc);
                docb.AGENTE_ACTUAL = docPadre.AGENTE_ACTUAL;
                docb.CANAL_ID  = docPadre.CANAL_ID ;
                docb.CANTIDAD_EV  = docPadre.CANTIDAD_EV ;
                docb.CIUDAD  = docPadre.CIUDAD ;
                docb.CONCEPTO  = docPadre.CONCEPTO ;
                docb.DOCUMENTO_REF  = docPadre.DOCUMENTO_REF ;
                docb.EJERCICIO  = docPadre.EJERCICIO ;
                docb.ESTADO  = docPadre.ESTADO ;
                docb.ESTATUS  = docPadre.ESTATUS ;
                docb.ESTATUS_C = docPadre.ESTATUS_C ;
                docb.ESTATUS_EXT  = docPadre.ESTATUS_EXT ;
                docb.ESTATUS_SAP = docPadre.ESTATUS_SAP ;
                docb.ESTATUS_WF  = docPadre.ESTATUS_WF ;
                docb.FECHAC  = docPadre.FECHAC ;
                docb.FECHAC_PLAN  = docPadre.FECHAC_PLAN ;
                docb.FECHAC_USER  = docPadre.FECHAC_USER ;
                docb.FECHAD  = docPadre.FECHAD ;
                docb.FECHAD_SOPORTE  = docPadre.FECHAD_SOPORTE ;
                docb.FECHAF_VIG  = docPadre.FECHAF_VIG ;
                docb.FECHAI_VIG  = docPadre.FECHAI_VIG ;
                docb.FECHA_PASO_ACTUAL  = docPadre.FECHA_PASO_ACTUAL ;
                docb.GRUPO_CTE_ID  = docPadre.GRUPO_CTE_ID ;
                docb.HORAC  = docPadre.HORAC ;
                docb.HORAC_USER  = docPadre.HORAC_USER ;
                docb.IMPUESTO = docPadre.IMPUESTO ;
                docb.LIGADA = "";
                if (docPadre.LIGADA == true)
                    docb.LIGADA = "X";
                docb.METODO_PAGO  = docPadre.METODO_PAGO ;
                docb.MONEDAL2_ID  = docPadre.MONEDAL2_ID ;
                docb.MONEDAL_ID  = docPadre.MONEDAL_ID ;
                docb.MONEDA_DIS  = docPadre.MONEDA_ID ;
                docb.MONTO_BASE_GS_PCT_MD  = docPadre.MONTO_BASE_GS_PCT_MD ;
                docb.MONTO_BASE_GS_PCT_ML  = docPadre.MONTO_BASE_GS_PCT_ML ;
                docb.MONTO_BASE_GS_PCT_ML2  = docPadre.MONTO_BASE_GS_PCT_ML2 ;
                docb.MONTO_BASE_NS_PCT_MD  = docPadre.MONTO_BASE_NS_PCT_MD ;
                docb.MONTO_BASE_NS_PCT_ML  = docPadre.MONTO_BASE_NS_PCT_ML ;
                docb.MONTO_BASE_NS_PCT_ML2  = docPadre.MONTO_BASE_NS_PCT_ML2 ;
                docb.MONTO_DOC_MD  = docPadre.MONTO_DOC_MD ;
                docb.MONTO_DOC_ML  = docPadre.MONTO_DOC_ML ;
                docb.MONTO_DOC_ML2  = docPadre.MONTO_DOC_ML2 ;
                docb.MONTO_FIJO_MD  = docPadre.MONTO_FIJO_MD ;
                docb.MONTO_FIJO_ML  = docPadre.MONTO_FIJO_ML ;
                docb.MONTO_FIJO_ML2  = docPadre.MONTO_FIJO_ML2;
                docb.NOTAS  = docPadre.NOTAS ;
                docb.NO_FACTURA  = docPadre.NO_FACTURA ;
                docb.NO_PROVEEDOR  = docPadre.NO_PROVEEDOR ;
                docb.PAIS_ID  = docPadre.PAIS_ID ;
                docb.PASO_ACTUAL  = docPadre.PASO_ACTUAL ;
                docb.PAYER_EMAIL  = docPadre.PAYER_EMAIL ;
                docb.PAYER_ID  = docPadre.PAYER_ID;
                docb.PAYER_NOMBRE  = docPadre.PAYER_NOMBRE ;
                docb.PERIODO  = docPadre.PERIODO ;
                docb.PORC_ADICIONAL  = docPadre.PORC_ADICIONAL ;
                docb.PORC_APOYO  = docPadre.PORC_APOYO ;
                docb.SOCIEDAD_ID  = docPadre.SOCIEDAD_ID ;
                docb.SOLD_TO_ID  = docPadre.SOLD_TO_ID ;
                docb.SPART  = docPadre.SPART ;
                docb.TALL_ID  = docPadre.TALL_ID ;
                docb.TIPO_CAMBIO = docPadre.TIPO_CAMBIO ;
                docb.TIPO_CAMBIOL = docPadre.TIPO_CAMBIOL ;
                docb.TIPO_CAMBIOL2 = docPadre.TIPO_CAMBIOL2 ;
                docb.TIPO_RECURRENTE  = docPadre.TIPO_RECURRENTE ;
                docb.TIPO_TECNICO  = docPadre.TIPO_TECNICO ;
                //docb.TIPO_TECNICO2  = docPadre. ;
                docb.TSOL_ID  = docPadre.TSOL_ID ;
                docb.USUARIOC_ID  = docPadre.USUARIOC_ID ;
                docb.VKORG  = docPadre.VKORG ;
                docb.VTWEG  = docPadre.VTWEG ;

                foreach(DOCUMENTOP dp in docPadre.DOCUMENTOPs.ToList())
                {
                    DOCUMENTOBORRP docBp = new DOCUMENTOBORRP();
                    docBp.APOYO_EST = dp.APOYO_EST;
                    docBp.APOYO_REAL = dp.APOYO_REAL;
                    docBp.CANTIDAD = dp.CANTIDAD;
                    docBp.MATKL = dp.MATKL;
                    docBp.MATNR = dp.MATNR;
                    docBp.MONTO = dp.MONTO;
                    docBp.MONTO_APOYO = dp.MONTO_APOYO;
                    docBp.PORC_APOYO = dp.PORC_APOYO;
                    docBp.POS = dp.POS;
                    docBp.PRECIO_SUG = dp.PRECIO_SUG;
                    docBp.VIGENCIA_AL = dp.VIGENCIA_AL;
                    docBp.VIGENCIA_DE = dp.VIGENCIA_DE;
                    docBp.VOLUMEN_EST = dp.VOLUMEN_EST;
                    docBp.VOLUMEN_REAL = dp.VOLUMEN_REAL;

                    docb.DOCUMENTOBORRPs.Add(docBp);
                }

                return docb;
            }
        }
    }
}