using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebGrease.Activities;

namespace Project_3.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult getNewCart(List<VariationCart> variationCarts)
        {
            var daoVariation = new ProductVariationDAO();
            var daoProduct = new ProductDAO();
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow();
            var listProduct = new List<Product>();
            var newVariations = new List<VariationCart>();
            if (variationCarts != null)
            {
                foreach (var item in variationCarts)
                {
                    ProductVariation pvariation = new ProductVariationDAO().getByForeignKey(item.ProId, item.proColorId, item.proSizeId);

                    //take item enough quantity
                    if ((pvariation.Quantity - pvariation.Ordered) >= item.Quantity)
                    {
                        Product p = listProduct.Where(a => a.ProId == item.ProId).FirstOrDefault();
                        //add product by id when product don't exist in list
                        if (p == null)
                        {
                            p = daoProduct.getById(item.ProId);
                            //get discount of product
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
                                }
                            }
                            listProduct.Add(p);
                        }

                        item.Price = p.Price.Value;
                        item.DiscountPrice = p.DiscountPrice;
                        item.Percent = p.Percent;
                        item.ProName = p.ProName;
                        var proImg = p.ProductImages.Where(pi => pi.ProColorID == item.proColorId).FirstOrDefault();
                        item.Image = "/Upload/Product/" + item.ProId + "/" + proImg.Image;
                        item.proSizeName = p.ProductVariations.Where(pv => pv.ProSizeID == item.proSizeId).FirstOrDefault().ProductSize.NameSize;
                        item.srcColor = "/Upload/ColorImage/" + item.proColorId + "/" + proImg.ProductColor.ImageColor;
                        newVariations.Add(item);
                    }
                }

            }

            var result = JsonConvert.SerializeObject(newVariations);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckQuantity(long ProId, int ProColorId, int ProSizeId, int newQuantity)
        {
            ProductVariation pv = new ProductVariationDAO().getByForeignKey(ProId, ProColorId, ProSizeId);
            if ((pv.Quantity - pv.Ordered) >= newQuantity)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
            
        }

        public ActionResult PaymentPage()
        {
            //Display info of customer
            if (Request.Cookies["CustomerId"] != null)
            {
                var id = long.Parse(Request.Cookies["CustomerId"].Value);
                var cus = new CustomerDAO().getById(id);
                ViewBag.Customer = cus;
            }
            else { ViewBag.Customer = null; }

            ViewBag.Vouchers = new VoucherDAO().getVoucherNow();

            return View();
        }

        [HttpPost]
        public ActionResult confirmEmail(string email)
        {
            try
            {
                Random random = new Random();
                string ma = random.NextString(4);
                Session["CodeConfirm"] = ma;
                Session["VerificationTime"] = DateTime.Now;
                string content = "<p>Mã xác nhận của bạn là: </p>" + ma + "<p>Vui lòng nhập mã này để xác nhận mua hàng</p>";
                MailHelper.SendMail(email, "Xác nhận mua hàng", content);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult checkConfirm(string id)
        {
            if((string)Session["CodeConfirm"] == id && ((DateTime)Session["VerificationTime"]).AddMinutes(3) >= DateTime.Now)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult InfoBill(long id)
        {
            Order order = new OrderDAO().getById(id);
            int totalOriginPrice = 0;
            foreach(var item in order.OrderDetails)
            {
                totalOriginPrice += (item.Price.Value * item.Quantity.Value);
            }
            ViewBag.TotalOriginPrice = totalOriginPrice;
            ViewBag.TotalDiscount = totalOriginPrice - order.MoneyTotal;
            return View("InfoBill",order);
        }

        [HttpPost]
        public ActionResult Order(Order order,List<CartItem> cartItems,string code)
        {
            try
            {
                if ((string)Session["CodeConfirm"] == code && ((DateTime)Session["VerificationTime"]).AddMinutes(3) >= DateTime.Now)
                {
                    ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                    order.OrderDate = DateTime.Now;
                    order.StatusOrderId = 1;
                    var o = new OrderDAO().Insert(order);
                    var listOrderDetail = new List<OrderDetail>();
                    foreach (var item in cartItems)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrdID = o.OrdID;
                        orderDetail.ProVariationID = productVariationDAO.getByForeignKey(item.ProId, item.ProColorID, item.ProSizeID).ProVariationID;
                        orderDetail.Price = item.Price;
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.DiscountPrice = item.DiscountPrice;
                        listOrderDetail.Add(orderDetail);
                    }
                    OrderDetailDAO orderDetailDAO = new OrderDetailDAO();
                    orderDetailDAO.Insert(listOrderDetail);
                    productVariationDAO.editOrdered(listOrderDetail);

                    if (o.VoucherId != 0)
                    {
                        VoucherDAO voucherDAO = new VoucherDAO();
                        voucherDAO.UseVoucher(o.VoucherId.Value);
                    }
                    return Json(o.OrdID,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}