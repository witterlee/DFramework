using System;

namespace DFramework
{
    public static class DoubleExtension
    {
        private static readonly DateTime MinDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public static double ToFixed(this double sourceNum, int fixedNum)
        {
            double sp = Convert.ToDouble(Math.Pow(10, fixedNum));

            if (sourceNum < 0)
                return Math.Truncate(sourceNum) + Math.Ceiling((sourceNum - Math.Truncate(sourceNum)) * sp) / sp;
            return Math.Truncate(sourceNum) + Math.Floor((sourceNum - Math.Truncate(sourceNum)) * sp) / sp;
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToLocalDateTime(this double target)
        {
            DateTime dtDateTime = MinDate.AddTicks((long)target * 10000000);

            return dtDateTime.ToLocalTime();
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime ToUtcDateTime(this double target)
        {
            DateTime dtDateTime = MinDate.AddTicks((long)target * 10000000);

            return dtDateTime;
        }

        /// <summary>
        /// convert unix timestamp to local datetime
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static DateTime? ToNullableLocalDateTime(this double target)
        {
            DateTime dtDateTime;
            if (target > 0)
                dtDateTime = MinDate.AddTicks((long)target * 10000000);

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
        public static DateTime? ToNullableUtcDateTime(this double target)
        {
            DateTime dtDateTime;
            if (target > 0)
                dtDateTime = MinDate.AddTicks((long)target * 10000000);

            else
            {
                return new DateTime?();
            }
            return dtDateTime.ToLocalTime();
        }
    }
}
