using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {

        // GET: Admin/Product
        public ActionResult Index(string searchResult, int page = 1, int pageSize = 8)
        {
            ProductDAO model = new ProductDAO();
            var list = model.getPage(searchResult,page,pageSize);
            ViewBag.searchResult = searchResult;
            return View(list);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            //các danh sách dùng để select
            var ListCategory = new ProductCategoryDAO().getAll();
            ViewBag.Categories = new SelectList(ListCategory, "ProCatId", "Name");
            ViewBag.ListSize = new ProductSizeDAO().getAll();
            ViewBag.ListColor = new ProductColorDAO().getAll();
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [validateantiforgerytoken]
        public JsonResult Create(Product product,List<ProductVariation> listVariation)
        {
            //thêm sản phẩm
            ProductDAO pModel = new ProductDAO();
            product.Slug = common.MethodCommnon.ToUrlSlug(product.ProName); //chuyển tên sản phâm thành slug
            long proId = pModel.Create(product);
            foreach (ProductVariation variation in listVariation)
            {
                variation.ProId = proId;
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
            var ListCategory = new ProductCategoryDAO().getAll();
            ViewBag.Categories = new SelectList(ListCategory, "CatId", "CatName");
            ViewBag.ListSize = new ProductSizeDAO().getAll();
            ViewBag.ListColor = new ProductColorDAO().getAll();

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
                    productImagesDAO.Delete(product.ProId);
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
        public JsonResult Delete(int id)
        {
            var check = false;
            try
            {
                ProductDAO productDAO = new ProductDAO();
                ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                ProductImagesDAO productImgDAO = new ProductImagesDAO();
                if (productVariationDAO.DeleteByProId(id))
                {
                    productDAO.Delete(id);
                    productImgDAO.Delete(id);
                    check = true;
                }
            }
            catch
            {
                
            }
            return Json(new
            {
                check = check
            });
        }

        [HttpPost]
        public string UploadImg(string ProId,string ProColorId,HttpPostedFileBase file, HttpPostedFileBase file1, HttpPostedFileBase file2, HttpPostedFileBase file3, HttpPostedFileBase file4, HttpPostedFileBase file5)
        {
            //validate

            //xử lý upload
            ProductImage productImage = new ProductImage();
            productImage.ProID = long.Parse(ProId);
            productImage.ProColorID = long.Parse(ProColorId);
            //lưu file ảnh vào thư mục /Upload/Product
            //check ảnh chính
            if (file != null)
            {
                var _fileName = Path.GetFileName(file.FileName);
                var _path = Path.Combine(Server.MapPath("/Upload/Product"), _fileName);
                file.SaveAs(_path);
                productImage.Image = _fileName; //ảnh chính
            }
            else
            {
                productImage.Image = "no-img.jpg";
            }

            //ảnh detail1
            if (file1 != null)
            {
                var _file1Name = Path.GetFileName(file1.FileName);
                var _path1 = Path.Combine(Server.MapPath("/Upload/Product"), _file1Name);
                file1.SaveAs(_path1);
                productImage.DetailImage1 = _file1Name; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage1 = "no-img.jpg";
            }

            //ảnh detail2
            if (file2 != null)
            {
                var _file2Name = Path.GetFileName(file2.FileName);
                var _path2 = Path.Combine(Server.MapPath("/Upload/Product"), _file2Name);
                file2.SaveAs(_path2);
                productImage.DetailImage2 = _file2Name; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage2 = "no-img.jpg";

            }

            //ảnh detail3
            if (file3 != null)
            {
                var _file3Name = Path.GetFileName(file3.FileName);
                var _path3 = Path.Combine(Server.MapPath("/Upload/Product"), _file3Name);
                file3.SaveAs(_path3);
                productImage.DetailImage3 = _file3Name; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage3 = "no-img.jpg";

            }

            //ảnh detail4
            if (file4 != null)
            {
                var _file4Name = Path.GetFileName(file4.FileName);
                var _path4 = Path.Combine(Server.MapPath("/Upload/Product"), _file4Name);
                file4.SaveAs(_path4);
                productImage.DetailImage4 = _file4Name; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage4 = "no-img.jpg";

            }

            //ảnh detail5
            if (file5 != null)
            {
                var _file5Name = Path.GetFileName(file5.FileName);
                var _path5 = Path.Combine(Server.MapPath("/Upload/Product"), _file5Name);
                file5.SaveAs(_path5);
                productImage.DetailImage5 = _file5Name; //ảnh chi tiết
            }
            else
            {
                productImage.DetailImage5 = "no-img.jpg";

            }

            //thực hiện lưu
            ProductImagesDAO productsDAO = new ProductImagesDAO();
            productsDAO.Insert(productImage);
            return "";
        }
    }
}
