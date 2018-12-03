using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClosedXML.Excel;

namespace TAT001.Services
{
    public class ExcelExport
    {
        public static string getRuta()
        {
            return "~/PdfTemp/";
        }

        public static string generarExcelHome(List<ExcelExportColumn> columnas, dynamic data, string nombreReporte, string ruta)
        {
            string archivo = "";
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            try
            {
                //int columna = 0;
                //for (int cell1 = 64; cell1 <= 90 && columna < columnas.Count; cell1++)
                //{
                //    for(int cell2 = 65; cell2 <= 90 && columna < columnas.Count; cell2++)
                //    {
                //        string celda;
                //        if (cell1 == 64)
                //            celda = (char)cell2 + "1";
                //        else
                //            celda = (char)cell1 + (char)cell2 + "1";
                //        worksheet.Cell(celda).Value = new[] { new { Banner = columnas[columna].titulo } };
                //        for(int registro =0; registro < data.Count; registro++)
                //        {
                //            if (cell1 == 64)
                //                celda = (char)cell2 + (registro + 2).ToString();
                //            else
                //                celda = (char)cell1 + (char)cell2 + (registro + 2).ToString();
                //            worksheet.Cell(celda).Value = new[] { new { Banner = data[registro].GetType().GetProperty(columnas[columna].columnaDatos).GetValue(data[registro], null) } };
                //        }
                //        columna++;
                //    }
                //}
                for (int columna = 0; columna < columnas.Count; columna++)
                {
                    worksheet.Cell(1, columna + 1).Value = columnas[columna].titulo;
                    for (int registro = 0; registro < data.Count; registro++)
                    {
                        worksheet.Cell(registro + 2, columna + 1).Value = ((columnas[columna].forceTextFormat)? "'" : String.Empty) + data[registro].GetType().GetProperty(columnas[columna].columnaDatos).GetValue(data[registro], null);
                        if (!String.IsNullOrEmpty(columnas[columna].hyperlink))
                        {
                            try
                            {
                                worksheet.Cell(registro + 2, columna + 1).Hyperlink = new XLHyperlink(new Uri(columnas[columna].hyperlink + "/" + data[registro].GetType().GetProperty(columnas[columna].columnaDatos).GetValue(data[registro], null)));
                            } catch (Exception e) { }
                        }
                    }
                }
                //worksheet.Cell("A1").Value = new[]
                //{
                //  new {
                //      BANNER = "Recurrencia"
                //      },
                //};
                archivo = nombreReporte + DateTime.Now.ToString("HHmmyyyyMMdd") + ".xlsx";
                workbook.SaveAs(ruta + @"\" + archivo);
                return archivo;
                // generarExcelHome(listaDocs.OrderByDescending(t => t.FECHAD).ThenByDescending(t => t.HORAC).ThenByDescending(t => t.NUM_DOC).ToList(), Server.MapPath("~/PdfTemp/"));
                // return File(Server.MapPath("~/pdfTemp/Doc" + DateTime.Now.ToShortDateString() + ".xlsx"), "application /vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doc" + DateTime.Now.ToShortDateString() + ".xlsx");
            }
            catch (Exception e)
            {
                var ex = e.ToString();
            }
            return archivo;
        }

    }

    public class ExcelExportColumn
    {
        public string titulo { get; set; }
        public string columnaDatos { get; set; }
        public string hyperlink { get; set; }
        public bool forceTextFormat { get; set; }

        public ExcelExportColumn(string titulo, string datos)
        {
            this.titulo = titulo;
            this.columnaDatos = datos;
            this.forceTextFormat = false;
        }

        public ExcelExportColumn(string titulo, string datos, bool textFormat)
        {
            this.titulo = titulo;
            this.columnaDatos = datos;
            this.forceTextFormat = textFormat;
        }

        public ExcelExportColumn(string titulo, string datos, string link, bool textFormat)
        {
            this.titulo = titulo;
            this.columnaDatos = datos;
            this.hyperlink = link;
            this.forceTextFormat = textFormat;
        }
    }
}