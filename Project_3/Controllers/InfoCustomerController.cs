using Models.Framework;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_3.common;

namespace Project_3.Controllers
{
    public class InfoCustomerController : Controller
    {
        // GET: InfoCustomer
        public ActionResult InfoAccount()
        {
            Customer customer = new Customer();
            if (Request.Cookies["CustomerId"] != null)
            {
                long id = int.Parse(Request.Cookies["CustomerId"].Value);
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
            var cusOld = dao.getById(cus.CusID);
            try
            {
                if (cus.Email != cusOld.Email && dao.CheckEmail(cus.Email))
                {
                    message = "ExistEmail";
                }
                else if (cus.Phone != cusOld.Phone && dao.CheckPhone(cus.Phone))
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
            if (Request.Cookies["CustomerId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}