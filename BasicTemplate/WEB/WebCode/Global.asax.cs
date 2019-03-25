using Common.Misc;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.AspNet.Mvc;

namespace WebCode
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalBase.Init();
            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityHelper.Container));


            //Timer timer = new Timer(5000);
            //timer.Elapsed += timer_Elapsed;
            //timer.Start();
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            //Server.ClearError();
            //这里记录错误日志信息
            ex.Error("MvcApplication 捕获异常");

            //先告诉系统异常错误已经处理了,防止跳转失败
            Server.ClearError();
            if (ex is HttpException && ((HttpException)ex).GetHttpCode() == 404)
            {
                Response.Redirect("/404.html");
            }
            else
            {
                //跳转到指定的自定义错误页
                Response.Redirect("/error.html");
            }
        }


        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetConnectionContext<SignalRConnection>();
            context.Connection.Broadcast("我在 " + DateTime.Now.ToString() + " 主动向浏览器发送数据。");
        }
    }
}
