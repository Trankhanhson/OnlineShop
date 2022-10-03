using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductDAO
    {
        private ClothesShopEntities _dbContext = null;

        public ProductDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<Product> ListAll()
        {
            List<Product> list = _dbContext.Products.ToList();
            return list;
        }


        public long Create(Product p)
        {
            _dbContext.Products.Add(p);
            _dbContext.SaveChanges();
            List<Product> list = _dbContext.Products.ToList<Product>();
            var lastProduct= list.LastOrDefault();
            if (lastProduct != null)
            {
                return lastProduct.ProId;
            }
            else
            {
                return 0;
            }
        }
    }
}
