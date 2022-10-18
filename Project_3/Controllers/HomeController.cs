using Models.Framework;
using Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ProductDAO productDAO = new ProductDAO();
            return View(productDAO.getAll());
        }

    }
}