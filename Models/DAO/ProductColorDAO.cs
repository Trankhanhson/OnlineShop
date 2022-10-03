using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductColorDAO
    {
        ClothesShopEntities _ClothesShopEntities = null;
        public ProductColorDAO()
        {
            _ClothesShopEntities = new ClothesShopEntities();
        }

        public List<ProductColor> getAll()
        {
            return _ClothesShopEntities.ProductColors.ToList();
        }

        public void Insert(ProductColor ps)
        {
            _ClothesShopEntities.ProductColors.Add(ps);
            _ClothesShopEntities.SaveChanges();
        }
    }
}
