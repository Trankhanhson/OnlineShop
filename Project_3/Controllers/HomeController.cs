using Models.Framework;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Newtonsoft.Json;
using Project_3.Model;
using Project_3.common;

namespace Project_3.Controllers
{
    public class HomeController : BaseClientController
    { 


        public ActionResult HomePage()
        {
            ProductDAO productDAO = new ProductDAO();
            NewDAO newDAO = new NewDAO();
            List<Section> sections = new SectionDAO().getSectionOfPage(1);
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            foreach (var item in sections)
            {
                item.ProductSections = getListDiscountAndLike(item.ProductSections.ToList(), DiscountDetails);
            }
            var listDiscount = DiscountDetails.Select(d => d.Product).ToList();
            var listBestSale = productDAO.getBestSale(8, "all");
            var listNewProduct = productDAO.getAllActive("all").Take(8).ToList();

            ViewBag.ListDiscount = getListDiscountAndLike(listDiscount, DiscountDetails);
            ViewBag.BestSale = getListDiscountAndLike(listBestSale,DiscountDetails);
            ViewBag.NewProduct = getListDiscountAndLike(listNewProduct, DiscountDetails);

            ViewBag.ListNew = newDAO.takeRecent(3);
            return View(sections);
        }

        public JsonResult getSearchDataClient(string searchText)
        {
            var result = "";
            var check = false;
            if (searchText.Trim() != "")
            {
                List<Product> products = new ProductDAO().getAll().Select(p => new Product()
                {
                    ProId = p.ProId,
                    ProName = p.ProName,
                    Price = p.Price,
                    Status = p.Status,
                    Slug = p.Slug,
                    firstImage = p.ProductImages.First().Image
                }).ToList();
                //Convert to Json
                products = products.Where(p => p.Slug.ToLower().Contains(MethodCommnon.ToUrlSlug(searchText.ToLower()))).ToList();
                result = JsonConvert.SerializeObject(products);
                check = true;
            }

            //set maxJsonLangth for ressult
            var jsonResult = Json(new
            {
                check = check,
                result = result
            }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public ActionResult Detail(long id)
        {
            Product product = new ProductDAO().getById(id);

            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            var pDiscount = getDiscount(product,DiscountDetails);
            product.DiscountPrice = pDiscount.DiscountPrice;
            product.Percent = pDiscount.Percent;

            //lấy 10 sản phẩm cung loại
            ProductDAO dao = new ProductDAO();
            List<Product> relatedList = dao.get10ByProCat(product.ProCatId);
            foreach (var p in relatedList)
            {
                var a = getDiscount(p, DiscountDetails); //reuturn a product with discountPrice and percent
                p.DiscountPrice = a.DiscountPrice;
                p.Percent = a.Percent;
            }
            ViewBag.RelatedProduct = relatedList;
            return View(product);
        }
    }
}