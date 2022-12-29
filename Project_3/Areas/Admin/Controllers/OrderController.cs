using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.common;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_3.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order

        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult WaitProcess()
        {
            return View();
        }

        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Tranfering()
        {
            return View();
        }

        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Success()
        {
            return View();
        }

        [HasCredential(RoleID = "VIEW_ORDER")]
        public ActionResult Canceled()
        {
            return View();
        }

        public JsonResult getPageData(int statusId,string searchText, int pageNumber = 1, int pageSize = 5)
        {
            List<Order> list = new OrderDAO().getAll().Where(a => a.StatusOrderId == statusId).Select(o => new Order()
            {
                OrdID = o.OrdID,
                ReceivingPhone = o.ReceivingPhone,
                OrderDate = o.OrderDate,
                StatusOrderId = o.StatusOrderId
            }).ToList();
            if (searchText.Trim() != "")
            {
                list = list.Where(c => c.ReceivingPhone.Contains(searchText)).ToList();
            }

            var pageData = Paggination.PagedResult(list, pageNumber, pageSize);
            var result = JsonConvert.SerializeObject(pageData);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(long id)
        {
            try
            {
                OrderDAO dao = new OrderDAO();
                dao.Delete(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getOrderById(long id)
        {
            Order order = new OrderDAO().getById(id);
            OrderDTO orderDTO = new OrderDTO();
            orderDTO.OrdID = order.OrdID;
            orderDTO.VoucherId = order.VoucherId;
            orderDTO.ReceivingPhone = order.ReceivingPhone;
            orderDTO.ReceivingCity = order.ReceivingCity;
            orderDTO.ReceivingDistrict = order.ReceivingDistrict;
            orderDTO.ReceivingWard = order.ReceivingWard;
            orderDTO.ReceivingAddress = order.ReceivingAddress;
            orderDTO.PaymentType = order.PaymentType;
            orderDTO.OrderDate = order.OrderDate;
            orderDTO.Status = order.StatusOrder.Status;
            orderDTO.Note = order.Note;
            orderDTO.CustomerName = order.ReceivingName;
            orderDTO.MoneyTotal = order.MoneyTotal;

            int totalOriginPrice = 0;
            List<OrderDetailDTO> list = new List<OrderDetailDTO>();
            foreach (var item in order.OrderDetails)
            {
                OrderDetailDTO dto = new OrderDetailDTO();
                dto.ProVariationID = item.ProVariationID;
                dto.Image = item.ProductVariation.Product.ProductImages.Where(pi => pi.ProColorID == item.ProductVariation.ProColorID).First().Image;
                dto.ProId = item.ProductVariation.ProId.Value;
                dto.ProColorId = item.ProductVariation.ProColorID.Value;
                dto.NameProduct = item.ProductVariation.Product.ProName;
                dto.NameColor = item.ProductVariation.ProductColor.NameColor;
                dto.NameSize = item.ProductVariation.ProductSize.NameSize;
                dto.Quantity = item.Quantity;
                dto.Price = item.Price;
                dto.DiscountPrice = item.DiscountPrice;
                list.Add(dto);
                totalOriginPrice += (dto.Price.Value* dto.Quantity.Value);
            }
            orderDTO.TotalOriginPrice = totalOriginPrice;
            orderDTO.OrderDetailDTOs = list;
            var result = JsonConvert.SerializeObject(orderDTO);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(long id)
        {
            OrderDAO dao = new OrderDAO();
            StatisticalDAO statisticalDAO = new StatisticalDAO();
            Order o = dao.ChangeStatus(id);
            string message = "";
            if(o.StatusOrderId == 2)
            {
                message = "Đang vận chuyển";
            }
            else
            {
                message = "Thành công";
                dao.onSuccess(o.OrderDetails.ToList());

                //cập thật thống kê
                DateTime dt = new DateTime(2022, 12, 5);
                var Statistical = statisticalDAO.getAll().Where(s=>s.Date.Value.Date == dt).FirstOrDefault();
                //nếu thống kê của ngày hôm nay đã tồn tại
                if(Statistical != null)
                {
                    Statistical.Revenue += o.MoneyTotal;
                    foreach(var d in o.OrderDetails)
                    {
                        Statistical.Quantity+=d.Quantity;
                        if(d.DiscountPrice > 0)
                        {
                            Statistical.Profit += (d.DiscountPrice - d.ProductVariation.Product.ImportPrice) * d.Quantity;
                        }
                        else
                        {
                            Statistical.Profit += (d.Price - d.ProductVariation.Product.ImportPrice) * d.Quantity;
                        }
                    }
                    Statistical.Total_Order += 1;
                }
                else //Trương hợp thống kê hôm nay chưa tồn tại
                {
                    Statistical statisticalNew = new Statistical();
                    statisticalNew.Revenue = o.MoneyTotal;
                    statisticalNew.Date = dt;
                    statisticalNew.Quantity = 0;
                    statisticalNew.Profit = 0;
                    foreach (var d in o.OrderDetails)
                    {
                        statisticalNew.Quantity += d.Quantity;
                        if (d.DiscountPrice > 0)
                        {
                            statisticalNew.Profit += (d.DiscountPrice - d.ProductVariation.Product.ImportPrice) * d.Quantity;
                        }
                        else
                        {
                            statisticalNew.Profit += (d.Price - d.ProductVariation.Product.ImportPrice) * d.Quantity;
                        }
                    }
                    statisticalNew.Total_Order = 1;
                    statisticalDAO.Insert(statisticalNew);
                }
                statisticalDAO.Savechages();
            }

            return Json(message,JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelOrder(long id)
        {
            try
            {
                OrderDAO dao = new OrderDAO();
                dao.CancelOrder(id);
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false,JsonRequestBehavior.AllowGet);    
            }
            
        }
    } 
}