﻿using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductImagesDAO
    {
        ClothesShopEntities _dbContext = null;
        public ProductImagesDAO()
        {
            _dbContext = new ClothesShopEntities();
        }

        public List<ProductImage> getAll()
        {
            return _dbContext.ProductImages.ToList();
        }

        public List<ProductImage> getByIdPro(long idPro)
        {
            List<ProductImage> list = _dbContext.ProductImages.Where(i=>i.ProID==idPro).ToList();
            return list;
        }

        public ProductImage getByKey(long? ProId,long? ProColorId)
        {
            return _dbContext.ProductImages.Where(x=>x.ProID==ProId && x.ProColorID==ProColorId).FirstOrDefault();
        }
        public void Insert(ProductImage proImg)
        {
            _dbContext.ProductImages.Add(proImg);
            _dbContext.SaveChanges();
        }

        public void Edit(ProductImage productImage)
        {
            ProductImage oldProImg = _dbContext.ProductImages.Where(pi => pi.ProColorID == productImage.ProColorID && pi.ProID == productImage.ProID).FirstOrDefault();
            oldProImg.Image = productImage.Image;
            oldProImg.DetailImage1 = productImage.DetailImage1;
            oldProImg.DetailImage2 = productImage.DetailImage2;
            oldProImg.DetailImage3 = productImage.DetailImage3;
            oldProImg.DetailImage4 = productImage.DetailImage4;
            oldProImg.DetailImage5 = productImage.DetailImage5;
            _dbContext.SaveChanges();
        }

        public void Delete(long ProId)
        {
            var list = _dbContext.ProductImages.Where(x => x.ProID == ProId).ToList();
            if(list.Count > 0)
            {
                _dbContext.ProductImages.RemoveRange(list);
                _dbContext.SaveChanges();
            }
        }
        
        public void DeleteRange(List<ProductImage> productImages)
        {

            _dbContext.ProductImages.RemoveRange(productImages);
            _dbContext.SaveChanges();
        }
    }
}
