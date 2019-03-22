using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebCode
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute("pc_index", "index.html", new { controller = "Template", action = "Index" });
            routes.MapRoute("pc_index", "index.html", new { controller = "Home", action = "Index" });

            routes.MapRoute(
               "Action1Html", // action伪静态  
               "{controller}/{action}.html",// 带有参数的 URL  
               new { controller = "Home", action = "Index", id = UrlParameter.Optional }// 参数默认值  
            );

            routes.MapRoute(
               "IDHtml", // id伪静态  
               "{controller}/{action}/{id}.html",// 带有参数的 URL  
               new { controller = "Home", action = "Index", id = UrlParameter.Optional }// 参数默认值  
            );

            routes.MapRoute(
                "ControllerHtml", // controller伪静态  
                "{controller}.html/{action}/{id}",// 带有参数的 URL  
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }// 参数默认值  
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
