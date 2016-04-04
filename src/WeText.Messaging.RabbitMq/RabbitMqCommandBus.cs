using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;

namespace WeText.Messaging.RabbitMq
{
    public class RabbitMqCommandBus : RabbitMqBus, ICommandBus
    {
        public RabbitMqCommandBus(string hostName, string exchangeName)
            : base(hostName, exchangeName)
        { }

    }
}
