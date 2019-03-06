using Common.Misc;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebCode
{
    public class SignalRConnection : PersistentConnection
    {
        private static IDictionary<string, string> dic = new Dictionary<string, string>();

        private static IDictionary<string, string> dicName = new Dictionary<string, string> {
            { "0","韩信"},
            { "1","李白"},
            { "2","程咬金"},
            { "3","张飞"},
            { "4","芈月"},
            { "5","赵云"},
            { "6","刘备"},
            { "7","风车车"},
            { "8","假老练"},
            { "9","莽娃儿"},
            { "10","吕布"},
            { "11","貂蝉"},
            { "12","无名氏"},
        };
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var value = CookieHelper.Get("connectionId");
            if (string.IsNullOrEmpty(value))
            {
                value = connectionId;
                CookieHelper.Set("connectionId", connectionId);
            }
            if (!dic.ContainsKey(value))
            {
                int random = new Random().Next(0, 12);
                dic.Add(value, dicName[$"{random}"]);

                CookieHelper.Set("connectionName", dicName[$"{random}"]);
            }
            Connection.Broadcast($"欢迎{dic[value]}进入聊天室~");
            return Connection.Send(value, "Welcome!");
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            var value = CookieHelper.Get("connectionId");
            value = string.IsNullOrEmpty(value) ? connectionId : value;
            //Connection.Send(connectionId, "我收到了：" + data);
            return Connection.Broadcast($"{dic[value]}:{data}");
        }
    }
}