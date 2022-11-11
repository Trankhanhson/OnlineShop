using Models;
using Models.DAO;
using Models.Framework;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            List<CartItem> list = new List<CartItem>();
            if(cart != null)
            {
                list=(List<CartItem>)cart;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View(list);
        }

        public ActionResult deleteCartItem(long id)
        {
            var cart = Session[CartSession];
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                foreach(var item in list)
                {
                    if(item.ProVariation.ProVariationID==id)
                    {
                        list.Remove(item);
                        break;
                    }    
                }
                Session[CartSession] = list;
            }
            if (list.Count==0)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult increaseQty(long proVariationId,int newQuantity)  
        {
            var cart = Session[CartSession];
            List<CartItem> list = new List<CartItem>();
            var newPrice = 0;
            bool checkQuantity = new ProductVariationDAO().CheckQuantity(newQuantity, proVariationId); //kiểm tra số lượng hàng tồn
            if (checkQuantity)
            {
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                    foreach (var item in list)
                    {
                        if (item.ProVariation.ProVariationID == proVariationId)
                        {
                            item.Quantity = newQuantity;
                            newPrice = item.ProVariation.Product.Price * newQuantity;
                            break;
                        }
                    }
                    Session[CartSession] = list;
                }
            }
            
            return Json(new
            {
                newPrice= newPrice,
                checkQuantity = checkQuantity
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult decreaseQty(long proVariationId, int newQuantity)
        {
            var cart = Session[CartSession];
            List<CartItem> list = new List<CartItem>();
            var newPrice = 0;
                if (cart != null)
                {
                    list = (List<CartItem>)cart;
                    foreach (var item in list)
                    {
                        if (item.ProVariation.ProVariationID == proVariationId)
                        {
                            item.Quantity = newQuantity;
                            newPrice = item.ProVariation.Product.Price * newQuantity;
                            break;
                        }
                    }
                    Session[CartSession] = list;
                }

            return Json(new
            {
                newPrice = newPrice
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddItem(long ProId,int ProColorId, int ProSizeId)
        {
            //Lấy biến thể vừa thêm 
            ProductVariation productVariation = new ProductVariationDAO().getByForeignKey(ProId,ProColorId,ProSizeId);
            string Img = new ProductImagesDAO().getByKey(ProId, ProColorId).Image;
            var cart = Session[CartSession];
            if(cart == null)
            {
                //Tạo 1 cartItem mới
                CartItem cartItem = new CartItem();
                cartItem.ProVariation = productVariation;
                cartItem.Quantity = 1;
                cartItem.Image = Img;
                List<CartItem> list = new List<CartItem> { cartItem };
                Session[CartSession] = list;
            }
            else
            {
                List<CartItem> list = (List<CartItem>)Session[CartSession];
                if (list.Exists(x => x.ProVariation.ProVariationID == productVariation.ProVariationID)) //nếu sản phẩm đã tòn tại trong cart thì + 1
                {
                    foreach(var item in list)
                    {
                        if (item.ProVariation.ProVariationID == productVariation.ProVariationID)
                        {
                            item.Quantity += 1;
                        }
                    }
                }
                else //nếu sản phẩm chưa tồn tại trong giỏ hàng
                {
                    //Tạo 1 cartItem mới
                    CartItem cartItem = new CartItem();
                    cartItem.ProVariation = productVariation;
                    cartItem.Quantity = 1;
                    cartItem.Image = Img;
                    list.Add(cartItem);
                }
                Session[CartSession] = list;
            }

            return Json(new
            {

            });
        }

        public ActionResult PaymentPage()
        {
            //Display info of customer
            if (Session["Customer"] != null)
            {
                var id = (long)Session["Customer"];
                var cus = new CustomnerDAO().getById(id);
                ViewBag.Customer = cus;
            }

            var cart = Session[CartSession];
            List<CartItem> list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            if(list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(list);
        }

        public ActionResult InfoBill()
        {
            var cart = Session[CartSession];
            if (cart != null)
            {

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Order(Order order)
        {
            try {
                order.OrderDate = DateTime.Now;
                var o = new OrderDAO().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var listOrderDetail = new List<OrderDetail>();
                foreach (var item in cart)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.ProVariationID = item.ProVariation.ProVariationID;
                    orderDetail.OrdID = o.OrdID;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Price = item.ProVariation.Product.Price;
                    listOrderDetail.Add(orderDetail);
                }
                OrderDetailDAO orderDetailDAO = new OrderDetailDAO();
                orderDetailDAO.Insert(listOrderDetail);

                ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                productVariationDAO.editQuantity(listOrderDetail);
            }
            catch
            {

            }
            return View();
        }
    }
}