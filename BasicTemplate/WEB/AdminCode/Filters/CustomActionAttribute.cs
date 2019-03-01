using Common.Enums;
using Common.Misc.Logs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBService.Security;
using WEBService.Util;

namespace AdminCode.Filters
{
    public class CustomActionAttribute : FilterAttribute, IActionFilter
    {

        private Stopwatch stopwatch = null;

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (stopwatch != null)
            {
                stopwatch.Stop();
                var time = stopwatch.Elapsed.TotalMilliseconds;
                if (time > 1000)
                {
                    List<string> lsParams = new List<string>();
                    var getParams = filterContext.RequestContext.HttpContext.Request.QueryString;
                    foreach (string key in getParams.Keys)
                    {
                        lsParams.Add($"{key}={getParams[key]}");
                    }
                    var postParams = filterContext.RequestContext.HttpContext.Request.Form;
                    foreach (string key in postParams.Keys)
                    {
                        lsParams.Add($"{key}={postParams[key]}");
                    }
                    string url = filterContext.RequestContext.HttpContext.Request.Url.ToString();
                    string httpMethod = filterContext.RequestContext.HttpContext.Request.HttpMethod;
                    HttpBrowserCapabilitiesBase browser = filterContext.RequestContext.HttpContext.Request.Browser;
                    string info = $"用户[{Authentication.Id}],耗时[{time}],类型[{httpMethod}],地址[{url}],参数[{string.Join("&", lsParams)}],浏览器名称[{browser.Browser}]浏览器版本[{browser.Version}]平台[{browser.Platform}]";
                    CustomLogger.WriterLog(LogType.SlowLog, info);
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authentication = Authentication.CheckAuthentication(); ;
            if (authentication == 1)
            {
                //filterContext.Result = FilterUtil.GetResult(filterContext, "/Admin/PermissionTips?type=000");

                filterContext.Result = FilterUtil.GetResult(filterContext, "/Home/Login");
            }
            else if (authentication == 2)
            {
                filterContext.Result = FilterUtil.GetResult(filterContext, "/Admin/PermissionTips?type=400");
            }
        }
    }
}