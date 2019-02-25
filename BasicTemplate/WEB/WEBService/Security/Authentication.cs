using Common.Misc;
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

        #region 内置函数
        private static string GetContextKey(object userId)
        {
            return $"LoginContext_{userId}";
        }
        #endregion

        #region <当前登录用户对象>

        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        internal static Context CacheContext
        {
            get
            {
                return GetCurrentContext();
            }
        }

        private static Context GetCurrentContext()
        {
            string loginSessionID = CurrentSessionID;
            Context context = new Context();
            try
            {
                if (!string.IsNullOrWhiteSpace(loginSessionID))
                {
                    //context = CacheUtil.Get<Context>(loginSessionID);
                    //if (context == null)
                    //{
                    //    string listId = CacheHelper.Get<string>(loginSessionID);
                    //    if (!string.IsNullOrWhiteSpace(listId))
                    //    {
                    //        context = CacheHelper.Get<Context>(listId);
                    //        if (context == null)
                    //        {
                    //            RemoveSessionID(loginSessionID, listId);
                    //            LogsManager.Error(string.Format("Redis缓存数据为空，key：{0}", listId));
                    //        }
                    //        else
                    //        {
                    //            context.LoginSessionID = loginSessionID;
                    //            AddSessionID(loginSessionID, context);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        RemoveSessionID(loginSessionID, listId);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                ex.Error("获取用户缓存信息异常");
            }
            if (context == null)
            {
                context = new Context();
            }
            return context;
        }
        #endregion

        public static bool Login(Context context, bool isIgnoreValid = false)
        {
            try
            {
                string listId = GetContextKey(context.Id);
                var CacheContext = "";// CacheHelper.Get<Context>(listId);
                if (CacheContext != null && isIgnoreValid == false)
                {
                    //var configRows = CompanyConfigUtil.GetCompanySysConfig(context.CompanyId);
                    //bool IsValidateUniqueLogin = configRows.GetConfigValue(CompanyConfigKeys.IsValidateUniqueLogin, true);
                    //if (IsValidateUniqueLogin)
                    //{
                    //    CacheHelper.Remove(CacheContext.LoginSessionID);
                    //    CacheHelper.Remove(listId);
                    //}
                }
                context.LoginStatus = context.Status;
                context.GuidKey = GuidUtil.GuidKey;
                //string url = SkinManager.Url.Replace("//www.", "").Replace("//WWW.", "");
                context.Token = EncryptionHelper.Md5Encryption(context.GuidKey, "");
                string loginSessionID = EncryptionHelper.Md5Encryption(context.GuidKey, context.Id.ToString(), "");//SessionRedis.SessionID
                context.LoginSessionID = $"{LoginToken}_{loginSessionID}";
                CookieHelper.Set(LoginToken, loginSessionID);
                AddSessionID(context.LoginSessionID, context);
                return true;
            }
            catch (Exception ex)
            {
                ex.Error(string.Format("用户登录写入COOKIE异常"));
                return false;
            }
        }


        public static void Logout(bool isSelf = false)
        {
            try
            {
                int userID = CacheContext.Id;
                if (userID > 0)
                {
                    var sessionId = CurrentSessionID;
                    LogsManager.Debug($"自主登出：用户[{userID}]自主登出，CurrentSessionID：{sessionId}");
                    string listId = GetContextKey(userID);

                    var setId = "";// GetOnlineUserKey(CacheContext.CompanyId, HomeView);
                    RemoveSessionID(sessionId, listId, setId, userID, isSelf);
                }
                //HttpContext.Current.Session.Remove(HighPassword);
                //ThirdPartyGamLoginOut();

            }
            catch (Exception ex)
            {
                ex.Error("Logout异常");
            }
            string[] cookieNames = new string[] { LoginToken, "LastRequestTime" };
            foreach (string cookieName in cookieNames)
            {
                CookieHelper.Remove(cookieName);
            }
        }
        private static void AddSessionID(string loginSessionID, Context context)
        {
            //UpdateGameOnlineInfo(context);

            string listId = GetContextKey(context.Id);
            //CacheUtil.Set(loginSessionID, context, RunTimeOut);
            //CacheUtil.Set(listId, loginSessionID, RunTimeOut);
            //CacheHelper.Set(loginSessionID, listId, RedisTimeOut);
            //CacheHelper.Set(listId, context, RedisTimeOut);

            //var setId = GetOnlineUserKey(context.CompanyId, context.HomeView);
            //var timeStamp = TimeStamp.DateTimeToUnix(DateTime.Now);
            //CacheHelper.AddItemToSortedSet(setId, context.Id.ToString(), timeStamp);
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
