using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class FeedbackDAO
    {
        ClothesShopEntities _dbContext = null;
        public FeedbackDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<Feedback> getByProduct(long idProduct)
        {
            return _dbContext.Feedbacks.Where(f=>f.ProductVariation.ProId == idProduct && f.Status == true).OrderByDescending(f=>f.FeedbackId).ToList();
        }

        public Feedback getById(long FeedbackId)
        {
            _dbContext.Configuration.LazyLoadingEnabled = false;
            return _dbContext.Feedbacks.Find(FeedbackId);
        }

        public long getIdByForeignKey(long ProId, long CusId)
        {
            Feedback feedback = _dbContext.Feedbacks.Where(f => f.ProductVariation.ProId == ProId && f.CusID == CusId).FirstOrDefault();
            if(feedback == null)
            {
                return 0;
            }
            else
            {
                return feedback.FeedbackId;
            }
        }
        public List<Feedback> getAll(bool status)
        {
            List<Feedback> Feedbacks = _dbContext.Feedbacks.Where(f=>f.Status==status).ToList();
            return Feedbacks;
        }


        public long Insert(Feedback Feedback)
        {
            Feedback f = _dbContext.Feedbacks.Add(Feedback);
            _dbContext.SaveChanges();
            return f.FeedbackId;
        }

        public bool Update(Feedback entity)
        {
            try
            {
                Feedback Feedback = _dbContext.Feedbacks.Find(entity.FeedbackId);
                Feedback.Stars = entity.Stars;
                Feedback.Content = entity.Content;
                Feedback.Image = entity.Image;
                Feedback.Status = entity.Status;
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ChangeStatus(long id)
        {
            var f = _dbContext.Feedbacks.Find(id);
            f.Status = !f.Status;
            _dbContext.SaveChanges();
        }
    }
}
