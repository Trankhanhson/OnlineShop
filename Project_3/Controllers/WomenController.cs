using Models;
using Models.DAO;
using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class WomenController : BaseClientController
    {
        // GET: Women
        public ActionResult Index()
        {
            ProductCategoryDAO productCategoryDAO = new ProductCategoryDAO();
            ViewBag.ProductCategory = productCategoryDAO.getByType("Nữ");
            ProductDAO productDAO = new ProductDAO();
            NewDAO newDAO = new NewDAO();
            List<Section> sections = new SectionDAO().getSectionOfPage(3);
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            foreach (var item in sections)
            {
                item.ProductSections = getListDiscountAndLike(item.ProductSections.ToList(), DiscountDetails);
            }
            var listDiscount = DiscountDetails.Select(d => d.Product).Where(p => p.ProductCat.Category.type == "Nữ").ToList();
            var listBestSale = productDAO.getBestSale(8, "Nữ");
            var listNewProduct = productDAO.getAllActive("Nữ").Take(8).ToList();

            ViewBag.ListDiscount = getListDiscountAndLike(listDiscount, DiscountDetails);
            ViewBag.BestSale = getListDiscountAndLike(listBestSale, DiscountDetails);
            ViewBag.NewProduct = getListDiscountAndLike(listNewProduct, DiscountDetails);
            return View(sections);
        }

        [ChildActionOnly]
        public ActionResult SubMenu()
        {
            CategoryDAO dao = new CategoryDAO();

            return PartialView(dao.getByType("Nữ"));
        }
    }
}