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
                list = db.Products.ToList().Select(p=>new Product()
                {
                    ProId = p.ProId,
                    Price = p.Price,
                    ProName = p.ProName,
                    ProductVariations = p.ProductVariations.Select(pv => new ProductVariation()
                    {
                        ProId = pv.ProId,
                        ProVariationID = pv.ProVariationID,                       
                        ProductColor = new ProductColor() { ProColorID = pv.ProColorID.Value, NameColor = pv.ProductColor.NameColor, ImageColor = pv.ProductColor.ImageColor },
                        ProductSize = new ProductSize() { ProSizeID = pv.ProSizeID.Value, NameSize = pv.ProductSize.NameSize },
                        Quantity = pv.Quantity,
                        Ordered = pv.Ordered,
                        DisplayImage = p.ProductImages.Where(pi => pi.ProID == p.ProId && pi.ProColorID == pv.ProColorID).FirstOrDefault().Image
                    }).ToList(),
                    ProductImages = p.ProductImages.Select(pi=>new ProductImage()
                    {
                        ProColorID = pi.ProColorID,
                        Image = pi.Image,
                        ImageColor = pi.ProductColor.ImageColor
                    }).ToList()
                }).ToList();
            }
            else
            {
                list = db.Products.Where(p => p.ProductCat.Category.type == o).ToList().Select(p => new Product()
                {
                    ProId = p.ProId,
                    Price = p.Price,
                    ProName = p.ProName,
                    ProductVariations = p.ProductVariations.Select(pv => new ProductVariation()
                    {
                        ProId = pv.ProId,
                        ProVariationID = pv.ProVariationID,                       
                        ProductColor = new ProductColor() { ProColorID = pv.ProColorID.Value, NameColor = pv.ProductColor.NameColor, ImageColor = pv.ProductColor.ImageColor },
                        ProductSize = new ProductSize() { ProSizeID = pv.ProSizeID.Value, NameSize = pv.ProductSize.NameSize },
                        Quantity = pv.Quantity,
                        Ordered = pv.Ordered,
                        DisplayImage = p.ProductImages.Where(pi => pi.ProID == p.ProId && pi.ProColorID == pv.ProColorID).FirstOrDefault().Image
                    }).ToList(),
                    ProductImages = p.ProductImages.Select(pi=>new ProductImage()
                    {
                        ProColorID = pi.ProColorID,
                        Image = pi.Image,
                        ImageColor = pi.ProductColor.ImageColor
                    }).ToList()
                }).ToList();

            }
            List<Product> listProDiscount = new List<Product>();
            foreach (var p in list)
            {
                var a = getDiscount(p); //reuturn a product with discountPrice and percent
                p.DiscountPrice = a.DiscountPrice;
                p.Percent = a.Percent;

                //if product discounted , add to list dicount
                if (p.DiscountPrice >= minMoney && p.DiscountPrice <= maxMoney)
                {
                    listProDiscount.Add(p);
                }
            }

            var result= JsonConvert.SerializeObject(listProDiscount);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}