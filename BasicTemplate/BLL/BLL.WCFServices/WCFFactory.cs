using Common.Entity;
using Common.Enums;
using Common.Interface.WcfInterface;
using Common.Misc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BLL.WCFServices
{
    /// <summary>
    /// WCF工厂类
    /// </summary>
    /// <typeparam name="T">服务接口类型</typeparam>
    public class WCFFactory<T> where T : IWCFInterface
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        protected TimeSpan TimeOut { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        protected string ServiceUrl { get; set; }
        /// <summary>
        /// 工厂对象
        /// </summary>
        private ChannelFactory<T> factory = null;

        public WCFFactory()
        {
            if (TimeOut.TotalSeconds == 0)
            {
                TimeOut = TimeSpan.FromMinutes(3);
            }
        }


        /// <summary>
        /// 获取WCF地址
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        protected string GetUrl(WCFServiceEnum serviceType)
        {
            try
            {
                string key = $"{CacheKeys.WCFRegisterCenter}_{serviceType.ToString()}";
                var cacheData = CacheUtil.Get<List<TWcfRegister>>(key);
                if (cacheData == null)
                {
                    cacheData = CacheHelper.Get<List<TWcfRegister>>(key);
                    if (cacheData == null)
                    {
                        cacheData = SqlHelper.Query<TWcfRegister>(null, "procGetWcfRegisterList").ToList().FindAll(a => a.FServiceName == serviceType.ToString());
                        CacheHelper.Set(key, cacheData);
                    }
                    if (cacheData != null)
                        CacheUtil.Set(key, cacheData, CacheTimeOut.FiveSecond);
                }
                if (cacheData.Any())
                {
                    var list = cacheData.Where(a => a.FStatus == 1);
                    if (list.Any())
                    {
                        if (SystemConfig.IsDevelopment)
                        {
                            ///就近原则，查找是否有本机服务
                            var localIp = ClientIpHelper.GetInternalIP();
                            var localService = list.FirstOrDefault(a => a.FIpAddress.Equals(localIp));
                            if (localService != null)
                            {
                                return localService.FServiceUrl;
                            }
                        }
                        if (list.Count() == 1)
                        {
                            return list.FirstOrDefault().FServiceUrl;
                        }
                        else
                        {
                            Random r = new Random(Guid.NewGuid().GetHashCode());
                            int index = r.Next(list.Count());
                            return list.ElementAt(index).FServiceUrl;
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                ex.Error("获取WCF地址异常");
                return string.Empty;
            }
        }

        /// <summary>
        /// WCF工厂对象
        /// </summary>
        protected T FactoryObject
        {
            get
            {
                factory = CreateFactory();
                //WCFChannelFactoryPoolManager<T>.Create(ServiceUrl, TimeOut, 5);
                //factory = WCFChannelFactoryPoolManager<T>.Pop();
                return factory.CreateChannel();
            }
        }

        /// <summary>
        /// 创建WCF工厂
        /// </summary>
        /// <returns></returns>
        public ChannelFactory<T> CreateFactory()
        {
            if (string.IsNullOrWhiteSpace(ServiceUrl))
            {
                throw new ArgumentNullException("ServiceUrl", "服务地址不能为空");
            }
            EndpointAddress endpointAddress = new EndpointAddress(ServiceUrl);
            BasicHttpBinding basicHttpBinding = GetBasicHttpBinding();
            return new ChannelFactory<T>(basicHttpBinding, endpointAddress);
        }

        private BasicHttpBinding GetBasicHttpBinding()
        {
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
            basicHttpBinding.OpenTimeout = TimeOut;
            basicHttpBinding.ReceiveTimeout = TimeOut;
            basicHttpBinding.UseDefaultWebProxy = true;
            basicHttpBinding.TransferMode = TransferMode.Buffered;
            basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
            basicHttpBinding.SendTimeout = TimeOut;
            basicHttpBinding.MaxReceivedMessageSize = int.MaxValue;
            basicHttpBinding.MaxBufferSize = int.MaxValue;
            basicHttpBinding.MaxBufferPoolSize = int.MaxValue;
            return basicHttpBinding;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        protected void ExecuteAction(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                _MethodBase mb = (new StackTrace(1, true)).GetFrame(0).GetMethod();
                ex.Error($"[WCF通讯异常]{mb.Name}执行异常");
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="action"></param>
        protected TResult ExecuteAction<TResult>(Func<TResult> action)
        {
            TResult result = default(TResult);
            try
            {
                result = action.Invoke();
            }
            catch (Exception ex)
            {
                _MethodBase mb = (new StackTrace(1, true)).GetFrame(0).GetMethod();
                ex.Error($"[WCF通讯异常]{mb.Name}");
            }
            finally
            {
                Close();
            }
            return result;
        }

        /// <summary>
        /// 关闭WCF通讯
        /// </summary>
        protected void Close()
        {
            if (factory != null)
            {
                //WCFChannelFactoryPoolManager<T>.Push(factory);
                factory.Close();
            }
        }

        #region <缓存操作>
        public string GetCache(string key)
        {
            string result = string.Empty;
            try
            {
                result = FactoryObject.GetCache(key);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]获取缓存");
            }
            finally
            {
                Close();
            }
            return result;
        }

        public List<string> GetAllKeys()
        {
            List<string> keys = new List<string>();
            try
            {
                keys = FactoryObject.GetAllKeys();
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]获取所有缓存KEY");
            }
            finally
            {
                Close();
            }
            return keys;
        }

        public void Clear()
        {
            try
            {
                FactoryObject.Clear();
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]清理所有缓存");
            }
            finally
            {
                Close();
            }
        }

        public void Remove(string key)
        {
            try
            {
                FactoryObject.Remove(key);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]移除单个缓存");
            }
            finally
            {
                Close();
            }
        }

        public void RemoveAll(string pattern)
        {
            try
            {
                FactoryObject.RemoveAll(pattern);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]移除匹配缓存");
            }
            finally
            {
                Close();
            }
        }

        public void RemoveKeys(List<string> keys)
        {
            try
            {
                FactoryObject.RemoveKeys(keys);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]移除指定的缓存集合");
            }
            finally
            {
                Close();
            }
        }

        public List<string> SearchKeys(string pattern)
        {
            List<string> keys = new List<string>();
            try
            {
                keys = FactoryObject.SearchKeys(pattern);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]查找匹配的缓存KEY");
            }
            finally
            {
                Close();
            }
            return keys;
        }

        public bool ContainsKey(string key)
        {
            var result = false;
            try
            {
                result = FactoryObject.ContainsKey(key);
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]查找匹配的缓存KEY");
            }
            finally
            {
                Close();
            }
            return result;
        }
        #endregion

        public bool TestConnect()
        {
            var result = false;
            try
            {
                result = FactoryObject.TestConnect();
            }
            catch (Exception ex)
            {
                ex.Error("[WCF通讯异常]测试");
            }
            finally
            {
                Close();
            }
            return result;
        }
    }
}
