using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Project_3.Areas.Admin.Controllers
{
    public class SectionController : BaseController
    {
        // GET: Admin/Section
        [HasCredential(RoleID = "VIEW_SECTION")]
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult getPageData(int PageId, string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Section> sections = new SectionDAO().getSectionOfPage(PageId).Select(s=>new Section()
            {
                SectionId = s.SectionId,
                Title = s.Title,    
                Page = new Page() { PageId = s.PageId.Value, PageName = s.Page.PageName},
                DisplayOrder = s.DisplayOrder,
            }).ToList();
            if (searchText.Trim() != "")
            {
                searchText = searchText.ToLower();
                sections = sections.Where(s=>MethodCommnon.ToUrlSlug(s.Title.ToLower()).Contains(MethodCommnon.ToUrlSlug(searchText))).ToList();
            }

            var pageData = Paggination.PagedResult(sections, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductOnly(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Product> products = new ProductDAO().getAll();
            if (searchText.Trim() != "")
            {
                var a = MethodCommnon.ToUrlSlug(searchText.ToLower());
                products = products.Where(p => p.Slug.Contains(a)).ToList();
            }
            List<ProductDiscount> listResult = products.Select(p => new ProductDiscount()
            {
                ProId = p.ProId,
                Price = p.Price,
                ProName = p.ProName,
                Slug = p.Slug,
                DiscountPrice = 0,
                firstImage = p.ProductImages.First().Image,
                TotalQty = CountTotalQuantity(p.ProductVariations.ToList()),
                Check = false
            }).ToList();
            listResult = getListDiscount(listResult);
            var pageData = Paggination.PagedResult(listResult, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public int CountTotalQuantity(List<ProductVariation> list)
        {
            int total = 0;
            foreach (var item in list)
            {
                total += item.Quantity.Value-item.Ordered.Value;
            }
            return total;
        }

        [HasCredential(RoleID = "ADD_SECTION")]
        public ActionResult Create(int id)
        {
            SectionDAO dao = new SectionDAO();
            var list = dao.getSectionOfPage(id);
            ViewBag.length = list.Count;
            ViewBag.PageId = id;
            ViewBag.PageName = dao.getPageName(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(int pageId,Section s, List<ProductSection> productSections)
        {
            try
            {
                SectionDAO sectionDAO = new SectionDAO();
                //nếu muốn thay thế vị trí của section cũ
                var newDisplayOrder = sectionDAO.getSectionOfPage(pageId).Count + 1;
                if (s.DisplayOrder != newDisplayOrder)
                {
                    //chuyển vị trị của section bị thay thế về sau cùng
                    sectionDAO.ChangeDisplayOrder(s.DisplayOrder.Value, newDisplayOrder);
                }
                int sectionId = sectionDAO.Insert(s,productSections);
                return Json(new
                {
                    check = true,
                    sectionId = sectionId
                },JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new
                {
                    check = false
                },JsonRequestBehavior.AllowGet);
            }
        }

        [HasCredential(RoleID = "EDIT_SECTION")]
        public ActionResult Edit(int sectionId,int pageId)
        {
            ViewBag.Id = sectionId;
            ViewBag.length = new SectionDAO().getSectionOfPage(pageId).Count;
            return View();
        }
        
        public ActionResult getById(int id)
        {
            SectionDAO dao = new SectionDAO();
            Section s = dao.getById(id);
            Section sResult = new Section()
            {
                SectionId = s.SectionId,
                Title = s.Title,
                Image1 = s.Image1,
                Image2 = s.Image2,
                Image3 = s.Image3,
                Link1 = s.Link1,
                Link2 = s.Link2,
                Link3 = s.Link3,
                PageId = s.PageId,
                Page = new Page() { PageName = s.Page.PageName },
                DisplayOrder = s.DisplayOrder
            };
            List<ProductDiscount> listProduct = dao.getProBySection(id).Select(p=>new ProductDiscount()
            {
                ProId = p.ProId,
                ProName = p.ProName,
                Price = p.Price,
                DiscountPrice = 0,
                firstImage = p.ProductImages.First().Image
            }).ToList();
            listProduct = getListDiscount(listProduct);
            var result = JsonConvert.SerializeObject(listProduct);
            return Json(new
            {
                s = sResult,
                listProduct = result
            },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Section s, List<ProductSection> productSections)
        {
            try
            {
                SectionDAO dao = new SectionDAO();
                dao.Edit(s, productSections);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HasCredential(RoleID = "DELETE_SECTION")]
        public ActionResult Delete(int id)
        {
            try
            {
                SectionDAO dao = new SectionDAO();
                //rearranging displayOrder
                var section = dao.getById(id);
                var sectionOfPage = dao.getSectionOfPage(section.PageId.Value);
                if(sectionOfPage != null)
                {
                    foreach (var item in sectionOfPage)
                    {
                        if (item.DisplayOrder.Value > section.DisplayOrder.Value)
                        {
                            item.DisplayOrder = item.DisplayOrder - 1;
                        }
                    }
                }
                dao.Delete(section);
                dao.SaveChange();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult Upload(long sectionId)
        {
            try
            {
                //tạo đường dẫn có mục là idproduct
                var _path = Server.MapPath("~/Upload/Section/" + sectionId + "/");
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase pf = Request.Files[key];
                    if (key.Contains("file1"))
                    {
                        pf.SaveAs(_path + "1.jpg");
                    }
                    else if (key.Contains("file2"))
                    {
                        pf.SaveAs(_path + "2.jpg");
                    }
                    else if (key.Contains("file3"))
                    {
                        pf.SaveAs(_path + "3.jpg");
                    }
                }
               
            }
            catch
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadEdit(long sectionId, List<string> listSignal)
        {
            try
            {
                //tạo đường dẫn có mục là idproduct
                var _path = Server.MapPath("~/Upload/Section/" + sectionId + "/");
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }
                for(int i = 0;i< listSignal.Count; i++)
                {
                    if (listSignal[i]=="add" || listSignal[i] == "delete")
                    {
                        DeleteFileFromFolder(sectionId, i+1);
                    }
                }
                foreach (string key in Request.Files)
                {
                    HttpPostedFileBase pf = Request.Files[key];
                    if (key.Contains("file1"))
                    {
                        pf.SaveAs(_path + "1.jpg");
                    }
                    else if (key.Contains("file2"))
                    {
                        pf.SaveAs(_path + "2.jpg");
                    }
                    else if (key.Contains("file3"))
                    {
                        pf.SaveAs(_path + "3.jpg");
                    }
                }

            }
            catch
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public void DeleteFileFromFolder(long sectionId,int numberImage)
        {

            string strPhysicalFolder = Server.MapPath("~/Upload/Section/"+ sectionId + "/"+ numberImage + ".jpg");

            if (System.IO.File.Exists(strPhysicalFolder))
            {
                System.IO.File.Delete(strPhysicalFolder);
            }
        }
        public void DeleteAllImg(string pathId)
        {
            string path = Server.MapPath(pathId);
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
    }
}