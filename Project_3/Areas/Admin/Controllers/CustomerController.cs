using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Admin/Customer
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Customer> customers = new CustomerDAO().getAll();
            if (searchText.Trim() != "")
            {
                customers = customers.Where(c=>c.Phone.Contains(searchText) || c.Email.Contains(searchText)).ToList();
            }

            var pageData = Paggination.PagedResult(customers, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeStatus(long id)
        {
            bool check = new CustomerDAO().ChangeSattus(id);
            return Json(check,JsonRequestBehavior.AllowGet);
        }
    }
}