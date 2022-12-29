using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class ImportBillDAO
    {
        ClothesShopEntities _dbContext = null;
        public ImportBillDAO()
        {
            _dbContext = new ClothesShopEntities();
        }
        public List<ImportBill> getAll()
        {
            _dbContext.Configuration.LazyLoadingEnabled = false;
            return _dbContext.ImportBills.Include(i => i.User).ToList();
        }

        public ImportBill getById(long id)
        {
            return _dbContext.ImportBills.Find(id);
        }
        public ImportBill Insert(ImportBill im)
        {
            try
            {
                ImportBill ressult = _dbContext.ImportBills.Add(im);
                _dbContext.SaveChanges();
                return ressult;
            }
            catch
            {
                return im;
            }
        }

        public void Delete(long id)
        {
            var im = _dbContext.ImportBills.Find(id);
            foreach(var detail in im.ImportBillDetails)
            {
                var pv = _dbContext.ProductVariations.Find(detail.ProVariationID);
                if(pv != null)
                {
                    pv.Quantity = pv.Quantity - detail.Quantity;
                }
            }
            _dbContext.ImportBillDetails.RemoveRange(im.ImportBillDetails);
            _dbContext.ImportBills.Remove(im);
            _dbContext.SaveChanges();
        }
    }
}
