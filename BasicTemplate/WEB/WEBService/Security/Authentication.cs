using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WEBService.Security
{
    public class Authentication
    {
        
        /// <summary>
        /// 登录存储客户端的Cookie名称
        /// </summary>
        private const string LoginToken = "LoginSessionID";

        public static bool Login(Context context, bool isIgnoreValid = false)
        {

            return true;
        }


        public static void Logout(bool isSelf = false)
        {


        }

        private static void RemoveSessionID(string loginSessionID, string listId, string setId = null, int userId = 0, bool isSelf = false)
        {


        }

        /// <summary>
        /// 用户客户端的SessionID
        /// </summary>
        private static string CurrentSessionID
        {
            get
            {
                string loginSessionId = string.Empty;
                if (true)
                {
                    loginSessionId = HttpContext.Current.Request.Headers.Get(LoginToken);
                    //LogsManager.Debug("IsApiloginSessionId：" + loginSessionId);
                }
                else if (true)
                {
                    loginSessionId = HttpContext.Current.Request["SessionId"] ?? "";
                    if (string.IsNullOrWhiteSpace(loginSessionId))
                    {
                        loginSessionId = HttpContext.Current.Request.Form["SessionId"] ?? "";
                    }
                    //LogsManager.Debug("IsApploginSessionId：" + loginSessionId);
                }
                else
                {
                    //loginSessionId = CookieHelpers.Get(LoginToken);
                    //LogsManager.Debug("loginSessionId：" + loginSessionId);
                }
                if (!string.IsNullOrWhiteSpace(loginSessionId))
                {
                    return $"{LoginToken}_{loginSessionId}";
                }
                return loginSessionId;
            }
        }

    }
}
