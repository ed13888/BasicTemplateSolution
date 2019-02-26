using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerService
{
    public class MainService : WCFServiceFactory
    {
        public override void Start()
        {
            try
            {
                lock (lockRoot)
                {
                    //GlobalApp.RunStatus = true;
                    //ThreadPoolManager.SetThreadCount(120, 300);
                    //GameDataManager.Start();
                    //CompanyDataManager.Start();
                    //if (SystemConfig.IsDevelopment)
                    //{
                    //    base.CreateHost<IBalanceService, BalanceService>(SystemConfig.BalanceServiceUrl);
                    //    base.CreateHost<IOrderBalanceService, OrderBalanceService>(SystemConfig.OrderBalanceServiceUrl);
                    //    base.CreateHost<IAccessMoneyService, AccessMoneyService>(SystemConfig.AccessMoneyServiceUrl);
                    //}
                    //WCFRegister.Register<IBalanceService, BalanceService>(WCFServiceEnum.BalanceService);
                    //WCFRegister.Register<IOrderBalanceService, OrderBalanceService>(WCFServiceEnum.OrderBalanceService);
                    //WCFRegister.Register<IAccessMoneyService, AccessMoneyService>(WCFServiceEnum.AccessMoneyService);
                    //TaskService.Instance.Start();
                }
            }
            catch (Exception ex)
            {
                ex.Error("系统启动异常");
            }
        }

        public override void Stop()
        {
            //try
            //{
            //    lock (lockRoot)
            //    {
            //        GlobalApp.RunStatus = false;
            //        base.Close();
            //        WCFRegister.Close();
            //        LogsManager.Info("BalanceService服务关闭成功");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.Error("系统关闭异常");
            //}
        }
    }
}
