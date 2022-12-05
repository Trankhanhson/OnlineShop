using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class VariationCart
    {
        public long ProId { get; set; }
        public int proSizeId { get; set; }
        public int proColorId { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Price { get; set; }
        public int Percent { get; set; }
        public string Image { get; set; }
        public string ProName { get; set; }
        public string proSizeName { get; set; }
        public string srcColor { get; set; }
    }
}