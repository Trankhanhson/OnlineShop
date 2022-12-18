using Models.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class SectionDAO
    {
        ClothesShopEntities _dbcontext = null;
        public SectionDAO()
        {
            _dbcontext = new ClothesShopEntities();
        }

        public List<Section> getSectionOfList(int PageId)
        {
            return _dbcontext.Sections.Where(s => s.PageId == PageId).ToList();
        }

        public int Insert(Section s, List<ProductSection> productSections)
        {
            var section = _dbcontext.Sections.Add(s);
            foreach(var item in productSections)
            {
                item.SectionId = section.SectionId;
            }
            _dbcontext.ProductSections.AddRange(productSections);
            _dbcontext.SaveChanges();
            return section.SectionId;
        }

        public void Edit(Section s, List<ProductSection> productSections)
        {
            var section = _dbcontext.Sections.Find(s.SectionId);
            if (s.DisplayOrder != section.DisplayOrder)
            {
                ChangeDisplayOrder(s.DisplayOrder.Value, section.DisplayOrder.Value);
            }
            section.Title = s.Title;
            section.Image1 = s.Image1;
            section.Image2 = s.Image2;
            section.Image3 = s.Image3;
            section.Link1 = s.Link1;
            section.Link2 = s.Link2;
            section.Link3 = s.Link3;
            section.DisplayOrder = s.DisplayOrder;
            _dbcontext.ProductSections.RemoveRange(section.ProductSections);
            _dbcontext.ProductSections.AddRange(productSections);
            _dbcontext.SaveChanges();
        }

        public void ChangeDisplayOrder(int DisplayOrder,int NewDisplayOrder)
        {
            Section section = _dbcontext.Sections.Where(s => s.DisplayOrder == DisplayOrder).FirstOrDefault();
            section.DisplayOrder  = NewDisplayOrder;
            _dbcontext.SaveChanges();
        }

        public bool Delete(int id)
        {
            try
            {
                var section = _dbcontext.Sections.Find(id);
                _dbcontext.ProductSections.RemoveRange(section.ProductSections);
                _dbcontext.Sections.Remove(section);
                _dbcontext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Section getById(int id)
        {
            return _dbcontext.Sections.Find(id);
        }

        public List<Product> getProBySection(int id)
        {
            _dbcontext.Configuration.LazyLoadingEnabled = true;
            var products = _dbcontext.ProductSections.Where(ps=>ps.SectionId == id).Select(p=>p.Product).ToList();
            return products;
        }
    }
}
