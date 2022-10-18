using Models.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models
{
    public class ProductDAO
    {
        private ClothesShopEntities _dbContext = null;

        public ProductDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<Product> getAll()
        {
            List<Product> list = _dbContext.Products.Include(pv=>pv.ProductVariations).Include(pi=>pi.ProductImages).ToList();
            return list;
        }

        public IEnumerable<Product> getPage(string searchResult, int page, int pageSize)
        {
            IQueryable<Product> model = _dbContext.Products;
            if (!string.IsNullOrEmpty(searchResult))
            {
                model = model.Where(x => x.ProName.Contains(searchResult) );
            }
            return model.OrderByDescending(x => x.ProName).ToPagedList(page, pageSize);
        }

        public long Create(Product p)
        {
            Product product =_dbContext.Products.Add(p);
            _dbContext.SaveChanges();
            return product.ProId;
        }

        public bool Delete(long id)
        {
            try
            {
                Product p = _dbContext.Products.Find(id);
                _dbContext.Products.Remove(p);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Product getById(long id)
        {
            return _dbContext.Products.Include(pi => pi.ProductImages).Include(pv=>pv.ProductVariations).Where(p=>p.ProId==id).FirstOrDefault();
        }
        
        public bool Edit(Product product)
        {
            try
            {
                Product p = _dbContext.Products.Find(product.ProId);
                p = product;
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
