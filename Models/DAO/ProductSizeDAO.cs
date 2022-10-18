﻿using Models.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductSizeDAO
    {
        ClothesShopEntities _dbContext = null;
        public ProductSizeDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductSize> getAll()
        {
            return _dbContext.ProductSizes.ToList();
        }

        public ProductSize getById(long id)
        {
            return _dbContext.ProductSizes.Find(id);
        }
        public bool Insert(ProductSize ps)
        {
            try
            {
                _dbContext.ProductSizes.Add(ps);
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
                ProductSize p = _dbContext.ProductSizes.Find(id);
                _dbContext.ProductSizes.Remove(p);
                _dbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool Update(ProductSize pnew)
        {
            try
            {
                ProductSize p = _dbContext.ProductSizes.Find(pnew.ProSizeID);
                p.NameSize=pnew.NameSize;
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
