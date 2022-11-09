using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Project_3.Areas.Admin.Controllers
{
    public class NewController : Controller
    {
        // GET: Admin/News
        public ActionResult Index()
        {

            return View();
        }

        public JsonResult getAllData()
        {
            List<New> list = new NewDAO().getAll();
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(list, Formatting.Indented, jss);
            var firstUserID = new UserDAO().getAll().First().UserID;
            return Json(new
            {
                result = result,
                firstUserID = firstUserID
            }, JsonRequestBehavior.AllowGet);
        }

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
                string path = Server.MapPath("~/Upload/New/" + NewID + "/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase pf = Request.Files[key];
                    pf.SaveAs(path + pf.FileName);
                }
            }
            catch
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

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
        public JsonResult Edit(New n, string nameOldImg)
        {
            bool UpdateSuccess = true;
            try
            {
                ////kiểm tra xem đã tồn tại đường dẫn với ảnh mưới chưa
                //string pathNew = Path.Combine("~/Upload/New/" + n.NewID + "/" + n.Image);
                //if (!(System.IO.File.Exists(pathNew))) //nếu ảnh cũ chưa tồn tại
                //{
                //    //xóa ảnh cũ để thêm ảnh mới
                //    string path = Server.MapPath("~/Upload/New/" + n.NewID + "/" + nameOldImg);
                //    System.IO.File.Delete(path);

                //    checkExistImg = false; //sẽ upload ảnh mới
                //}
                DeleteAllImgByIdPro(n.NewID);
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

        public void DeleteAllImgByIdPro(long idNew)
        {
            string path = Server.MapPath("~/Upload/New/" + idNew);
            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);
                EmptyFolder(directory);
            }
        }

        public void EmptyFolder(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                EmptyFolder(subdirectory);
                subdirectory.Delete();
            }
        }

        // POST: Admin/New/Delete/5
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

                    //xóa ảnh trong folder
                    string path = Server.MapPath("~/Upload/New/" + n.NewID + "/" + n.Image);
                    System.IO.File.Delete(path);

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
