using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Common;
using TAT001.Entities;
using TAT001.Models;

namespace TAT001.Controllers
{
    public class ModificacionesGlobalesController : Controller
    {
        readonly TAT001Entities db = new TAT001Entities();
        
        // GET: ModificacionesGlobales
        public ActionResult Index()
        {
            int pagina_id = 240;//ID EN BASE DE DATOS
            FnCommon.ObtenerConfPage(db, pagina_id, User.Identity.Name, this.ControllerContext.Controller);

            ModificacionesGlobalesViewModel modelView = new ModificacionesGlobalesViewModel();
            return View(modelView);
        }
        
        }
}