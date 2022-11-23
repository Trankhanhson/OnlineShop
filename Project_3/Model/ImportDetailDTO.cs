using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class ImportDetailDTO
    {
        public long ProVariationID { get; set; }
        public long ProId { get; set; }
        public string Image { get; set; }
        public string NameProduct { get; set; }
        public long ProColorId { get; set; }
        public string NameColor { get; set; }
        public string NameSize { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> ImportPrice { get; set; }
    }
}