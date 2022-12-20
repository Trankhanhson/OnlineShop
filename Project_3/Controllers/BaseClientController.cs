using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.DAO;
using Project_3.Model;

namespace Project_3.Controllers
{
    public class BaseClientController : Controller
    {
        ClothesShopEntities db = new ClothesShopEntities();

        /// <summary>
        /// Trả về danh sách đã có discount
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Product> getListDiscount(List<Product> list)
        {
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            foreach(var p in list)
            {
                p.Percent = 0;
                p.DiscountPrice = 0;
                foreach (var dt in DiscountDetails)
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
            return list;
        }

        /// <summary>
        /// Trả về danh sách đã có discount vaf like
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Product> getListDiscountAndLike(List<Product> list)
        {
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            if (Request.Cookies["CustomerId"] != null)
            {
                int CusId = int.Parse(Request.Cookies["CustomerId"].Value);
                List<ProductLike> listLike = db.ProductLikes.Where(pl => pl.CusID == CusId).ToList();
                foreach (var p in list)
                {
                    p.Percent = 0;
                    p.DiscountPrice = 0;
                    p.Liked = listLike.Where(prol => prol.ProId == p.ProId).FirstOrDefault() != null;
                    foreach (var dt in DiscountDetails)
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
            }
            else
            {
                foreach (var p in list)
                {
                    p.Percent = 0;
                    p.DiscountPrice = 0;
                    foreach (var dt in DiscountDetails)
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
            }
            return list;
        }

        /// <summary>
        /// Trả về danh sách đã có discount vaf like
        /// </summary>
        /// <param name="list"></param>
        /// <returns> List<ProductSection></returns>
        public List<ProductSection> getListDiscountAndLike(List<ProductSection> listProSection, List<DiscountDetail> DiscountDetails)
        {
            if (Request.Cookies["CustomerId"] != null)
            {
                int CusId = int.Parse(Request.Cookies["CustomerId"].Value);
                List<ProductLike> listLike = db.ProductLikes.Where(pl => pl.CusID == CusId).ToList();
                foreach (var ps in listProSection)
                {
                    ps.Product.Liked = listLike.Where(prol => prol.ProId == ps.Product.ProId).FirstOrDefault() != null;
                    ps.Product.Percent = 0;
                    ps.Product.DiscountPrice = 0;
                    foreach (var dt in DiscountDetails)
                    {
                        if (dt.ProId == ps.Product.ProId)
                        {
                            if (dt.TypeAmount == "0") //giảm giá theo tiền
                            {
                                ps.Product.DiscountPrice = ps.Product.Price.Value - dt.Amount.Value;
                            }
                            else  //giảm giá theo %
                            {
                                ps.Product.Percent = dt.Amount.Value;
                                ps.Product.DiscountPrice = Math.Round(ps.Product.Price.Value - ((Convert.ToDecimal(dt.Amount.Value) / 100) * ps.Product.Price.Value), 0);
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var ps in listProSection)
                {
                    ps.Product.Percent = 0;
                    ps.Product.DiscountPrice = 0;
                    foreach (var dt in DiscountDetails)
                    {
                        if (dt.ProId == ps.Product.ProId)
                        {
                            if (dt.TypeAmount == "0") //giảm giá theo tiền
                            {
                                ps.Product.DiscountPrice = ps.Product.Price.Value - dt.Amount.Value;
                            }
                            else  //giảm giá theo %
                            {
                                ps.Product.Percent = dt.Amount.Value;
                                ps.Product.DiscountPrice = Math.Round(ps.Product.Price.Value - ((Convert.ToDecimal(dt.Amount.Value) / 100) * ps.Product.Price.Value), 0);
                            }
                            break;
                        }
                    }
                }
            }
            return listProSection;
        }

        /// <summary>
        /// Hàm trả về 1 đối tượng product đã có discount
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Product getDiscount(Product p,List<DiscountDetail> DiscountDetails)
        {
            p.Percent = 0;
            p.DiscountPrice = 0;
            foreach (var dt in DiscountDetails)
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
            return p;
        }

        public List<Product> selectProduct(List<Product> list)
        {

            if (Request.Cookies["CustomerId"] != null)
            {
                int CusId = int.Parse(Request.Cookies["CustomerId"].Value);
                List<ProductLike> listLike = db.ProductLikes.Where(pl=>pl.CusID==CusId).ToList();
                return list.Select(p => new Product()
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
                    Liked = listLike.Where(prol=>prol.ProId==p.ProId).FirstOrDefault()!=null,
                    DiscountPrice = p.DiscountPrice,
                    Percent = p.Percent,
                    ProductImages = p.ProductImages.Select(pi => new ProductImage()
                    {
                        ProColorID = pi.ProColorID,
                        Image = pi.Image,
                        ImageColor = pi.ProductColor.ImageColor
                    }).ToList()
                }).ToList();
            }
            else
            {
                return list.Select(p => new Product()
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
                    Liked = false,
                    DiscountPrice = p.DiscountPrice,
                    Percent = p.Percent,
                    ProductImages = p.ProductImages.Select(pi => new ProductImage()
                    {
                        ProColorID = pi.ProColorID,
                        Image = pi.Image,
                        ImageColor = pi.ProductColor.ImageColor
                    }).ToList()
                }).ToList();
            }
        }
    }
}