using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResultTransfer.Client
{
    public class MQBuilder
    {
        private static MQBuilder _instance = null;
        public static MQBuilder Instance => _instance ?? (_instance = new MQBuilder());

        /// <summary>
        /// MQ链接地址
        /// </summary>
        private ConnectionFactory rabbitMqFactory = default(ConnectionFactory);

        private MQBuilder()
        {
            rabbitMqFactory = new ConnectionFactory()
            {
                // HostName = "localhost",     //本地docker
                HostName = "129.211.170.215", //自己的云
                UserName = "JhMQ",
                Password = "123456",
                Port = 5672,
                VirtualHost = "JhMQ",
            };
        }

        /// <summary>
        /// 消息Customer接收消息
        /// </summary>
        /// <param name="ExchangeName">交换机名称</param>
        /// <param name="QueueName">队列名称</param>
        /// <returns></returns>
        public string CustomerReciver(string ExchangeName, string QueueName)
        {
            using (IConnection conn = rabbitMqFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    //定义交换机 - 广播
                    channel.ExchangeDeclare(ExchangeName, "fanout");
                    //创建队列
                    channel.QueueDeclare(QueueName, durable: true, autoDelete: true, exclusive: false, arguments: null);
                    channel.QueueBind(QueueName, ExchangeName, routingKey: "userid");
                    while (true)
                    {
                        BasicGetResult msgResponse = channel.BasicGet(QueueName, autoAck: true);
                        if (msgResponse != null)
                        {
                            var JSON = Encoding.UTF8.GetString(msgResponse.Body.ToArray());
                            return JSON;
                        }
                    }
                }
            }
        }
    }
}
