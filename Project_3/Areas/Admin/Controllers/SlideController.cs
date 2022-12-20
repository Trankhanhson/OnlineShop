using Models.Framework;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_3.Areas.Admin.Controllers
{
    public class SlideController : Controller
    {
        ClothesShopEntities db = new ClothesShopEntities();
        // GET: Admin/Slide
        [HasCredential(RoleID = "VIEW_SLIDE")]
        public ActionResult Index()
        {

            return View(db.Slides.ToList());
        }

        [HttpPost]
        public ActionResult Edit(Slide slide, HttpPostedFileBase file)
        {
            try
            {
                slide.Image = true;
                if (file != null)
                {
                    string fullPath = Request.MapPath("~/Upload/Slide/" + slide.PageId + ".jpg");
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    var _path = Server.MapPath("~/Upload/Slide/");
                    file.SaveAs(_path + slide.PageId + ".jpg");
                }
                var s = db.Slides.Find(slide.SlideId);
                s.Image = slide.Image;
                s.Link = slide.Link;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}