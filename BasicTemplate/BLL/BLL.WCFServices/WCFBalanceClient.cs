using Common.Enums;
using Common.Interface.WcfInterface;
using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.WCFServices
{
    public class WCFBalanceClient : WCFFactory<IBalanceService>, IWCFInterface
    {
        public WCFBalanceClient()
        {
            ServiceUrl = GetUrl(WCFServiceEnum.BalanceService);
            if (string.IsNullOrWhiteSpace(ServiceUrl))
            {
                throw new ArgumentNullException(ServiceUrl, "WCF地址不能为空");
            }
            base.TimeOut = TimeSpan.FromMinutes(3);
        }

        public WCFBalanceClient(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
            base.TimeOut = new TimeSpan(0, 0, 3);
        }

        /// <summary>
        ///  获取用户余额
        /// </summary>
        /// <param name="accountId">用户余额</param>
        /// <returns></returns>
        public decimal GetAccountBalance(int accountId)
        {
            var balance = 0m;
            try
            {
                balance = FactoryObject.GetAccountBalance(accountId);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]获取用户余额");
            }
            finally
            {
                Close();
            }
            return balance;
        }

    }
}
