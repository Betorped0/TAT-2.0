using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        const string CMB_TIPOS = "TIP";
        const string CMB_CONDCAMPOS = "CCAMP";
        const string CMB_CONDVALORES = "CVAL";


        // GET: Alertas
        public ActionResult Index()
        {
            int pagina_id = 540;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);

            AlertaViewModel modelView = new AlertaViewModel();
            modelView.alertas = db.WARNINGPs.ToList();
            modelView.alertaMensajes = db.WARNINGPTs.ToList();
            modelView.alertaCondiciones = db.WARNING_COND.ToList();
            CargarSelectList(ref modelView, new string[] { CMB_CONDVALORES });
            return View(modelView);
        }

        // GET: Alertas/Create
        public ActionResult Create()
        {
            int pagina_id = 541;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);

            AlertaViewModel modelView = new AlertaViewModel();
            modelView.alertaMensajes = new List<WARNINGPT> { new WARNINGPT { SPRAS_ID = "ES" }, new WARNINGPT { SPRAS_ID = "EN" }, new WARNINGPT { SPRAS_ID = "PT" } };
            modelView.alertaCondiciones = new List<WARNING_COND> { new WARNING_COND {POS=1 },  new WARNING_COND { POS=2, ORAND=")"} };
            CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES, CMB_TIPOSSOLICITUD, CMB_TABS,CMB_TIPOS, CMB_CONDCAMPOS, CMB_CONDVALORES });
            return View(modelView);
        }

        // POST: Alertas/Create
        [HttpPost]
        public ActionResult Create(AlertaViewModel modelView)
        {
            int pagina_id = 541;//ID EN BASE DE DATOS
            try
            {
                WARNINGP warningP= modelView.alerta;
                List<WARNINGPT> warningts = modelView.alertaMensajes;
                List<WARNING_COND> warningconds = modelView.alertaCondiciones;

                warningP.ID = warningP.ID.Replace(" ","_");
                warningP.ACCION = "focusout";
                warningP.CAMPOVAL_ID = warningP.CAMPO_ID;

                if (!ValidarAlertaExistente(warningP, warningconds))
                {
                    throw new Exception();
                }
                //Guardar Alerta
                db.WARNINGPs.Add(warningP);
                //Guardar Mensajes de la Alerta
                warningts.ForEach(x=>
                {
                    x.TAB_ID = warningP.TAB_ID;
                    x.WARNING_ID = warningP.ID;
                    db.WARNINGPTs.Add(x);
                });
                //Guardar Condiciones de la Alerta
                warningconds.ForEach(x =>
                {
                    if (x.CONDICION_ID!=null && x.VALOR_COMP!=null) {
                        x.TAB_ID = warningP.TAB_ID;
                        x.WARNING_ID = warningP.ID;
                        x.ACTIVO = true;
                        db.WARNING_COND.Add(x);
                    }
                });
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ObtenerConfPage(pagina_id);
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES, CMB_TIPOSSOLICITUD, CMB_TABS, CMB_TIPOS,CMB_CAMPOS }, modelView.alerta.TAB_ID);
                return View(modelView);
            }
        }

        // GET: Alertas/Edit
        public ActionResult Edit(string warning_id, string tab_id)
        {
            int pagina_id = 542;//ID EN BASE DE DATOS
            ObtenerConfPage(pagina_id);

            AlertaViewModel modelView = new AlertaViewModel();
            modelView.alerta = db.WARNINGPs.Where(x=>x.ID== warning_id && x.TAB_ID== tab_id).FirstOrDefault();
            if (modelView.alerta==null){ return RedirectToAction("Index");}

            modelView.alertaMensajes = new List<WARNINGPT>();         
            modelView.alertaMensajes.Add(ObtenerWarningT("ES", warning_id, tab_id));
            modelView.alertaMensajes.Add(ObtenerWarningT("EN", warning_id, tab_id));
            modelView.alertaMensajes.Add(ObtenerWarningT("PT", warning_id, tab_id));

            modelView.alertaCondiciones = db.WARNING_COND.Where(x=>x.WARNING_ID== warning_id && x.TAB_ID == tab_id).ToList();
            if (modelView.alerta.SOCIEDAD_ID!=null && modelView.alerta.TSOL_ID!=null && modelView.alertaCondiciones.Count()==1)
            {
                modelView.alertaCondiciones.Add( new WARNING_COND { POS = 2, ORAND = ")" } );
            }

            CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + modelView.alerta.SOCIEDAD_ID, CMB_TIPOSSOLICITUD + "," + modelView.alerta.TSOL_ID, CMB_TABS + "," + modelView.alerta.TAB_ID, CMB_TIPOS,CMB_CAMPOS + "," + modelView.alerta.CAMPO_ID, CMB_CONDCAMPOS, CMB_CONDVALORES }, modelView.alerta.TAB_ID);
            return View(modelView);
        }

        // POST: Alertas/Edit
        [HttpPost]
        public ActionResult Edit(AlertaViewModel modelView)
        {
            int pagina_id = 542;//ID EN BASE DE DATOS          
            try
            {
                WARNINGP warningP = modelView.alerta;
                List<WARNINGPT> warningts = modelView.alertaMensajes;
                List<WARNING_COND> warningconds = modelView.alertaCondiciones;

                //Guardar Alerta
                db.Entry(warningP).State = EntityState.Modified;
                //Guardar Mensajes de la Alerta
                warningts.ForEach(x =>
                {
                    if (x.TAB_ID==null && x.WARNING_ID ==null )
                    {
                        x.TAB_ID = warningP.TAB_ID;
                        x.WARNING_ID = warningP.ID;
                        db.WARNINGPTs.Add(x);
                    }
                    else
                    {
                        db.Entry(x).State = EntityState.Modified;                     
                    }              
                });
                //Guardar Condiciones de la Alerta
                warningconds.ForEach(x =>
                {
                    if (x.TAB_ID == null && x.WARNING_ID == null && x.CONDICION_ID != null && x.VALOR_COMP != null)
                    {
                        x.TAB_ID = warningP.TAB_ID;
                        x.WARNING_ID = warningP.ID;
                        x.ACTIVO = true;
                        db.WARNING_COND.Add(x);                     
                    }
                    else if (x.CONDICION_ID != null && x.VALOR_COMP != null)
                    {
                        db.Entry(x).State = EntityState.Modified;
                    }
                    
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ObtenerConfPage(pagina_id);
                CargarSelectList(ref modelView, new string[] { CMB_SOCIEDADES + "," + modelView.alerta.SOCIEDAD_ID, CMB_TIPOSSOLICITUD + "," + modelView.alerta.TSOL_ID, CMB_TABS + "," + modelView.alerta.TAB_ID, CMB_TIPOS, CMB_CAMPOS + "," + modelView.alerta.CAMPO_ID, CMB_CONDCAMPOS, CMB_CONDVALORES }, modelView.alerta.TAB_ID);
                return View(modelView);
            }
        }

        // GET: Alertas/Delete
        public ActionResult Delete(string warning_id, string tab_id)
        {
            WARNINGP warningP= db.WARNINGPs.Where(x => x.ID == warning_id && x.TAB_ID == tab_id).FirstOrDefault();
            if (warningP == null) { return RedirectToAction("Index"); }
            List<WARNINGPT> warningts = db.WARNINGPTs.Where(x => x.WARNING_ID == warning_id && x.TAB_ID == tab_id).ToList();
            List<WARNING_COND> warningconds= db.WARNING_COND.Where(x => x.WARNING_ID == warning_id && x.TAB_ID == tab_id).ToList();

            db.WARNING_COND.RemoveRange(warningconds);
            db.WARNINGPTs.RemoveRange(warningts);
            db.WARNINGPs.Remove(warningP);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        

        [HttpPost]
        public ActionResult ObtenerCampos(string id)
        {
            AlertaViewModel modelView = new AlertaViewModel();
            CargarSelectList(ref modelView, new string[] { CMB_CAMPOS }, id);
            return Json(modelView.campos);
        }

        WARNINGPT ObtenerWarningT(string spras_id, string warning_id,string tab_id)
        {
            WARNINGPT mensaje = db.WARNINGPTs.Where(x => x.WARNING_ID == warning_id && x.TAB_ID== tab_id && x.SPRAS_ID == spras_id).FirstOrDefault();
            return (mensaje == null ? new WARNINGPT { SPRAS_ID = spras_id } : mensaje);
        }
        bool ValidarAlertaExistente(WARNINGP warningP,List<WARNING_COND> warningConds)
        {
            int pagina_id = 540;
            if (db.WARNINGPs.Any(x=>x.ID== warningP.ID && x.TAB_ID==warningP.TAB_ID))
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjExisteAlerta");
                return false;
            }

            bool existeCondParaTabCampo = false;
            List<WARNING_COND> warningPs=db.WARNINGPs
                .Where(x=>x.CAMPO_ID== warningP.CAMPO_ID && x.TAB_ID== warningP.TAB_ID && x.ID != warningP.ID)
                .Join(db.WARNING_COND, w => w.ID, wc => wc.WARNING_ID, (w, wc) => wc)
                .ToList();
            warningPs.ForEach(x =>
            {
                
                if (warningConds.Any(y=>y.VALOR_COMP==x.VALOR_COMP && y.CONDICION_ID==x.CONDICION_ID))
                {
                    existeCondParaTabCampo = true;                 
                }
            });
            if (existeCondParaTabCampo)
            {
                ViewBag.mnjError = ObtenerTextoMnj(pagina_id, "lbl_mnjExisteCondAlerta");
                return false;
            }

            return true;
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
            ViewBag.spras_id = user.SPRAS_ID;
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
                int? idAux = null;
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
                        modelView.campos = db.TAB_CAMPO
                            .Where(x=>x.TAB_ID== tab_id)
                            .Join(db.TEXTOes, tc => tc.CAMPO_ID, te => te.CAMPO_ID, (ta, te) => te)
                            .Where(x => x.SPRAS_ID == spras_id && x.PAGINA_ID == 202 && (x.CAMPO_ID == id || id == null))
                            .Select(x => new SelectListItem
                            {
                                Value = x.CAMPO_ID,
                                Text = x.TEXTOS
                            }).ToList();
                        break;
                    case CMB_TIPOS:
                        int pagina_id = 540;//ID EN BASE DE DATOS
                        string error = ObtenerTextoMnj(pagina_id, "lbl_error");
                        string alerta = ObtenerTextoMnj(pagina_id, "lbl_alerta");
                        modelView.tipos = new List<SelectListItem> {
                            new SelectListItem{ Value = "A", Text = "A - "+alerta},
                            new SelectListItem {Value = "E",Text = "E - "+error}
                        };
                        break;
                    case CMB_CONDCAMPOS:
                         idAux = (id == null ? null : (int?)int.Parse(id));
                        modelView.condCampos = db.CONDICIONs 
                            .Where(x=>x.ACTIVO==true && (x.COND== "=" || x.COND == "!=" || x.COND == ">" || x.COND == "<"))
                            .Join(db.CONDICIONTs, c => c.ID, ct => ct.CONDICION_ID, (c, ct) => ct)
                            .Where(x => x.SPRAS_ID == spras_id && (x.CONDICION_ID == idAux || idAux == null))
                            .Select(x => new SelectListItem
                             {
                                 Value = x.CONDICION_ID.ToString(),
                                 Text = x.TXT050
                             }).ToList();
                        modelView.condCampos1=modelView.condCampos;
                        break;
                    case CMB_CONDVALORES:
                         idAux = (id == null ? null : (int?)int.Parse(id));
                        modelView.condValores = db.CONDICIONs
                            .Where(x => x.ACTIVO == true && (x.COND == "e" || x.COND == "dec" || x.COND == "0" || x.COND == "n" || x.COND == "c" || x.COND == ""))
                            .Join(db.CONDICIONTs, c => c.ID, ct => ct.CONDICION_ID, (c, ct) => ct)
                            .Where(x => x.SPRAS_ID == spras_id && (x.CONDICION_ID == idAux || idAux == null))
                            .Select(x => new SelectListItem
                            {
                                Value = x.CONDICION.COND,
                                Text = x.TXT050
                            }).ToList();
                        modelView.condValores1=modelView.condValores;
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
