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

      
    }
}
