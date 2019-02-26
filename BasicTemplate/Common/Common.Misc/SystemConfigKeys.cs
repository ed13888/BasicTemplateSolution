using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 系统设置KEYS
    /// </summary>
    public class SystemConfigKeys
    {
        /// <summary>
        /// Redis配置
        /// </summary>
        public const string RedisConfig = "RedisConfig";

        /// <summary>
        /// 消息队列链接字符串
        /// </summary>
        public const string RabbitMQConnection = "RabbitMQConnection";

        /// <summary>
        /// 是否开发环境
        /// </summary>
        public const string IsDevelopment = "IsDevelopment";

        /// <summary>
        /// 余额、账变服务地址
        /// </summary>
        public const string BalanceServiceUrl = "BalanceServiceUrl";
    }
}
