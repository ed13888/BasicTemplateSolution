using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TimerService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {

            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CommonService()
            };
            ServiceBase.Run(ServicesToRun);
        }


        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception != null)
            {
                LogsManager.Error(e.Exception.Message + "\n" + "InnerException:" + e.Exception.InnerException
               + "\n" + "Source:" + e.Exception.Source
               + "\n" + "TargetSite:" + e.Exception.TargetSite
               + "\n" + "StackTrace:" + e.Exception.StackTrace);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                Exception ex = e.ExceptionObject as Exception;
                LogsManager.Error(ex.Message + "\n" + "InnerException:" + ex.InnerException
                + "\n" + "Source:" + ex.Source
                + "\n" + "TargetSite:" + ex.TargetSite
                + "\n" + "StackTrace:" + ex.StackTrace);
            }
        }
    }
}
