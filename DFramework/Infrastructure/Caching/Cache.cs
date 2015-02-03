
namespace DFramework
{
    using System;
    using System.Diagnostics;
    using DFramework.Utilities;
    public static class Cache
    {
        private static ICache _internalCache;

        static Cache()
        {
            _internalCache = IoC.Resolve<ICache>();
        }

        /// <summary>
        /// get cache data
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static object Get(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.Get(key);
        }
        /// <summary>
        /// try get cache data
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <param name="value">cache data</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool TryGet(string key, out object value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.TryGet(key, out value);
        }
        /// <summary>
        ///  get cache data
        /// </summary>
        /// <typeparam name="T">cache data's type</typeparam>
        /// <param name="key">cache data's key</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Get<T>(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.Get<T>(key);
        }
        /// <summary>
        /// try get cache data
        /// </summary>
        /// <typeparam name="T">cache data's type</typeparam>
        /// <param name="key">cache data's key</param>
        /// <param name="value">获取到的cache data</param>
        /// <returns>如果获取成功返回true,否则false</returns>
        [DebuggerStepThrough]
        public static bool TryGet<T>(string key, out T value)
        {
            Check.Argument.IsNotEmpty(key, "key");

            return _internalCache.TryGet<T>(key, out value);
        }

        /// <summary>
        /// add data to cache
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <param name="value">cache data</param>
        /// <param name="absoluteExpiration">  </param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value, DateTime absoluteExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotInPast(absoluteExpiration, "absoluteExpiration");

            _internalCache.Add(key, value, absoluteExpiration);
        }
        /// <summary>
        /// add data to cache
        /// </summary>
        /// <param name="key">cache data's key</param>
        /// <param name="value">cache data</param>
        /// <param name="absoluteExpiration"> </param>
        [DebuggerStepThrough]
        public static void Add<T>(string key, T value, TimeSpan slidingExpiration)
        {
            Check.Argument.IsNotEmpty(key, "key");
            Check.Argument.IsNotNegativeOrZero(slidingExpiration, "slidingExpiration");

            _internalCache.Add(key, value, slidingExpiration);
        }
        /// <summary>
        /// remove cache data  
        /// </summary>
        /// <param name="key">the cache data key</param>
        [DebuggerStepThrough]
        public static void Remove(string key)
        {
            Check.Argument.IsNotEmpty(key, "key");

            _internalCache.Remove(key);
        }
    }
}
