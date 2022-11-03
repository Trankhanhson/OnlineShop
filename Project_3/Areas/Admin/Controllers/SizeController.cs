using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class SizeController : Controller
    {
        public JsonResult getAllData()
        {
            List<ProductSize> productSizeList = new ProductSizeDAO().getAll();
            var result = JsonConvert.SerializeObject(productSizeList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // POST: Admin/Size/Create
        [HttpPost]
        public ActionResult Create(ProductSize proSize)
        {
            try
            {             
                ProductSizeDAO dao = new ProductSizeDAO();
                var newPz = dao.Insert(proSize);
                return Json(new
                {
                    message = true,
                    newPz = newPz
                });
            }
            catch
            {
                return Json(new
                {
                    message = false
                });
            }
        }

        // POST: Admin/Size/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductSize proSize)
        {
            bool check = true;
            try
            {
                ProductSizeDAO dao = new ProductSizeDAO();
                check = dao.Update(proSize);
                return Json(check);
            }
            catch
            {
                return Json(check);
            }
        }

        // POST: Admin/Size/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            string message = "";
            bool check = true;
            try
            {
                ProductSizeDAO dao = new ProductSizeDAO();
                check = dao.Delete(id);
                if (check)
                {
                    message = "Xóa thành công";
                }
                else
                {
                    message = "Kích thước này đang được dùng ở sản phẩm";
                }
            }
            catch
            {
                message = "Xóa thất bại";
            }
            return Json(new
            {
                message = message,
                check = check
            });
        }
    }
}
