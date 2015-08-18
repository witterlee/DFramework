using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DFramework.Utilities
{
    /// <summary>
    /// 检查器
    /// </summary>
    public class Check
    {
        internal Check()
        {
        }
        /// <summary>
        /// 参数检查器
        /// </summary>
        public class Argument
        {
            internal Argument()
            {
            }

            [DebuggerStepThrough]
            public static void IsNotEmpty(Guid argument, string argumentName)
            {
                if (argument == Guid.Empty)
                {
                    throw new ArgumentException("\"{0}\" 不能为空guid.".FormatWith(argumentName), argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotEmpty(string argument, string argumentName)
            {
                if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
                {
                    throw new ArgumentException("\"{0}\" 不能为空.".FormatWith(argumentName), argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotOutOfLength(string argument, int length, string argumentName)
            {
                if (argument.Trim().Length > length)
                {
                    throw new ArgumentException("\"{0}\" 不能超过 {1} 个字符.".FormatWith(argumentName, length), argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNull(object argument, string argumentName)
            {
                if (argument == null)
                {
                    throw new ArgumentNullException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegative(int argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotZero(int argument, string argumentName)
            {
                if (argument == 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(int argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegative(long argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotZero(long argument, string argumentName)
            {
                if (argument == 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }


            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(long argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegative(float argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotZero(float argument, string argumentName)
            {
                if (argument == 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(float argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }


            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(double argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegative(decimal argument, string argumentName)
            {
                if (argument < 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotZero(decimal argument, string argumentName)
            {
                if (argument == 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(decimal argument, string argumentName)
            {
                if (argument <= 0)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }


            [DebuggerStepThrough]
            public static void IsNotInPast(DateTime argument, string argumentName)
            {
                if (argument < DateTime.Now)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotInFuture(DateTime argument, string argumentName)
            {
                if (argument > DateTime.Now)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegative(TimeSpan argument, string argumentName)
            {
                if (argument < TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotNegativeOrZero(TimeSpan argument, string argumentName)
            {
                if (argument <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotEmpty<T>(IEnumerable<T> argument, string argumentName)
            {
                IsNotNull(argument, argumentName);

                if (argument.Count() == 0)
                {
                    throw new ArgumentException("集合(Collection)不能为空.", argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotOutOfRange(int argument, int min, int max, string argumentName)
            {
                if ((argument < min) || (argument > max))
                {
                    throw new ArgumentOutOfRangeException(argumentName, "{0} 必须在 \"{1}\"-\"{2}\"之间.".FormatWith(argumentName, min, max));
                }
            }

            [DebuggerStepThrough]
            public static void IsNotInvalidEmail(string argument, string argumentName)
            {
                IsNotEmpty(argument, argumentName);

                if (!argument.IsEmail())
                {
                    throw new ArgumentException("\"{0}\" 不是一个合法的Email地址.".FormatWith(argumentName), argumentName);
                }
            }

            [DebuggerStepThrough]
            public static void IsNotInvalidWebUrl(string argument, string argumentName)
            {
                IsNotEmpty(argument, argumentName);

                if (!argument.IsWebUrl())
                {
                    throw new ArgumentException("\"{0}\" 不是合法的URL.".FormatWith(argumentName), argumentName);
                }
            }
        }
    }
}
