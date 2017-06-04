using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficMoniterRabbitMQ.Models;

namespace TrafficMoniterRabbitMQ
{
    public class Client : IDisposable
    {
        private IConnection connection;
        private IModel channel;
        
        public Client()
        {
            var factory = new ConnectionFactory() {
                HostName = "localhost",
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/",
                Protocol = Protocols.AMQP_0_9_1
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public IConnection GetConnection()
        {
            return connection;
        }

        public void ConsumerCountsQueue(EventHandler<BasicDeliverEventArgs> callBack)
        {
            channel.ExchangeDeclare(exchange: "counts", type: "fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "counts", routingKey: "");
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += callBack;
            channel.BasicConsume(queue: queueName, noAck: true, consumer: consumer);
        }

        public void PublishMessage(string _body, string name)
        {
            channel.QueueBind(queue: name,exchange: name,routingKey: "",arguments: null);
            var body = Encoding.UTF8.GetBytes(_body);
            channel.BasicPublish(exchange: name, routingKey: name, basicProperties: null, body: body);
        }
        
        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
