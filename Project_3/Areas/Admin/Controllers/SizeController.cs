using Models;
using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class SizeController : Controller
    {
        // GET: Admin/Size
        public ActionResult Index()
        {
            List<ProductSize> list = new ProductSizeDAO().getAll();

            return View(list);
        }

        // GET: Admin/Size/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Size/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Size/Create
        [HttpPost]
        public ActionResult Create(ProductSize pz)
        {
            try
            {
                ProductSizeDAO dao = new ProductSizeDAO();
                dao.Insert(pz);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Size/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Size/Edit/5
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

        // GET: Admin/Size/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Size/Delete/5
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
