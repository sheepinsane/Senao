using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWorkflow.ExtUni
{
    public static class DateTimeExtensions
    {
        public static string pTimestampString(this DateTime dateTime)
        {
            long unixTimestampMilliseconds = (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return unixTimestampMilliseconds.ToString();
        }
    }

    public static class StringExt 
    {
        public static int pToInt(this string source)
        {
            int i = 0;
            int.TryParse(source, out i);
            return i;
        }
    }
}