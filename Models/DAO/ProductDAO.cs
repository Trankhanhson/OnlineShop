using Models.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.NetworkInformation;

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
            List<Product> list = _dbContext.Products.ToList();
            return list;
        }

        public IEnumerable<Product> getPage(string searchResult, int page, int pageSize)
        {
            IQueryable<Product> model = _dbContext.Products;

            if (!string.IsNullOrEmpty(searchResult))
            {
                model = model.Where(x => x.ProName.Contains(searchResult));
            }
            return model.OrderByDescending(x => x.ProName).ToPagedList(page, pageSize);
        }

        public long Create(Product p)
        {
            Product product =_dbContext.Products.Add(p);
            _dbContext.SaveChanges();
            return product.ProId;
        }

        public List<Product> get10ByProCat(int? proCatId)
        {
            return _dbContext.Products.Where(x => x.ProCatId == proCatId).Include(pi => pi.ProductImages).Include(pv => pv.ProductVariations).OrderByDescending(p=>p.ProId).Take(10).ToList();
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
            return _dbContext.Products.Find(id);
        }

        public void EditPrice(List<Product> products)
        {
            foreach(var product in products)
            {
                Product p = _dbContext.Products.Find(product.ProId);
                p.Price = product.Price;
                p.ImportPrice = product.ImportPrice;
            }
  
            _dbContext.SaveChanges();
        }

        public bool Edit(Product product, List<ProductVariation> variations)
        {
            try
            {
                Product p = _dbContext.Products.Find(product.ProId);
                p.ProName = product.ProName;
                p.Material = product.Material;
                p.Description = product.Description;
                p.ProCatId = product.ProCatId;
                p.ImportPrice = product.ImportPrice;
                p.Price = product.Price;

                var oldVariation = p.ProductVariations;
                List<ProductVariation> listAdd = new List<ProductVariation>();
                List<ProductVariation> listRemove = new List<ProductVariation>();

                //Tìm các biến thể bị xóa => danh sách cũ có, mới không có
                foreach(var o in oldVariation)
                {
                    ProductVariation pv = variations.Where(n => n.ProSizeID == o.ProSizeID && n.ProColorID == o.ProColorID).FirstOrDefault();
                    //Phần tử cũ không tồn tại trong danh sách mới
                    if(pv == null)
                    {
                        listRemove.Add(o);
                    }
                }

                //Tìm các biến thể sẽ được thêm => danh sách cũ có, mới không có
                foreach (var n in variations)
                {
                    ProductVariation po = oldVariation.Where(o=>o.ProColorID == n.ProColorID && o.ProSizeID == n.ProSizeID).FirstOrDefault();
                    //phẩn tử mới không tồn tại trong danh sách cũ
                    if (po == null)
                    {
                        n.Ordered = 0;
                        listAdd.Add(n);
                    }
                }

                if(listRemove.Count > 0)
                {
                    _dbContext.ProductVariations.RemoveRange(listRemove);
                }
                if(listAdd.Count > 0)
                {
                    _dbContext.ProductVariations.AddRange(listAdd);
                }
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool? ChangeStatus(long ProId)
        {
            var product = _dbContext.Products.Find(ProId);
            product.Status = !product.Status;
            _dbContext.SaveChanges();
            return product.Status;
        }
    }
}
