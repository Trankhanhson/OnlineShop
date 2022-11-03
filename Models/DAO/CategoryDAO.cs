using Models.Framework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class CategoryDAO
    {
        ClothesShopEntities _dbContext = null;
        public CategoryDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<Category> getAll()
        {
            _dbContext.Configuration.ProxyCreationEnabled = false;
            List<Category> categories = _dbContext.Categories.ToList();
            return categories;
        }
        public List<Category> getByType(string type)
        {
            _dbContext.Configuration.ProxyCreationEnabled = false;
            List<Category> categories = _dbContext.Categories.Include(x=>x.ProductCats).Where(x => x.type == type).ToList();
            return categories;
        }

        public Category getByCatID(int CatID)
        {
            return _dbContext.Categories.SingleOrDefault(x => x.CatID == CatID);
        }
        public Category Insert(Category Category)
        {

            Category cat = _dbContext.Categories.Add(Category);
            _dbContext.SaveChanges();
            return cat;
        }

        public bool Update(Category entity)
        {
            try
            {
                Category category = _dbContext.Categories.Find(entity.CatID);
                category.Name = entity.Name;
                category.Slug = entity.Slug;
                category.type = entity.type;
                category.Status = entity.Status;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeSattus(long id)
        {
            try
            {
                Category Category = _dbContext.Categories.Find(id);
                Category.Status = !Category.Status;
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
                Category Category = _dbContext.Categories.Find(id);
                _dbContext.Categories.Remove(Category);
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
