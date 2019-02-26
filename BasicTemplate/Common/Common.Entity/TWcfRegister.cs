using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity
{
    /// <summary>
    /// WCF注册
    /// </summary>
    public class TWcfRegister
    {
        /// <summary>
        /// 主键 服务地址
        /// </summary>
        public string FServiceUrl { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string FServiceName { get; set; }
        /// <summary>
        /// 服务IP地址
        /// </summary>
        public string FIpAddress { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int FPort { get; set; }
        /// <summary>
        /// 状态：1-正常，2-停用
        /// </summary>
        public int FStatus { get; set; }
    }
}
