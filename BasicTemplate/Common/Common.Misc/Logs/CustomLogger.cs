using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc.Logs
{
    /// <summary>
    /// 自定义日志
    /// </summary>
    public static class CustomLogger
    {
        /// <summary>
        /// 写日志文件,保存到指定的文件
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="log"></param>
        public static void WriterLog(LogType logType, string log)
        {
            var loger = Common.Misc.Logs.CustomLog4.GetLogger(logType.ToString());
            loger.Info(log);
        }

        /// <summary>
        /// 写错误日志,保存到指定的文件
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="log"></param>
        public static void WriterErrorLog(LogType logType, Exception ex)
        {
            string log = $"错误：【{ex.Message}】,堆栈信息：【{ex.StackTrace}】";
            var loger = Common.Misc.Logs.CustomLog4.GetLogger(logType.ToString());
            loger.Error(log);
        }
    }
}
