using Models.DAO;
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
            return View();
        }

        [ChildActionOnly]
        public ActionResult SubMenu()
        {
            CategoryDAO dao = new CategoryDAO();

            return PartialView(dao.getByType("Nữ"));
        }
    }
}