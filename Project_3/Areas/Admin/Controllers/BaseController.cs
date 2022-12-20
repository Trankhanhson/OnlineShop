using Models.Framework;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Models.DAO;
using Project_3.Model;

namespace Project_3.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // GET: Admin/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }
        public Product getDiscount(Product p, List<DiscountDetail> DiscountDetails)
        {
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
            return p;
        }

        /// <summary>
        /// Trả về danh sách đã có discount
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<Product> getListDiscount(List<Product> list)
        {
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            foreach (var p in list)
            {
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
        /// Trả về danh sách đã có discount
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<ProductDiscount> getListDiscount(List<ProductDiscount> list)
        {
            var DiscountDetails = new DiscountDetailDAO().getDiscountDetailNow(); //lấy danh sách discountDetail giảm dần thoe thời gian tạo
            foreach (var p in list)
            {
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
                            p.DiscountPrice = Math.Round(p.Price.Value - ((Convert.ToDecimal(dt.Amount.Value) / 100) * p.Price.Value), 0);
                        }
                        break;
                    }
                }
            }
            return list;
        }
    }
}