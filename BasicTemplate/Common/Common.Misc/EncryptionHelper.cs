using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public static class EncryptionHelper
    {
        private static string GetEncryptionCode { get; set; } = "$u#e7$4g7asdrs*ds&7=";

        public static string Encryption(params string[] paramValues)
        {
            string token = GetEncryptionCode;

            using (MD5 md5 = MD5.Create())
            {
                foreach (string param in paramValues)
                {
                    token = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(param + token))).ToLower();
                }
            }

            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public static string Md5Encryption(params string[] paramValues)
        {
            List<string> list = new List<string>();
            foreach (string param in paramValues)
            {
                list.Add(param);
            }
            string token = string.Join("_", list);
            return EncryptUtil.EncryptMd5(token);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramValues"></param>
        /// <returns></returns>
        public static string Md5EncryptionV2(params string[] paramValues)
        {
            List<string> list = new List<string>();
            foreach (string param in paramValues)
            {
                list.Add(param);
            }
            string token = string.Join("", list);
            return EncryptUtil.EncryptMd5(token);
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey">公钥</param>
        /// <param name="content">待加密内容</param>
        /// <returns>加密后的密文</returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">私钥</param>
        /// <param name="content">待解密内容</param>
        /// <returns>解密后的明文</returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }


        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string Base64Encode(string source)
        {
            return Base64Encode(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        private static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        private static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }
}
