using Models;
using Models.DAO;
using Models.Framework;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                var cus = new CustomnerDAO().getById(id);
                ViewBag.Customer = cus;
            }

            return View();
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
        public ActionResult Order(Order order,List<CartItem> cartItems)
        {
            try
            {
                ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                order.OrderDate = DateTime.Now;
                order.StatusOrderId = 1;
                var o = new OrderDAO().Insert(order);
                var listOrderDetail = new List<OrderDetail>();
                foreach(var item in cartItems)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.OrdID = o.OrdID;
                    orderDetail.ProVariationID = productVariationDAO.getByForeignKey(item.ProId, item.ProColorID, item.ProSizeID).ProVariationID;
                    orderDetail.Price = item.Price;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.DiscountPrice =item.DiscountPrice;
                    listOrderDetail.Add(orderDetail);
                }
                OrderDetailDAO orderDetailDAO = new OrderDetailDAO();
                orderDetailDAO.Insert(listOrderDetail);


                productVariationDAO.editOrdered(listOrderDetail);
                return Json(o.OrdID);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}