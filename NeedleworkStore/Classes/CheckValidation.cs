using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeedleworkStore.Classes
{
    static internal class CheckValidation
    {      
        public static bool CheckLogin(string txt)
        {
            foreach (char ch in txt)
            {
                if (ch == '&' || ch == '+' || ch == '=' || ch == '<' || ch == '>' || ch == ',' || ch == '-' || ch == '"' || ch == '\'')
                {
                    return false;
                }
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
