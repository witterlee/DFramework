using System.Security.Cryptography;
using System.Text;

namespace DFramework.Utilities
{
    public class PasswordHelper
    {
        /// <summary>
        /// 利用MD5进行加密    
        /// </summary>
        /// <param name="astr_Value"></param>
        /// <returns></returns>
        public static string EncryptMD5(string astr_Value)
        {
            string cl1 = astr_Value;
            string pwd = "";
            MD5 md5 = MD5Cng.Create();
            // 加密后是一个字节类型的数组 
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得 
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x");
            }

            return pwd;
        }
    }
}
