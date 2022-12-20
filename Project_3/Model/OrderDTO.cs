using Models.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class OrderDTO
    {
        public long OrdID { get; set; }
        public Nullable<int> VoucherId { get; set; }
        public string ReceivingPhone { get; set; }
        public string ReceivingCity { get; set; }
        public string ReceivingMail { get; set; }
        public string ReceivingDistrict { get; set; }
        public string ReceivingWard { get; set; }
        public string ReceivingAddress { get; set; }
        public string PaymentType { get; set; }
        public Nullable<int> StatusOrderId { get; set; }
        public Nullable<int> MoneyTotal { get; set; }
        public int TotalOriginPrice { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string Note { get; set; }
        public int StatusOderId { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        

        public List<OrderDetailDTO> OrderDetailDTOs { get; set; }
    }
}