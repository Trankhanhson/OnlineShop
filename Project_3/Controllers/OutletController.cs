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
    public class OutletController : Controller
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
                    firstImage = p.ProductImages.First().Image,
                    ProductVariations = p.ProductVariations.Select(pv => new ProductVariation()
                    {
                        ProId = pv.ProId,
                        ProVariationID = pv.ProVariationID,                       
                        ProductColor = new ProductColor() { ProColorID = pv.ProColorID.Value, NameColor = pv.ProductColor.NameColor, ImageColor = pv.ProductColor.ImageColor },
                        ProductSize = new ProductSize() { ProSizeID = pv.ProSizeID.Value, NameSize = pv.ProductSize.NameSize },
                        Quantity = pv.Quantity,
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
                    firstImage = p.ProductImages.First().Image,
                    ProductVariations = p.ProductVariations.Select(pv => new ProductVariation()
                    {
                        ProId = pv.ProId,
                        ProVariationID = pv.ProVariationID,                       
                        ProductColor = new ProductColor() { ProColorID = pv.ProColorID.Value, NameColor = pv.ProductColor.NameColor, ImageColor = pv.ProductColor.ImageColor },
                        ProductSize = new ProductSize() { ProSizeID = pv.ProSizeID.Value, NameSize = pv.ProductSize.NameSize },
                        Quantity = pv.Quantity,
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
            var discountNow = new DiscountDAO().getDiscountNow();
            List<Product> listProDiscount = new List<Product>();
            foreach (var p in list)
            {
                //kiểm tra giảm có giảm giá không
                foreach (var d in discountNow)
                {
                    foreach (var dt in d.DiscountDetails)
                    {
                        if (dt.ProId == p.ProId)
                        {
                            if (dt.TypeAmount == "0") //giảm giá theo tiền
                            {
                                p.DiscountPrice = p.Price.Value - dt.Amount.Value;
                            }
                            else  //giảm giá theo %
                            {
                                p.Percent = dt.Amount.Value;
                                p.DiscountPrice = Math.Round(p.Price.Value - ((Convert.ToDecimal(dt.Amount.Value) / 100) * p.Price.Value), 0);
                            }
                            break;
                        }
                    }
                }
                if(p.DiscountPrice > 0)
                {
                    listProDiscount.Add(p);
                }
            }

            //lấy danh sách product discount thỏa mãn
            List<Product> listOutlet = new List<Product>();
            if (maxMoney == 0) //trường hợp lấy 249 trở lên
            {
                listOutlet = listProDiscount.Where(p=>p.DiscountPrice>minMoney).ToList();
            }
            else
            {
                listOutlet = listProDiscount.Where(p => p.DiscountPrice >= minMoney && p.DiscountPrice <= maxMoney).ToList();
            }

            var result= JsonConvert.SerializeObject(listOutlet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}