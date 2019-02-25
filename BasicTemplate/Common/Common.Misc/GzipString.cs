using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>字符串GZIP</summary>
    public class GzipString
    {
        /// <summary>压缩</summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Compress(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Compress, true))
                    gzipStream.Write(bytes, 0, bytes.Length);
                memoryStream.Position = 0L;
                byte[] buffer = new byte[memoryStream.Length];
                memoryStream.Read(buffer, 0, buffer.Length);
                byte[] inArray = new byte[buffer.Length + 4];
                Buffer.BlockCopy((Array)buffer, 0, (Array)inArray, 4, buffer.Length);
                Buffer.BlockCopy((Array)BitConverter.GetBytes(bytes.Length), 0, (Array)inArray, 0, 4);
                return Convert.ToBase64String(inArray);
            }
        }

        /// <summary>解压</summary>
        /// <param name="compressedText"></param>
        /// <returns></returns>
        public static string Decompress(string compressedText)
        {
            if (string.IsNullOrWhiteSpace(compressedText))
                return "";
            byte[] buffer = Convert.FromBase64String(compressedText);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int int32 = BitConverter.ToInt32(buffer, 0);
                memoryStream.Write(buffer, 4, buffer.Length - 4);
                byte[] numArray = new byte[int32];
                memoryStream.Position = 0L;
                using (GZipStream gzipStream = new GZipStream((Stream)memoryStream, CompressionMode.Decompress))
                    gzipStream.Read(numArray, 0, numArray.Length);
                return Encoding.UTF8.GetString(numArray);
            }
        }
    }
}
