using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            //Danh sách dối tượng
            var listType = new List<string>()
            {
                "Nam","Nữ","Bé trai","Bé gái"
            };
            ViewBag.TypeSelect = listType;

            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Category> categories = new CategoryDAO().getAll().Select(c=>new Category()
            {
                CatID = c.CatID,
                Name = c.Name,
                type = c.type,
                Status = c.Status
            }).ToList();
            if (searchText.Trim() != "")
            {
                categories = categories.Where(pc => pc.Slug.Contains(MethodCommnon.ToUrlSlug(searchText.ToLower()))).ToList();
            }

            var pageData = Paggination.PagedResult(categories,pageNumber,pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getByType(string id)
        {
            List<Category> categories = new CategoryDAO().getByType(id).Select(c => new Category()
            {
                CatID = c.CatID,
                Name = c.Name,
                type = c.type,
                Status = c.Status,
                ProductCats = c.ProductCats.Select(pc=>new ProductCat() { ProCatId=pc.ProCatId, Name = pc.Name}).ToList()
            }).ToList();
            var result = JsonConvert.SerializeObject(categories);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Category/Create

        [HttpPost]
        public JsonResult Create(Category category)
        {
            try
            {
                category.Slug = common.MethodCommnon.ToUrlSlug(category.Name);
                CategoryDAO categoryDAO=new CategoryDAO();
                Category cat = categoryDAO.Insert(category);

                return Json(new
                {
                    message = true,
                    cat = cat
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

        // POST: Admin/Category/Edit/5
        [HttpPost]
        public JsonResult Edit(Category category)
        {
            bool check = true;
            try
            {
                CategoryDAO categoryDAO = new CategoryDAO();
                check = categoryDAO.Update(category);
                return Json(check);
            }
            catch
            {
                return Json(check);
            }
            
        }
        // POST: Admin/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string message = "";
            bool check = true;
            try
            {
                CategoryDAO categoryDAO = new CategoryDAO();
                check = categoryDAO.Delete(id);
                if (check)
                {
                    message = "Xóa thành công";
                }
                else
                {
                    message = "Danh mục này đang được dùng ở danh mục con";
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
            }) ;
        }


        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            bool check = categoryDAO.ChangeSattus(id);
            return Json(check);
        }

    }
}
