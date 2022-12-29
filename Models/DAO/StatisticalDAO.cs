using Models.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class StatisticalDAO
    {
        private ClothesShopEntities db = null;
        public StatisticalDAO()
        {
            db = new ClothesShopEntities();
        }

        public List<Statistical> getAll()
        {
            return db.Statisticals.ToList();
        }

        public Statistical getByDate(DateTime date)
        {
            return db.Statisticals.Where(s=> s.Date.Value.Year == date.Date.Year && s.Date.Value.Month == date.Date.Month && s.Date.Value.Day == date.Date.Day).FirstOrDefault();
        }

            public void Savechages()
            {
                db.SaveChanges();
            }
        public void Insert(Statistical s)
        {
            db.Statisticals.Add(s);
        }
    }
}
