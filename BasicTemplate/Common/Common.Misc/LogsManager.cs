using Common.Entity;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public static class LogsManager
    {
        private static ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Info(string message)
        {
            logger.Info(message);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            logger.InfoFormat(format, args);
            string message = string.Format(format, args);
        }

        public static void Error(string message)
        {
            logger.Error(message);
            SendMessage(message, "Error");
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            logger.ErrorFormat(format, args);
            string message = string.Format(format, args);
            SendMessage(message, "Error");
        }

        public static void Error(this Exception ex, string message = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                _MethodBase mb = (new StackTrace(1, true)).GetFrame(0).GetMethod();
                message = mb.Name;
            }
            message = string.Format("{0}，{1}：{2}", message, ex.Message, ex.ToString());
            logger.Error(message);
            SendMessage(message, "Error");
        }

        public static void Debug(string message)
        {
            logger.Debug(message);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            logger.DebugFormat(format, args);
            string message = string.Format(format, args);
        }

        public static void Fatal(string message)
        {
            logger.Fatal(message);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            logger.FatalFormat(format, args);
            string message = string.Format(format, args);
        }

        public static void Warn(string message)
        {
            logger.Warn(message);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.WarnFormat(format, args);
        }

        public static void Operation(string message)
        {
            logger.Info(message);
            SendMessage(message, "Operation");
        }

        public static void OperationFormat(string format, params object[] args)
        {
            string message = string.Format(format, args);
            logger.InfoFormat(format, args);
            SendMessage(message, "Operation");
        }

        public static void SendMessage(string Message, string logsType)
        {
            try
            {
                var module = System.Reflection.MethodBase.GetCurrentMethod().Module;
                LogsMessage item = new LogsMessage();
                item.AssemblyName = module.Name;
                item.ClinetIP = ClientIpHelper.GetClientIp();
                item.ServiceIP = GetAddressIP();
                item.ModuleName = "";// LogsModule.Default;
                item.Message = Message;
                item.LogsType = logsType;
                item.OperatorID = 0;
                item.TakeTimes = 0;
                var data = JsonConvert.SerializeObject(item);
                //RabbitMQUtil.Publish(RabbitMQName.LogsCommand, "日志", data);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        static string GetAddressIP()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress _IPAddress in addressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }
}
