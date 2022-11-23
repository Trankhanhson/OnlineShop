using Models.DAO;
using Models.Framework;
using Newtonsoft.Json;
using Project_3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Project_3.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        // GET: Admin/Order
        public ActionResult WaitConfirm()
        {
            return View();
        }

        public ActionResult WaitProcess()
        {
            return View();
        }

        public ActionResult Tranfering()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Canceled()
        {
            return View();
        }

        public JsonResult getAllData(int id)
        {
            List<Order> list = new OrderDAO().getAll().Select(o=>new Order()
            {
                OrdID = o.OrdID,
                ReceivingPhone = o.ReceivingPhone,
                OrderDate = o.OrderDate,
                StatusOrderId = o.StatusOrderId 
            }).Where(a=>a.StatusOrderId==id).ToList();

            //loại bỏ các phần tử bị lặp và tuần tự hóa thành json
            JsonSerializerSettings jss = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            var result = JsonConvert.SerializeObject(list, Formatting.Indented, jss);
            return Json(new
            {
                result = result
            }, JsonRequestBehavior.AllowGet);
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
            orderDTO.CustomerName = order.Customer.Name;
            orderDTO.MoneyTotal = order.MoneyTotal;
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
                list.Add(dto);
            }
            orderDTO.OrderDetailDTOs = list;
            var result = JsonConvert.SerializeObject(orderDTO);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangeStatus(long id)
        {
            OrderDAO dao = new OrderDAO();
            Order o = dao.ChangeStatus(id);
            string message = "";
            if(o.StatusOrderId == 2)
            {
                message = "Chờ xử lý";
            } 
            else if(o.StatusOrderId == 3)
            {
                message = "Đang vận chuyển";
            }
            else
            {
                message = "Thành công";
                dao.onSuccess(o.OrderDetails.ToList());
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