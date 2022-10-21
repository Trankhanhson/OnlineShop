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
        public HttpPostedFileBase file { get; set; }
        public HttpPostedFileBase file1 { get; set; }
        public HttpPostedFileBase file2 { get; set; }
        public HttpPostedFileBase file3 { get; set; }
        public HttpPostedFileBase file4 { get; set; }
        public HttpPostedFileBase file5 { get; set; }
    }
}