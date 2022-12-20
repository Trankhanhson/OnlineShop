using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class NewClientController : Controller
    {
        private NewDAO dao = new NewDAO();
        // GET: New
        public ActionResult Index()
        {
            ViewBag.RecentNew = dao.take10Reccent();
            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<New> news = new NewDAO().getAll().Select(n => new New()
            {
                NewID = n.NewID,
                Title = n.Title,
                UserID = n.UserID,
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

        public ActionResult DetailNew(int id)
        {
            New n = dao.getById(id);
            return View(n);
        }
    }
}