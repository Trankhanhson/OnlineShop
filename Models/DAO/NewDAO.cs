using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class NewDAO
    {
        ClothesShopEntities _dbContext = null;
        public NewDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<New> getAll()
        {
            _dbContext.Configuration.ProxyCreationEnabled = false;

            List<New> News = _dbContext.News.Include(x=>x.User).ToList();
            return News;
        }

        public New getById(int id)
        {
            return _dbContext.News.Find(id);
        }
        public New Insert(New entity)
        {

            New n = _dbContext.News.Add(entity);
            _dbContext.SaveChanges();
            return n;
        }

        public bool Update(New entity)
        {
            try
            {
                New New = _dbContext.News.Find(entity.NewID);
                New.Title = entity.Title;
                New.Content = entity.Content;
                New.Image = entity.Image;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            try
            {
                New New = _dbContext.News.Find(id);
                New.Status = !New.Status;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                New New = _dbContext.News.Find(id);
                _dbContext.News.Remove(New);
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
