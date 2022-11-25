using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public void Update(Customer cus)
        {
            var customer = db.Customers.Find(cus.CusID);
            customer.Email = cus.Email;
            customer.Name = cus.Name;
            customer.Phone = cus.Phone;
            customer.Address = cus.Address;
            db.SaveChanges();
        }
        public bool CheckPhone(string phone)
        {
            return db.Customers.Count(c => c.Phone == phone) > 0;
        }

        public bool CheckEmail(string Email)
        {
            return db.Customers.Count(c=>c.Email==Email) > 0;
        }

        public int Login(string usename, string password)
        {
            Customer cus = db.Customers.Where(c => c.Email == usename).SingleOrDefault();
            if (cus == null)
            {
                return 0; //sai username
            }
            else
            {
                if (cus.Password == password)
                {
                    return 1;
                }
                else
                {
                    return -1; //sai mk
                }
            }
        }

        public Customer getById(long id)
        {
            return db.Customers.Find(id);
        }

        public long getIdByUsername(string username)
        {
            return db.Customers.Where(c=>c.Email==username || c.Phone==username).SingleOrDefault().CusID;
        }
    }
}
