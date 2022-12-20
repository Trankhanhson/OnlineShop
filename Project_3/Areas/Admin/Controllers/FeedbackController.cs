using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class FeedbackController : Controller
    {
        private FeedbackDAO feedbackDAO = new FeedbackDAO();
        // GET: Admin/Feedback
        [HasCredential(RoleID = "VIEW_FEEDBACK")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getPageData(bool StatusFeedback,string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Feedback> feedbacks = feedbackDAO.getAll(StatusFeedback).Select(f => new Feedback()
            {
                FeedbackId = f.FeedbackId,
                Status = f.Status,
                ProductVariation = new ProductVariation()
                {
                    Product = new Product() { ProName = f.ProductVariation.Product.ProName }
                },
                Customer = new Customer() { CusID = f.CusID.Value,Name = f.Customer.Name }
            }).ToList();
            if (searchText.Trim() != "")
            {
                feedbacks = feedbacks.Where(f=>f.ProductVariation.Product.ProName.Contains(searchText)).ToList();
            }

            var pageData = Paggination.PagedResult(feedbacks, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(long id)
        {
            try
            {
                feedbackDAO.ChangeStatus(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Create(Feedback feedback)
        {
            try
            {
                feedback.Status = false;
                feedback.Datetime = DateTime.Now;
                var id = feedbackDAO.Insert(feedback);
                return Json(new
                {
                    check = true,
                    idFeedback = id
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

        public ActionResult Upload(long FeedbackId, bool isEdit)
        {
            try
            {
                if (isEdit)
                {
                    DeleteFileFromFolder(FeedbackId);
                }
                string path = Server.MapPath("~/Upload/Feedback/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase pf = Request.Files[key];
                    pf.SaveAs(path + FeedbackId + ".jpg");
                }
            }
            catch
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult getById(long id)
        {
            Feedback fb = feedbackDAO.getById(id);
            var result = JsonConvert.SerializeObject(fb);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Feedback feedback)
        {
            feedback.Status = false;
            bool check = feedbackDAO.Update(feedback);
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteImgFeedback(long id)
        {
            try
            {
                DeleteFileFromFolder(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public void DeleteFileFromFolder(long feedbackId)
        {

            string strPhysicalFolder = Server.MapPath("~/Upload/Feedback/" + feedbackId + ".jpg");

            if (System.IO.File.Exists(strPhysicalFolder))
            {
                System.IO.File.Delete(strPhysicalFolder);
            }
        }

        public ActionResult getByProduct(long proId,int star, bool? Image)
        {
            List<Feedback> feedbacks = feedbackDAO.getByProduct(proId).Select(f => new Feedback()
            {
                FeedbackId = f.FeedbackId,
                Datetime = f.Datetime,
                Status = f.Status,
                Content = f.Content,
                Image = f.Image,
                Stars = f.Stars,
                ProductVariation = new ProductVariation()
                {
                    ProductSize = new ProductSize() {NameSize = f.ProductVariation.ProductSize.NameSize},
                    ProductColor = new ProductColor() { NameColor = f.ProductVariation.ProductColor.NameColor}
                },
                Customer = new Customer() { CusID = f.CusID.Value, Name = f.Customer.Name }
            }).ToList();

            if(star!=0 && Image != null)
            {
                feedbacks = feedbacks.Where(f => f.Stars == star && f.Image == Image).ToList();
            }
            else if(star!=0 && Image == null)
            {
                feedbacks = feedbacks.Where(f => f.Stars == star).ToList();
            }
            else if(Image != null && star == 0)
            {
                feedbacks = feedbacks.Where(f => f.Image == Image).ToList();
            }

            var result = JsonConvert.SerializeObject(feedbacks);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getTotalReview(long id)
        {
            var list = feedbackDAO.getByProduct(id);
            
            if(list != null)
            {
                int totalRating = 0;
                foreach(var item in list)
                {
                    totalRating += item.Stars.Value;
                }
                int length = list.Count;
                decimal ratingAverage = (decimal)totalRating/length;
                return Json(new
                {
                    check = true,
                    totalReview = length,
                    ratingAverage = ratingAverage
                },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    check = false
                },JsonRequestBehavior.AllowGet);
            }
        }
    }
}