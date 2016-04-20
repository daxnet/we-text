using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public class RabbitMqQueueSubscriber : DisposableObject, IMessageSubscriber
    {
        private readonly string queueName;
        private readonly IConnection connection;
        private readonly IModel channel;
        private bool disposed;


        public RabbitMqQueueSubscriber(string hostName, string queueName)
        {
            this.queueName = queueName;
            var factory = new ConnectionFactory() { HostName = hostName };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Subscribe()
        {
            channel.QueueDeclare(queue: this.queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

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

        private void OnMessageReceived(MessageReceivedEventArgs messageReceivedEventArgs)
        {
            this.MessageReceived?.Invoke(this, messageReceivedEventArgs);
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
    }
}
