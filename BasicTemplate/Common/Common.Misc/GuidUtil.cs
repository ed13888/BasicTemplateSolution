using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public class GuidUtil
    {
        /// <summary>
        /// 获取一个GUID
        /// </summary>
        /// <returns></returns>
        public static string GuidKey
        {
            get
            {
                return Guid.NewGuid().ToString("N");
            }
        }

        /// <summary>
        /// 获取16位GUID
        /// </summary>
        /// <returns></returns>
        public static string ShortGuid
        {
            get
            {
                long i = 1;
                foreach (byte b in Guid.NewGuid().ToByteArray())
                {
                    i *= ((int)b + 1);
                }
                return string.Format("{0:x}", i - DateTime.Now.Ticks);
            }
        }

        #region <生成随机字母字符串(数字字母混和) >
        private static int rep = 0;
        /// <summary> 
        /// 生成随机字母字符串(数字字母混和) 
        /// </summary> 
        /// <param name="codeCount">待生成的位数</param> 
        /// <returns>生成的字母字符串</returns> 
        public static string GetUniqueCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            if (rep > 1000000)
            {
                rep = 0;
            }

            return str;
        }
        #endregion

        #region <获取一个唯一随机码>
        /// <summary>
        /// 获取一个唯一随机码
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyUniqueCode(int companyID, int codeCount)
        {
            string uniqueCode = string.Empty;
            do
            {
                var code = GetUniqueCode(codeCount).ToLower();
                string key = string.Format("TGeneralizeCode_{0}_{1}", companyID, code);
                //if (!CacheHelper.ContainsKey(key))
                //{
                //    uniqueCode = code;
                //    CacheHelper.Set(key, code);
                //    break;
                //}
                LogsManager.Debug($"获取一个唯一随机码：{key}已被使用，重新生成");
            } while (true);
            return uniqueCode;
        }
        #endregion
    }
}
