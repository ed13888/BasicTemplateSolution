using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Misc
{
    /// <summary>
    /// 针对于单个用户的Redis操作类
    /// 所存取的KEY会自动加上用户当前的SessionID
    /// </summary>
    public static class SessionRedis
    {
        /// <summary>
        /// Cookie中的ASP.NET_SessionId
        /// </summary>
        public static string SessionID => FeatureHelper.IsApi ? HttpContext.Current.Request.Headers.Get("ASP.NET_SessionId") : CookieHelper.Get("ASP.NET_SessionId");
    }
}
