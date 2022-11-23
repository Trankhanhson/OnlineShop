using Models;
using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Project_3.Areas.Admin.Controllers
{
    public class ImportBillController : Controller
    {
        // GET: Admin/ImportBill
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getPageData(string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<ImportBill> importBills = new ImportBillDAO().getAll().Select(i=>new ImportBill()
            {
                ImpId = i.ImpId,
                User = new User() { UserID=i.User.UserID, Name = i.User.Name},
                ImpDate = i.ImpDate,
                MoneyTotal = i.MoneyTotal
            }).ToList();

            if (searchText.Trim() != "")
            {
                importBills = importBills.Where(pc => pc.ImpDate.ToString().Contains(searchText)).ToList();
            }

            var pagedData = Paggination.PagedResult(importBills, pageNumber, pageSize);

            var result = JsonConvert.SerializeObject(pagedData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            ViewBag.listUser = new UserDAO().getAll();
            return View();
        }

        public JsonResult getById(long id)
        {
            var list = new ImportBillDetailDAO().getByIdImportBill(id).Select(imd => new ImportDetailDTO()
            {
                ProVariationID = imd.ProVariationID,
                ProId = imd.ProductVariation.ProId.Value,
                Image = imd.ProductVariation.Product.ProductImages.Where(pi => pi.ProColorID == imd.ProductVariation.ProColorID).First().Image,
                NameProduct = imd.ProductVariation.Product.ProName,
                NameColor = imd.ProductVariation.ProductColor.NameColor,
                ProColorId = imd.ProductVariation.ProColorID.Value,
                NameSize = imd.ProductVariation.ProductSize.NameSize,
                Quantity = imd.Quantity,
                ImportPrice = imd.ImportPrice
            });
            var result = JsonConvert.SerializeObject(list);
            return Json(result, JsonRequestBehavior.AllowGet);   
        }

        [HttpPost]
        public JsonResult Create(ImportBill ImportBill, List<ImportBillDetail> billDetails,List<Product> listProduct)
        {
            try
            {
                ImportBillDAO ImportBillDAO = new ImportBillDAO();
                ImportBillDetailDAO importBillDetailDAO = new ImportBillDetailDAO();
                ImportBill.ImpDate = DateTime.Now;
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