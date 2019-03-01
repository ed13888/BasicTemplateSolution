using Common.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using WEBService.Security;

namespace WebCode.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            List<string> roles = Roles.Split(',').ToList();
            bool Pass = false;
            try
            {
                var status = Authentication.CheckAuthentication();
                if (status != 0)
                {
                    httpContext.Response.StatusCode = 401;//无权限状态码
                    Pass = false;
                }
                else if (!roles.Contains(Authentication.Role))
                {
                    httpContext.Response.StatusCode = 401;//无权限状态码
                    Pass = false;
                }
                else
                {
                    Pass = true;
                }

            }
            catch (Exception ex)
            {
                return Pass;
            }
            return Pass;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            else
            {
                string path = filterContext.HttpContext.Request.Path;
                string strUrl = "/Home/Login?returnUrl={0}";

                filterContext.HttpContext.Response.Redirect(string.Format(strUrl, HttpUtility.UrlEncode(path)), true);

            }
        }
    }
}