using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CustomnerDAO
    {
        ClothesShopEntities db = null;
        public CustomnerDAO()
        {
            db = new ClothesShopEntities();
        }

        public bool Insert(Customer customer)
        {
            try
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckPhone(string phone)
        {
            return db.Customers.Count(c => c.Phone == phone) > 0;
        }

        public bool CheckEmail(string Email)
        {
            return db.Customers.Count(c=>c.Email==Email) > 0;
        }
    }
}
