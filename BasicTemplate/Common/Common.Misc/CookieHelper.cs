using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Misc
{
    public class CookieHelper
    {
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        public static void Set(string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
                cookie = new HttpCookie(key);
            cookie.Value = HttpUtility.UrlEncode(value);
            //if (expires > 0)
            //    cookie.Expires = DateTime.Now.AddHours(expires);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request.Cookies[key] != null)
            {
                return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[key].Value);
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                HttpCookie myCookie = new HttpCookie(key);
                myCookie.Expires = DateTime.Now.AddDays(-100d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
    }
}
