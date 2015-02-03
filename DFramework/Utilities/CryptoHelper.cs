using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DFramework.Utilities
{
    public class CryptoHelper
    {
        private static byte[] _DESSIV = new byte[] { 0x94, 0x67, 0xb9, 0x60, 0x32, 0x7c, 0x30, 0x88 };

        public static string Encrypt(string encryptKey, string encodeString)
        {
            Check.Argument.IsNotEmpty(encryptKey, "encryptKey");

            var desKey = Encoding.Default.GetBytes(encryptKey);
            var toEncrypt = Encoding.Default.GetBytes(encodeString);

            TripleDESCryptoServiceProvider objDES = new TripleDESCryptoServiceProvider();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, objDES.CreateEncryptor(desKey, _DESSIV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(toEncrypt, 0, toEncrypt.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string decryptKey, string encodeString)
        {
            Check.Argument.IsNotEmpty(decryptKey, "decryptKey");

            var desKey = Encoding.Default.GetBytes(decryptKey);

            TripleDESCryptoServiceProvider objDES = new TripleDESCryptoServiceProvider();
            byte[] fromEncrypt = Convert.FromBase64String(encodeString);

            using (MemoryStream memoryStream = new MemoryStream(fromEncrypt))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, objDES.CreateDecryptor(desKey, _DESSIV), CryptoStreamMode.Read))
                {
                    using (StreamReader streamRead = new StreamReader(cryptoStream))
                    {
                        return streamRead.ReadToEnd();
                    }
                }
            }

        }

        public static string MD5(string targetStr)
        {
            Check.Argument.IsNotEmpty(targetStr, "targetStr");

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] targetBytes = Encoding.UTF8.GetBytes(targetStr);
            var md5bytes = md5.ComputeHash(targetBytes);

            string result = BitConverter.ToString(md5bytes).Replace("-",string.Empty);

            return result;
        }
    }
}
