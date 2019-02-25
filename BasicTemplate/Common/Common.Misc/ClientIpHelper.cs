using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Misc
{
    public class ClientIpHelper
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIp()
        {
            string strIpAddress = null;
            if (HttpContext.Current == null || HttpContext.Current.Request == null)
            {
                return strIpAddress;
            }
            try
            {
                //if (HttpContext.Current.Request.Url.Host.ToLower().Equals("localhost"))
                //{
                //    strIpAddress = "127.0.0.1";
                //}
                //else
                //{
                strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(strIpAddress))
                {
                    strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else if (strIpAddress.Contains(','))
                {
                    string[] ips = strIpAddress.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ips != null && ips.Length > 0)
                    {
                        strIpAddress = ips[0];
                    }
                }
                if (string.IsNullOrEmpty(strIpAddress))
                {
                    strIpAddress = HttpContext.Current.Request.UserHostAddress;
                }
                //}
                string ipv4 = string.Empty;
                IPAddress ip;
                if (IPAddress.TryParse(strIpAddress, out ip))
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        ipv4 = ip.MapToIPv4().ToString();
                        //foreach (IPAddress ipAddr in Dns.GetHostEntry(strIpAddress).AddressList)
                        //{
                        //    if (ipAddr.AddressFamily.ToString() == "InterNetwork")
                        //    {
                        //        if (!string.IsNullOrWhiteSpace(ipAddr.ToString()))
                        //        {
                        //            ipv4 = ipAddr.ToString();
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
                if (!string.IsNullOrWhiteSpace(ipv4))
                {
                    strIpAddress = ipv4;
                }
                if (strIpAddress == "127.0.0.1") strIpAddress = "unknown";
                return strIpAddress;
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("获取IP异常:{0}", ex.ToString());
                return "unknown:" + strIpAddress;
            }
        }

        /// <summary>
        /// 获取平台
        /// </summary>
        /// <returns></returns>
        public static string GetPlatform()
        {
            string result = "Windows";
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null)
                {
                    return result;
                }
                string agent = HttpContext.Current.Request.UserAgent;
                if (string.IsNullOrEmpty(agent) == false)
                {
                    string[] keywords = { "Android", "iPhone", "iPod", "iPad", "Windows Phone", "MQQBrowser" };
                    //排除 Windows 桌面系统            
                    if (!agent.Contains("Windows NT") || (agent.Contains("Windows NT") && agent.Contains("compatible; MSIE 9.0;")))
                    {
                        //排除 苹果桌面系统                
                        if (!agent.Contains("Windows NT") && !agent.Contains("Macintosh"))
                        {
                            foreach (string item in keywords)
                            {
                                if (agent.Contains(item))
                                {
                                    return item;
                                }
                            }
                        }
                        return "Macintosh";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("获取平台时出错：{0}", ex.ToString()));
            }
            return result;
        }

        /// <summary>
        /// 是否本地
        /// </summary>
        public static bool IsLocalHost
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Request.Url.Host.ToLower().Equals("localhost"))
                    {
                        return true;
                    }
                    else
                    {
                        var REMOTE_ADDR = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        if (REMOTE_ADDR.Equals("127.0.0.1") || REMOTE_ADDR.Equals("::1"))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 判断IP类型。
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns>0:无效IP；1:局域网IP；2:广域网IP</returns>
        public static int GetIPType(string ipAddress)
        {
            if (ipAddress == "127.0.0.1")
            {
                return 1;
            }

            string[] ipAddressList = ipAddress.Split('.');
            int ipAddressTemp;

            //检查IP地址是否有效
            if (ipAddressList.Length != 4)
            {
                return 0;
            }
            if (!(int.TryParse(ipAddressList[0], out ipAddressTemp) && int.TryParse(ipAddressList[1], out ipAddressTemp)
                && int.TryParse(ipAddressList[2], out ipAddressTemp) && int.TryParse(ipAddressList[3], out ipAddressTemp)))
            {
                return 0;
            }
            if (!(int.Parse(ipAddressList[0]) >= 0 && int.Parse(ipAddressList[0]) <= 255
                    && int.Parse(ipAddressList[1]) >= 0 && int.Parse(ipAddressList[1]) <= 255
                    && int.Parse(ipAddressList[2]) >= 0 && int.Parse(ipAddressList[2]) <= 255
                    && int.Parse(ipAddressList[3]) >= 0 && int.Parse(ipAddressList[3]) <= 255))
            {
                return 0;
            }

            //局域网IP
            if (int.Parse(ipAddressList[0]) == 10
                    || (int.Parse(ipAddressList[0]) == 172 && int.Parse(ipAddressList[1]) >= 16 && int.Parse(ipAddressList[1]) <= 31)
                    || (int.Parse(ipAddressList[0]) == 192 && int.Parse(ipAddressList[1]) == 168))
            {
                return 1;
            }
            return 2;
        }

        /// <summary>
        /// 本机IP
        /// </summary>
        private static string internalIp = string.Empty;
        /// <summary>
        /// 获取本机内网IP
        /// </summary>
        /// <returns></returns>
        public static string GetInternalIP()
        {
            if (string.IsNullOrWhiteSpace(internalIp))
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        internalIp = ip.ToString();
                        break;
                    }
                }
            }
            return internalIp;
        }
    }
}
