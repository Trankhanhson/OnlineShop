using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class ReportDTO
    {
        public int date { get; set; }
        public Nullable<int> Revenue { get; set; }
        public Nullable<int> Profit { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> Total_Order { get; set; }
    }
}