using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CategoryDAO
    {
        private ClothesShopEntities _ClothesShopEntities = null;

        public CategoryDAO()
        {
            _ClothesShopEntities = new ClothesShopEntities();
        }

        public List<Category> ListAll()
        {
            List<Category> list = _ClothesShopEntities.Categories.ToList();
            return list;
        }

        public void Create(int CatId,string CatName, int ParentID)
        {
            Category category = new Category();
            category.CatId = CatId;
            category.CatName = CatName;
            category.ParentID = ParentID;
             _ClothesShopEntities.Categories.Add(category);
            _ClothesShopEntities.SaveChanges();
        }

        public void Edit(Category cat)
        {
            var category = _ClothesShopEntities.Categories.Where(c => c.CatId == cat.CatId).FirstOrDefault<Category>();
            category.CatName = cat.CatName;
            category.ParentID = cat.ParentID;
            _ClothesShopEntities.SaveChanges();
        }
    }
}
