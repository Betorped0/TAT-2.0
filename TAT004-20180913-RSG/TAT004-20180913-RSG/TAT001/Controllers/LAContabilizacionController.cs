using ClosedXML.Excel;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Filters;
using TAT001.Models;

namespace TAT001.Controllers
{
    [Authorize]
    [LoginActive]
    public class LAContabilizacionController : Controller
    {
        private TAT001Entities db = new TAT001Entities();
        // GET: LAContabilizacion
        public ActionResult Index()
        {
            FnCommon.ObtenerConfPage(db, 560, User.Identity.Name, this.ControllerContext.Controller);
            string spras_id = ViewBag.spras_id;
            return View();
        }

        // GET: LAContabilizacion/Create
        [HttpPost]
        public ActionResult Descargar()
        {
            List<FileStreamResult> excels = new List<FileStreamResult>();
            List<SolxContabilizar> doctosxconta = db.DOCUMENTOes.Where(t => t.ESTATUS_SAP == "P").Select(x => new SolxContabilizar {NUM_DOC=x.NUM_DOC,SOCIEDAD_ID= x.SOCIEDAD_ID }).ToList();
            var cocodes = doctosxconta.DistinctBy(x => x.SOCIEDAD_ID).ToList();
            foreach (var co in cocodes)
            {
                var lista = doctosxconta.Where(t => t.SOCIEDAD_ID == co.SOCIEDAD_ID).ToList();
                var excel=  generarExcel(lista,co.SOCIEDAD_ID);
                excels.Add(excel);
            }
            var outputStream = new MemoryStream();
            using (var zip = new ZipFile())
            {
                foreach (var f in excels)
                {
                    zip.AddEntry(f.FileDownloadName, f.FileStream);
                }
                zip.Save(outputStream);
            }

            outputStream.Position = 0;
            return File(outputStream, "application/zip", "SolxContabilizar.zip");
        }

        public FileStreamResult generarExcel(List<SolxContabilizar> lst,string cocode)
        {
            ArchivoContable acontable = new ArchivoContable();
            List<ArchivoC> l_ac = new List<ArchivoC>();
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet 1");
            //foreach (var l in lst)
            //{
                acontable.generarArchivo(1100002287, 0, 0, "L", ref l_ac);
            //}
            var fila = 1;
            foreach(var la in l_ac)
            {
                worksheet.Cell(fila, 1).Value = la.tab.TIPO_DOC;
                worksheet.Cell(fila, 2).Value = la.tab.SOCIEDAD;
                worksheet.Cell(fila, 3).Value = la.tab.FECHA_DOCU;
                worksheet.Cell(fila, 4).Value = la.tab.FECHA_DOCU;
                worksheet.Cell(fila, 5).Value = la.tab.SOCIEDAD;
                worksheet.Cell(fila, 6).Value = la.tab.HEADER_TEXT;
                worksheet.Cell(fila, 7).Value = la.tab.REFERENCIA;
                worksheet.Cell(fila, 8).Value = la.tab.CALC_TAXT!=false?"1":"";
                worksheet.Cell(fila, 9).Value = la.tab.NOTA;
                worksheet.Cell(fila, 10).Value = la.tab.CORRESPONDENCIA;
                var fdetalle = fila+2;
                foreach (var d in la.det)
                {
                    worksheet.Cell(fdetalle, 1).Value = d.POS_TYPE;
                    worksheet.Cell(fdetalle, 2).Value = d.COMP_CODE;
                    worksheet.Cell(fdetalle, 3).Value = d.BUS_AREA;
                    worksheet.Cell(fdetalle, 4).Value = d.POST_KEY;
                    worksheet.Cell(fdetalle, 5).Value = d.ACCOUNT;
                    worksheet.Cell(fdetalle, 6).Value = d.COST_CENTER;
                    worksheet.Cell(fdetalle, 7).Value = d.BALANCE;
                    worksheet.Cell(fdetalle, 8).Value = d.TEXT;
                    worksheet.Cell(fdetalle, 9).Value = d.SALES_ORG;
                    worksheet.Cell(fdetalle, 10).Value = d.DIST_CHANEL;
                    worksheet.Cell(fdetalle, 11).Value = d.DIVISION;
                                   
                    worksheet.Cell(fdetalle, 15).Value = d.CUSTOMER;
                    worksheet.Cell(fdetalle, 16).Value = d.PRODUCT;
                    worksheet.Cell(fdetalle, 22).Value = d.ASSIGNMENT;
                    worksheet.Cell(fdetalle, 23).Value = "";
                    worksheet.Cell(fdetalle, 24).Value = "";
                    worksheet.Cell(fdetalle, 25).Value = "";
                    worksheet.Cell(fdetalle, 26).Value = "";
                    fdetalle += 1;
                }
                fdetalle += 2;
            }
            var fileDownloadName = "SolxContabilizar_"+cocode+"_" + DateTime.Now.ToShortDateString() + ".xlsx";
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            var fileStream = new MemoryStream();
            workbook.SaveAs(fileStream);
            fileStream.Position = 0;

            var fsr = new FileStreamResult(fileStream, contentType);
            fsr.FileDownloadName = fileDownloadName;

            return fsr;

        }


    }
}
