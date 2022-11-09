using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using OnlineShop.Common;

namespace Project_3.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAllData()
        {
            var listUser = new UserDAO().getAll();
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(listUser, Formatting.Indented, jss);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/User/Create
        [HttpPost]
        public JsonResult Create(User user)
        {
            try
            {
                UserDAO UserDAO = new UserDAO();
                if (user.Status == null)
                {
                    user.Status = true;
                }
                User u = UserDAO.Insert(user);

                return Json(new
                {
                    message = true,
                    u = u
                });
            }
            catch
            {
                return Json(new
                {
                    message = false
                });
            }
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public JsonResult Edit(User user)
        {
            bool check = true;
            try
            {
                UserDAO UserDAO = new UserDAO();
                check = UserDAO.Update(user);
                return Json(check);
            }
            catch
            {
                return Json(check);
            }

        }
        // POST: Admin/User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string message = "";
            bool check = true;
            try
            {
                UserDAO UserDAO = new UserDAO();
                check = UserDAO.Delete(id);
                if (check)
                {
                    message = "Xóa thành công";
                }
                else
                {
                    message = "Thông tin nhân viên này đã được sửa dụng";
                }
            }
            catch
            {
                message = "Xóa thất bại";
            }
            return Json(new
            {
                message = message,
                check = check
            });
        }


        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            try
            {
                UserDAO UserDAO = new UserDAO();
                UserDAO.ChangeSattus(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}
