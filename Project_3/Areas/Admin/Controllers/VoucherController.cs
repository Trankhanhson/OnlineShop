using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class VoucherController : Controller
    {
        // GET: Admin/Voucher
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Voucher> list = new VoucherDAO().getAll();
            if (searchText.Trim() != "")
            {
                list = list.Where(v=>v.Name.ToLower().Contains(searchText.ToLower())).ToList();
            }

            var pageData = Paggination.PagedResult(list, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Admin/Voucher/Create

        [HttpPost]
        public JsonResult Create(Voucher Voucher)
        {
            try
            {
                
                VoucherDAO VoucherDAO = new VoucherDAO();
                Voucher v = VoucherDAO.Insert(Voucher);
                var result = JsonConvert.SerializeObject(v);
                return Json(new
                {
                    message = true,
                    v = result
                });
            }
            catch
            {
                return Json(new
                {
                    message = false
                });
            }
        }

        // POST: Admin/Voucher/Edit/5
        [HttpPost]
        public JsonResult Edit(Voucher Voucher)
        {
            bool check = true;
            try
            {
                VoucherDAO VoucherDAO = new VoucherDAO();
                VoucherDAO.Edit(Voucher);
                return Json(check);
            }
            catch
            {
                return Json(check);
            }

        }
        // POST: Admin/Voucher/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string message = "";
            bool check = true;
            try
            {
                VoucherDAO VoucherDAO = new VoucherDAO();
                VoucherDAO.Delete(id);
                message = "Đã xóa thành công";
            }
            catch
            {
                message = "Xóa thất bại";
                check = false;
            }
            return Json(new
            {
                message = message,
                check = check
            });
        }
    }
}