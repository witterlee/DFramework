using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DFramework
{
    public static class EnumerableExtension
    {
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable != null && enumerable.Count() > 0)
                foreach (T item in enumerable)
                {
                    action(item);
                }
        }
    }
}
