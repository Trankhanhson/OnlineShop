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
            _dbContext.Configuration.LazyLoadingEnabled = false;
            List<Product> list = _dbContext.Products.Include(pv => pv.ProductVariations.Select(pc => pc.ProductColor))
                .Include(pv => pv.ProductVariations.Select(ps => ps.ProductSize))
                .Include(pi => pi.ProductImages).Include(pc => pc.ProductCat).ToList()
                .Select(p => new Product()
                {
                    ProId = p.ProId,
                    ProName = p.ProName,
                    ProCatId = p.ProCatId,
                    Price = p.Price,
                    ImportPrice = p.ImportPrice,
                    Status = p.Status,
                    ProductCat = new ProductCat() { Name = p.ProName, ProCatId = p.ProCatId},
                    ProductVariations = p.ProductVariations.Select(pv => new ProductVariation()
                    {
                        ProId = pv.ProId,
                        ProVariationID = pv.ProVariationID,
                        ProColorID = pv.ProColorID,
                        ProSizeID = pv.ProSizeID,
                        ProductColor = new ProductColor() {  NameColor = pv.ProductColor.NameColor, ImageColor = pv.ProductColor.ImageColor },
                        ProductSize = new ProductSize() {NameSize = pv.ProductSize.NameSize },
                        Quantity = pv.Quantity
                    }).ToList(),
                    ProductImages = p.ProductImages.Select(pi=>new ProductImage()
                    {
                        ProductColor = new ProductColor() { ProColorID=pi.ProColorID, NameColor=pi.ProductColor.NameColor, ImageColor = pi.ProductColor.ImageColor},
                        Image = pi.Image
                    }).ToList()

                }).ToList();
            return list;
        }

        public List<Product> getAllDefault()
        {
            return _dbContext.Products.Include(pi=>pi.ProductImages).Include(pv=>pv.ProductVariations).ToList();
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
            return _dbContext.Products.Include(pi => pi.ProductImages).Include(pc=>pc.ProductCat).Include(pv=>pv.ProductVariations).Where(p=>p.ProId==id).FirstOrDefault();
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

        public bool Edit(Product product)
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
