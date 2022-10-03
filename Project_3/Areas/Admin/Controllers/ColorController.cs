using Models;
using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class ColorController : Controller
    {
        // GET: Admin/Color
        public ActionResult Index()
        {
            List<ProductColor> list = new ProductColorDAO().getAll();
            return View(list);
        }

        // GET: Admin/Color/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Color/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Color/Create
        [HttpPost]
        public ActionResult Create(ProductColor proColor)
        {
            try
            {
                ProductColorDAO dao = new ProductColorDAO();
                dao.Insert(proColor);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Color/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Color/Edit/5
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

        // GET: Admin/Color/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Color/Delete/5
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
