using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;

namespace Models.DAO
{
    public class DiscountDAO
    {
        ClothesShopEntities db = null;
        public DiscountDAO()
        {
            db = new ClothesShopEntities();
        }

        public List<DiscountProduct> getAll()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.DiscountProducts.Include(d => d.DiscountDetails).ToList();
        }

        public int Insert(DiscountProduct d)
        {
            var a = db.DiscountProducts.Add(d);
            db.SaveChanges();
            return a.DiscountProductId;
        }

        public List<DiscountProduct> CheckDiscount()
        {
            var now = DateTime.Now;
            return db.DiscountProducts.OrderByDescending(d=>d.DiscountProductId).Where(d=>d.StartDate<=now && d.EndDate>=now).ToList();
        }
    }
}
