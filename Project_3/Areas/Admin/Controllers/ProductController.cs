using Models;
using Models.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index()
        {
            ProductDAO model = new ProductDAO();
            List<Product> list = model.ListAll();
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
            var ListCategory = new CategoryDAO().ListAll();
            ViewBag.Parents = new SelectList(ListCategory, "CatId", "CatName");
            ViewBag.ListSize = new ProductSizeDAO().getAll();
            ViewBag.ListColor = new ProductColorDAO().getAll();
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [validateantiforgerytoken]
        public JsonResult Create(Product product,List<ProductVariation> listVariation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //thêm sản phẩm
                    ProductDAO pModel = new ProductDAO();
                    long proId = pModel.Create(product);
                    foreach (ProductVariation variation in listVariation)
                    {
                        variation.ProId=proId;
                    }
                    ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                    productVariationDAO.Insert(listVariation);
                    return Json(new
                    {
                        Proid = proId
                    });

                }
                else
                {
                    ModelState.AddModelError("", "Dữ liệu không đúng");
                }
            }
            catch
            {
                
            }
            return Json(new
            {
                Success = false
            });
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Product/Edit/5
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

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Product/Delete/5
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
                file.SaveAs(Server.MapPath("~/Upload/Product/" + file.FileName));
                productImage.Image = "~/Upload/Product/" + file.FileName; //ảnh chính
            }
            else
            {
                productImage.Image = "~/Upload/Product/no-img.jpg";
            }

            //ảnh detail1
            if (file1 != null)
            {
                file.SaveAs(Server.MapPath("~/Upload/Product/" + file1.FileName));
                productImage.DetailImage1 = "~/Upload/Product/" + file1.FileName; //ảnh chính
            }
            else
            {
                productImage.DetailImage1 = "~/Upload/Product/no-img.jpg";
            }

            //ảnh detail2
            if (file2 != null)
            {
                file.SaveAs(Server.MapPath("~/Upload/Product/" + file2.FileName));
                productImage.DetailImage2 = "~/Upload/Product/" + file2.FileName; //ảnh chính
            }
            else
            {
                productImage.DetailImage2 = "~/Upload/Product/no-img.jpg";

            }

            //ảnh detail3
            if (file3 != null)
            {
                file.SaveAs(Server.MapPath("~/Upload/Product/" + file3.FileName));
                productImage.DetailImage3 = "~/Upload/Product/" + file3.FileName; //ảnh chính
            }
            else
            {
                productImage.DetailImage3 = "~/Upload/Product/no-img.jpg";

            }

            //ảnh detail4
            if (file4 != null)
            {
                file.SaveAs(Server.MapPath("~/Upload/Product/" + file4.FileName));
                productImage.DetailImage4 = "~/Upload/Product/" + file4.FileName; //ảnh chính
            }
            else
            {
                productImage.DetailImage4 = "~/Upload/Product/no-img.jpg";

            }

            //ảnh detail5
            if (file5 != null)
            {
                file.SaveAs(Server.MapPath("~/Upload/Product/" + file5.FileName));
                productImage.DetailImage5 = "~/Upload/Product/" + file5.FileName; //ảnh chính
            }
            else
            {
                productImage.DetailImage5 = "~/Upload/Product/no-img.jpg";

            }

            //thực hiện lưu
            ProductImagesDAO productsDAO = new ProductImagesDAO();
            productsDAO.Insert(productImage);
            return "";
        }
    }
}
