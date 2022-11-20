using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Project_3.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchResult, int page = 1, int pageSize = 5)
        {
            ProductDAO model = new ProductDAO();
            var list = model.getPage(searchResult, page, pageSize);
            foreach (var p in list)
            {
                foreach(var pv in p.ProductVariations)
                {
                    pv.DisplayImage = p.ProductImages.Where(pi => pi.ProID == pv.ProId && pi.ProColorID == pv.ProColorID).First().Image;
                }
            }

            ViewBag.searchResult = searchResult;
            return View(list);
        }

        public JsonResult getAllData()
        {
            List<Product> products = new ProductDAO().getAll().Select(p => new Product()
            {
                ProId = p.ProId,
                ProName = p.ProName,
                ProCatId = p.ProCatId,
                Price = p.Price,
                ImportPrice = p.ImportPrice,
                Status = p.Status,
                ProductCat = new ProductCat() { Name = p.ProName, ProCatId = p.ProCatId },
                ProductVariations = p.ProductVariations.Select(pv => new ProductVariation()
                {
                    ProId = pv.ProId,
                    ProVariationID = pv.ProVariationID,
                    ProColorID = pv.ProColorID,
                    ProSizeID = pv.ProSizeID,
                    ProductColor = new ProductColor() { NameColor = pv.ProductColor.NameColor, ImageColor = pv.ProductColor.ImageColor },
                    ProductSize = new ProductSize() { NameSize = pv.ProductSize.NameSize },
                    Quantity = pv.Quantity,
                    DisplayImage = p.ProductImages.Where(pi => pi.ProID == p.ProId && pi.ProColorID == pv.ProColorID).FirstOrDefault().Image
                }).ToList()

            }).ToList();

            //Convert to Json
            var result = JsonConvert.SerializeObject(products);

            //set maxJsonLangth for ressult
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult getProductOnly()
        {
            List<Product> products = new ProductDAO().getAll();
            var listResult = products.Select(p => new Product()
            {
                ProId = p.ProId,
                Price = p.Price,
                ProName = p.ProName,
                firstImage = p.ProductImages.First().Image,
                TotalQty = CountTotalQuantity(p.ProductVariations.ToList())
            });
            var result = JsonConvert.SerializeObject(listResult);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public int CountTotalQuantity(List<ProductVariation> list)
        {
            int total = 0;
            foreach(var item in list)
            {
                total += item.Quantity.Value;
            }
            return total;
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            //Danh sách danh mục
            CategoryDAO categoryDAO = new CategoryDAO();
            ViewBag.CatList = categoryDAO.getAll();
            //các danh sách dùng để select
            //ViewBag.listproCat = new ProductCategoryDAO().getAll();
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [validateantiforgerytoken]
        public JsonResult Create(Product product, List<ProductVariation> listVariation)
        {
            //thêm sản phẩm
            ProductDAO pModel = new ProductDAO();
            product.Slug = common.MethodCommnon.ToUrlSlug(product.ProName); //chuyển tên sản phâm thành slug

            product.Status = true;
            if (product.Price==null)
            {
                product.Price = 0;
            }
            if (product.ImportPrice == null)
            {
                product.ImportPrice = 0;
            }

            long proId = pModel.Create(product);
            foreach (ProductVariation variation in listVariation)
            {
                variation.ProId = proId;
                variation.Ordered = 0;
            }
            ProductVariationDAO productVariationDAO = new ProductVariationDAO();
            productVariationDAO.Insert(listVariation);
            return Json(new
            {
                Proid = proId
            });
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(long id)
        {
            //các danh sách dùng để select
            CategoryDAO categoryDAO = new CategoryDAO();
            ViewBag.CatList = categoryDAO.getAll();

            //Lấy đối tượng muốn sửa
            var product = new ProductDAO().getById(id);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        public JsonResult Edit(Product product, List<ProductVariation> listVariation)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                ProductImagesDAO productImagesDAO = new ProductImagesDAO();
                ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                if (productDAO.Edit(product)) 
                {
                    productVariationDAO.Edit(listVariation);
                }
            }
            catch
            {

            }
            return Json(new
            {
                Proid = product.ProId
            });
        }

        // POST: Admin/Product/Delete/5
        [HttpPost]
        public JsonResult Delete(long id)
        {
            var check = false;
            try
            {
                ProductDAO productDAO = new ProductDAO();
                ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                ProductImagesDAO productImgDAO = new ProductImagesDAO();
                if (productVariationDAO.DeleteByProId(id))
                {
                    productImgDAO.Delete(id);
                    productDAO.Delete(id);
                    DeleteAllImg("~/Upload/Product/" + id); //xóa các Img của ProId từ folder
                    check = true;
                }
            }
            catch
            {

            }
            return Json(new
            {
                check = check
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteVariation(long idVariation)
        {
            ProductVariationDAO dao = new ProductVariationDAO();
            bool check = dao.Delete(idVariation);
            return Json(new
            {
                check = check
            });
        }

        [HttpPost]
        public string UploadImg(string ProId, string ProColorId, HttpPostedFileBase file, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            //xử lý upload
            ProductImage productImage = new ProductImage();
            productImage.ProID = long.Parse(ProId);
            productImage.ProColorID = long.Parse(ProColorId);

            //tạo đường dẫn có mục là idproduct
            var _path = Server.MapPath("~/Upload/Product/" + ProId + "/");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            //check ảnh chính
            if (file != null)
            {
                

                file.SaveAs(_path + ProColorId + "0" + ".jpg");
                productImage.Image = ProColorId + "0" + ".jpg"; //ảnh chính
            }
            else
            {
                productImage.Image = "";
            }

            //ảnh detail1
            if (file1 != null)
            {
                file1.SaveAs(_path + ProColorId + "1" + ".jpg");
                productImage.DetailImage1 = ProColorId + "1" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage1 = "";
            }

            //ảnh detail2
            if (file2 != null)
            {
                file2.SaveAs(_path + ProColorId + "2" + ".jpg");
                productImage.DetailImage2 = ProColorId + "2" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage2 = "";

            }

            //ảnh detail3
            if (file3 != null)
            {
                file3.SaveAs(_path + ProColorId + "3" + ".jpg");
                productImage.DetailImage3 = ProColorId + "3" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage3 = "";

            }

            //ảnh detail4
            if (file4 != null)
            {
                file4.SaveAs(_path + ProColorId + "4" + ".jpg");
                productImage.DetailImage4 = ProColorId + "4" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage4 = "";

            }

            //ảnh detail5
            if (file5 != null)
            {
                file5.SaveAs(_path + ProColorId + "5" + ".jpg");
                productImage.DetailImage5 = ProColorId + "5" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage5 = "";

            }

            //thực hiện lưu
            ProductImagesDAO productsDAO = new ProductImagesDAO();
            productsDAO.Insert(productImage);
            return "";
        }

        [HttpPost]
        public string UploadImgEdit(string ProId, string ProColorId, HttpPostedFileBase file, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5,string ListNoSelect)
        {
            ProductImagesDAO productsDAO = new ProductImagesDAO();

            string[] listCheckNoSelct = ListNoSelect.Split(',');
            //xử lý upload
            ProductImage productImage = new ProductImage();
            productImage.ProID = long.Parse(ProId);
            productImage.ProColorID = long.Parse(ProColorId);

            ProductImage oldProImg = productsDAO.getByKey(productImage.ProID, productImage.ProColorID);

            //tạo đường dẫn có mục là idproduct
            var _path = Server.MapPath("~/Upload/Product/" + ProId + "/");
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            //lưu ảnh bằng cách ProColorId + số ảnh mấy
            
            if (file != null) //nếu người dùng  upload file mới
            {
                //Xóa ảnh cũ trong folder
                DeleteFileFromFolder(ProColorId + "0" + ".jpg", ProId);

                //lưu ảnh mới
                file.SaveAs(_path + ProColorId + "0" + ".jpg");
                productImage.Image = ProColorId + "0" + ".jpg"; //ảnh chính
            }
            else
            {
                if (listCheckNoSelct[0] == "noSelect") //người dùng vẫn giữ nguyên ảnh
                {
                    productImage.Image = oldProImg.Image; //ảnh chính
                }
                else //người dùng đã xóa ảnh cũ
                {
                    //Xóa ảnh cũ trong folder và không upload ảnh mới
                    DeleteFileFromFolder(ProColorId + "0" + ".jpg", ProId);
                    productImage.Image = "";
                }   
            }

            //ảnh detail1
            if (file1 != null)
            {
                //Xóa ảnh cũ trong folder nếu có
                DeleteFileFromFolder(ProColorId + "1" + ".jpg", ProId);

                //Lưu ảnh mới
                file1.SaveAs(_path + ProColorId + "1" + ".jpg");
                productImage.DetailImage1 = ProColorId + "1" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                if (listCheckNoSelct[1] == "noSelect") //người dùng vẫn giữ nguyên ảnh
                {
                    productImage.DetailImage1 = oldProImg.DetailImage1; 
                }
                else //người dùng đã xóa ảnh cũ
                {
                    //Xóa ảnh cũ trong folder và không upload ảnh mới
                    DeleteFileFromFolder(ProColorId + "1" + ".jpg", ProId);
                    productImage.DetailImage1 = "";
                }
            }

            //ảnh detail1
            if (file2 != null)
            {
                //Xóa ảnh cũ trong folder
                DeleteFileFromFolder(ProColorId + "2" + ".jpg", ProId);

                //lưu ảnh mới
                file2.SaveAs(_path + ProColorId + "2" + ".jpg");
                productImage.DetailImage2 = ProColorId + "2" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                if (listCheckNoSelct[2] == "noSelect") //người dùng vẫn giữ nguyên ảnh
                {
                    productImage.DetailImage2 = oldProImg.DetailImage2;
                }
                else //người dùng đã xóa ảnh cũ
                {
                    //Xóa ảnh cũ trong folder và không upload ảnh mới
                    DeleteFileFromFolder(ProColorId + "2" + ".jpg", ProId);
                    productImage.DetailImage2 = "";
                }
            }

            //ảnh detail1
            if (file3 != null)
            {
                //Xóa ảnh cũ trong folder
                DeleteFileFromFolder(ProColorId + "3" + ".jpg", ProId);

                //lưu ảnh mới
                file3.SaveAs(_path + ProColorId + "3" + ".jpg");
                productImage.DetailImage3 = ProColorId + "3" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                if (listCheckNoSelct[3] == "noSelect") //người dùng vẫn giữ nguyên ảnh
                {
                    productImage.DetailImage3 = oldProImg.DetailImage3;
                }
                else //người dùng đã xóa ảnh cũ
                {
                    //Xóa ảnh cũ trong folder và không upload ảnh mới
                    DeleteFileFromFolder(ProColorId + "3" + ".jpg", ProId);
                    productImage.DetailImage3 = "";
                }
            }

            //ảnh detail1
            if (file4 != null)
            {
                //Xóa ảnh cũ trong folder
                 DeleteFileFromFolder(ProColorId + "4" + ".jpg", ProId);


                file4.SaveAs(_path + ProColorId + "4" + ".jpg");
                productImage.DetailImage4 = ProColorId + "4" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                if (listCheckNoSelct[4] == "noSelect") //người dùng vẫn giữ nguyên ảnh
                {
                    productImage.DetailImage4 = oldProImg.DetailImage4;
                }
                else //người dùng đã xóa ảnh cũ
                {
                    //Xóa ảnh cũ trong folder và không upload ảnh mới
                    DeleteFileFromFolder(ProColorId + "4" + ".jpg", ProId);
                    productImage.DetailImage4 = "";
                }
            }

            //ảnh detail1
            if (file5 != null)
            {
                //Xóa ảnh cũ trong folder
                DeleteFileFromFolder(ProColorId + "5" + ".jpg", ProId);


                file5.SaveAs(_path + ProColorId + "5" + ".jpg");
                productImage.DetailImage5 = ProColorId + "5" + ".jpg"; //ảnh chi tiết
            }
            else
            {
                if (listCheckNoSelct[5] == "noSelect") //người dùng vẫn giữ nguyên ảnh
                {
                    productImage.DetailImage5 = oldProImg.DetailImage5;
                }
                else //người dùng đã xóa ảnh cũ
                {
                    //Xóa ảnh cũ trong folder và không upload ảnh mới
                    DeleteFileFromFolder(ProColorId + "5" + ".jpg", ProId);
                    productImage.DetailImage5 = "";
                }
            }


            //thực hiện edit
            productsDAO.Edit(productImage);
            return "";
        }

        [HttpGet]
        public JsonResult ChangeStatus(long id)
        {
            bool check = true;
            try
            {
                var dao = new ProductDAO().ChangeStatus(id);
            }
            catch
            {
                check = false;
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
        public void DeleteFileFromFolder(string StrFilename,string ProId)
        {

            string strPhysicalFolder = Server.MapPath("~/Upload/Product/" + ProId + "/");

            string strFileFullPath = strPhysicalFolder + StrFilename;

            if (System.IO.File.Exists(strFileFullPath))
            {
                System.IO.File.Delete(strFileFullPath);
            }
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