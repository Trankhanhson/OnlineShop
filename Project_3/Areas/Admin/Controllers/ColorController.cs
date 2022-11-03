using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
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
            List<ProductColor> list = new ProductColorDAO().getAll();
            var result = JsonConvert.SerializeObject(list);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Create(ProductColor proColor)
        {
            try
            {
                ProductColor pco = new ProductColorDAO().Insert(proColor);
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

        public ActionResult Upload(long proColorId)
        {
            try
            {
                string path = Server.MapPath("~/Upload/ColorImage/" + proColorId + "/");
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

        // POST: Admin/ProductColor/Edit/5
        [HttpPost]
        public JsonResult Edit(ProductColor proColor)
        {
            bool UpdateSuccess = true;
            bool checkExistImg = true;
            try
            {
                //kiểm tra xem đã tồn tại đường dẫn với ảnh mới chưa
                string pathNew = Path.Combine("~/Upload/ColorImage/" + proColor.ProColorID + "/" + proColor.ImageColor);
                if (!(System.IO.File.Exists(pathNew))) //nếu ảnh mới chưa tồn tại
                {
                    //xóa ảnh cũ để thêm ảnh mới
                    DeleteAllImg("~/Upload/ColorImage/" + proColor.ProColorID);

                    checkExistImg = false; //sẽ upload ảnh mới
                }
                ProductColorDAO dao = new ProductColorDAO();
                UpdateSuccess = dao.Update(proColor); //update đối tượng
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

        // POST: Admin/ProductColor/Delete/5
        [HttpPost]
        public ActionResult Delete(ProductColor proColor)
        {
            ProductColorDAO dao = new ProductColorDAO();
            bool check = dao.Delete(proColor.ProColorID);
            if (check)
            {
                //xóa ảnh trong folder
                DeleteAllImg("~/Upload/ColorImage/" + proColor.ProColorID);
            }

            return Json(new
            {
                check = check
            });
        }

        public void DeleteAllImg(string pathId)
        {
            string path = Server.MapPath(pathId);
            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                EmptyFolder(directory);
            }
        }

        public void EmptyFolder(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                EmptyFolder(subdirectory);
                subdirectory.Delete();
            }
        }
    }
}
