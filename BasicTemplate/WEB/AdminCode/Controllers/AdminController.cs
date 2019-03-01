using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBService.Security;

namespace AdminCode.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout(bool isClear = false)
        {
            int userID = Authentication.Id;
            Authentication.Logout();
            //SignalRHubUtil.HubSendRemoveUser(userID);
            //SignalRChat.Hubs.OnlineHub.RemoveOnlineUser(userID);
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 超权提示页
        /// </summary>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult PermissionTips(string type = "", string url = "", string msg = "")
        {
            ViewBag.Type = type;
            ViewBag.Url = url;
            ViewBag.Msg = msg;
            return View();
        }

        public ActionResult Error(string msg = "")
        {
            ViewBag.Message = msg;
            return View();
        }


    }
}