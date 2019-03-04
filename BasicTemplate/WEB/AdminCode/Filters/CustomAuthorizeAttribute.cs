using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBService.Security;
using WEBService.Util;

namespace AdminCode.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute()
        {

        }
        public CustomAuthorizeAttribute(string role)
        {
            this.Roles = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //权限限制
            var roles = Roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (roles != null&&!roles.Contains<string>(Authentication.Role))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 没有权限
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = FilterUtil.GetResult(filterContext, "/Admin/PermissionTips?type=404");
        }
    }
}