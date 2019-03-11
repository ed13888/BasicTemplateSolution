using Common.Interface;
using Common.Interface.AccountInterface;
using Common.Interface.AccountInterface.DAL;
using Common.Interface.BusinessInterface;
using Common.Interface.BusinessInterface.DAL;
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
        #region <后台>
        #region <Service>
        public static IAccountService AccountService => UnityHelper.Container.Resolve<IAccountService>("AccountService");
        #endregion

        #region <Repository>
        public static IAccountRepository AccountRepository => UnityHelper.Container.Resolve<IAccountRepository>("AccountRepository");

        public static ISystemConfigRepository SystemConfigRepository => null;// ObjectFactory.GetInstance<ISystemConfigRepository>();
        #endregion
        #endregion



        #region <前台>


        #region <Service>
        public static ITemplateService TemplateService => UnityHelper.Container.Resolve<ITemplateService>("TemplateService");
        public static ICustomerTemplateInfoService CustomerTemplateInfoService => UnityHelper.Container.Resolve<ICustomerTemplateInfoService>("CustomerTemplateInfoService");
        #endregion

        #region <Repository>
        public static ITemplateRepository TemplateRepository => UnityHelper.Container.Resolve<ITemplateRepository>("TemplateRepository");
        public static ICustomerTemplateInfoRepository CustomerTemplateInfoRepository => UnityHelper.Container.Resolve<ICustomerTemplateInfoRepository>("CustomerTemplateInfoRepository");
        #endregion


        #endregion
    }
}
