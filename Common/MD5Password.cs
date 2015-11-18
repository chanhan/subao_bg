using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class MD5Password
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="password">明文</param>
        /// <returns>加密后</returns>
        public static string GetMD5Password(string password)
        {
            string passwordStr = "";
            MD5 md5 = MD5.Create();  //实例化一个md5对像
            byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(password));//加密后是一个字节类型的数组
            for (int i = 0; i < bytes.Length; i++)
            {
                passwordStr = passwordStr + bytes[i].ToString("X2");
            }
            return passwordStr;
        }
        public static string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }  
    }
}