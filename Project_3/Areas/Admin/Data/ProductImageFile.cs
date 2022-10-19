using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Areas.Admin.Data
{
    public class ProductImageFile
    {
        public long ProColorID { get; set; }
        public long ProID { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public HttpPostedFileBase DetailImage1 { get; set; }
        public HttpPostedFileBase DetailImage2 { get; set; }
        public HttpPostedFileBase DetailImage3 { get; set; }
        public HttpPostedFileBase DetailImage4 { get; set; }
        public HttpPostedFileBase DetailImage5 { get; set; }
    }
}