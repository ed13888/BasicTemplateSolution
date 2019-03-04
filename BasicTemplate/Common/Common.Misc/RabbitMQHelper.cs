using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Misc
{
    public class RabbitMQHelper
    {
        private static ConnectionFactory rabbitMqFactory;
        private static IConnection rabbitMqConn;
        private static IModel _sendChannel;
        private static IModel _receiveChannel;
        private static string _subName { get; set; } = "MYMQ";

        private static Dictionary<string, IModel> __sendChannelPools = new Dictionary<string, IModel>();
        private static Dictionary<string, IModel> _receiveChannelPools = new Dictionary<string, IModel>();
        private static Dictionary<string, object> _receiverActionPools = new Dictionary<string, object>();

        public RabbitMQHelper()
        {
            //if (model == null)
            //    throw new ArgumentNullException(nameof(model));

            //try
            //{
            //    rabbitMqFactory = new ConnectionFactory
            //    {
            //        HostName = model.HostName,
            //        Port = model.HostPort,
            //        VirtualHost = model.ExtendInfo,
            //        UserName = model.UserName,
            //        Password = model.Password
            //    };
            //    _subName = model.SubName;
            //    rabbitMqConn = rabbitMqFactory.CreateConnection();
            //    _sendChannel = rabbitMqConn.CreateModel();
            //    _receiveChannel = rabbitMqConn.CreateModel();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    //ex.Error($"[{MQName}]", "RabbitMQManager");
            //}
        }

        ~RabbitMQHelper()
        {
            _sendChannel.Close();
            _receiveChannel.Close();
            rabbitMqConn.Close();

        }

        public static void Init()
        {
            try
            {
                //amqp://user:pass@hostName:port/vhost

                //rabbitMqFactory = new ConnectionFactory
                //{
                //    HostName = model.HostName,
                //    Port = model.HostPort,
                //    VirtualHost = model.ExtendInfo,
                //    UserName = model.UserName,
                //    Password = model.Password
                //};
                var conn = SystemConfig.RabbitMQConnection;
                if (string.IsNullOrEmpty(conn)) return;

                rabbitMqFactory = new ConnectionFactory
                {
                    Uri = new Uri(conn)
                };

                //_subName = model.SubName;
                rabbitMqConn = rabbitMqFactory.CreateConnection();
                _sendChannel = rabbitMqConn.CreateModel();
                _receiveChannel = rabbitMqConn.CreateModel();
            }
            catch (Exception ex)
            {
                throw ex;
                //ex.Error($"[{MQName}]", "RabbitMQManager");
            }
        }
        public static void Subscribe(string queuesName, Func<string, Task> tempAction, MsgType msgType = MsgType.Queue)
        {
            try
            {
                if (_receiveChannel == null) _receiveChannel = rabbitMqFactory.CreateConnection().CreateModel();
                var consumer = new EventingBasicConsumer(_receiveChannel);

                consumer.Received += (model, ea) =>
                {
                    var msgBody = Encoding.UTF8.GetString(ea.Body);
                    tempAction(msgBody);
                };
                switch (msgType)
                {
                    case MsgType.Fanout:
                        _receiveChannel.ExchangeDeclare(exchange: queuesName, type: "fanout", durable: true, autoDelete: false, arguments: null);
                        var _queue = _receiveChannel.QueueDeclare($"{queuesName}_{_subName}", true, false, false, null);
                        _receiveChannel.QueueBind(_queue.QueueName, queuesName, queuesName);
                        _receiveChannel.BasicConsume(queue: _queue.QueueName, autoAck: true, consumer: consumer);
                        break;
                    case MsgType.Queue:
                        _receiveChannel.QueueDeclare(queuesName, true, false, false, null);
                        _receiveChannel.BasicConsume(queue: queuesName, autoAck: true, consumer: consumer);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                //ex.Error($"[{MQName}] Receive=>", queuesName);
            }
        }


        public static void Publish(string queuesName, string message, MsgType msgType = MsgType.Queue)
        {
            try
            {
                var props = _sendChannel.CreateBasicProperties();
                props.Persistent = true;

                var msgBody = Encoding.UTF8.GetBytes(message);

                switch (msgType)
                {
                    case MsgType.Fanout:
                        _sendChannel.ExchangeDeclare(exchange: queuesName, type: "fanout", durable: true, autoDelete: false, arguments: null);
                        _sendChannel.BasicPublish(exchange: queuesName, routingKey: "", basicProperties: props, body: msgBody);
                        break;
                    case MsgType.Queue:
                        _sendChannel.QueueDeclare(queuesName, true, false, false, null);
                        _sendChannel.BasicPublish(exchange: "", routingKey: queuesName, basicProperties: props, body: msgBody);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                //ex.Error($"[{MQName}] Send=>", queuesName);
            }
        }


    }
    public enum MsgType
    {
        /// <summary>
        /// 广播模式
        /// </summary>
        Fanout,
        /// <summary>
        /// 队列模式
        /// </summary>
        Queue
    }
}
