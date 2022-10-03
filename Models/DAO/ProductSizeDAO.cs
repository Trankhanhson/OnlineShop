using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductSizeDAO
    {
        ClothesShopEntities _ClothesShopEntities = null;
        public ProductSizeDAO()
        {
            _ClothesShopEntities = new ClothesShopEntities();
        }

        public List<ProductSize> getAll()
        {
            return _ClothesShopEntities.ProductSizes.ToList();
        }

        public void Insert(ProductSize ps)
        {
            _ClothesShopEntities.ProductSizes.Add(ps);
            _ClothesShopEntities.SaveChanges();
        }
    }
}
