using Models.Framework;
using Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_3.common;
using Newtonsoft.Json;
using Project_3.Model;

namespace Project_3.Controllers
{
    public class InfoCustomerController : Controller
    {
        // GET: InfoCustomer
        public ActionResult InfoAccount()
        {
            Customer customer = new Customer();
            if (Request.Cookies["CustomerId"] != null)
            {
                long id = int.Parse(Request.Cookies["CustomerId"].Value);
                customer = new CustomerDAO().getById(id);
                return View(customer);
            }
            else
            {
                return RedirectToAction("HomePage", "Home");
            }
        }

        [HttpPost]
        public ActionResult UpdateInfoCustomer(Customer cus)
        {
            var dao = new CustomerDAO();
            var message = "";
            var cusOld = dao.getById(cus.CusID);
            try
            {
                if (cus.Email != cusOld.Email && dao.CheckEmail(cus.Email))
                {
                    message = "ExistEmail";
                }
                else if (cus.Phone != cusOld.Phone && dao.CheckPhone(cus.Phone))
                {
                    message = "ExistPhone";
                }
                else
                {
                    dao.Update(cus);
                    message = "success";
                }
            }
            catch
            {
                message = "fail";
            }
            return Json(new
            {
                message = message
            });
        }

        public ActionResult OrderHistory()
        {
            try
            {
                if (Request.Cookies["CustomerId"] != null)
                {
                    int id = int.Parse(Request.Cookies["CustomerId"].Value);
                    Customer customer = new CustomerDAO().getById(id);
                    return View(customer);
                }
                else
                {
                    return RedirectToAction("Homepage", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Homepage", "Home");
            }
        }

        public ActionResult getOrderByCusId(int id, int statusId)
        {
            List<Order> list = new OrderDAO().getOrderByCusId(id,statusId).Select(o => new Order()
            {
                OrdID = o.OrdID,
                MoneyTotal = o.MoneyTotal,
                OrderDate = o.OrderDate,
                StatusOrder = new StatusOrder() {StatusOderId = o.StatusOrderId.Value, Status = o.StatusOrder.Status}
            }).ToList();

            var result = JsonConvert.SerializeObject(list);
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
            orderDTO.ReceivingMail = order.ReceivingMail;
            orderDTO.ReceivingCity = order.ReceivingCity;
            orderDTO.ReceivingDistrict = order.ReceivingDistrict;
            orderDTO.ReceivingWard = order.ReceivingWard;
            orderDTO.ReceivingAddress = order.ReceivingAddress;
            orderDTO.PaymentType = order.PaymentType;
            orderDTO.OrderDate = order.OrderDate;
            orderDTO.Status = order.StatusOrder.Status;
            orderDTO.StatusOderId = order.StatusOrder.StatusOderId;
            orderDTO.Note = order.Note;
            orderDTO.CustomerName = order.Customer.Name;
            orderDTO.MoneyTotal = order.MoneyTotal;
            List<OrderDetailDTO> list = new List<OrderDetailDTO>();

            int originPrice = 0;

            var FeedbackDAO = new FeedbackDAO();
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
                dto.FeedbackId = FeedbackDAO.getIdByForeignKey(dto.ProId, order.CusID.Value);
                list.Add(dto);
                originPrice += dto.Price.Value * dto.Quantity.Value;
            }
            orderDTO.OrderDetailDTOs = list;
            var result = JsonConvert.SerializeObject(orderDTO);
            return Json(new
            {
                Order = result,
                originPrice = originPrice

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CancelOrder(long id)
        {
            try
            {
                var dao = new OrderDAO().CancelOrder(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }
    }
}