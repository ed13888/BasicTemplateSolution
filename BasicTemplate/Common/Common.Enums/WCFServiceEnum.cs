using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Enums
{
    /// <summary>
    /// WCF服务类型
    /// </summary>
    public enum WCFServiceEnum
    {
        /// <summary>
        /// 机器人服务
        /// </summary>
        [Description("机器人服务")]
        RobotService,
        /// <summary>
        /// 账户服务
        /// </summary>
        [Description("账户服务")]
        AccountService,
        /// <summary>
        /// 余额服务
        /// </summary>
        [Description("余额服务")]
        BalanceService,
    }
}
