using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFramework
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 获得指定类型、特定key的缓存数据
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <returns></returns>
        object Get(string key);
        /// <summary>
        /// 试着获取指定类型、特定key的缓存数据
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">获取到的缓存数据</param>
        /// <returns>如果获取成功返回true,否则false</returns>
        bool TryGet(string key, out object value);
        /// <summary>
        /// 获得指定类型、特定key的缓存数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存数据key</param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 试着获取指定类型、特定key的缓存数据
        /// </summary>
        /// <typeparam name="T">缓存数据类型</typeparam>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">获取到的缓存数据</param>
        /// <returns>如果获取成功返回true,否则false</returns>
        bool TryGet<T>(string key, out T value) ;
        /// <summary>
        /// 添加数据到缓存中
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        void Add<T>(string key, T value, DateTime absoluteExpiration);
        /// <summary>
        /// 添加数据到缓存中
        /// </summary>
        /// <param name="key">缓存数据key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="slidingExpiration">相对过期时间</param>
        void Add<T>(string key, T value, TimeSpan slidingExpiration);
        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
