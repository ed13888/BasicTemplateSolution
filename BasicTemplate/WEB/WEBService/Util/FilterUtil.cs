using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WEBService.Util
{
    public class FilterUtil
    {
        public static ActionResult GetResult(ControllerContext filterContext, string url)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return new JavaScriptResult() { Script = "window.location.href='" + url + "';" };
            }
            else
            {
                return new RedirectResult(url);
            }
        }
    }
}
