using Common.Interface.WcfInterface;
using Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace BLL.WCFServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class WCFServiceBase : IWCFInterface
    {
        public WCFServiceBase()
        {
            OperationContext context = OperationContext.Current;
            if (context != null)
            {
                //获取客户端请求的路径
                string AbsolutePath = context.EndpointDispatcher.EndpointAddress.Uri.AbsolutePath;
                //获取客户端ip和端口
                MessageProperties properties = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                string client_ip = endpoint.Address;
                //int client_port = endpoint.Port;

                //获取客户端请求的契约信息
                //string contract_name = context.EndpointDispatcher.ContractName;
                //获取客户端请求的路径
                //Uri request_uri = context.EndpointDispatcher.EndpointAddress.Uri;
                string sessionid = context.SessionId;
                //string wcfappname = HeaderOperater.GetServiceWcfAppNameHeader(context);
                //wcfappname = wcfappname == null ? "" : wcfappname;
                context.Channel.Closed += (object sender, EventArgs e) =>
                {
                    //MonitorData.Instance.UpdateUrlConnNums(client_ip + "_" + wcfappname, AbsolutePath, false);
                    //WCFMonitorData.Instance.Reduce();
                    //LogsManager.Debug($"[{sessionid}]于[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] 断开连接");
                };

                //WCFMonitorData.Instance.Add();
                //LogsManager.Debug($"[{sessionid}]于[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] 连接成功");
            }
        }

        #region <缓存操作>
        public string GetCache(string key)
        {
            var obj = CacheUtil.Get<object>(key);
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public List<string> GetAllKeys()
        {
            return CacheUtil.GetAllKeys().ToList();
        }

        public void Clear()
        {
            CacheUtil.Clear();
        }

        public void Remove(string key)
        {
            CacheUtil.Remove(key);
        }

        public void RemoveAll(string pattern)
        {
            var keys = CacheUtil.SearchKeys(pattern);
            CacheUtil.RemoveAll(keys);
        }

        public void RemoveKeys(List<string> keys)
        {
            CacheUtil.RemoveAll(keys);
        }

        public List<string> SearchKeys(string pattern)
        {
            return CacheUtil.SearchKeys(pattern).ToList();
        }

        public bool ContainsKey(string key)
        {
            return CacheUtil.ContainsKey(key);
        }

        public bool TestConnect()
        {
            return true;
        }
        #endregion
    }
}
