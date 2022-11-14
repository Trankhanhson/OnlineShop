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
    }
}
