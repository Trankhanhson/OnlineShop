using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class ProductCategoryDAO
    {
        ClothesShopEntities _dbContext = null;
        public ProductCategoryDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductCat> getAll()
        {
            List<ProductCat> ProductCats = _dbContext.ProductCats.Include(x=>x.Category).ToList();
            return ProductCats;
        }
      

        public List<ProductCat> getByCatID(int CatID)
        {
            return _dbContext.ProductCats.Where(x => x.CatID == CatID).ToList();
        }

        public ProductCat getById(int id)
        {
            return _dbContext.ProductCats.Find(id);
        }
        public ProductCat Insert(ProductCat ProductCat)
        {
            ProductCat proc =_dbContext.ProductCats.Add(ProductCat);
            proc.Category = _dbContext.Categories.Where(x => x.CatID == proc.CatID).FirstOrDefault(); //lấy thêm category khi trả về trên view
            _dbContext.SaveChanges();
            return proc;
        }

        public bool Update(ProductCat entity)
        {
            try
            {
                ProductCat ProductCat = _dbContext.ProductCats.Find(entity.ProCatId);
                ProductCat.Name = entity.Name;
                ProductCat.Slug = entity.Slug;
                ProductCat.CatID = entity.CatID;
                ProductCat.Image = entity.Image;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeSattus(int id)
        {
            try
            {
                ProductCat ProductCat = _dbContext.ProductCats.Find(id);
                ProductCat.Status = !ProductCat.Status;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                ProductCat ProductCat = _dbContext.ProductCats.Find(id);
                _dbContext.ProductCats.Remove(ProductCat);
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
