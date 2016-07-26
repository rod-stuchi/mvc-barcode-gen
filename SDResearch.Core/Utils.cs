using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SDResearch.Core
{
    public class Utils
    {
        private static Regex rg = new Regex(@"\d", RegexOptions.None);
        private static DateTime FirstDateBoleto = new DateTime(1997, 10, 7);
        public static DateTime FromInt32ToDateTime(int totalDays)
        {
            DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date = baseDate.AddDays(totalDays).ToLocalTime();

            return date;
        }

        public static string GetFactorFromDate(string date)
        {
            string zeroFactor = "0000";
            if (String.IsNullOrEmpty(date))
                return zeroFactor;

            DateTime dt;
            if (DateTime.TryParse(date, out dt))
            {
                if (dt < FirstDateBoleto)
                    return zeroFactor;

                TimeSpan ts = dt - FirstDateBoleto;

                return ((int)ts.TotalDays).ToString().PadLeft(4, '0');
            }
            else
            {
                return zeroFactor;
            }
        }

        public static DateTime GetDateFromFactor(string factor)
        {
            if (String.IsNullOrEmpty(factor))
                return DateTime.Now;

            int days = 0;
            if (Int32.TryParse(factor, out days))
            {
                DateTime dt = new DateTime(FirstDateBoleto.Ticks).AddDays(days);
                if (dt < FirstDateBoleto)
                    return FirstDateBoleto;
                else
                    return dt;
            }
            else
            {
                return DateTime.Now;
            }
        }

        public static string GetFactorFromValueFormatted(string value, int padding = 10)
        {
            string zeroFactor = "0".PadLeft(padding, '0');
            if (String.IsNullOrEmpty(value))
                return zeroFactor;
            try
            {
                string vFactor = String.Join("", rg.Matches(value).Cast<Match>().Select(x => x.Value).ToArray()).PadLeft(padding, '0');

                return vFactor;
            }
            catch (Exception)
            {

                return zeroFactor;
            }
        }

        public static string GetValueFormattedFromFactor(string factor)
        {
            if (String.IsNullOrEmpty(factor))
                return String.Empty;

            string sV = factor.Insert(factor.Length - 2, ",");

            double dV;
            Double.TryParse(sV, out dV);

            return dV.ToString("N");
        }

        public static string GetNumbersFromLine(string value)
        {
            try
            {
                string v = String.Join("", rg.Matches(value).Cast<Match>().Select(x => x.Value).ToArray());
                return v;
            }
            catch (Exception)
            {

                return "";
            }
        }

        public static int GetBankFromLine(string line)
        {
            int i = 1;

            Int32.TryParse(line.Substring(0, 3), out i);

            return i;
        }

        public static string ConvertToBarcode(string line)
        {
            if (line.Length == 51)
            {
                string l = String.Join("", rg.Matches(line).Cast<Match>().ToArray().SelectMany(x => x.Value));
                StringBuilder sb = new StringBuilder();
                sb.Append(l.Substring(0, 11));
                sb.Append(l.Substring(12, 11));
                sb.Append(l.Substring(24, 11));
                sb.Append(l.Substring(36, 11));
                return sb.ToString();
            }

            if (line.Length == 54)
            {
                string l = String.Join("", rg.Matches(line).Cast<Match>().ToArray().SelectMany(x => x.Value));
                StringBuilder sb = new StringBuilder();
                sb.Append(l.Substring(0, 4));
                sb.Append(l.Substring(32, 15));
                sb.Append(l.Substring(4, 1));
                sb.Append(l.Substring(5, 4));
                sb.Append(l.Substring(10, 10));
                sb.Append(l.Substring(21, 10));
                return sb.ToString();
            }

            return "";
        }

        public static string FormatLine(string line)
        {
            if (String.IsNullOrEmpty(line))
                return String.Empty;

            if (line.Length == 48)
            {
                string formattedLine = line.Insert(12, " ")
                                           .Insert(25, " ")
                                           .Insert(38, " ");

                return formattedLine;
            }

            
            if (line.Length == 47)
            {
                string formattedLine = line.Insert(05, ".")
                                           .Insert(11, " ")
                                           .Insert(17, ".")
                                           .Insert(24, " ")
                                           .Insert(30, ".")
                                           .Insert(37, " ")
                                           .Insert(39, " ");

                return formattedLine;
            }

            return line;
        }
    }
}
