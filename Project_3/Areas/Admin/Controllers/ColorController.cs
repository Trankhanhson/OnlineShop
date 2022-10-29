using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
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
        ClothesShopEntities db = new ClothesShopEntities();
        public JsonResult getAllData()
        {
            return Json(db.ProductColors.Select(x => new { x.ProColorID, x.ImageColor, x.NameColor }).ToList(),JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(ProductColor productColor)
        {
            try
            {
                ProductColor pco = new ProductColorDAO().Insert(productColor);
                return Json(new
                {
                    check = true,
                    pco = pco
                });
            }
            catch
            {
                return Json(new
                {
                    check = false
                });
            }
        }

        public ActionResult Upload(long ProCatId)
        {
            try
            {
                string path = Server.MapPath("~/Upload/CatPro/" + ProCatId + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase pf = Request.Files[key];
                    pf.SaveAs(path + pf.FileName);
                }
            }
            catch
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/ProductCat/Edit/5
        [HttpPost]
        public JsonResult Edit(ProductCat proCat, string nameOldImg)
        {
            bool UpdateSuccess = true;
            bool checkExistImg = true;
            try
            {
                //kiểm tra xem đã tồn tại đường dẫn với ảnh mưới chưa
                string pathNew = Path.Combine("~/Upload/CatPro/" + proCat.ProCatId + "/" + proCat.Image);
                if (!(System.IO.File.Exists(pathNew))) //nếu ảnh cũ chưa tồn tại
                {
                    //xóa ảnh cũ để thêm ảnh mới
                    string path = Server.MapPath("~/Upload/CatPro/" + proCat.ProCatId + "/" + nameOldImg);
                    System.IO.File.Delete(path);

                    checkExistImg = false; //sẽ upload ảnh mới
                }
                ProductCategoryDAO dao = new ProductCategoryDAO();
                UpdateSuccess = dao.Update(proCat); //update đối tượng
                //khi update thành công thành công thì upload ảnh
            }
            catch
            {
                UpdateSuccess = false;
            }
            return Json(new
            {
                checkExistImg = checkExistImg,
                UpdateSuccess = UpdateSuccess
            });
        }

        // POST: Admin/ProductCat/Delete/5
        [HttpPost]
        public ActionResult Delete(ProductCat proCat)
        {
            string message = "";
            bool check = true;
            try
            {
                ProductCategoryDAO dao = new ProductCategoryDAO();
                check = dao.Delete(proCat.ProCatId);
                if (check)
                {

                    //xóa ảnh trong folder
                    string path = Server.MapPath("~/Upload/CatPro/" + proCat.ProCatId + "/" + proCat.Image);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    message = "Xóa thành công";
                }
                else
                {
                    message = "Loại sản phẩm này đang được dùng ở sản phẩm";
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
