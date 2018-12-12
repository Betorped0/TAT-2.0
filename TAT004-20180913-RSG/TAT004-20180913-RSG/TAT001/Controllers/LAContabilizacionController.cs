using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TAT001.Filters;

namespace TAT001.Controllers
{
    [Authorize]
    [LoginActive]
    public class LAContabilizacionController : Controller
    {
        // GET: LAContabilizacion
        public ActionResult Index()
        {

            return View();
        }

        // GET: LAContabilizacion/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LAContabilizacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LAContabilizacion/Create
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

        // GET: LAContabilizacion/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LAContabilizacion/Edit/5
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

        // GET: LAContabilizacion/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LAContabilizacion/Delete/5
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
    }
}
