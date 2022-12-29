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
    public class GirlController : BaseClientController
    {
        // GET: Girl
        public ActionResult Index()
        {
            ProductCategoryDAO productCategoryDAO = new ProductCategoryDAO();
            ViewBag.ProductCategory = productCategoryDAO.getByType("Bé gái");

            ProductDAO productDAO = new ProductDAO();
            NewDAO newDAO = new NewDAO();
            List<Section> sections = new SectionDAO().getSectionOfPage(5);
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            foreach (var item in sections)
            {
                item.ProductSections = getListDiscountAndLike(item.ProductSections.ToList(), DiscountDetails);
            }
            var listDiscount = DiscountDetails.Select(d => d.Product).Where(p => p.ProductCat.Category.type == "Bé gái").ToList();
            var listBestSale = productDAO.getBestSale(8, "Bé gái");
            var listNewProduct = productDAO.getAllActive("Bé gái").Take(8).ToList();

            ViewBag.ListDiscount = getListDiscountAndLike(listDiscount, DiscountDetails);
            ViewBag.BestSale = getListDiscountAndLike(listBestSale, DiscountDetails);
            ViewBag.NewProduct = getListDiscountAndLike(listNewProduct, DiscountDetails);
            return View(sections);
        }

        [ChildActionOnly]
        public ActionResult SubMenu()
        {
            CategoryDAO dao = new CategoryDAO();

            return PartialView(dao.getByType("Bé gái"));
        }
    }
}