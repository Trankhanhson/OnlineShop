using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class OrderDAO
    {
        ClothesShopEntities db = null;
        public OrderDAO()
        {
            db = new ClothesShopEntities();
        }

        public Order Insert(Order order)
        {
            Order result = db.Orders.Add(order);
            db.SaveChanges();
            return result;
        }

        public Order getById(long id)
        {
            Order order = db.Orders.Where(o => o.OrdID == id).FirstOrDefault();
            return order;
        }

        public List<Order> getAll()
        {
            return db.Orders.OrderByDescending(o=>o.OrderDate).ToList();
        }

        public List<Order> getOrderByCusId(int id, int statusId)
        {
            List<Order> orders = new List<Order>();
            if (statusId == 0)
            {
                orders = db.Orders.Where(o => o.CusID == id).OrderByDescending(d => d.OrderDate).ToList();
            }
            else
            {
                orders = db.Orders.Where(o => o.CusID == id && o.StatusOrderId == statusId).OrderByDescending(d => d.OrderDate).ToList();

            }
            return orders;
        }

        public void Delete(long id)
        {
            Order o = db.Orders.Find(id);
            db.OrderDetails.RemoveRange(o.OrderDetails);
            db.Orders.Remove(o);
            db.SaveChanges();
        }

        /// <summary>
        /// Trả về status của order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Order ChangeStatus(long id)
        {
            Order o = db.Orders.Find(id);
            o.StatusOrderId += 1;
            db.SaveChanges();
            return o;
        }

        public Order CancelOrder(long id)
        {
            Order o = db.Orders.Find(id);
            o.StatusOrderId = 4;
            var productVariations = db.ProductVariations;
            foreach (var item in o.OrderDetails)
            {
                ProductVariation v = productVariations.Find(item.ProVariationID);
                v.Ordered -= item.Quantity;
            }
            db.SaveChanges();
            return (o);
        }

        /// <summary>
        /// Hàm trừ tồn kho và update lại số lượng đã đặt
        /// </summary>
        /// <param name="listOrderDetail"></param>
        public void onSuccess(List<OrderDetail> listOrderDetail)
        {
            foreach (var item in listOrderDetail)
            {
                ProductVariation v = db.ProductVariations.Find(item.ProVariationID);
                v.Quantity -= item.Quantity;
                v.Ordered -= item.Quantity;
                v.Product.Saled += item.Quantity;
            }
            db.SaveChanges();
        }
    }
}
