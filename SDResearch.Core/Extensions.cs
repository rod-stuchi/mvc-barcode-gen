using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDResearch.Core
{
    public static class Extensions
    {
        public static int ToInt32(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return Convert.ToInt32(Math.Ceiling(unixTime.TotalDays));
        }
    }
}
