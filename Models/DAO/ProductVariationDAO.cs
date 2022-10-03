using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductVariationDAO
    {
        ClothesShopEntities _dbContext = null;
        public ProductVariationDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductVariation> getAll()
        {
            return _dbContext.ProductVariations.ToList();
        }

        public void Insert(List<ProductVariation> ps)
        {
            foreach (ProductVariation v in ps)
            {
                _dbContext.ProductVariations.Add(v);
            }
            _dbContext.SaveChanges();
        }
    }
}
