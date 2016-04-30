using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public class RabbitMqEventPublisher : RabbitMqMessagePublisher, IEventPublisher
    {
        public RabbitMqEventPublisher(string uri, string exchangeName)
            : base(uri, exchangeName)
        { }
    }
}
