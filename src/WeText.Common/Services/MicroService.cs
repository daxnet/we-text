using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;

namespace WeText.Common.Services
{
    public abstract class Microservice : Service
    {
        private readonly ICommandConsumer commandConsumer;
        private readonly IEventConsumer eventConsumer;

        public Microservice(ICommandConsumer commandConsumer, IEventConsumer eventConsumer)
        {
            this.commandConsumer = commandConsumer;
            this.eventConsumer = eventConsumer;
        }

        public override void Start(object[] args)
        {
            this.commandConsumer.Subscriber.Subscribe();
            this.eventConsumer.Subscriber.Subscribe();
        }
    }
}
