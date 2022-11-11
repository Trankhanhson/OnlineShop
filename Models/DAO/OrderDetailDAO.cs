using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class OrderDetailDAO
    {
        ClothesShopEntities db = null;
        public OrderDetailDAO()
        {
            db = new ClothesShopEntities();
        }

        public bool Insert(List<OrderDetail> list)
        {
            try
            {
                db.OrderDetails.AddRange(list);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
