using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers.Catalogos
{
    public class AlertasController : Controller
    {

        readonly TAT001Entities db = new TAT001Entities();

        const string CMB_SOCIEDADES = "SOC";
        const string CMB_TIPOSSOLICITUD = "TSOL";
        const string CMB_TABS = "TAB";
        const string CMB_CAMPOS = "CAMP";


        // GET: Alertas
        public ActionResult Index()
        {
            int pagina_id = 540;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);

            AlertaViewModel modelView = new AlertaViewModel();
            modelView.alertas = db.WARNINGPs.ToList();

            return View(modelView);
        }

        // GET: Alertas/Create
        public ActionResult Create()
        {
            AlertaViewModel modelView = new AlertaViewModel();
            modelView.alertaMensajes = new List<WARNINGPT> { new WARNINGPT { SPRAS_ID = "ES" }, new WARNINGPT { SPRAS_ID = "EN" }, new WARNINGPT { SPRAS_ID = "PT" } };
            return View();
        }

        // POST: Alertas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Alertas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Alertas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Alertas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Alertas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        void ObtenerConfPage(int pagina)//ID EN BASE DE DATOS
        {
            var user = ObtenerUsuario();
            ViewBag.permisos = db.PAGINAVs.Where(a => a.ID.Equals(user.ID)).ToList();
            ViewBag.carpetas = db.CARPETAVs.Where(a => a.USUARIO_ID.Equals(user.ID)).ToList();
            ViewBag.usuario = user;
            ViewBag.returnUrl = Request.Url.PathAndQuery; ;
            ViewBag.rol = user.PUESTO.PUESTOTs.Where(a => a.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.Title = db.PAGINAs.Where(a => a.ID.Equals(pagina)).FirstOrDefault().PAGINATs.Where(b => b.SPRAS_ID.Equals(user.SPRAS_ID)).FirstOrDefault().TXT50;
            ViewBag.warnings = db.WARNINGVs.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();
            ViewBag.textos = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina) || a.PAGINA_ID.Equals(0)) && a.SPRAS_ID.Equals(user.SPRAS_ID)).ToList();

        }
        void CargarSelectList(ref AlertaViewModel modelView, string[] combos, string tab_id=null)
        {
            USUARIO user = ObtenerUsuario();
            string spras_id = user.SPRAS_ID;

            for (int i = 0; i < combos.Length; i++)
            {
                string[] combosSplit = combos[i].Split(',');
                string combo = combosSplit[0];
                string id = combosSplit.Length > 1 ? combosSplit[1] : null;

                switch (combo)
                {
                    case CMB_SOCIEDADES:
                        modelView.sociedades = db.SOCIEDADs
                            .Where(x => x.BUKRS == id || id == null)
                            .Select(x => new SelectListItem
                            {
                                Value = x.BUKRS,
                                Text = x.BUKRS
                            }).ToList();
                        break;
                    case CMB_TIPOSSOLICITUD:
                        modelView.tiposSolicitud = db.TSOLs
                            .Join(db.TSOLTs, s => s.ID, st => st.TSOL_ID, (s, st) => st)
                            .Where(x => x.SPRAS_ID == spras_id && (x.TSOL_ID == id || id == null))
                            .Select(x => new SelectListItem
                            {
                                Value = x.TSOL_ID,
                                Text = (x.TSOL_ID + "-" + x.TXT50)
                            }).ToList();
                        break;
                    case CMB_TABS:
                        modelView.tabs = db.TABs
                            .Join(db.TEXTOes, ta => ta.ID, te => te.CAMPO_ID, (ta, te) => te)
                            .Where(x => x.SPRAS_ID == spras_id && x.PAGINA_ID == 202  && (x.CAMPO_ID == id || id == null))
                            .Select(x => new SelectListItem
                            {
                                Value = x.CAMPO_ID,
                                Text = x.TEXTOS
                            }).ToList();
                        break;
                    case CMB_CAMPOS:
                       /* modelView.tabs = db.TAB_CAMPO
                            .Where(x=>x.TAB_ID== tab_id)
                            .Join(db.TEXTOes, tc => tc.CAMPO_ID, te => te.CAMPO_ID, (ta, te) => te)
                            .Where(x => x.SPRAS_ID == spras_id && (x.CAMPO_ID == id || id == null))
                            .Select(x => new SelectListItem
                            {
                                Value = x.CAMPO_ID,
                                Text = x.TEXTOS
                            }).ToList();*/
                        break;
                    default:
                        break;
                }
            }
        }

        USUARIO ObtenerUsuario()
        {
            string u = User.Identity.Name;
            return db.USUARIOs.Where(a => a.ID.Equals(u)).FirstOrDefault();
        }
        string ObtenerTextoMnj(int pagina_id, string campo_id)
        {
            USUARIO usuario = ObtenerUsuario();
            string texto = db.TEXTOes.Where(a => (a.PAGINA_ID.Equals(pagina_id) && a.SPRAS_ID.Equals(usuario.SPRAS_ID) && a.CAMPO_ID.Equals(campo_id))).FirstOrDefault().TEXTOS;
            return texto;
        }
    }
}
