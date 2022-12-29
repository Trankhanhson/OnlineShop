using Models.DAO;
using Models.Framework;
using Project_3.common;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class LoginClientController : Controller
    {
        // GET: LoginClient
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(Customer cus)
        {
            var dao = new CustomerDAO();
            var message = "";
            if (dao.CheckPhone(cus.Phone))
            {
                message = "ExistPhone";
            }
            else
            {
                cus.Password = EncryptorClient.Encrypt(cus.Password);
                cus.Status = true;
                bool check = dao.Insert(cus);
                if (check)
                {
                    message = "success";
                }
                else
                {
                    message = "fail";
                }
            }
            return Json(new
            {
                message = message
            });
        }


        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            var dao = new CustomerDAO();
            var message = "";
            try
            {
                int result = dao.Login(username, EncryptorClient.Encrypt(password));
                if (result == 0)
                {
                    message = "usename";
                }
                else if (result == -1)
                {
                    message = "password";
                }
                else
                {
                    message = "success";
                    long id = dao.getIdByUsername(username);
                    //trả về cookie có id và username
                    HttpCookie Cookie = new HttpCookie("CustomerId", id.ToString());
                    Cookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(Cookie);
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

        public ActionResult Logout()
        {
            HttpCookie Cookie = new HttpCookie("CustomerId");
            Cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(Cookie);
            return RedirectToAction("HomePage","Home");
        }

        /// <summary>
        /// View login sau khi lấy lại mk
        /// </summary>
        /// <returns></returns>  
        public ActionResult LoginView(long id)
        {
            var c = new CustomerDAO().getById(id);
            ViewBag.Phone = c.Phone;
            ViewBag.Password = EncryptorClient.Decrypt(c.Password);
            return View();
        }

        public ActionResult ConfirmPhone(string Phone)
        {
            try
            {
                ClothesShopEntities db = new ClothesShopEntities();
                string resetCode = Guid.NewGuid().ToString();
                var getUser = db.Customers.Where(c => c.Phone == Phone).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return Json(new
                    {
                        check = true,
                        resetCode = resetCode
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        check = false
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new
                {
                    check = false
                }, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            ClothesShopEntities db = new ClothesShopEntities();
            var user = db.Customers.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
            if (user != null)
            {
                ViewBag.ResetCode = id;
                return View();
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                ClothesShopEntities db = new ClothesShopEntities();
                var user = db.Customers.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                if (user != null)
                {
                    user.Password = EncryptorClient.Encrypt(model.NewPassword);
                    //gán lại ResetPasswordCode bằng rỗng
                    user.ResetPasswordCode = "";
                    //to avoid validation issues, disable it

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                return Json(new
                {
                    check = true,
                    id = user.CusID
                },JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    check = false
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}