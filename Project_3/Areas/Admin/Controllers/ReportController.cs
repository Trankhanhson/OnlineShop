using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        private StatisticalDAO statisticalDAO = new StatisticalDAO();
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult getReportDay(DateTime date)
        {
            var s = statisticalDAO.getByDate(date);
            return Json(s,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getReportMonth(DateTime date)
        {
            var statistical = statisticalDAO.getAll().Where(s=>s.Date.Value.Year == date.Year && s.Date.Value.Month == date.Month).ToList();
            int TotalRevenueMonth = 0;
            int TotalProfitMonth = 0;
            foreach (var item in statistical)
            {
                //lấy tổng doanh thu lợi nhuận
                TotalRevenueMonth += item.Revenue.Value;
                TotalProfitMonth+=item.Profit.Value;

            }

            var resultList = JsonConvert.SerializeObject(statistical);
            return Json(new
            {
                resultList = resultList,
                TotalRevenueMonth = TotalRevenueMonth,
                TotalProfitMonth = TotalProfitMonth
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getReportYear(int year) {
            var statisticals = statisticalDAO.getAll().Where(s => s.Date.Value.Year == year).ToList();
            List<ReportDTO> list = new List<ReportDTO>();

            int TotalRevenueYear = 0;
            int TotalProfitYear = 0;
            foreach (var statistical in statisticals) {
                TotalRevenueYear += statistical.Revenue.Value;
                TotalProfitYear += statistical.Profit.Value;


                ReportDTO reportYear = list.Where(s => s.date == statistical.Date.Value.Month).FirstOrDefault();

                //nếu tháng này đã tồn tại trong danh sách báo cáo năm
                if (reportYear != null)
                {
                    reportYear.Profit += statistical.Profit;
                    reportYear.Revenue+=statistical.Revenue;
                    reportYear.Quantity += statistical.Quantity;
                    reportYear.Total_Order += statistical.Total_Order;
                }
                else
                {
                    ReportDTO reportYearNew = new ReportDTO();
                    reportYearNew.date = statistical.Date.Value.Month;
                    reportYearNew.Profit = statistical.Profit;
                    reportYearNew.Revenue = statistical.Revenue;
                    reportYearNew.Quantity = statistical.Quantity;
                    reportYearNew.Total_Order = statistical.Total_Order;
                    list.Add(reportYearNew);
                }
            }
            var resultList = JsonConvert.SerializeObject(list);
            return Json(new
            {
                resultList = resultList,
                TotalRevenueYear = TotalRevenueYear,
                TotalProfitYear = TotalProfitYear
            }, JsonRequestBehavior.AllowGet);
        }
    }
}