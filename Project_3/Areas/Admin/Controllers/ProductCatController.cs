using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Project_3.Areas.Admin.Controllers
{
    public class ProductCatController : BaseController
    {
        // GET: Admin/ProductCat
        public ActionResult Index()
        {
            //Danh sách danh mục
            CategoryDAO categoryDAO = new CategoryDAO();
            ViewBag.CatList = categoryDAO.getAll();
            return View();
        }

        public JsonResult getAllData()
        {
            List<ProductCat> list = new ProductCategoryDAO().getAll();

            //loại bỏ các phần tử bị lặp và tuần tự hóa thành json
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(list, Formatting.Indented, jss);
            var firstCatId = new CategoryDAO().getAll().First().CatID;
            return Json(new
            {
                result = result,
                firstCatId = firstCatId
            },JsonRequestBehavior.AllowGet);
        }

        public JsonResult getNameImg()
        {
            List<ProductCat> list = new ProductCategoryDAO().getAll();
            var resultList = list.Select(pc => new ProductCat()
            {
                ProCatId = pc.ProCatId,
                Name = pc.Name,
                Image = pc.Image
            });
            var result = JsonConvert.SerializeObject(resultList);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getById(int id)
        {
            ProductCat productCat = new ProductCategoryDAO().getById(id);
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(productCat, Formatting.Indented, jss);
            return Json(result, JsonRequestBehavior.AllowGet);  
        }

        // POST: Admin/ProductCat/Create
        [HttpPost]
        public JsonResult Create(ProductCat proCat)
        {
            try
            {
                proCat.Slug = common.MethodCommnon.ToUrlSlug(proCat.Name);
                proCat.Status = true; //khi thêm vào thì mặc định là true
                ProductCategoryDAO dao = new ProductCategoryDAO();
                ProductCat procat = dao.Insert(proCat);
                JsonSerializerSettings js = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
                var pc = JsonConvert.SerializeObject(procat, Formatting.Indented, js);
                return Json(new
                {
                    check = true,
                    pc = pc
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
                string path = Server.MapPath("~/Upload/CatPro/"+ ProCatId + "/");
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
        public JsonResult Edit(ProductCat proCat,string nameOldImg)
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


        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            ProductCategoryDAO dao = new ProductCategoryDAO();
            bool check = dao.ChangeSattus(id);
            return Json(check);
        }
    }
}
