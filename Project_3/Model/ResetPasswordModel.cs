using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_3.Model
{
    public class ResetPasswordModel
    {
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
        public string ResetCode { get; set; }
    }
}