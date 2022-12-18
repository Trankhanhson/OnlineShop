using Models.DAO;
using Models.Framework;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Project_3.Controllers
{
    public class OutletController : BaseClientController
    {
        // GET: Outlet
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult FilterOutlet(string o, int minMoney,int maxMoney)
        {
            ClothesShopEntities db = new ClothesShopEntities();
            List<Product> list = new List<Product>();
            if (o == "All")
            {
                list = db.Products.ToList();
            }
            else
            {
                list = db.Products.Where(p => p.ProductCat.Category.type == o).ToList();
            }
            List<Product> listProDiscount = new List<Product>();
            listProDiscount = getListDiscount(list);
            listProDiscount = selectProduct(listProDiscount).Where(p=> p.DiscountPrice >= minMoney && p.DiscountPrice <= maxMoney).ToList();

            var result= JsonConvert.SerializeObject(listProDiscount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}