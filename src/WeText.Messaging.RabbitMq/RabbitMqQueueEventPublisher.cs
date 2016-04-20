using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public class RabbitMqQueueEventPublisher : RabbitMqQueueProducer, IEventPublisher
    {
        public RabbitMqQueueEventPublisher(string hostName, string queueName) : base(hostName, queueName)
        {
        }
    }
}
