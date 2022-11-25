using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using System.CodeDom;

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

        public List<DiscountProduct> getDiscountNow()
        {
            return db.DiscountProducts.Where(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).OrderByDescending(d => d.DiscountProductId).ToList();
        }

        public int Insert(DiscountProduct d)
        {
            var a = db.DiscountProducts.Add(d);
            db.SaveChanges();
            return a.DiscountProductId;
        }

        public DiscountProduct getById(int id)
        {
            return db.DiscountProducts.Find(id);
        }

        public void Edit(DiscountProduct d)
        {
            var dOld = db.DiscountProducts.Find(d.DiscountProductId);
            dOld.Name = d.Name;
            dOld.StartDate = d.StartDate;
            dOld.EndDate = d.EndDate;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var listDetail = db.DiscountDetails.Where(d => d.DiscountProductId == id).ToList();
            db.DiscountDetails.RemoveRange(listDetail);

            var discount = db.DiscountProducts.Find(id);
            db.DiscountProducts.Remove(discount);

            db.SaveChanges();
        }

        public List<DiscountProduct> CheckDiscount()
        {
            var now = DateTime.Now;
            return db.DiscountProducts.OrderByDescending(d=>d.DiscountProductId).Where(d=>d.StartDate<=now && d.EndDate>=now).ToList();
        }
    }
}
