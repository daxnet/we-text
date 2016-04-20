using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public abstract class RabbitMqQueueProducer : DisposableObject, IMessagePublisher
    {
        private readonly string queueName;
        private readonly IConnection connection;
        private readonly IModel channel;
        private bool disposed;

        protected RabbitMqQueueProducer(string hostName, string queueName)
        {
            this.queueName = queueName;
            var factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.channel.Dispose();
                    this.connection.Dispose();
                    disposed = true;
                }
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            channel.QueueDeclare(queue: this.queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonConvert.SerializeObject(message, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var bytes = Encoding.UTF8.GetBytes(json);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "", routingKey: this.queueName, basicProperties: properties, body: bytes);
        }
    }
}
