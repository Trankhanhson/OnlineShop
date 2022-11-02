using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Areas.Admin.Data
{
    public class ProductCus
    {
        public long ProId { get; set; }
        public string ProName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> ImportPrice { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public Nullable<System.DateTime> StartPromotion { get; set; }
        public Nullable<System.DateTime> StopPromotion { get; set; }
        public string Slug { get; set; }
        public Nullable<bool> Status { get; set; }

        public List<ProductVariation> ProductVariations { get; set; }
        public ProductCat ProductCat { get; set; }
    }
}