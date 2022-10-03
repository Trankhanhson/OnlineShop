using Models.Framework;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Project_3.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            CategoryDAO model = new CategoryDAO();
            List<Category> categories = model.ListAll();
            return View(categories);
        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            var ListCategory = new CategoryDAO().ListAll();
            ViewBag.Parents = new SelectList(ListCategory, "CatId", "CatName");
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken] //tránh bị hack post liên tục
        public ActionResult Create(Category collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CategoryDAO model = new CategoryDAO();
                    model.Create(collection.CatId, collection.CatName, collection.ParentID);
                    return RedirectToAction("Index");
                }
                else{
                    ModelState.AddModelError("", "Dữ liệu không đúng");
                    return View();
                }
            }
            catch
            {
                return View(collection);
            }
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit()
        {
            var ListCategory = new CategoryDAO().ListAll();
            ViewBag.Parents = new SelectList(ListCategory, "CatId", "CatName");
            return View();
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            try
            {
                CategoryDAO model = new CategoryDAO();
                model.Edit(category);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Category/Delete/5
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
