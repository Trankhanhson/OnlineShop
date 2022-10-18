using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models
{
    public class ProductVariationDAO
    {
        private ClothesShopEntities _dbContext = new ClothesShopEntities();
        public ProductVariationDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductVariation> getAll()
        {
            return _dbContext.ProductVariations.ToList();
        }

        public List<ProductVariation> getById(long id)
        {
            _dbContext.Configuration.ProxyCreationEnabled = false;
            var list = _dbContext.ProductVariations.Include(pz=>pz.ProductSize).Include(pc=>pc.ProductColor).Where(p=>p.ProId==id).ToList();
            return list;
        }

        public void Insert(List<ProductVariation> ps)
        {
            _dbContext.ProductVariations.AddRange(ps);
            _dbContext.SaveChanges();
        }

        public bool DeleteByProId(long ProId)
        {
            try
            {
                var listPv = _dbContext.ProductVariations.Where(x=>x.ProId==ProId).ToList();
                _dbContext.ProductVariations.RemoveRange(listPv);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Edit(List<ProductVariation> variations)
        {
            try
            {
                List<ProductVariation> oldVariations = _dbContext.ProductVariations.Where(x => x.ProId == variations[0].ProId).ToList();
                _dbContext.ProductVariations.RemoveRange(oldVariations);
                _dbContext.ProductVariations.AddRange(variations);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public ProductVariation getByForeignKey(long ProId,int ProColorId,int ProSizeId)
        {
            return _dbContext.ProductVariations.Where(x=>x.ProId==ProId && x.ProColorID==ProColorId && x.ProSizeID==ProSizeId).FirstOrDefault();
        }
    }
}
