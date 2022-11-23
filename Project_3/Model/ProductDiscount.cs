using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class ProductDiscount
    {
        public long ProId { get; set; }
        public string ProName { get; set; }
        public string Slug { get; set; }
        public Nullable<int> Price { get; set; }
        public string firstImage { get; set; }
        public int TotalQty { get; set; }
        public bool Check { get; set; }
    }
}