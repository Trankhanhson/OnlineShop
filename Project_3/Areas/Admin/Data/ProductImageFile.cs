using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_3.Areas.Admin.Data
{
    public class ProductImageFile
    {
        public long ProColorId { get; set; }
        public long ProId { get; set; }
        public HttpPostedFileWrapper file { get; set; }
        public HttpPostedFileWrapper file1 { get; set; }
        public HttpPostedFileWrapper file2 { get; set; }
        public HttpPostedFileWrapper file3 { get; set; }
        public HttpPostedFileWrapper file4 { get; set; }
        public HttpPostedFileWrapper file5 { get; set; }
    }
}