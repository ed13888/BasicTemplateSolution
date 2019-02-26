using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface.WcfInterface
{
    /// <summary>
    /// 用户的余额操作接口
    /// </summary>
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IBalanceService : IWCFInterface
    {
        /// <summary>
        /// 获取用户余额
        /// </summary>
        /// <param name="accountId">用户编号</param>
        /// <returns></returns>
        [OperationContract]
        decimal GetAccountBalance(int accountId);
    }
}
