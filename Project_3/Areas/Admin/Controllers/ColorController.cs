using Models;
using Models.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult Create(ProductColor proColor,HttpPostedFileBase fileImage)
        {
            string _fileName = "";
            string _path = "";
            try
            {
                if (fileImage!=null)
                {
                    _fileName = Path.GetFileName(fileImage.FileName);
                    _path = Path.Combine(Server.MapPath("/Upload/ColorImage"), _fileName);
                    fileImage.SaveAs(_path);
                    proColor.ImageColor = "/Upload/ColorImage/" + _fileName;
                }
                else
                {
                    proColor.ImageColor = "/Upload/ColorImage/no-img.jpg";
                }
            }
            catch
            {
            }

            if (ModelState.IsValid)
            {
                ProductColorDAO dao = new ProductColorDAO();
                dao.Insert(proColor);
                return RedirectToAction("Index");
            }
            return View();
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

        // POST: Admin/Color/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            ProductColorDAO productColorDAO = new ProductColorDAO();
            bool check = productColorDAO.Delete(id);
            return Json(new
            {
                result = check
            });
        }
    }
}
