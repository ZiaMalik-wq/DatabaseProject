﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace FYP_Management.Validations1
{
    public static class ValidationsHelper
    {
        public static bool email(string email)
        {
            if (email.Length >= 30)
            {
                return false;
            }
            string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";
            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
        public static bool name(string name)
        {
            return name.Length >= 3 && name.Length<100;
        }
        public static bool age16plus(DateTime dt)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dt_18 = dt.AddYears(18);
            if(dtNow < dt_18)
            {
                return false;
            }
            return true;
        }
        public static bool contact(string number)
        {
            if(number.Length>=20)
            {
                return false;
            }
            int count = 0;
            for(int i=0;i<number.Length;i++)
            {
                if (number[i] == ' ')
                {
                    continue;
                }
                else if (isInt(number[i]))
                {
                    count++;
                }
            }
            return (count == 11);
        }
        public static bool isInt(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }
        public static bool GreaterThanZero(int num)
        {
            return num > 0;
        }
        public static bool NumberInput(TextCompositionEventArgs e)
        {
            return new Regex("[^0-9]+").IsMatch(e.Text);
        }
        public static bool description(string text)
        {
            return text.Length > 50;
        }
        public static bool percent(int num)
        {
            return (num >= 0 && num <= 100);
        }

    }
}
