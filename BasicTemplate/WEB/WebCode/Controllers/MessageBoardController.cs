using Common.Entity.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WEBService.WebBusiness;

namespace WebCode.Controllers
{
    public class MessageBoardController : Controller
    {
        // GET: MessageBoard
        public ActionResult Index()
        {
            var list = BusinessBll.GetMessageBoardList(this);
            return View(list);
        }

        public ActionResult Submit(MessageBoardEntity entity)
        {
            IPAddressSearch.IpAddressSearchWebServiceSoapClient client = new IPAddressSearch.IpAddressSearchWebServiceSoapClient();
            string theIpAddress = GetIP();
            entity.FIp = theIpAddress;
            if (theIpAddress != "127.0.0.1")
            {
                string[] IPAddress = client.getCountryCityByIp(theIpAddress);
                string IPAddressProviceInfo = IPAddress[1].Substring(0, 3);    //provice
                string IPAddressCityInfo = IPAddress[1].Substring(3, 3);  //city
                entity.FProvince = IPAddressProviceInfo;
                entity.FCity = IPAddressCityInfo;
            }
            else
            {
                entity.FIp = theIpAddress;
                entity.FProvince = "银河系";
                entity.FCity = "汪星";
            }
            var val = BusinessBll.CreateMessageBoard(this, entity);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public string GetIP()
        {
            // 穿过代理服务器取远程用户真实IP地址
            string Ip = string.Empty;
            if (HttpContext.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                {
                    if (HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        Ip = HttpContext.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    else
                        if (HttpContext.Request.ServerVariables["REMOTE_ADDR"] != null)
                        Ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    else
                        Ip = "0.0.0.0";
                }
                else
                    Ip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                Ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"].ToString();
                if (Ip == "::1") Ip = "127.0.0.1";
            }
            else
            {
                Ip = "0.0.0.0";
            }
            return Ip;
        }
    }
}