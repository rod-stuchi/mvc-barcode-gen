using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core.Barcode
{
    public class Mod11
    {
        public static string GetMod11Digit(string number, int replaceZeroBy = 0)
        {
            try
            {

                int NumDig = 1;
                int LimMult = 9;
                int Mult, Soma;

                string digit = "";


                for (int n = 1; n <= NumDig; n++)
                {
                    Soma = 0;
                    Mult = 2;
                    for (int i = number.Length - 1; i >= 0; i--)
                    {
                        Soma += (Mult * int.Parse(number[i].ToString()));
                        if (++Mult > LimMult) Mult = 2;
                    }
                    digit = (((Soma * 10) % 11) % 10).ToString();
                }

                if (digit.Equals("0"))
                    digit = replaceZeroBy.ToString();

                return digit;

            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public static bool CheckMod11Digit(string number, int replaceZeroBy = 0)
        {
            int dig = int.Parse(GetMod11Digit(number.Substring(0, number.Length - 1), replaceZeroBy));
            int toCheck = int.Parse(number.Substring(number.Length - 1, 1));

            return dig == toCheck;
        }

        public static string ConcactMod11Digit(string number)
        {
            return string.Concat(number, GetMod11Digit(number));
        }
    }
}
