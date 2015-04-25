using System;
using System.Diagnostics;

namespace DFramework
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        /// <summary>
        /// convert datetime to unix timestamp
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int ToUnixTimestamp(this DateTime target)
        {
            return (int)((target.ToUniversalTime() - MinDate).TotalSeconds);
        }

        /// <summary>
        /// convert datetime to unix timestamp
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double ToDoubleUnixTimestamp(this DateTime target)
        {
            return (target.ToUniversalTime() - MinDate).TotalSeconds;
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToLocalDateTime(this int target)
        {
            DateTime dtDateTime = MinDate.AddTicks((long)target * (long)10000000);

            return dtDateTime.ToLocalTime();
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToUtcDateTime(this int target)
        {
            DateTime dtDateTime = MinDate.AddTicks((long)target * (long)10000000);

            return dtDateTime;
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime? ToNullableLocalDateTime(this int target)
        {
            DateTime dtDateTime;
            if (target > 0)
                dtDateTime = MinDate.AddTicks((long)target * (long)10000000);

            else
            {
                return new DateTime?();
            }
            return dtDateTime.ToLocalTime();
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime? ToNullableUtcDateTime(this int target)
        {
            DateTime dtDateTime;
            if (target > 0)
                dtDateTime = MinDate.AddTicks((long)target * (long)10000000);

            else
            {
                return new DateTime?();
            }
            return dtDateTime.ToLocalTime();
        }
    }
}
