using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class DiscountDetailDAO
    {
        ClothesShopEntities db = null;
        public DiscountDetailDAO()
        {
            db = new ClothesShopEntities();
        }

        public void Insert(List<DiscountDetail> list)
        {
            db.DiscountDetails.AddRange(list);
            db.SaveChanges();
        }

        public void Edit(List<DiscountDetail> listNew)
        {
            var id = listNew[0].DiscountProductId;
            var listOld = db.DiscountDetails.Where(d=>d.DiscountProductId==id).ToList();
            db.DiscountDetails.RemoveRange(listOld);
            db.DiscountDetails.AddRange(listNew);
            db.SaveChanges();
        }

        public List<DiscountDetail> getDiscountDetailNow()
        {
            return db.DiscountDetails.Where(d=>d.DiscountProduct.StartDate<=DateTime.Now && d.DiscountProduct.EndDate>=DateTime.Now).OrderByDescending(dt=>dt.DiscountDetailId).ToList();
        }
    }
}
