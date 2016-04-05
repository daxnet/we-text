using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public class RabbitMqEventBus : RabbitMqBus, IEventPublisher
    {
        public RabbitMqEventBus(string hostName, string exchangeName)
            : base(hostName, exchangeName)
        { }
    }
}
