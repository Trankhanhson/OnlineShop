using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Models.DAO
{
    public class ImportBillDetailDAO
    {
        ClothesShopEntities _dbContext = null;
        public ImportBillDetailDAO()
        {
            _dbContext = new ClothesShopEntities();
        }
        public List<ImportBillDetail> getAll()
        {
            return _dbContext.ImportBillDetails.ToList();
        }
        public bool Insert(List<ImportBillDetail> listDetail)
        {
            try
            {
                _dbContext.ImportBillDetails.AddRange(listDetail);
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
