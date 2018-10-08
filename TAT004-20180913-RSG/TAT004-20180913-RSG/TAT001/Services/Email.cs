using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;

namespace TAT001.Services
{
    public class Email
    {
        private TAT001Entities db = new TAT001Entities();
        public void enviaMailC(decimal id, bool ban, string spras, string UrlDirectory, string page, string image)
        {
            //int pagina = 203; //ID EN BASE DE DATOS
            //ViewBag.Title = "Solicitud";
            try
            {
                if (id != 0)
                {
                    DOCUMENTO dOCUMENTO = db.DOCUMENTOes.Find(id);
                    if (dOCUMENTO != null)
                    {

                        dOCUMENTO.CLIENTE = db.CLIENTEs.Where(a => a.VKORG.Equals(dOCUMENTO.VKORG)
                                                                & a.VTWEG.Equals(dOCUMENTO.VTWEG)
                                                                & a.SPART.Equals(dOCUMENTO.SPART)
                                                                & a.KUNNR.Equals(dOCUMENTO.PAYER_ID)).First();
                        var workflow = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id)).OrderByDescending(a => a.POS).FirstOrDefault();
                        //ViewBag.acciones = db.FLUJOes.Where(a => a.NUM_DOC.Equals(id) & a.ESTATUS.Equals("P") & a.USUARIOA_ID.Equals(User.Identity.Name)).FirstOrDefault();

                        string mailt = ConfigurationManager.AppSettings["mailt"];
                        string mtest = ConfigurationManager.AppSettings["mailtest"]; //B20180803 MGC Correos
                        string mailTo = "";
                        if (mtest == "X")
                            mailTo = "rogelio.sanchez@sf-solutionfactory.com"; //B20180803 MGC Correos
                        else
                            mailTo = workflow.USUARIO.EMAIL;
                        CONMAIL conmail = db.CONMAILs.Find(mailt);
                        if (conmail != null)
                        {
                            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage(conmail.MAIL, mailTo);
                            SmtpClient client = new SmtpClient();
                            if (conmail.SSL)
                            {
                                client.Port = (int)conmail.PORT;
                                client.EnableSsl = conmail.SSL;
                                client.UseDefaultCredentials = false;
                                client.Credentials = new NetworkCredential(conmail.MAIL, conmail.PASS);
                            }
                            else
                            {
                                client.UseDefaultCredentials = true;
                                client.Credentials = new NetworkCredential(conmail.MAIL, conmail.PASS);
                            }
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.Host = conmail.HOST;


                            if (workflow == null)
                                mail.Subject = "N" + dOCUMENTO.NUM_DOC + "-" + DateTime.Now.ToShortTimeString();
                            else
                                mail.Subject = workflow.ESTATUS + dOCUMENTO.NUM_DOC + "-" + DateTime.Now.ToShortTimeString();
                            mail.IsBodyHtml = true;
                            //UrlDirectory = UrlDirectory.Substring(0, UrlDirectory.LastIndexOf("/"));
                            UrlDirectory = UrlDirectory.Replace("Solicitudes/Create", "Correos/" + page);
                            UrlDirectory = UrlDirectory.Replace("Solicitudes/Details", "Correos/" + page);
                            UrlDirectory = UrlDirectory.Replace("Solicitudes/Edit", "Correos/" + page);
                            UrlDirectory = UrlDirectory.Replace("Flujos/Procesa", "Correos/" + page);
                            //UrlDirectory += "/" + dOCUMENTO.NUM_DOC + "?mail=true"; //B20180803 MGC Correos
                            UrlDirectory += "/" + dOCUMENTO.NUM_DOC + ""; //B20180803 MGC Correos
                            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(UrlDirectory);
                            myRequest.Method = "GET";
                            WebResponse myResponse = myRequest.GetResponse();
                            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                            string result = sr.ReadToEnd();
                            sr.Close();
                            myResponse.Close();

                            //mail.Body = result;//B20180803 MGC Correos

                            mail.AlternateViews.Add(Mail_Body(result, image));//B20180803 MGC Correos
                            mail.IsBodyHtml = true;//B20180803 MGC Correos

                            client.Send(mail);

                        }

                    }
                }
                //return View(dOCUMENTO);
            }
            catch { }
        }

        private AlternateView Mail_Body(string strr, string path)
        {

            //string path = "";
            //path = HttpContext.Current.Server.MapPath("/images/logo_kellogg.png");

            //string path = "C:/Users/matias/Documents/GitHub/TAT004/TAT001/images/logo_kellogg.png";// HttpContext.Current.Server.MapPath(@"images/6792532.jpg");
            LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
            Img.ContentId = "logo_img";

            strr = strr.Replace("\"miimg_id\"", "cid:logo_img");

            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(strr, null, MediaTypeNames.Text.Html);
            AV.LinkedResources.Add(Img);
            return AV;
        }

    }
}
