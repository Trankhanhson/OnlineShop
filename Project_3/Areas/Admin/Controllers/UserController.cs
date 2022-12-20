using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;

namespace Project_3.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {

        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index()
        {
            ViewBag.ListGroups = new UserDAO().GetUserGroups();
            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<User> users = new UserDAO().getAll().Select(u=>new User()
            {
                UserID = u.UserID,
                Name = u.Name,
                UserName = u.UserName,
                UserPhone = u.UserPhone,
                UserAdress = u.UserAdress,
                Password = u.Password,
                Status = u.Status,
                GroupId = u.GroupId,
                UserGroup = new UserGroup() { Name = u.UserGroup.Name}
            }).ToList();
            if (searchText.Trim() != "")
            {
                users = users.Where(c => MethodCommnon.ToUrlSlug(c.Name).Contains(MethodCommnon.ToUrlSlug(searchText)) || c.UserPhone.Contains(searchText)).ToList();
            }

            var pageData = Paggination.PagedResult(users, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/User/Create
        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public JsonResult Create(User user)
        {
            try
            {
                UserDAO UserDAO = new UserDAO();
                if (UserDAO.ExistUserName(user.UserName.Trim()))
                {
                    return Json(new
                    {
                        check = false,
                        message = "Tên tài khoản đã tồn tại"
                    });
                }
                else
                {
                    if (user.Status == null)
                    {
                        user.Status = true;
                    }
                    user.Password = Encryptor.MD5Hash(user.Password);
                    User u = UserDAO.Insert(user);
                    return Json(new
                    {
                        check = true,
                        message = "Thêm người dùng thành công",
                        u = u
                    });
                }


            }
            catch
            {
                return Json(new
                {
                    check = false,
                    message = "Đã có lỗi xảy ra"
                });
            }
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult Edit(User user)
        {
            bool check = true;
            string message = "";
            try
            {
                UserDAO UserDAO = new UserDAO();
                if (UserDAO.ExistUserNameEdit(user))
                {
                    check = false;
                    message = "Tên đăng nhập đã tồn tại";
                }
                else
                {
                    if (user.Password.Trim() != "")
                    {
                        user.Password = Encryptor.MD5Hash(user.Password);
                    }
                    UserDAO.Update(user);
                    check = true;
                    message = "Cập nhật thành công";
                }
            }
            catch
            {
                check = false;
                message = "Đã có lỗi xảy ra";
            }
            return Json(new
            {
                check = check,
                message = message
            },JsonRequestBehavior.AllowGet);

        }
        // POST: Admin/User/Delete/5
        [HttpPost]
        [HasCredential(RoleID = "DELETE_USER")]
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
