using Models.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CustomerDAO
    {
        ClothesShopEntities db = null;
        public CustomerDAO()
        {
            db = new ClothesShopEntities();
        }

        public bool Insert(Customer customer)
        {
            try
            {
                Customer c = db.Customers.Add(customer);
                Cart cart = new Cart();
                cart.CusID = c.CusID;
                db.Carts.Add(cart);
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

        public List<Customer> getAll()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Customer> customers = db.Customers.ToList();
            return customers;
        }

        public bool ChangeSattus(long id)
        {
            try
            {
                Customer cus = db.Customers.Find(id);
                cus.Status = !cus.Status;
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
            return db.Customers.Where(c=>c.Email==username).SingleOrDefault().CusID;
        }

        public Customer getByEmail(string email)
        {
            return db.Customers.Where(c=>c.Email==email).SingleOrDefault();
        }
    }
}
