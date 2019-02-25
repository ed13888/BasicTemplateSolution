using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public class EncryptHelper
    {
        private static byte[] _skey = new byte[8]
        {
      (byte) 88,
      (byte) 226,
      (byte) 66,
      (byte) 162,
      (byte) 54,
      (byte) 246,
      (byte) 233,
      (byte) 169
        };
        private static byte[] _sIV = new byte[8]
        {
      (byte) 170,
      (byte) 58,
      (byte) 43,
      (byte) 68,
      (byte) 49,
      (byte) 37,
      (byte) 113,
      (byte) 144
        };
        private static char connectStrSplit = ';';

        public static string EncryptPassword(string password, string guid)
        {
            byte[] decryptKey = EncryptHelper.GetDecryptKey(guid);
            byte[] decryptIv = EncryptHelper.GetDecryptIV(guid);
            return EncryptHelper.Encrypt(password, decryptKey, decryptIv);
        }

        internal static string TryDecryptPassword(string password, string guid)
        {
            byte[] decryptKey = EncryptHelper.GetDecryptKey(guid);
            byte[] decryptIv = EncryptHelper.GetDecryptIV(guid);
            return EncryptHelper.Decrypt(password, decryptKey, decryptIv);
        }

        private static byte[] GetDecryptKey(string guid)
        {
            guid = guid.Replace("-", "");
            byte[] numArray = new byte[8];
            for (int index = 0; index < 8; ++index)
                numArray[index] = (byte)((uint)byte.Parse(guid.Substring(index * 2, 2), NumberStyles.HexNumber) ^ (uint)EncryptHelper._skey[index]);
            return numArray;
        }

        private static byte[] GetDecryptIV(string guid)
        {
            guid = guid.Replace("-", "");
            byte[] numArray = new byte[8];
            for (int index = 0; index < 8; ++index)
                numArray[index] = (byte)((uint)byte.Parse(guid.Substring(index * 2 + 16, 2), NumberStyles.HexNumber) ^ (uint)EncryptHelper._sIV[index]);
            return numArray;
        }

        private static string Encrypt(string Text, byte[] sKey, byte[] sIV)
        {
            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(Text);
            cryptoServiceProvider.Key = sKey;
            cryptoServiceProvider.IV = sIV;
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in memoryStream.ToArray())
                stringBuilder.AppendFormat("{0:X2}", (object)num);
            return stringBuilder.ToString();
        }

        private static string Decrypt(string Text, byte[] sKey, byte[] sIV)
        {
            DESCryptoServiceProvider cryptoServiceProvider = new DESCryptoServiceProvider();
            int length = Text.Length / 2;
            byte[] buffer = new byte[length];
            for (int index = 0; index < length; ++index)
            {
                int int32 = Convert.ToInt32(Text.Substring(index * 2, 2), 16);
                buffer[index] = (byte)int32;
            }
            cryptoServiceProvider.Key = sKey;
            cryptoServiceProvider.IV = sIV;
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Default.GetString(memoryStream.ToArray());
        }

        public static string DecryptConnctString(string connString, string guid)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            string[] strArray = connString.Split(EncryptHelper.connectStrSplit);
            connectionStringBuilder.DataSource = strArray[0].Split('=')[1];
            connectionStringBuilder.InitialCatalog = strArray[1].Split('=')[1];
            connectionStringBuilder.UserID = strArray[2].Split('=')[1];
            connectionStringBuilder.Password = EncryptHelper.TryDecryptPassword(strArray[3].Split('=')[1], guid);
            connectionStringBuilder.MinPoolSize = 5;
            if (strArray.Length == 5 && !string.IsNullOrWhiteSpace(strArray[4]))
            {
                string str = strArray[4].Split('=')[1];
                if (!string.IsNullOrEmpty(str))
                    connectionStringBuilder.MaxPoolSize = Convert.ToInt32(str);
            }
            return connectionStringBuilder.ConnectionString;
        }

        public static string DecryptConnctString(string connString, int minPool, string guid)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            string[] strArray = connString.Split(EncryptHelper.connectStrSplit);
            connectionStringBuilder.DataSource = strArray[0].Split('=')[1];
            connectionStringBuilder.InitialCatalog = strArray[1].Split('=')[1];
            connectionStringBuilder.UserID = strArray[2].Split('=')[1];
            connectionStringBuilder.Password = EncryptHelper.TryDecryptPassword(strArray[3].Split('=')[1], guid);
            connectionStringBuilder.MinPoolSize = minPool;
            if (strArray.Length == 5 && !string.IsNullOrWhiteSpace(strArray[4]))
            {
                string str = strArray[4].Split('=')[1];
                if (!string.IsNullOrEmpty(str))
                    connectionStringBuilder.MaxPoolSize = Convert.ToInt32(str);
            }
            return connectionStringBuilder.ConnectionString;
        }
    }
}
