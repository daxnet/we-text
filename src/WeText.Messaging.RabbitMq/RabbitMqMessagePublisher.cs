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
    public abstract class RabbitMqMessagePublisher : DisposableObject, IMessagePublisher
    {
        private readonly string exchangeName;
        private readonly IConnection connection;
        private readonly IModel channel;
        private bool disposed;

        protected RabbitMqMessagePublisher(string uri, string exchangeName)
        {
            this.exchangeName = exchangeName;
            var factory = new ConnectionFactory() { Uri = uri };
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
            channel.ExchangeDeclare(exchange: this.exchangeName, type: "fanout");

            var json = JsonConvert.SerializeObject(message, Formatting.Indented, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var bytes = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: this.exchangeName, routingKey: "", basicProperties: null, body: bytes);
        }
    }
}
