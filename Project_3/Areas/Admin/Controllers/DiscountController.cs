using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using Project_3.Model;

namespace Project_3.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        // GET: Admin/Discount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<DiscountProduct> list = new DiscountDAO().getAll().Select(d => new DiscountProduct()
            {
                DiscountProductId = d.DiscountProductId,
                Name = d.Name,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }).ToList();
            if (searchText.Trim() != "")
            {
                var a = MethodCommnon.ToUrlSlug(searchText.ToLower());
                list = list.Where(p => p.Name.Contains(a)).ToList();
            }
            var pageData = Paggination.PagedResult(list, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(new
            {
                result = result
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getProductOnly(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Product> products = new ProductDAO().getAll();
            List<ProductDiscount> listResult = products.Select(p => new ProductDiscount()
            {
                ProId = p.ProId,
                Price = p.Price,
                ProName = p.ProName,
                Slug = p.Slug,
                firstImage = p.ProductImages.First().Image,
                TotalQty = CountTotalQuantity(p.ProductVariations.ToList()),
                Check = false
            }).ToList();
            if (searchText.Trim() != "")
            {
                var a = MethodCommnon.ToUrlSlug(searchText.ToLower());
                listResult = listResult.Where(p => p.Slug.Contains(a)).ToList();
            }
            var pageData = Paggination.PagedResult(listResult, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }

        public int CountTotalQuantity(List<ProductVariation> list)
        {
            int total = 0;
            foreach (var item in list)
            {
                total += item.Quantity.Value;
            }
            return total;
        }

        public ActionResult getById(int id)
        {
            var dao = new DiscountDAO().getById(id);

            DiscountProduct dp = new DiscountProduct()
            {
                DiscountProductId = dao.DiscountProductId,
                Name = dao.Name,
                StartDate = dao.StartDate,
                EndDate = dao.EndDate,
                DiscountDetails = dao.DiscountDetails.Select(d => new DiscountDetail()
                {
                    DiscountDetailId = d.DiscountDetailId,
                    Product = new Product()
                    {
                        ProId = d.Product.ProId,
                        firstImage = d.Product.ProductImages.First().Image,
                        ProName = d.Product.ProName,
                        Price = d.Product.Price
                    },
                    priceAfter = MethodCommnon.CountDiscountPrice(d.Product.Price.Value, d.Amount.Value, d.TypeAmount),
                    Amount = d.Amount,
                    TypeAmount = d.TypeAmount,
                    MaxQuantityPerUser = d.MaxQuantityPerUser
                }).ToList()
            };
            var result = JsonConvert.SerializeObject(dp);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/Discount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Discount/Create
        [HttpPost]
        public ActionResult Create(DiscountProduct discountPro, List<DiscountDetail> listDiscountDetail)
        {
            try
            {
                DiscountDAO discountDAO = new DiscountDAO();
                int id = discountDAO.Insert(discountPro);

                foreach(var item in listDiscountDetail)
                {
                    item.DiscountProductId = id;
                }

                DiscountDetailDAO discountDetailDAO = new DiscountDetailDAO();
                discountDetailDAO.Insert(listDiscountDetail);

                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        // GET: Admin/Discount/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        // POST: Admin/Discount/Edit/5
        [HttpPost]
        public ActionResult Edit(DiscountProduct discountPro, List<DiscountDetail> listDiscountDetail)
        {
            try
            {
                DiscountDAO discountDAO = new DiscountDAO();
                discountDAO.Edit(discountPro);
                DiscountDetailDAO discountDetailDAO = new DiscountDetailDAO();
                discountDetailDAO.Edit(listDiscountDetail);

                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                DiscountDAO discountDAO = new DiscountDAO();
                discountDAO.Delete(id);
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
        }
    }
}
