﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace Project_3.common
{
    public static class MethodCommnon
    {
        public static string ToUrlSlug(string text)

        {
            for (int i = 32; i < 48; i++)

            {

                text = text.Replace(((char)i).ToString(), " ");

            }

            text = text.Replace(".", "-");

            text = text.Replace(" ", "-");

            text = text.Replace(",", "-");

            text = text.Replace(";", "-");

            text = text.Replace(":", "-");



            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");



            string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

        }

        public static decimal CountDiscountPrice(int Price,int Amount, string TypeAmount)
        {
            decimal DiscountPrice = 0;
            if (TypeAmount == "0") //giảm giá theo tiền
            {
                DiscountPrice = Price - Amount;
            }
            else  //giảm giá theo %
            {
                DiscountPrice = Math.Round(Price - ((Convert.ToDecimal(Amount) / 100) * Price), 0);
            }
            return DiscountPrice;
        }

        public static string NextString(this Random random, int length)
        {
            const string chars = "0123456789";

            IEnumerable<string> string_Enumerable = Enumerable.Repeat(chars, length);
            char[] arr = string_Enumerable.Select(s => s[random.Next(s.Length)]).ToArray();
            return new string(arr);
        }
    }
}