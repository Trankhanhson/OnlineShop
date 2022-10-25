using Models.Framework;
using Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Newtonsoft.Json;

namespace Project_3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> list = productDAO.getAll();
            return View(list);
        }

        public ActionResult Detail(long id)
        {
            Product product = new ProductDAO().getById(id);

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