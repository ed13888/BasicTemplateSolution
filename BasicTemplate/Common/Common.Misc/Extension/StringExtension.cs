using Common.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Misc
{
    public static class StringExtension
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T Value<T>(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                try
                {
                    return (T)Convert.ChangeType(str.Trim().ToLower(), typeof(T), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// boolean值转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string str)
        {
            if (str != null && (str == "1" || str == "是"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 字符串转换成整形数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string str)
        {
            if (string.IsNullOrEmpty(str)) return null;
            else
            {
                var regex = new Regex(@"\d+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                return regex.Matches(str).Cast<Match>().Select(m => int.Parse(m.Groups[0].Value)).ToArray();
            }
        }

        /// <summary>
        /// 隐藏不应该给会员看到的异常消息
        /// </summary>
        /// <param name="soureErr"></param>
        /// <returns></returns>
        public static string ErrorShow(string soureErr)
        {
            string result = soureErr.ToLower();
            string[] noShowKey = new string[] { "事务" };
            foreach (string item in noShowKey)
            {
                if (result.Contains(item))
                {
                    result = "";
                    break;
                }
            }
            return result;
        }


        public static void WriterLog(this LogType logType, string logs)
        {
            //CustomLogger.WriterLog(logType, logs);
        }
    }
}
