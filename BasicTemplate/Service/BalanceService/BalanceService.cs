using BLL.WCFServices;
using Common.Interface.WcfInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BalanceService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class BalanceService : WCFServiceBase, IBalanceService
    {
        public decimal GetAccountBalance(int accountId)
        {
            return 0;
        }
    }
}
