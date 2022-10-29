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
        ClothesShopEntities _dbContext = null;
        public ProductColorDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductColor> getAll()
        {
            return _dbContext.ProductColors.ToList();
        }
        public ProductColor getById(long id)
        {
            return _dbContext.ProductColors.Find(id);
        }
        public ProductColor Insert(ProductColor ps)
        {
            try
            {
                var result = _dbContext.ProductColors.Add(ps);
                _dbContext.SaveChanges();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                ProductColor p = _dbContext.ProductColors.Find(id);
                _dbContext.ProductColors.Remove(p);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(ProductColor pnew)
        {
            try
            {
                ProductColor p = _dbContext.ProductColors.Find(pnew.ProColorID);
                p.NameColor = pnew.NameColor;
                p.ImageColor = pnew.ImageColor;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
