using Models;
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
    public class ShowCatController : BaseClientController
    {
        private CategoryDAO categoryDAO = new CategoryDAO();
        private ProductCategoryDAO productCategoryDAO = new ProductCategoryDAO();
        private ProductDAO productDAO = new ProductDAO();
        // GET: ShowCat
        public ActionResult ShowCat(int id)
        {
            Category c = categoryDAO.getByCatID(id);
            ViewBag.ListColor = new ProductColorDAO().getAll();
            return View(c);
        }

        public ActionResult getCatData(int CatId, int? colorId, int? minPrice, int? maxPrice, int pageNumber = 1, int pageSize = 20)
        {
            //lấy danh sách product của cat và lấy discount
            var listProduct = productDAO.getAll().Where(p => p.ProductCat.CatID == CatId).ToList(); 
            listProduct = getListDiscount(listProduct);

            if(colorId == null && minPrice != null && maxPrice != null)
            {
                //chỉ tìm giá
                listProduct = listProduct.Where(p =>p.DiscountPrice >=minPrice && p.DiscountPrice<=maxPrice).ToList();
            }
            else if(colorId != null && minPrice != null && maxPrice != null)
            {
                //tìm cả gái và color
                listProduct = listProduct.Where(p => p.ProductVariations.Where(pv=>pv.ProColorID == colorId).FirstOrDefault() != null &&  p.DiscountPrice >= minPrice && p.DiscountPrice <= maxPrice).ToList();
            }
            else if(colorId != null && minPrice == null && maxPrice == null)
            {
                //Chỉ tìm color
                listProduct = listProduct.Where(p => p.ProductVariations.Where(pv => pv.ProColorID == colorId).FirstOrDefault() != null).ToList();
            }

            listProduct = selectProduct(listProduct);
            var pageData = Paggination.PagedResult(listProduct, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult SideBar_ShowCat(string type, int id)
        {
            CategoryDAO dao = new CategoryDAO();
            ViewBag.CatId = id;
            return PartialView(dao.getByType(type));
        }

        public ActionResult ShowProCat(int id)
        {
            ViewBag.ListColor = new ProductColorDAO().getAll();
            return View(productCategoryDAO.getById(id));
        }

        public ActionResult getProCatData(int ProCatId, int? colorId, int? minPrice, int? maxPrice, int pageNumber = 1, int pageSize = 20)
        {
            //lấy danh sách product của cat và lấy discount
            var listProduct = productDAO.getAll().Where(p => p.ProCatId == ProCatId).ToList();
            listProduct = getListDiscount(listProduct);

            if (colorId == null && minPrice != null && maxPrice != null)
            {
                //chỉ tìm giá
                listProduct = listProduct.Where(p => p.DiscountPrice >= minPrice && p.DiscountPrice <= maxPrice).ToList();
            }
            else if (colorId != null && minPrice != null && maxPrice != null)
            {
                //tìm cả gái và color
                listProduct = listProduct.Where(p => p.ProductVariations.Where(pv => pv.ProColorID == colorId).FirstOrDefault() != null && p.DiscountPrice >= minPrice && p.DiscountPrice <= maxPrice).ToList();
            }
            else if (colorId != null && minPrice == null && maxPrice == null)
            {
                //Chỉ tìm color
                listProduct = listProduct.Where(p => p.ProductVariations.Where(pv => pv.ProColorID == colorId).FirstOrDefault() != null).ToList();
            }

            listProduct = selectProduct(listProduct);
            var pageData = Paggination.PagedResult(listProduct, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult SideBar_ShowProCat(string type, int id)
        {
            CategoryDAO dao = new CategoryDAO();
            ViewBag.ProCatId = id;
            return PartialView(dao.getByType(type));
        }

        [ChildActionOnly]
        public ActionResult SubMenu(string type)
        {
            CategoryDAO dao = new CategoryDAO();

            return PartialView(dao.getByType(type));
        }


    }
}