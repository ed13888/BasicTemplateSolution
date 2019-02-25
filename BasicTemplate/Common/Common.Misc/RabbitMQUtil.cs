using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class RabbitMQUtil
    {
        /// <summary>
        /// 发送消息对消息队列
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="instanceName"></param>
        public static void Publish(string messageID, string title, string body, string instanceName = "DefaultInstance")
        {
            //RabbitMQMessage msg = new RabbitMQMessage
            //{
            //    MessageID = messageID,
            //    MessageBody = body,
            //    MessageTitle = title
            //};
            //msg.MessageRouter = $"{msg.MessageID}.Router";
            //RabbitMQHelper.Publish(msg, instanceName).ConfigureAwait(false);
        }

        ///// <summary>
        ///// 发送消息对消息队列
        ///// </summary>
        ///// <param name="messageID"></param>
        ///// <param name="title"></param>
        ///// <param name="body"></param>
        ///// <param name="instanceName"></param>
        //public static void PublishToOther(string exchangeName, string routerId, RabbitMQMessage message, string instanceName = "DefaultInstance")
        //{
        //    //RabbitMQHelper.Publish(exchangeName, routerId, message, instanceName).ConfigureAwait(false);
        //}
    }
}
