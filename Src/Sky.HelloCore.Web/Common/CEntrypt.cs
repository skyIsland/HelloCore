using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sky.Common
{
    public class CEntrypt
    {
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串,加密失败，返回原窜
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey = "sdmadmin")
        {
            if (string.IsNullOrWhiteSpace(encryptString))
                return encryptString;
            try
            {
                if (!encryptString.Equals(Decode(encryptString, encryptKey), StringComparison.CurrentCultureIgnoreCase))
                    return encryptString;
                encryptKey = CutString(encryptKey, 8, "");
                encryptKey = encryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串,解密失败，返回原字符
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey = "sdmadmin")
        {
            if (string.IsNullOrEmpty(decryptString))
                return decryptString;
            decryptString = decryptString.Trim();
            if (string.IsNullOrEmpty(decryptString))
                return decryptString;
            try
            {
                decryptKey = CutString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <param name="endStr">超过时用什么符号（如…），默认…</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len, string endStr = "…")
        {
            if (inputString == null)
                return "";
            var s = inputString.ToString();
            string temp = s.Substring(0, (s.Length < len + 1) ? s.Length : len + 1);
            byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(temp);
            string outputStr = "";
            int count = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if ((int)encodedBytes[i] == 63)
                    count += 2;
                else
                    count += 1;

                if (count <= len - endStr.Length)
                    outputStr += temp.Substring(i, 1);
                else if (count > len)
                    break;
            }

            if (count <= len)
            {
                outputStr = temp;
                endStr = "";
            }
            outputStr += endStr;
            return outputStr;
        }
    }
}