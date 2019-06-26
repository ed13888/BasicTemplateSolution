using Common.Entity.Business;
using Common.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Submit(MessageBoardEntity entity)
        {
            try
            {
                string theIpAddress = GetIP();
                entity.FIp = theIpAddress;
                entity.FProvince = "外太空";
                entity.FCity = "汪星";

                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync($"http://opendata.baidu.com/api.php?query={theIpAddress}&resource_id=6006&ie=utf8&oe=gbk&cb=op_aladdin_callback&format=json&tn=baidu");
                var reg = new Regex("{.+}");
                var json = reg.Match(response).Value;
                var result = JsonConvert.DeserializeAnonymousType(json, new
                {
                    status = "",
                    data = new List<Dictionary<string, string>>()

                });
                if (result.status == "0" && result.data.Count > 0)
                {
                    var location = result.data[0]["location"];
                    var splitStr = location.Split(new char[] { '省', '市' });
                    string province = splitStr[0], city = "";
                    if (splitStr.Length > 1)
                        city = splitStr[1];

                    if (theIpAddress != "127.0.0.1" && province != "菲律宾")
                    {
                        entity.FProvince = province;
                        entity.FCity = city;
                    }
                }



                #region webservice 获取地区   暂时弃用
                //IPAddressSearch.IpAddressSearchWebServiceSoapClient client = new IPAddressSearch.IpAddressSearchWebServiceSoapClient();
                //string theIpAddress = GetIP();
                //entity.FIp = theIpAddress;
                //string[] IPAddress = client.getCountryCityByIp(theIpAddress);
                //string IPAddressProviceInfo = IPAddress[1].Substring(0, 3);    //provice
                //string IPAddressCityInfo = IPAddress[1].Substring(3, 3);  //city
                //if (theIpAddress == "127.0.0.1" || IPAddressProviceInfo == "菲律宾")
                //{
                //    entity.FProvince = "银河系";
                //    entity.FCity = "汪星";
                //}
                //else
                //{
                //    entity.FProvince = IPAddressProviceInfo;
                //    entity.FCity = IPAddressCityInfo;
                //}
                #endregion


                var val = BusinessBll.CreateMessageBoard(this, entity);
                LogsManager.Info(val ? "留言成功" : $"留言失败:{JsonConvert.SerializeObject(entity)}");
            }
            catch (Exception ex)
            {
                LogsManager.Error(ex);
            }
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