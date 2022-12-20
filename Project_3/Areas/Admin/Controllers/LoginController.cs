using Models.DAO;
using Models.Framework;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User account)
        {
            if (ModelState.IsValid)
            {

                var dao = new UserDAO();
                var result = dao.Login(account.UserName, Encryptor.MD5Hash(account.Password),true);
                if (result == 1)
                {
                    var user = dao.getByUserName(account.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.Name = user.Name;
                    userSession.UserID = user.UserID;
                    userSession.GroupId = user.GroupId.Value;
                    var listCredential = dao.GetListCredential(user);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS,listCredential);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if(result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền đăng nhập");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập thất bại");

                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            if (Session[CommonConstants.USER_SESSION] != null)
            {
                Session[CommonConstants.USER_SESSION] = null;
            }
            return RedirectToAction("Index");
        }
    }
}