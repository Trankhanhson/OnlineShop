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
            return View(product);
        }
    }
}