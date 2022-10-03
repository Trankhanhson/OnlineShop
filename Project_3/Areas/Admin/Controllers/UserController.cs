using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.Framework;
using OnlineShop.Common;

namespace Project_3.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        // GET: Admin/User2
        public ActionResult Index()
        {
            UserDAO userDAO = new UserDAO();
            List<User> list = userDAO.getAll();
            return View(list);
        }

        // GET: Admin/User2/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new UserDAO();

                    //mã hóa mk
                    var Md5Pas = Encryptor.MD5Hash(user.Password);
                    user.Password = Md5Pas;
                    dao.Insert(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Nhập không thỏa mãn");
                }
            }
            catch
            {


            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            User user = new UserDAO().getById(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var dao = new UserDAO();
                    if (string.IsNullOrEmpty(user.Password))
                    {
                        var Md5Pas = Encryptor.MD5Hash(user.Password);
                        user.Password= Md5Pas;
                    }
                    bool check = dao.Update(user);
                    if (check)
                    {
                        return RedirectToAction("Index","User");
                    }
                    else
                    {
                        ModelState.AddModelError("","Sửa không thành công");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nhập không thỏa mãn");
                }
            }
            catch
            {

            }
            return View(user);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            bool check = new UserDAO().Delete(id);
            if (check)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Xóa thất bại");
            }
            return View();
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var rerult = new UserDAO().ChangeSattus(id);
            return Json(new
            {
                status = rerult
            });
        }
    }
}
