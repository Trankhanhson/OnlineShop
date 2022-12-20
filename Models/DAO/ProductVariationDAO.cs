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

        public void editQuantity(List<ImportBillDetail> billDetails)
        {
            foreach(var detail in billDetails)
            {
                ProductVariation v = _dbContext.ProductVariations.Find(detail.ProVariationID);
                v.Quantity += detail.Quantity;
                
            }
            _dbContext.SaveChanges();
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

        public bool Delete(long idVariation)
        {
            try
            {
                var item = _dbContext.ProductVariations.FirstOrDefault(x => x.ProVariationID == idVariation);
                _dbContext.ProductVariations.Remove(item);
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

        public void editOrdered(List<OrderDetail> listOrderDetail)
        {
            foreach (var item in listOrderDetail)
            {
                ProductVariation v = _dbContext.ProductVariations.Find(item.ProVariationID);
                v.Ordered += item.Quantity;
            }
            _dbContext.SaveChanges();
        }
    }
}
