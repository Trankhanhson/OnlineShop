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
        public ProductVariation ProVariation { get; set; }    
        public int Quantity { get; set; }

        public string Image { get; set; }
    }
}