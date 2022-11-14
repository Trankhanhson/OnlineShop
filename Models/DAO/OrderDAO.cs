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
            db.Configuration.LazyLoadingEnabled = false;
            return db.Orders.Include(o=>o.OrderDetails).ToList();
        }

        public int ChangeStatus(long id)
        {
            Order o = db.Orders.Find(id);
            o.StatusOrderId += 1;
            db.SaveChanges();
            return o.StatusOrderId.Value;
        }

        public void CancelOrder(long id)
        {
            Order o = db.Orders.Find(id);
            o.StatusOrderId = 5;
            db.SaveChanges();
        }
    }
}
