using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace Project_3.common
{
    public class MailHelper
    {
        public static void SendMail(string toEmailAddress, string subject, string content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmailAddress);
            mail.From = new MailAddress("transon30082002@gmail.com");
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587; // 25 465
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("transon30082002@gmail.com", "tatzefsanbfrxkwi");
            smtp.Send(mail);
        }
    }
}