using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class PositionController : Controller
    {
        // GET: Admin/Position
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<UserGroup> list = new PositionDAO().getAll().ToList().Select(p=>new UserGroup()
            {
                GroupId = p.GroupId,
                Name = p.Name,
                Credentials = p.Credentials.Select(c=>new Credential()
                {
                    GroupId=c.GroupId,
                    Role = new Role() { RoleId = c.RoleId, Name = c.Role.Name}
                }).ToList(),
            }).ToList();
            if (searchText.Trim() != "")
            {
                list = list.Where(c => MethodCommnon.ToUrlSlug(c.Name.ToLower()).Contains(MethodCommnon.ToUrlSlug(searchText.ToLower()))).ToList();
            }

            var pageData = Paggination.PagedResult(list, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        
        // POST: Admin//Create

        [HttpPost]
        public JsonResult Create(UserGroup userGroup, List<Credential> credentials)
        {
            try
            {

                UserGroup user = new PositionDAO().Insert(userGroup,credentials);

                return Json(new
                {
                    message = true,
                    userGroup = userGroup
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

        // POST: Admin//Edit/5
        [HttpPost]
        public JsonResult Edit(UserGroup userGroup, List<Credential> credentials)
        {
            
            try
            {
                PositionDAO dao = new PositionDAO();
                dao.Update(userGroup,credentials);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }

        }
        // POST: Admin/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                PositionDAO dao = new PositionDAO();
                dao.Delete(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }
    }
}