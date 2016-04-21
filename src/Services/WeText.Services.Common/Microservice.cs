using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;
using WeText.Common.Services;

namespace WeText.Services.Common
{
    public abstract class Microservice : Service
    {
        private readonly ICommandConsumer commandConsumer;
        private readonly IEventConsumer eventConsumer;
        private bool disposed;

        public Microservice(ICommandConsumer commandConsumer, IEventConsumer eventConsumer)
        {
            this.commandConsumer = commandConsumer;
            this.eventConsumer = eventConsumer;
        }

        public ICommandConsumer CommandConsumer => commandConsumer;

        public IEventConsumer EventConsumer => eventConsumer;


        public override void Start(object[] args)
        {
            this.commandConsumer.Subscriber.Subscribe();
            this.eventConsumer.Subscriber.Subscribe();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.commandConsumer.Dispose();
                    this.eventConsumer.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
