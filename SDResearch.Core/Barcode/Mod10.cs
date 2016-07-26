using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Barcode
{
    public class Mod10
    {
        public static string GetMod10Digit(string number)
        {
            try
            {
                var sum = 0;
                var alt = true;
                var digits = number.ToCharArray();
                for (int i = digits.Length - 1; i >= 0; i--)
                {
                    var curDigit = (digits[i] - 48);
                    if (alt)
                    {
                        curDigit *= 2;
                        if (curDigit > 9)
                            curDigit -= 9;
                    }
                    sum += curDigit;
                    alt = !alt;
                }
                if ((sum % 10) == 0)
                {
                    return "0";
                }
                return (10 - (sum % 10)).ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static bool CheckMod10Digit(string number)
        {
            int dig = int.Parse(GetMod10Digit(number.Substring(0, number.Length - 1)));
            int toCheck = int.Parse(number.Substring(number.Length - 1, 1));

            return dig == toCheck;
        }

        public static string ConcactMod10Digit(string number)
        {
            return string.Concat(number, GetMod10Digit(number));
        }
    }
}
