using BLL.WCFServices;
using Common.Entity;
using Common.Enums;
using Common.Interface.WcfInterface;
using Common.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace BLL.WEBCommon
{
    public class WCFRegister
    {
        private static Dictionary<string, ServiceHost> hostList = new Dictionary<string, ServiceHost>();

        /// <summary>
        /// 创建WCF通道
        /// </summary>
        /// <typeparam name="IService">接口</typeparam>
        /// <typeparam name="Service">实例</typeparam>
        /// <param name="host">宿主</param>
        /// <param name="serviceUrl">地址</param>
        public static void Register<IService, Service>(WCFServiceEnum serviceType)
        {
            string serviceName = serviceType.ToString();
            var configValue = ServiceConfigHelper.GetConfig(serviceName);
            Remove(serviceName, configValue);

            int newPort = GetFirstAvailablePort();
            var localIp = ClientIpHelper.GetInternalIP();

            string serviceUrl = $"http://{localIp}:{newPort}/Services/{serviceName}/";
            if (string.IsNullOrWhiteSpace(serviceUrl))
            {
                throw new Exception("WCF地址不能为空");
            }
            Uri endpointAddress = new Uri(serviceUrl);
            ServiceHost host = new ServiceHost(typeof(Service), endpointAddress);
            BasicHttpBinding basicHttpBinding = CreateBasicHttpBinding();
            //实例化type，binding，address
            ServiceEndpoint sed = host.AddServiceEndpoint(typeof(IService), basicHttpBinding, "");
            //设置Service的ServiceContract特性的SessionMode
            sed.Contract.SessionMode = SessionMode.Allowed;

            //寻找元数据
            #region <ServiceMetadataBehavior>
            ServiceMetadataBehavior serviceMetadataBehavior = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (serviceMetadataBehavior == null)
            {
                serviceMetadataBehavior = new ServiceMetadataBehavior();
                host.Description.Behaviors.Add(serviceMetadataBehavior);
            }
            serviceMetadataBehavior.HttpGetEnabled = true;
            serviceMetadataBehavior.HttpsGetEnabled = true;
            #endregion

            #region <ServiceBehaviorAttribute>
            ServiceBehaviorAttribute behaviorAttribute = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            if (behaviorAttribute == null)
            {
                behaviorAttribute = new ServiceBehaviorAttribute();
                host.Description.Behaviors.Add(behaviorAttribute);
            }
            //设置服务类Service的ServiceBehavior特性的UseSynchronizationContext、ConcurrencyMode、InstanceContextMode
            behaviorAttribute.UseSynchronizationContext = false;
            behaviorAttribute.ConcurrencyMode = ConcurrencyMode.Multiple;
            behaviorAttribute.InstanceContextMode = InstanceContextMode.PerCall;
            #endregion

            #region <ServiceThrottlingBehavior>
            ServiceThrottlingBehavior serviceThrottlingBehavior = host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (serviceThrottlingBehavior == null)
            {
                serviceThrottlingBehavior = new ServiceThrottlingBehavior();
                host.Description.Behaviors.Add(serviceThrottlingBehavior);
            }
            serviceThrottlingBehavior.MaxConcurrentCalls = 5000;
            serviceThrottlingBehavior.MaxConcurrentInstances = 5000;
            serviceThrottlingBehavior.MaxConcurrentSessions = 5000;
            #endregion

            #region <ServiceDebugBehavior>
            ServiceDebugBehavior serviceDebugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (serviceMetadataBehavior == null)
            {
                serviceDebugBehavior = new ServiceDebugBehavior();
                host.Description.Behaviors.Add(serviceDebugBehavior);
            }
            serviceDebugBehavior.IncludeExceptionDetailInFaults = true;
            #endregion

            #region <DataContractSerializerOperationBehavior>
            //ContractDescription contractDescription = host.Description.Endpoints[0].Contract;
            //foreach (OperationDescription operationDescription in contractDescription.Operations)
            //{
            //    if (operationDescription != null)
            //    {
            //        // Find the serializer behavior.
            //        DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
            //        // If the serializer is not found, create one and add it.
            //        if (dataContractSerializerOperationBehavior == null)
            //        {
            //            dataContractSerializerOperationBehavior = new DataContractSerializerOperationBehavior(operationDescription);
            //            operationDescription.Behaviors.Add(dataContractSerializerOperationBehavior);
            //        }
            //        // Change the settings of the behavior.
            //        dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 2147483647;
            //        dataContractSerializerOperationBehavior.IgnoreExtensionDataObject = true;
            //    }
            //}
            #endregion

            #region <注册WCF>
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            host.Opened += delegate
            {
                LogsManager.Info(string.Format("{0}开始监听Uri为：[{1}]", typeof(Service).Name, endpointAddress.ToString()));
            };
            host.Open();
            lock (hostList)
            {
                hostList[serviceName] = host;
            }
            #endregion

            #region <更新Redis缓存>
            TWcfRegister newModel = new TWcfRegister()
            {
                FServiceName = serviceName,
                FIpAddress = localIp,
                FPort = newPort,
                FServiceUrl = serviceUrl
            };
            CommonClass.SystemConfigRepository.InsertWcfRegister(newModel);
            LoadWcf(serviceName);
            ServiceConfigHelper.SetConfig(serviceName, $"{localIp}:{newPort}");
            #endregion
        }

        private static void Remove(string serviceName, string configValue)
        {
            if (!string.IsNullOrWhiteSpace(configValue))
            {
                var arr = configValue.Split(':');
                TWcfRegister oldModel = new TWcfRegister()
                {
                    FServiceName = serviceName,
                    FIpAddress = arr[0],
                    FPort = arr[1].Value<int>()
                };
                CommonClass.SystemConfigRepository.DeleteWcfRegister(oldModel);
            }
        }

        private static void LoadWcf(string serviceName)
        {
            var list = CommonClass.SystemConfigRepository.GetWcfRegisterList().FindAll(a => a.FServiceName == serviceName);
            CacheHelper.Set($"{CacheKeys.WCFRegisterCenter}_{serviceName}", list);
        }

        private static BasicHttpBinding CreateBasicHttpBinding()
        {
            var sendTimeout = "00:10:00";
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding()
            {
                Name = "NoneSecurity",
                TransferMode = TransferMode.Streamed,
                MaxReceivedMessageSize = int.MaxValue,
                MaxBufferSize = 65536000,
                MaxBufferPoolSize = 65536000,
                ReceiveTimeout = TimeSpan.MaxValue,
                SendTimeout = TimeSpan.Parse(sendTimeout),
                UseDefaultWebProxy = false,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxArrayLength = int.MaxValue,
                    MaxStringContentLength = int.MaxValue,
                    MaxBytesPerRead = int.MaxValue,
                },
                Security = new BasicHttpSecurity()
                {
                    Mode = BasicHttpSecurityMode.None
                }
            };
            return basicHttpBinding;
        }

        /// <summary>
        /// 获取第一个可用的端口号
        /// </summary>
        /// <returns></returns>
        public static int GetFirstAvailablePort()
        {
            int MAX_PORT = 6000; //系统tcp/udp端口数最大是65535            
            int BEGIN_PORT = 5000;//从这个端口开始检测

            for (int i = BEGIN_PORT; i < MAX_PORT; i++)
            {
                if (PortIsAvailable(i)) return i;
            }

            return -1;
        }

        /// <summary>
        /// 获取操作系统已用的端口号
        /// </summary>
        /// <returns></returns>
        private static IList PortIsUsed()
        {
            //获取本地计算机的网络连接和通信统计数据的信息
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            IList allPorts = new ArrayList();
            foreach (IPEndPoint ep in ipsTCP) allPorts.Add(ep.Port);
            foreach (IPEndPoint ep in ipsUDP) allPorts.Add(ep.Port);
            foreach (TcpConnectionInformation conn in tcpConnInfoArray) allPorts.Add(conn.LocalEndPoint.Port);

            return allPorts;
        }

        /// <summary>
        /// 检查指定端口是否已用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        private static bool PortIsAvailable(int port)
        {
            bool isAvailable = true;

            IList portUsed = PortIsUsed();

            foreach (int p in portUsed)
            {
                if (p == port)
                {
                    isAvailable = false; break;
                }
            }

            return isAvailable;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public static void Close()
        {
            try
            {
                lock (hostList)
                {
                    foreach (var item in hostList)
                    {
                        if (item.Value != null && item.Value.State == CommunicationState.Opened)
                        {
                            item.Value.Close();
                        }
                        string serviceName = item.Key;
                        var configValue = ServiceConfigHelper.GetConfig(serviceName);
                        Remove(serviceName, configValue);
                        LoadWcf(serviceName);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Error("WCF服务停止异常");
            }
        }

        /// <summary>
        /// 检查服务
        /// </summary>
        public static void CheckService()
        {
            var list = CommonClass.SystemConfigRepository.GetWcfRegisterList();
            var rows = EnumUtilty.GetEnumList<WCFServiceEnum>().Elements("row");
            foreach (var row in rows)
            {
                string serviceName = row.Element("name").Value;
                var services = list.Where(a => a.FServiceName == serviceName);
                if (services.Count() > 1)
                {
                    foreach (var item in services)
                    {
                        IWCFInterface wcf = null;
                        WCFServiceEnum serviceType = EnumUtilty.StringToEnum<WCFServiceEnum>(item.FServiceName);
                        switch (serviceType)
                        {
                            //case WCFServiceEnum.ReportService: wcf = new WCFReportClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.BetService: wcf = new WCFBetClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.GameService: wcf = new WCFGameClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.SysConfigService: wcf = new WCFSysConfigClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.RobotService: wcf = new WCFRobotClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.AccountService: wcf = new WCFAccountClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.ActivityService: wcf = new WCFActivityClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.DayWagesService: wcf = new WCFDayWagesClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.SettlementService: wcf = new WCFSettlementClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.PeriodService: wcf = new WCFPeriodClient(item.FServiceUrl); break;
                            //case WCFServiceEnum.RebateReportService: wcf = new WCFRebateReportClient(item.FServiceUrl); break;
                            case WCFServiceEnum.BalanceService: wcf = new WCFBalanceClient(item.FServiceUrl); break;
                                //case WCFServiceEnum.OrderBalanceService: wcf = new WCFOrderBalanceClient(item.FServiceUrl); break;
                                //case WCFServiceEnum.AccessMoneyService: wcf = new WCFAccessMoneyClient(item.FServiceUrl); break;
                        }
                        if (wcf != null)
                        {
                            var flag = wcf.TestConnect();
                            if (flag == false)
                            {
                                if (item.FStatus == 1)
                                {
                                    item.FStatus = 2;
                                    CommonClass.SystemConfigRepository.UpdateWcfRegister(item);
                                }
                            }
                            else
                            {
                                if (item.FStatus == 2)
                                {
                                    item.FStatus = 1;
                                    CommonClass.SystemConfigRepository.UpdateWcfRegister(item);
                                }
                            }
                        }
                    }
                }
                CacheHelper.Set($"{CacheKeys.WCFRegisterCenter}_{serviceName}", services);
            }
        }
    }
}
