using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class ProductDTO
    {
        public long ProId { get; set; }
        public int ProCatId { get; set; }
        public string ProName { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<int> ImportPrice { get; set; }
        public string Slug { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> ImportDate { get; set; }
        public string firstImage { get; set; }
        public int TotalQty { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Percent { get; set; }
        public virtual ProductCat ProductCat { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<DiscountDetail> DiscountDetails { get; set; }
        public ICollection<ProductVariation> ProductVariations { get; set; }
    }
}