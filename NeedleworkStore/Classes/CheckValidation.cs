using NeedleworkStore.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeedleworkStore.Classes
{
    static internal class CheckValidation
    {
        public static char[] phone = new char[11];
        public static bool CheckLogin(string txt)
        {
            if (txt.Length < 3)
                return false;
            foreach (char ch in txt)
            {
                if (ch == '&' || ch == '+' || ch == '=' || ch == '<' || ch == '>' 
                    || ch == ',' || ch == '"' || ch == '\'' || ch == '`' || ch == '~')                
                    return false;
            }
            return true;
        }
        public static bool CheckPass(string txt)
        {
            if (txt.Length < 8)
                return false;
            bool isCapital = false;
            bool isNumber = false;
            foreach (char ch in txt)
            {
                if(ch > 'A' && ch < 'Z')
                    isCapital = true;
                if(ch > '0' && ch < '9')
                    isNumber = true;
            }
            return isCapital && isNumber;
        }
        public static bool CheckPhone(string txt)
        {
            if (txt.Length < 11)
                return false;
            int i = 0;
            foreach (char ch in txt)
            {
                if (ch > '0' && ch < '9')
                {
                    phone[i] = ch;
                    ++i;
                }                    
            }
            if ((phone.Length != 11) || (phone.Length == 11 && (phone[0] != 7 && phone[0] != 8)))
            {
                Array.Clear(phone, 0, phone.Length);
                return false;
            }
            return phone.Length == 11;
        }
        public static string CorrectPhone()
        {
            return "+7(" + phone[1] + phone[2] + phone[3] + ")"
                + phone[4] + phone[5] + phone[6] + "-"
                + phone[7] + phone[8] + "-"
                + phone[9] + phone[10];
        }
        public static bool CheckEmail(string txt)
        {
            bool isAt = false;
            bool isDot = false;
            foreach (char ch in txt)
            {
                if(ch == '@')
                    isAt = true;
                if(ch == '.')
                    isDot = true;
            }
            return isAt && isDot;
        }
        public static bool CheckDate(DateTime date)
        {            
            return (DateTime.Today.Year - date.Year) <= 100 && (DateTime.Today.Year - date.Year)  >= 14 || date == null;
        }
        public static bool CheckNames(string txt)
        {
            if (!CheckValidation.CheckEmptyNull(txt))
                return true;
            foreach (char ch in txt)
            {
                if ((ch < 'А' && ch > 'Я' || ch < 'а' && ch > 'я') && (ch != '-' || ch != ' '))
                    return false;
            }
            return true;
        }
        public static bool CheckEmptyNull(string txt)
        {
            return !String.IsNullOrEmpty(txt) && !String.IsNullOrWhiteSpace(txt);
        }
        public static bool CheckInt(string txt)
        {            
            return Int32.TryParse(txt.Trim(), out int a);
        }
    }
}
