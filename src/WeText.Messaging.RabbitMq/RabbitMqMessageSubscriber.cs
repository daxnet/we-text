using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public class RabbitMqMessageSubscriber : IMessageSubscriber
    {
        private readonly string exchangeName;
        private readonly IConnection connection;
        private readonly IModel channel;
        private bool disposed;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        ~RabbitMqMessageSubscriber()
        {
            this.Dispose(false);
        }

        public RabbitMqMessageSubscriber(string hostName, string exchangeName)
        {
            this.exchangeName = exchangeName;
            var factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public void Subscribe()
        {
            channel.ExchangeDeclare(exchange: this.exchangeName, type: "fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: this.exchangeName,
                              routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                this.OnMessageReceived(new MessageReceivedEventArgs(message));
                channel.BasicAck(e.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queue: queueName,
                                 noAck: false,
                                 consumer: consumer);
        }

        private void OnMessageReceived(MessageReceivedEventArgs e)
        {
            var handler = this.MessageReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void Dispose(bool disposing)
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
