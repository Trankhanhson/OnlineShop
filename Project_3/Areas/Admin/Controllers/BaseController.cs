using Models.Framework;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Models.DAO;

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
    }

    
}