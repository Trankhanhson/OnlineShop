using Models;
using Models.DAO;
using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class WomenController : Controller
    {
        // GET: Women
        public ActionResult Index()
        {
            ProductCategoryDAO productCategoryDAO = new ProductCategoryDAO();
            ViewBag.ProductCategory = productCategoryDAO.getAll();
            ProductDAO productDAO = new ProductDAO();
            List<Product> list = productDAO.getAll();
            return View(list);
        }

        [ChildActionOnly]
        public ActionResult SubMenu()
        {
            CategoryDAO dao = new CategoryDAO();

            return PartialView(dao.getByType("Nữ"));
        }
    }
}