using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductImagesDAO
    {
        ClothesShopEntities _dbContext = null;
        public ProductImagesDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductImage> getAll()
        {
            return _dbContext.ProductImages.ToList();
        }

        public void Insert(ProductImage ps)
        {
            _dbContext.ProductImages.Add(ps);
            try
            {
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
