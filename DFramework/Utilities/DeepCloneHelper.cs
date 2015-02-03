using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DFramework.Utilities
{
    public static class DeepCloneHelper
    {
        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">需拷贝的对象</param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
