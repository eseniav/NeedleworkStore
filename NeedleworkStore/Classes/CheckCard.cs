using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeedleworkStore.Classes
{
    static public class CheckCard
    {
        static public bool ContainsOnlyDigits(string input) => !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        static public bool IsValidMonth(string monthText)
        {
            if (!ContainsOnlyDigits(monthText) || monthText.Length != 2)
                return false;
            int month = int.Parse(monthText);
            return month >= 1 && month <= 12;
        }
        static public bool IsValidYear(string yearText)
        {
            if (!ContainsOnlyDigits(yearText) || yearText.Length != 2)
                return false;
            int year = int.Parse(yearText);
            return year >= DateTime.Now.Year % 100;
        }
        static public bool IsNotExpired(string monthText, string yearText)
        {
            if (!IsValidMonth(monthText) || !IsValidYear(yearText))
                return false;
            int year = int.Parse(yearText);
            int currentYear = DateTime.Now.Year % 100;
            if (year > currentYear)
                return true;
            return int.Parse(monthText) >= DateTime.Now.Month;
        }
        static public bool CheckCardNumber(string card)
        {
            return ContainsOnlyDigits(card) && card.Length == 16;
        }
    }
}
