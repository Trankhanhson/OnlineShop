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
            List<Product> list = productDAO.getAll();
            list = getListDiscountAndLike(list);
            return View(list);
        }

        public ActionResult Detail(long id)
        {
            Product product = new ProductDAO().getById(id);

            var pDiscount = getDiscount(product);
            product.DiscountPrice = pDiscount.DiscountPrice;
            product.Percent = pDiscount.Percent;

            //lấy 10 sản phẩm cung loại
            ProductDAO dao = new ProductDAO();
            List<Product> relatedList = dao.get10ByProCat(product.ProCatId);
            foreach (var p in relatedList)
            {
                var a = getDiscount(p); //reuturn a product with discountPrice and percent
                p.DiscountPrice = a.DiscountPrice;
                p.Percent = a.Percent;
            }
            ViewBag.RelatedProduct = relatedList;
            return View(product);
        }
    }
}