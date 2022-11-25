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
    public class HomeController : Controller
    { 


        public ActionResult HomePage()
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> list = productDAO.getAll();
            var discountNow = new DiscountDAO().getDiscountNow();
            foreach (var p in list)
            {
                //kiểm tra giảm có giảm giá không
                foreach (var d in discountNow)
                {
                    foreach(var dt in d.DiscountDetails)
                    {
                        if (dt.ProId == p.ProId)
                        {
                            if (dt.TypeAmount == "0") //giảm giá theo tiền
                            {
                                p.DiscountPrice = p.Price.Value - dt.Amount.Value;
                            }
                            else  //giảm giá theo %
                            {
                                p.Percent = dt.Amount.Value;
                                p.DiscountPrice = Math.Round(p.Price.Value - ((Convert.ToDecimal(dt.Amount.Value) / 100) * p.Price.Value), 0);
                            }
                        }
                    }
                }
            }
            return View(list);
        }

        public ActionResult Detail(long id)
        {
            Product product = new ProductDAO().getById(id);
            DiscountDAO discountDAO = new DiscountDAO();
            List<DiscountProduct> list = discountDAO.CheckDiscount();
            //if (d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            //{
            //    foreach (var dt in d.DiscountDetails)
            //    {
            //        if (dt.ProId == product.ProId)
            //        {
            //            if (dt.TypeAmount == "0") //giảm giá theo tiền
            //            {
            //                product.DiscountPrice = product.Price.Value - dt.Amount.Value;
            //            }
            //            else  //giảm giá theo %
            //            {
            //                product.Percent = dt.Amount.Value;
            //                product.DiscountPrice = Math.Round(product.Price.Value - ((Convert.ToDecimal(dt.Amount.Value) / 100) * product.Price.Value), 0);
            //            }
            //        }
            //    }
            //}

            //lấy 10 sản phẩm cung loại
            ProductDAO dao = new ProductDAO();
            List<Product> relatedList = dao.get10ByProCat(product.ProCatId);
            foreach(var p in relatedList)
            {
                p.firstImage = p.ProductImages.First().Image; //thêm hình ảnh đc hiển thị đầu tiên
            }
            ViewBag.RelatedProduct = relatedList;
            return View(product);
        }

        

    }
}