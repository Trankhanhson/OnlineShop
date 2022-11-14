using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class ImportBillController : Controller
    {
        // GET: Admin/ImportBill
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAllData()
        {
            List<ImportBill> importBills = new ImportBillDAO().getAll();
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(importBills, Formatting.Indented, jss);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            ViewBag.listUser = new UserDAO().getAll();
            return View();
        }

        [HttpPost]
        public JsonResult Create(ImportBill ImportBill, List<ImportBillDetail> billDetails,List<Product> listProduct)
        {
            try
            {
                ImportBillDAO ImportBillDAO = new ImportBillDAO();
                ImportBillDetailDAO importBillDetailDAO = new ImportBillDetailDAO();
                ImportBill newImBill = ImportBillDAO.Insert(ImportBill); //luu import bill
                foreach(var detail in billDetails)
                {
                    detail.ImpId = newImBill.ImpId;
                }
                importBillDetailDAO.Insert(billDetails); //luu inport detail

                //Sua so luong trong variation
                ProductVariationDAO productVariationDAO = new ProductVariationDAO();
                productVariationDAO.editQuantity(billDetails);

                //sua gia nhap va gia ban cua san pham
                ProductDAO productDAO = new ProductDAO();
                productDAO.EditPrice(listProduct);

                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }
    }
}