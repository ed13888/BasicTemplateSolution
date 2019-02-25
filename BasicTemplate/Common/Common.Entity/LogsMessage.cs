using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    /// <summary>
    /// 日志消息实体
    /// </summary>
    [Serializable]
    public class LogsMessage
    {
        /// <summary>
        /// 功能模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public string LogsType { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        public string AssemblyName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServiceIP { get; set; }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClinetIP { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public int OperatorID { get; set; }
        /// <summary>
        /// 耗时时间
        /// </summary>
        public double TakeTimes { get; set; }
    }
}
