using Common.Interface;
using Common.Interface.AccountInterface;
using Common.Interface.AccountInterface.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Common.Misc
{
    public static class CommonClass
    {
        #region <Service>
        public static IAccountService AccountService => UnityHelper.Container.Resolve<IAccountService>("");
        #endregion

        #region <Repository>
        public static IAccountRepository AccountRepository => UnityHelper.Container.Resolve<IAccountRepository>("");

        public static ISystemConfigRepository SystemConfigRepository => null;// ObjectFactory.GetInstance<ISystemConfigRepository>();
        #endregion
    }
}
