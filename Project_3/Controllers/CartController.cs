using Models;
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
            return View(list);
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
    }
}