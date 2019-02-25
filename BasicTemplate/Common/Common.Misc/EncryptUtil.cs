using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public static class EncryptUtil
    {
        /// <summary>
        /// 8位字符的密钥字符串(需要和加密时相同)
        /// </summary>
        private const string KEY_64 = "LXDCKEY1";
        /// <summary>
        /// 8位字符的初始化向量字符串(需要和加密时相同)
        /// </summary>
        private const string IV_64 = "LXDCKEY2";

        /// <summary>
        /// 根据类型加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encryptType"></param>
        /// <returns></returns>
        public static string GetEncrypt(this string str, string encryptType)
        {
            switch (encryptType.Trim().ToUpper())
            {
                case "DES": return DESEncrypt(str);
                case "MD5": return EncryptMd5(str);
                default: return str;
            }
        }

        /// <summary>根据类型加密</summary>
        /// <param name="str"></param>
        /// <param name="encryptType"></param>
        /// <returns></returns>
        public static string GetEncrypt(this string str, string encryptType, string key = "")
        {
            string upper = encryptType.Trim().ToUpper();
            if (upper == "DES")
                return DESEncrypt(str);
            if (upper == "MD5")
                return EncryptMd5(str);
            if (upper == "AES")
                return AESEncrypt(str, key);//AESEncrypt(str, key);
            return str;
        }

        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="encryptStr">明文</param>  
        /// <param name="key">密钥</param>  
        /// <returns></returns>  
        public static string AESEncrypt(string encryptStr, string key)
        {
            try
            {
                byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encryptStr);

                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = rDel.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #region <生成MD5密文>
        /// <summary>
        /// 生成MD5密文
        /// </summary>
        /// <param name="str">源串</param>
        /// <returns>密文</returns>
        public static string EncryptMd5(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            byte[] bytes = Encoding.Default.GetBytes(str);
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] buffer2 = md5.ComputeHash(bytes);
                return BitConverter.ToString(buffer2).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 生成MD5密文
        /// </summary>
        /// <param name="str">源串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>密文</returns>
        public static string EncryptMd5(string str, Encoding encoding)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var bytes = encoding.GetBytes(str);
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                var buffer2 = md5.ComputeHash(bytes);
                return BitConverter.ToString(buffer2).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// MD5加密 .NET自带
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncryptStringMD5(string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
        }

        /// <summary>
        ///  将字符串经过md5加密，返回加密后的字符串的小写表示
        /// </summary>
        /// <param name="strToBeEncrypt"></param>
        /// <returns></returns>
        public static string Md5Encrypt(string strToBeEncrypt)
        {
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                Byte[] FromData = System.Text.Encoding.GetEncoding("utf-8").GetBytes(strToBeEncrypt);
                Byte[] TargetData = md5.ComputeHash(FromData);
                string Byte2String = "";
                for (int i = 0; i < TargetData.Length; i++)
                {
                    Byte2String += TargetData[i].ToString("x2");
                }
                return Byte2String.ToLower();
            }
        }

        #endregion

        #region <DES加密>
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data">加密数据</param>
        /// <returns></returns>
        public static string DESEncrypt(string data)
        {
            if (String.IsNullOrWhiteSpace(data) == true) { return ""; }

            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
            {
                int i = cryptoProvider.KeySize;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cst))
                        {
                            sw.Write(data);
                            sw.Flush();
                            cst.FlushFinalBlock();
                            sw.Flush();
                            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                        }
                    }
                }
            }
        }
        #endregion

        #region <DES解密>
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <returns></returns>
        public static string DESDecrypt(string data)
        {
            if (String.IsNullOrWhiteSpace(data) == true) { return ""; }

            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(KEY_64);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV_64);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(data);
            }
            catch (Exception e)
            {
                return null;
            }

            try
            {
                using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
                {
                    using (MemoryStream ms = new MemoryStream(byEnc))
                    {
                        using (CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cst))
                            {
                                string result = sr.ReadToEnd();
                                return result;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
        #endregion


        //AES加密 
        public static string Encrypt(string toEncrypt, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cTransform = rDel.CreateEncryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
        }


        //AES解密
        public static string Decrypt(string toDecrypt, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = keyArray;
                rDel.Mode = CipherMode.ECB;
                rDel.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cTransform = rDel.CreateDecryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return UTF8Encoding.UTF8.GetString(resultArray);
                }
            }
        }
    }
}
