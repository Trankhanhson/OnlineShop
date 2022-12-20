using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Linq;

namespace Project_3.Areas.Admin.Controllers
{
    public class NewController : BaseController
    {
        // GET: Admin/News
        [HasCredential(RoleID = "VIEW_CONTENT")]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<New> news = new NewDAO().getAll().Select(n=>new New()
            {
                NewID = n.NewID,
                Title = n.Title,
                UserID = n.UserID,
                User = new User() { Name=n.User.Name},
                Status = n.Status,
                Image = n.Image,
                PublicDate = n.PublicDate
            }).ToList();
            if (searchText.Trim() != "")
            {
                news = news.Where(ne => MethodCommnon.ToUrlSlug(ne.Title).Contains(MethodCommnon.ToUrlSlug(searchText))).ToList();
            }

            var pageData = Paggination.PagedResult(news, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HasCredential(RoleID = "ADD_CONTENT")]
        public ActionResult Create()
        {
            //Danh sách nhân viên
            UserDAO dao = new UserDAO();
            ViewBag.ListUser = dao.getAll();
            return View();
        }
        // POST: Admin/New/Create
        [HttpPost]
        public JsonResult Create(New n)
        {
            try
            {
                n.Status = true;
                n.PublicDate = DateTime.Now;
                NewDAO dao = new NewDAO();
                New newHasId = dao.Insert(n);
                
                return Json(new
                {
                    check = true,
                    newHasId = newHasId
                });
            }
            catch
            {
                return Json(new
                {
                    check = false
                });
            }
        }

        public ActionResult Upload(long NewID)
        {
            try
            {
                string path = Server.MapPath("~/Upload/New/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase pf = Request.Files[key];
                    pf.SaveAs(path + NewID + ".jpg");
                }
            }
            catch
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HasCredential(RoleID = "EDIT_CONTENT")]
        public ActionResult Edit(int id)
        {
            //Danh sách nhân viên
            ViewBag.ListUser = new UserDAO().getAll();
            NewDAO dao = new NewDAO();
            New n = dao.getById(id);
            return View(n);
        }

        // POST: Admin/New/Edit/5
        [HttpPost]
        public JsonResult Edit(New n, bool editImage)
        {
            bool UpdateSuccess = true;
            try
            {
                if (editImage)
                {
                    DeleteFileFromFolder(n.NewID);
                }
                NewDAO dao = new NewDAO();
                UpdateSuccess = dao.Update(n); //update đối tượng
                //khi update thành công thành công thì upload ảnh
            }
            catch
            {
                UpdateSuccess = false;
            }
            return Json(new
            {
                UpdateSuccess = UpdateSuccess
            });
        }

        public void DeleteFileFromFolder(long newId)
        {

            string strPhysicalFolder = Server.MapPath("~/Upload/New/" + newId + ".jpg");

            if (System.IO.File.Exists(strPhysicalFolder))
            {
                System.IO.File.Delete(strPhysicalFolder);
            }
        }


        [HasCredential(RoleID = "DELETE_CONTENT")]
        [HttpPost]
        public ActionResult Delete(New n)
        {
            string message = "";
            bool check = true;
            try
            {
                NewDAO dao = new NewDAO();
                check = dao.Delete(n.NewID);
                if (check)
                {

                    DeleteFileFromFolder(n.NewID);

                    message = "Xóa thành công";
                }
                else
                {
                    message = "Loại sản phẩm này đang được dùng ở sản phẩm";
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
            NewDAO dao = new NewDAO();
            bool check = dao.ChangeStatus(id);
            return Json(check);
        }

    }
}
