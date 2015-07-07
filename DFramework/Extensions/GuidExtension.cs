using System;
using System.Diagnostics;

namespace DFramework
{
    public static class GuidExtension
    {
        [DebuggerStepThrough]
        public static string Shrink(this Guid target)
        {
            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty(this Guid target)
        {
            return target == null || target == Guid.Empty;
        }
    }
}
