using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class VoucherDAO
    {
        ClothesShopEntities db = null;
        public VoucherDAO()
        {
            db = new ClothesShopEntities();
        }

        public List<Voucher> getVoucherNow()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Vouchers.Where(v=>v.StartDate<=DateTime.Now && v.EndDate>=DateTime.Now).ToList();
        }

        public List<Voucher> getAll()
        {
            db.Configuration.LazyLoadingEnabled = false;
            return db.Vouchers.ToList();
        }

        public Voucher Insert(Voucher d)
        {
            var a = db.Vouchers.Add(d);
            db.SaveChanges();
            return a;
        }

        public Voucher getById(int id)
        {
            return db.Vouchers.Find(id);
        }

        public void Edit(Voucher v)
        {
            var vOld = db.Vouchers.Find(v.VoucherId);
            vOld.Name = v.Name;
            vOld.Description = v.Description;
            vOld.StartDate = v.StartDate;
            vOld.EndDate = v.EndDate;
            vOld.Amount = v.Amount;
            vOld.TypeAmount = v.TypeAmount;
            vOld.MiximumMoney = v.MiximumMoney;
            vOld.MaximumMoney = v.MaximumMoney;
            vOld.MaxUses = v.MaxUses;
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var voucher = db.Vouchers.Find(id);
            db.Vouchers.Remove(voucher);
            db.SaveChanges();
        }
    }
}
