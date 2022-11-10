using Models.Framework;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Common;

namespace Project_3.Controllers
{
    public class InfoCustomerController : Controller
    {
        // GET: InfoCustomer
        public ActionResult InfoAccount()
        {
            Customer customer = new Customer();
            if (Session["Customer"] != null)
            {
                long id = (long)Session["Customer"];
                customer = new CustomnerDAO().getById(id);
                return View(customer);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult UpdateInfoCustomer(Customer cus)
        {
            var dao = new CustomnerDAO();
            var message = "";
            try
            {
                if (dao.CheckEmail(cus.Email))
                {
                    message = "ExistEmail";
                }
                else if (dao.CheckPhone(cus.Phone))
                {
                    message = "ExistPhone";
                }
                else
                {
                    dao.Update(cus);
                    message = "success";
                }
            }
            catch
            {
                message = "fail";
            }
            return Json(new
            {
                message = message
            });
        }

        public ActionResult OrderHistory()
        {
            return View();
        }
    }
}