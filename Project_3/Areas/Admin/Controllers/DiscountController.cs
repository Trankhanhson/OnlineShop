using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.Framework;

namespace Project_3.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        // GET: Admin/Discount
        public ActionResult Index()
        {
            return View();
        }

        // GET: Admin/Discount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Discount/Create
        [HttpPost]
        public ActionResult Create(DiscountProduct discountPro, List<DiscountDetail> listDiscountDetail)
        {
            try
            {
                DiscountDAO discountDAO = new DiscountDAO();
                int id = discountDAO.Insert(discountPro);

                foreach(var item in listDiscountDetail)
                {
                    item.DiscountProductId = id;
                }

                DiscountDetailDAO discountDetailDAO = new DiscountDetailDAO();
                discountDetailDAO.Insert(listDiscountDetail);

                return Json(true);
            }
            catch
            {
                return Json(true);
            }
        }

        // GET: Admin/Discount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Discount/Edit/5
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

        // GET: Admin/Discount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Discount/Delete/5
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
