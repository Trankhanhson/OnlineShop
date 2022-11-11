using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
