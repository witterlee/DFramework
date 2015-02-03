using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    public static class DoubleExtension
    {
        public static double ToFixed(this double sourceNum, int fixedNum)
        {
            double sp = Convert.ToDouble(Math.Pow(10, fixedNum));

            if (sourceNum < 0)
                return Math.Truncate(sourceNum) + Math.Ceiling((sourceNum - Math.Truncate(sourceNum)) * sp) / sp;
            else
                return Math.Truncate(sourceNum) + Math.Floor((sourceNum - Math.Truncate(sourceNum)) * sp) / sp;
        }
    }
}
