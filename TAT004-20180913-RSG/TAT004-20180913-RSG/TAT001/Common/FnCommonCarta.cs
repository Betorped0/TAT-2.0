﻿using System;
using System.Collections.Generic;
using System.Linq;
using TAT001.Entities;
using TAT001.Models;
using TAT001.Services;

namespace TAT001.Common
{
    public static class FnCommonCarta
    {
       public static string ObtenerTexto(TAT001Entities db, string spras_id, string campo)
        {
            return db.TEXTOCVs.Where(x => x.SPRAS_ID == spras_id & x.CAMPO == campo).Select(x => x.TEXTO).FirstOrDefault();
        }

        public static void ObtenerCartaProductos(TAT001Entities db, DOCUMENTO d,CartaV v, string spras_id,bool guardar,
            ref List<string> lista,
            ref List<listacuerpoc> armadoCuerpoTab,
            ref List<string> armadoCuerpoTabStr,  
            ref List<int> numfilasTabla,
            ref List<string>  cabeza,
            ref bool editmonto)
        {
            int contadorTabla = 0;
            FormatosC format = new FormatosC();
            string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos
            bool fact =  db.TSOLs.First(ts => ts.ID == d.TSOL_ID).FACTURA;
           
            var con = db.DOCUMENTOPs.Select(x => new { x.NUM_DOC, x.VIGENCIA_DE, x.VIGENCIA_AL }).Where(a => a.NUM_DOC.Equals(d.NUM_DOC)).GroupBy(f => new { f.VIGENCIA_DE, f.VIGENCIA_AL }).ToList();
                foreach (var item in con)
                {
                    lista.Add(item.Key.VIGENCIA_DE.ToString() + item.Key.VIGENCIA_AL.ToString());
                }
            int indexp = 1;
            for (int i = 0; i < lista.Count; i++)
                {
                    contadorTabla = 0;

                    DateTime a1 = DateTime.Parse(lista[i].Remove(lista[i].Length / 2));
                    DateTime a2 = DateTime.Parse(lista[i].Remove(0, lista[i].Length / 2));

                    var con2 = db.DOCUMENTOPs
                                            .Join(db.MATERIALs, x => x.MATNR, y => y.ID, (x, y) => new { x, y })
                                            .Join(db.MATERIALTs, t => t.x.MATNR, z => z.MATERIAL_ID, (t, z) => new { t, z })
                                            .Where(xy => xy.t.x.NUM_DOC.Equals(d.NUM_DOC) & xy.t.x.VIGENCIA_DE == a1 && xy.t.x.VIGENCIA_AL == a2 & xy.z.SPRAS == spras_id)
                                            .Select(xyz => new
                                            {
                                                xyz.t.x.NUM_DOC,
                                                xyz.t.x.MATNR,
                                                xyz.t.y.MATERIALGP.DESCRIPCION,
                                                xyz.z.MAKTG,
                                                xyz.t.x.MONTO,
                                                xyz.t.y.PUNIT,
                                                xyz.t.x.PORC_APOYO,
                                                xyz.t.x.MONTO_APOYO,
                                                resta = (xyz.t.x.MONTO - xyz.t.x.MONTO_APOYO),
                                                xyz.t.x.PRECIO_SUG,
                                                xyz.t.x.APOYO_EST,
                                                xyz.t.x.APOYO_REAL,
                                                xyz.t.x.VOLUMEN_EST,
                                                xyz.t.x.VOLUMEN_REAL,
                                                xyz.t.x.VIGENCIA_DE,
                                                xyz.t.x.VIGENCIA_AL
                                            }).ToList();



                    //Definición si la distribución es monto o porcentaje
                    string porclass = "";//B20180710 MGC 2018.07.18 total es input o text
                    string totalm = "";//B20180710 MGC 2018.07.18 total es input o text
               
                if (d.TIPO_TECNICO == "M")
                    {
                        porclass = " tipom";
                        totalm = " total";
                    }
                    else if (d.TIPO_TECNICO == "P")
                    {
                        porclass = " tipop";
                        totalm = " ni";
                    }

                    if (con2.Count > 0)
                    {
                    if (armadoCuerpoTab!=null) {
                        foreach (var item2 in con2)
                        {

                            //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                            listacuerpoc lc1 = new listacuerpoc();
                            lc1.val = item2.MATNR.TrimStart('0');
                            lc1.clase = "ni";
                            armadoCuerpoTab.Add(lc1);

                            listacuerpoc lc2 = new listacuerpoc();
                            lc2.val = item2.DESCRIPCION;
                            lc2.clase = "ni";
                            armadoCuerpoTab.Add(lc2);

                            listacuerpoc lc3 = new listacuerpoc();
                            lc3.val = item2.MAKTG;
                            lc3.clase = "ni";
                            armadoCuerpoTab.Add(lc3);

                            //Costo unitario
                            listacuerpoc lc4 = new listacuerpoc();
                            lc4.val = format.toShow(Math.Round(item2.MONTO, 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc4.clase = "input_oper numberd input_dc mon" + porclass;
                            armadoCuerpoTab.Add(lc4);

                            //Porcentaje de apoyo
                            listacuerpoc lc5 = new listacuerpoc();
                            lc5.val = format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc5.clase = "input_oper numberd porc input_dc" + porclass;
                            armadoCuerpoTab.Add(lc5);

                            //Apoyo por pieza
                            listacuerpoc lc6 = new listacuerpoc();
                            lc6.val = format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc6.clase = "input_oper numberd costoa input_dc mon" + porclass;
                            armadoCuerpoTab.Add(lc6);

                            //Costo con apoyo
                            listacuerpoc lc7 = new listacuerpoc();
                            lc7.val = format.toShow(Math.Round(item2.resta, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc7.clase = "input_oper numberd costoa input_dc mon" + porclass;//Importante costoa para validación en vista
                            armadoCuerpoTab.Add(lc7);

                            //Precio Sugerido
                            listacuerpoc lc8 = new listacuerpoc();
                            lc8.val = format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            lc8.clase = "input_oper numberd input_dc mon" + porclass;
                            armadoCuerpoTab.Add(lc8);

                            //Modificación 9 y 10 dependiendo del campo de factura en tsol
                            //fact = true es real
                            //Volumen
                            listacuerpoc lc9 = new listacuerpoc();
                            if (fact)
                            {
                                lc9.val = format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                lc9.val = format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            lc9.clase = "input_oper numberd input_dc num" + porclass;
                            armadoCuerpoTab.Add(lc9);

                            //Apoyo
                            listacuerpoc lc10 = new listacuerpoc();
                            if (fact)
                            {
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales);//B20180730 MGC 2018.07.30 Formatos
                            }
                            lc10.clase = "input_oper numberd input_dc mon" + totalm + "" + porclass;
                            armadoCuerpoTab.Add(lc10);

                            contadorTabla++;
                        }
                    }
                    if (armadoCuerpoTabStr != null)
                    {
                        foreach (var item2 in con2)
                        {
                            if (guardar) {
                               int pos = db.CARTAs.Where(a => a.NUM_DOC.Equals(d.NUM_DOC)).OrderByDescending(a => a.POS).First().POS;
                                GuardarCartaPMateriales(db, v, item2.MATNR, pos,ref indexp, fact, item2.VIGENCIA_DE.Value,item2.VIGENCIA_AL.Value);
                            }
                            armadoCuerpoTabStr.Add(item2.MATNR.TrimStart('0'));
                            armadoCuerpoTabStr.Add(item2.DESCRIPCION);
                            armadoCuerpoTabStr.Add(item2.MAKTG);
                            armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                         //armadoCuerpoTab.Add(Math.Round(item2.PORC_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                            armadoCuerpoTabStr.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                                  //armadoCuerpoTab.Add(Math.Round(item2.MONTO_APOYO, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                            armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                               //armadoCuerpoTab.Add(Math.Round(item2.resta, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                            armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.resta, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                         //armadoCuerpoTab.Add(Math.Round(item2.PRECIO_SUG, 2).ToString());//B20180730 MGC 2018.07.30 Formatos
                            armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                           //B20180726 MGC 2018.07.26
                                                                                                           //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_EST), 2).ToString());
                                                                                                           //armadoCuerpoTab.Add(Math.Round(Convert.ToDouble(item2.APOYO_REAL), 2).ToString());
                                                                                                           //Volumen y apoyo
                            if (fact)
                            {
                                armadoCuerpoTabStr.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTabStr.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                armadoCuerpoTabStr.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTabStr.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                            }
                            contadorTabla++;
                        }
                    }
                    }
                    else
                    {
                        var con3 = db.DOCUMENTOPs
                                            .Where(x => x.NUM_DOC.Equals(d.NUM_DOC) & x.VIGENCIA_DE == a1 && x.VIGENCIA_AL == a2)
                                            .Join(db.MATERIALGPs, x => x.MATKL, y => y.ID, (x, y) => new
                                            {
                                                x.NUM_DOC,
                                                x.MATNR,
                                                x.MATKL,
                                                y.ID,
                                                x.MONTO,
                                                x.PORC_APOYO,
                                                TXT50 = y.DESCRIPCION,//RSG 03.10.2018
                                                x.MONTO_APOYO,
                                                resta = (x.MONTO - x.MONTO_APOYO),
                                                x.PRECIO_SUG,
                                                x.APOYO_EST,
                                                x.APOYO_REAL,
                                                x.VOLUMEN_EST,
                                                x.VOLUMEN_REAL,
                                                x.VIGENCIA_DE,
                                                x.VIGENCIA_AL
                                            }) //B20180710 MGC 2018.07.10 Se agregó x.VOLUMEN_EST, x.VOLUMEN_REAL})
                                            .ToList();
                    if (d.TIPO_TECNICO == "P")
                    {
                        editmonto = true;
                    }

                    if (armadoCuerpoTab != null)
                    {
                        foreach (var item2 in con3)
                        {
                             //B20180710 MGC 2018.07.10 Modificaciones para editar los campos de distribución se agrego los objetos
                            listacuerpoc lc1 = new listacuerpoc();
                            lc1.val = "";
                            lc1.clase = "ni";
                            armadoCuerpoTab.Add(lc1);

                            listacuerpoc lc2 = new listacuerpoc();
                            lc2.val = item2.MATKL;
                            lc2.clase = "ni";
                            armadoCuerpoTab.Add(lc2);

                            listacuerpoc lc3 = new listacuerpoc();
                            lc3.val = item2.TXT50;
                            lc3.clase = "ni";
                            armadoCuerpoTab.Add(lc3);

                            //Costo unitario
                            listacuerpoc lc4 = new listacuerpoc();
                            lc4.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc4.clase = "ni";
                            armadoCuerpoTab.Add(lc4);

                            //Porcentaje de apoyo
                            listacuerpoc lc5 = new listacuerpoc();
                            //Definición si la distribución es monto o porcentaje
                            if (d.TIPO_TECNICO == "M")
                            {
                                lc5.val = format.toShowPorc(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            }
                            else if (d.TIPO_TECNICO == "P")
                            {
                                lc5.val = format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                            }

                            lc5.clase = "ni";
                            armadoCuerpoTab.Add(lc5);

                            //Apoyo por pieza
                            listacuerpoc lc6 = new listacuerpoc();
                            lc6.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc6.clase = "ni";
                            armadoCuerpoTab.Add(lc6);

                            //Costo con apoyo
                            listacuerpoc lc7 = new listacuerpoc();
                            lc7.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc7.clase = "ni";
                            armadoCuerpoTab.Add(lc7);

                            //Precio Sugerido
                            listacuerpoc lc8 = new listacuerpoc();
                            lc8.val = format.toShow(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            lc8.clase = "ni";
                            armadoCuerpoTab.Add(lc8);
                            //Modificación 9 y 10 dependiendo del campo de factura en tsol
                            //fact = true es real

                            //Volumen
                            listacuerpoc lc9 = new listacuerpoc();
                            if (fact)
                            {
                                lc9.val = format.toShowNum(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                lc9.val = format.toShowNum(0, decimales); //B20180730 MGC 2018.07.30 Formatos
                            }
                            lc9.clase = "ni";
                            armadoCuerpoTab.Add(lc9);

                            //Apoyo
                            listacuerpoc lc10 = new listacuerpoc();
                            if (fact)
                            {
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                            }
                            else
                            {
                                lc10.val = format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales); //B20180730 MGC 2018.07.30 Formatos
                            }
                            //Definición si la distribución es monto o porcentaje
                            if (d.TIPO_TECNICO == "M")
                            {
                                lc10.clase = "input_oper numberd input_dc total cat mon";
                            }
                            else if (d.TIPO_TECNICO == "P")
                            {
                                lc10.clase = "ni";
                            }

                            armadoCuerpoTab.Add(lc10);

                            contadorTabla++;
                        }
                    }
                    if (armadoCuerpoTabStr != null)
                    {
                        foreach (var item2 in con3)
                        {
                            if (guardar)
                            {
                                int pos = db.CARTAs.Where(a => a.NUM_DOC.Equals(d.NUM_DOC)).OrderByDescending(a => a.POS).First().POS;
                                GuardarCartaPCategorias(db, v, item2.MATKL, pos, ref indexp, fact, item2.VIGENCIA_DE.Value, item2.VIGENCIA_AL.Value);
                            }
                           
                            if (v != null)
                            {
                                DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();
                                docmod = v.DOCUMENTOP.Where(x => x.MATKL_ID == item2.MATKL).FirstOrDefault();

                                armadoCuerpoTabStr.Add("");
                                armadoCuerpoTabStr.Add(item2.MATKL);
                                //armadoCuerpoTab.Add(item2.TXT50);
                                MATERIALGP mt = db.MATERIALGPs.Where(x => x.ID == item2.MATKL).FirstOrDefault();//RSG 03.10.2018
                                if (mt != null)
                                    armadoCuerpoTabStr.Add(mt.DESCRIPCION);//RSG 03.10.2018
                                else
                                    armadoCuerpoTabStr.Add("");

                                if (v.costoun_x == true)
                                {
                                    armadoCuerpoTabStr.Add(format.toShow(Math.Round(docmod.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.apoyo_x == true)
                                {
                                    armadoCuerpoTabStr.Add(format.toShowPorc(Math.Round(docmod.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.apoyop_x == true)
                                {
                                    armadoCuerpoTabStr.Add(format.toShow(Math.Round(docmod.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }

                                if (v.costoap_x == true)
                                {
                                    armadoCuerpoTabStr.Add(format.toShow(Math.Round((docmod.MONTO - docmod.MONTO_APOYO), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                if (v.precio_x == true)
                                {
                                    armadoCuerpoTabStr.Add(format.toShow(Math.Round(docmod.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }

                                //Volumen
                                //B20180726 MGC 2018.07.26
                                if (v.volumen_x == true)
                                {
                                    if (fact)
                                    {
                                        armadoCuerpoTabStr.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                                                                   //carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                                    }
                                    else
                                    {
                                        armadoCuerpoTabStr.Add(format.toShowNum(Math.Round(Convert.ToDecimal(docmod.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                                                                                                                                  //carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                                    }
                                }

                                //Apoyo
                                //B20180726 MGC 2018.07.26
                                if (v.apoyototal_x == true)
                                {
                                    if (fact)
                                    {
                                        armadoCuerpoTabStr.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    }
                                    else
                                    {
                                        armadoCuerpoTabStr.Add(format.toShow(Math.Round(Convert.ToDecimal(docmod.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    }
                                }
                            }
                            else
                            {
                                armadoCuerpoTabStr.Add("");
                                armadoCuerpoTabStr.Add(item2.MATKL);
                                armadoCuerpoTabStr.Add(item2.TXT50);
                                armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.MONTO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTabStr.Add(format.toShowPorc(Math.Round(item2.PORC_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.MONTO_APOYO, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.resta, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                armadoCuerpoTabStr.Add(format.toShow(Math.Round(item2.PRECIO_SUG, 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                //B20180726 MGC 2018.07.26
                                if (fact)
                                {
                                    armadoCuerpoTabStr.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTabStr.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_REAL), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }
                                else
                                {
                                     armadoCuerpoTabStr.Add(format.toShowNum(Math.Round(Convert.ToDecimal(item2.VOLUMEN_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                    armadoCuerpoTabStr.Add(format.toShow(Math.Round(Convert.ToDecimal(item2.APOYO_EST), 2), decimales));//B20180730 MGC 2018.07.30 Formatos
                                }

                            }

                            contadorTabla++;
                        }
                    }
                    }
                    numfilasTabla.Add(contadorTabla);
                }

                //var cabeza = new List<string>(); //B20180720P MGC 2018.07.25
                cabeza.Add(ObtenerTexto(db, spras_id, "materialC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "categoriaC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "descripcionC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "costouC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "apoyopoC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "apoyopiC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "costoaC"));
                cabeza.Add(ObtenerTexto(db, spras_id, "preciosC"));
                //B20180710 MGC 2018.07.12 Apoyo es real o es estimado
                //Volumen
                if (fact)
                {
                    cabeza.Add(ObtenerTexto(db, spras_id, "volumenrC"));
                }
                else
                {
                    cabeza.Add(ObtenerTexto(db, spras_id, "volumeneC"));
                }
                //Apoyo
                if (fact)
                {
                    cabeza.Add(ObtenerTexto(db, spras_id, "apoyorC"));
                }
                else
                {
                    cabeza.Add(ObtenerTexto(db, spras_id, "apoyoeC"));
                }
            
        }


     

      static  void GuardarCartaPMateriales(TAT001Entities db,CartaV v,string MATNR,int pos,ref int indexp,bool fact,DateTime VIGENCIA_DE,DateTime VIGENCIA_AL)
        {
            DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();
            try
            {
                docmod = v.DOCUMENTOP.Where(x => x.MATNR == MATNR.TrimStart('0')).FirstOrDefault();
                if (docmod != null )
                {
                    CARTAP carp = new CARTAP();
                    //Armado para registro en bd
                    carp.NUM_DOC = v.num_doc;
                    carp.POS_ID = pos;
                    carp.POS = indexp;
                    carp.MATNR = MATNR;
                    carp.MATKL = "";
                    carp.CANTIDAD = 1;
                    carp.MONTO = docmod.MONTO;
                    carp.PORC_APOYO = docmod.PORC_APOYO;
                    carp.MONTO_APOYO = docmod.MONTO_APOYO;
                    carp.PRECIO_SUG = docmod.PRECIO_SUG;

                    //Volumen
                    //B20180726 MGC 2018.07.26
                    if (v.volumen_x == true)
                    {
                        if (fact)
                        {
                            carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                            carp.VOLUMEN_EST = 0;
                        }
                        else
                        {
                            carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                            carp.VOLUMEN_REAL = 0;
                        }
                    }

                    //Apoyo
                    //B20180726 MGC 2018.07.26
                    if (v.apoyototal_x == true)
                    {
                        if (fact)
                        {
                            carp.APOYO_REAL = docmod.APOYO_REAL;
                            carp.APOYO_EST = 0;
                        }
                        else
                        {
                            carp.APOYO_EST = docmod.APOYO_EST;
                            carp.APOYO_REAL = 0;
                        }
                    }

                    //Fechas
                    carp.VIGENCIA_DE = VIGENCIA_DE;
                    carp.VIGENCIA_AL = VIGENCIA_AL;

                    try
                    {
                        //Guardar en CARPETAP
                        db.CARTAPs.Add(carp);
                        db.SaveChanges();
                        indexp++;
                    }
                    catch (Exception e)
                    {
                        Log.ErrorLogApp(e, "Carta", "Create");
                    }
                }


            }
            catch (Exception e)
            {
                Log.ErrorLogApp(e, "Carta", "Create");
            }
        }

      static void GuardarCartaPCategorias(TAT001Entities db, CartaV v, string MATKL, int pos, ref int indexp, bool fact, DateTime VIGENCIA_DE, DateTime VIGENCIA_AL)
        {
            DOCUMENTOP_MOD docmod = new DOCUMENTOP_MOD();

            try
            {
                docmod = v.DOCUMENTOP.Where(x => x.MATKL_ID == MATKL).FirstOrDefault();

                if (docmod != null)
                {
                    CARTAP carp = new CARTAP();
                    //Armado para registro en bd
                    carp.NUM_DOC = v.num_doc;
                    carp.POS_ID = pos;
                    carp.POS = indexp;
                    carp.MATNR = "";
                    carp.MATKL = MATKL;
                    carp.CANTIDAD = 1;
                    carp.MONTO = docmod.MONTO;
                    carp.PORC_APOYO = docmod.PORC_APOYO;
                    carp.MONTO_APOYO = docmod.MONTO_APOYO;
                    carp.PRECIO_SUG = docmod.PRECIO_SUG;

                    //Volumen
                    //B20180726 MGC 2018.07.26
                    if (v.volumen_x == true)
                    {
                        if (fact)
                        {
                            carp.VOLUMEN_REAL = docmod.VOLUMEN_REAL;
                            carp.VOLUMEN_EST = 0;
                        }
                        else
                        {
                            carp.VOLUMEN_EST = docmod.VOLUMEN_EST;
                            carp.VOLUMEN_REAL = 0;
                        }
                    }

                    //Apoyo
                    //B20180726 MGC 2018.07.26
                    if (v.apoyototal_x == true)
                    {
                        if (fact)
                        {
                            carp.APOYO_REAL = docmod.APOYO_REAL;
                            carp.APOYO_EST = 0;
                        }
                        else
                        {
                            carp.APOYO_REAL = 0;
                            carp.APOYO_EST = docmod.APOYO_EST;
                        }
                    }

                    //Fechas
                    carp.VIGENCIA_DE = VIGENCIA_DE;
                    carp.VIGENCIA_AL = VIGENCIA_AL;

                    try
                    {
                        //Guardar en CARPETAP
                        db.CARTAPs.Add(carp);
                        db.SaveChanges();
                        indexp++;
                    }
                    catch (Exception e)
                    {
                        Log.ErrorLogApp(e, "Carta", "Create");
                    }
                }
            }
            catch (Exception e)
            {
                Log.ErrorLogApp(e, "Carta", "Create");
            }
        }
      public static void ObtenerCartaRecurrentes(TAT001Entities db, DOCUMENTO d, string spras_id,
                 ref List<string> cabeza2,
                 ref List<string> armadoCuerpoTab2,
                 ref int rowsRecs,
                 bool esPdf)
        {
            FormatosC format = new FormatosC();
            string decimales = d.PAI.DECIMAL; //B20180730 MGC 2018.07.30 Formatos
            bool varligada = Convert.ToBoolean(d.LIGADA);

            cabeza2.Add(ObtenerTexto(db, spras_id, "posC2"));
            cabeza2.Add(ObtenerTexto(db, spras_id, "tipoC2"));
            cabeza2.Add(ObtenerTexto(db, spras_id, "fechaC2"));
            if (varligada)
            {
                cabeza2.Add(ObtenerTexto(db, spras_id, "objetivo"));
                cabeza2.Add(ObtenerTexto(db, spras_id, "porcentajeC2"));
            }
            else
            {
                if (d.TIPO_TECNICO == "P")
                {
                    if (!esPdf)
                    {
                        cabeza2.Add("");
                    }
                    cabeza2.Add(ObtenerTexto(db, spras_id, "porcentajeC2"));
                }
                else
                {
                    cabeza2.Add(ObtenerTexto(db, spras_id, "montoC2"));
                    if (!esPdf)
                    {
                        cabeza2.Add("");
                    }
                }
            }

            var con4 = db.DOCUMENTORECs
                                    .Where(x => x.NUM_DOC.Equals(d.NUM_DOC))
                                    .Join(db.DOCUMENTOes, x => x.NUM_DOC, y => y.NUM_DOC, (x, y) => new { x.POS, y.TSOL_ID, x.FECHAF, x.MONTO_BASE, x.PORC })
                                    .ToList();

            foreach (var item in con4)
            {
                DateTime a = Convert.ToDateTime(item.FECHAF);

                armadoCuerpoTab2.Add(item.POS.ToString());
                armadoCuerpoTab2.Add(db.TSOLs.Where(x => x.ID == item.TSOL_ID).Select(x => x.DESCRIPCION).First());
                armadoCuerpoTab2.Add(a.ToShortDateString());
                if (varligada)
                {
                    DOCUMENTORAN docRan = db.DOCUMENTORANs.First(x => x.LIN == 1 && x.NUM_DOC == d.NUM_DOC);
                    armadoCuerpoTab2.Add(format.toShow(Math.Round(docRan.OBJETIVOI.Value, 2), decimales));
                    armadoCuerpoTab2.Add(docRan.PORCENTAJE.Value.ToString("##.00"));
                }
                else
                {
                    if (d.TIPO_TECNICO == "P")
                    {
                        if (!esPdf)
                        {
                            armadoCuerpoTab2.Add("");
                        }
                        armadoCuerpoTab2.Add(item.PORC.Value.ToString("##.00"));
                    }
                    else
                    {
                        armadoCuerpoTab2.Add(format.toShow(Math.Round(item.MONTO_BASE.Value, 2), decimales));
                        if (!esPdf)
                        {
                            armadoCuerpoTab2.Add("");
                        }
                    }
                }
            }
            rowsRecs = con4.Count();
        }
    }
}