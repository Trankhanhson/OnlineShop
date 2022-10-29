using Models;
using Models.DAO;
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
        ClothesShopEntities db = new ClothesShopEntities();
        public JsonResult getAllData()
        {
            return Json(db.ProductSizes.Select(x => new { x.ProSizeID, x.NameSize}).ToList(), JsonRequestBehavior.AllowGet);
        }
        // POST: Admin/Size/Create
        [HttpPost]
        public ActionResult Create(ProductSize pz)
        {
            try
            {
                
                

                return Json(new
                {
                    message = true,
                    
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
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool check = true;
            try
            {
                //CategoryDAO categoryDAO = new CategoryDAO();
                //check = categoryDAO.Update(category);
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
                CategoryDAO categoryDAO = new CategoryDAO();
                check = categoryDAO.Delete(id);
                if (check)
                {
                    message = "Xóa thành công";
                }
                else
                {
                    message = "Danh mục này đang được dùng ở danh mục con";
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
