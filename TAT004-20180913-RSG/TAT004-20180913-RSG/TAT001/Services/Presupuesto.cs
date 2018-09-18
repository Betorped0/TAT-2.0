using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Services
{
    public class Presupuesto
    {
        public PRESUPUESTO_MOD getPresupuesto(string kunnr)//RSG 07.06.2018---------------------------------------------
        {
            TAT001Entities db = new TAT001Entities();
            PRESUPUESTO_MOD pm = new PRESUPUESTO_MOD();
            try
            {
                if (kunnr == null)
                    kunnr = "";

                //Obtener presupuesto
                Calendario445 c445 = new Calendario445();
                string mes = c445.getPeriodo(DateTime.Now.Date) + "";
                var presupuesto = db.CSP_PRESU_CLIENT(cLIENTE: kunnr, pERIODO: mes).Select(p => new { DESC = p.DESCRIPCION.ToString(), VAL = p.VALOR.ToString() }).ToList();
                string clien = db.CLIENTEs.Where(x => x.KUNNR == kunnr).Select(x => x.BANNERG).First();
                var clien2 = db.CLIENTEs.Where(x => x.KUNNR == kunnr).FirstOrDefault();
                string desCanal = db.CANALs.Where(x => x.CANAL1 == clien2.CANAL).FirstOrDefault().CDESCRIPCION;

                if (presupuesto != null)
                {
                    pm.CANAL = desCanal;
                    pm.CLIENTE = clien2.NAME1;
                    if (String.IsNullOrEmpty(clien))
                    {
                        //pm.P_CANAL = presupuesto[0].VAL;
                        //pm.P_BANNER = presupuesto[1].VAL;
                        //pm.PC_C = (float.Parse(presupuesto[4].VAL) + float.Parse(presupuesto[5].VAL) + float.Parse(presupuesto[6].VAL)).ToString();
                        //pm.PC_A = presupuesto[8].VAL;
                        //pm.PC_P = presupuesto[9].VAL;
                        //pm.PC_T = presupuesto[10].VAL;
                        //pm.CONSU = (float.Parse(presupuesto[1].VAL) - float.Parse(presupuesto[10].VAL)).ToString();
                        pm.P_CANAL = decimal.Parse(presupuesto[0].VAL);
                        pm.P_BANNER = decimal.Parse(presupuesto[1].VAL);
                        pm.PC_C = (decimal.Parse(presupuesto[4].VAL) + decimal.Parse(presupuesto[5].VAL) + decimal.Parse(presupuesto[6].VAL));
                        pm.PC_A = decimal.Parse(presupuesto[8].VAL);
                        pm.PC_P = decimal.Parse(presupuesto[9].VAL);
                        pm.PC_T = pm.PC_C + pm.PC_A + pm.PC_P;
                        pm.CONSU = (decimal.Parse(presupuesto[1].VAL) - pm.PC_T);
                    }
                    else
                    {
                        pm.P_CANAL = decimal.Parse(presupuesto[0].VAL);
                        pm.P_BANNER = decimal.Parse(presupuesto[0].VAL);
                        pm.PC_C = (decimal.Parse(presupuesto[4].VAL) + decimal.Parse(presupuesto[5].VAL) + decimal.Parse(presupuesto[6].VAL));
                        pm.PC_A = decimal.Parse(presupuesto[8].VAL);
                        pm.PC_P = decimal.Parse(presupuesto[9].VAL);
                        pm.PC_T = pm.PC_C + pm.PC_A + pm.PC_P;
                        pm.CONSU = (decimal.Parse(presupuesto[0].VAL) - pm.PC_T);
                    }
                }
            }
            catch
            {

            }
            db.Dispose();
            return pm;
        }
    }
}
