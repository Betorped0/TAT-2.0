using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TAT001.Entities;

namespace TAT001.Services
{
    public class Calendario445
    {
        public int getPeriodo(DateTime fecha)
        {
            TAT001Entities db = new TAT001Entities();
            int periodo = 0;
            List<PERIODO445> pp = db.PERIODO445.Where(a => a.EJERCICIO == fecha.Year).ToList();
            //PERIODO445 p = pp.Where(a => a.MES_NATURAL == fecha.Month).FirstOrDefault();
            PERIODO445 p = pp.Where(a => a.MES_NATURAL == fecha.Month && a.DIA_NATURAL >= fecha.Day).OrderBy(a => a.DIA_NATURAL).LastOrDefault();
            if (p == null)
                p = pp.Where(a => a.MES_NATURAL == fecha.Month).OrderBy(a => a.DIA_NATURAL).LastOrDefault();
            if (fecha.Day > p.DIA_NATURAL)
            {
                periodo = p.PERIODO + 1;
            }
            else
            {
                periodo = p.PERIODO;
            }

            return periodo;
        }
        public int getPeriodoF(DateTime fecha)
        {
            TAT001Entities db = new TAT001Entities();
            int periodo = 0;
            List<PERIODO445> pp = db.PERIODO445.Where(a => a.EJERCICIO == fecha.Year).ToList();
            //PERIODO445 p = pp.Where(a => a.MES_NATURAL == fecha.Month).FirstOrDefault();
            PERIODO445 p = pp.Where(a => a.MES_NATURAL == fecha.Month && a.DIA_NATURAL == fecha.Day).OrderBy(a => a.DIA_NATURAL).FirstOrDefault();
            if (p == null)
            {
                p = pp.Where(a => a.MES_NATURAL == fecha.Month).OrderBy(a => a.DIA_NATURAL).FirstOrDefault();
            }
            else
            {
                if (fecha.Day > p.DIA_NATURAL)
                {
                    periodo = p.PERIODO + 1;
                }
                else
                {
                    periodo = p.PERIODO;
                }
            }

            return periodo;
        }

        public int getEjercicio(DateTime fecha)
        {
            TAT001Entities db = new TAT001Entities();
            int periodo = 0;
            List<PERIODO445> pp = db.PERIODO445.Where(a => a.EJERCICIO == fecha.Year).ToList();
            PERIODO445 p = pp.Where(a => a.MES_NATURAL == fecha.Month && a.DIA_NATURAL >= fecha.Day).OrderBy(a => a.DIA_NATURAL).LastOrDefault();
            //if (fecha.Day > p.DIA_NATURAL)
            //{
            //    periodo = p.PERIODO + 1;
            //}
            //else
            //{
            //    periodo = p.PERIODO;
            //}
            if (p == null)
            {
                return fecha.Year;
            }

            return p.EJERCICIO + p.SUMA;
        }

        public DateTime getPrimerDia(int ejercicio, int periodo)
        {
            TAT001Entities db = new TAT001Entities();
            DateTime dia = new DateTime();
            if (periodo == 1)
            {
                ejercicio--;
                periodo = 13;
            }
            List<PERIODO445> pp = db.PERIODO445.Where(a => a.EJERCICIO == ejercicio).ToList();
            PERIODO445 p = pp.Where(a => a.MES_NATURAL == periodo - 1).FirstOrDefault();
            if (p == null)
            {
                dia = new DateTime(ejercicio, 1, 1);
            }
            else
            {
                dia = new DateTime(ejercicio, p.PERIODO, p.DIA_NATURAL);
                dia = dia.AddDays(1);
            }

            return dia;
        }

        public DateTime getUltimoDia(int ejercicio, int periodo)
        {
            TAT001Entities db = new TAT001Entities();
            DateTime dia = new DateTime();
            List<PERIODO445> pp = db.PERIODO445.Where(a => a.EJERCICIO == ejercicio).ToList();
            PERIODO445 p = pp.Where(a => a.MES_NATURAL == periodo).FirstOrDefault();
            if (p == null)
            {
                dia = new DateTime(ejercicio, 1, 1);
            }
            else
            {
                dia = new DateTime(ejercicio, p.PERIODO, p.DIA_NATURAL);
            }

            return dia;
        }

        public DateTime getNextViernes(DateTime f)
        {
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)f.DayOfWeek + 7) % 7;
            DateTime nextFridat = f.AddDays(daysUntilFriday);
            return nextFridat;
        }

        public DateTime getNextLunes(DateTime f)
        {
            int daysUntilMonday = ((int)DayOfWeek.Monday - (int)f.DayOfWeek + 7) % 7;

            DateTime nextMonday = f.AddDays(daysUntilMonday);
            return nextMonday;
        }

        //ROMG 13/09/18 BEGIN----------------------------------------------------
        public int fechaAint(DateTime fecha) 
        {
            if (fecha != null)
            {
                return (int)(fecha.Date - new DateTime(1900, 1, 1)).TotalDays + 2;
            }
            else
                fecha = new DateTime(0);
            return (int)(fecha.Date - new DateTime(1900, 1, 1)).TotalDays + 2; ;
        }

        public int getUltimoDiaNum(int ejercicio, int periodo)
        {
            TAT001Entities db = new TAT001Entities();
            DateTime dia = new DateTime();
            List<PERIODO445> pp = db.PERIODO445.Where(a => a.EJERCICIO == ejercicio).ToList();
            PERIODO445 p = pp.Where(a => a.MES_NATURAL == periodo).FirstOrDefault();
            if (p == null)
            {
                dia = new DateTime(ejercicio, 1, 1);
            }
            else
            {
                dia = new DateTime(ejercicio, p.PERIODO, p.DIA_NATURAL);
            }

            return (int)(dia.Date - new DateTime(1900, 1, 1)).TotalDays + 2;
        }
        //ROMG 13/09/18 END----------------------------------------------------
    }
}