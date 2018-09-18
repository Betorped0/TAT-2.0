using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TAT001.Services
{
    public class Cadena
    {
        public string completaMaterial(string material)//RSG 07.06.2018---------------------------------------------
        {
            string m = material;
            try
            {
                long matnr1 = long.Parse(m);
                int l = 18 - m.Length;
                for (int i = 0; i < l; i++)
                {
                    m = "0" + m;
                }
            }
            catch
            {

            }
            return m;
        }
    }
}