using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    [Serializable]
    public class CartItem
    {
        public long ProId { get; set; }
        public int ProSizeID { get; set; }
        public int ProColorID { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}