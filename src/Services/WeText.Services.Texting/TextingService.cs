using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;
using WeText.Common.Services;

namespace WeText.Services.Texting
{
    public class TextingService : Service
    {
        private readonly ICommandConsumer commandConsumer;
        private readonly IEventConsumer eventConsumer;
        private readonly IMessageConsumer commandRedirector;
        private readonly IMessageConsumer eventRedirector;
        private bool disposed;

        public TextingService(IMessageConsumer commandRedirector, IMessageConsumer eventRedirector, ICommandConsumer commandConsumer, IEventConsumer eventConsumer)
        {
            this.commandRedirector = commandRedirector;
            this.eventRedirector = eventRedirector;
            this.commandConsumer = commandConsumer;
            this.eventConsumer = eventConsumer;
        }

        public override void Start(object[] args)
        {
            this.commandRedirector.Subscriber.Subscribe();
            this.eventRedirector.Subscriber.Subscribe();
            this.commandConsumer.Subscriber.Subscribe();
            this.eventConsumer.Subscriber.Subscribe();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.commandRedirector.Dispose();
                    this.eventRedirector.Dispose();
                    this.commandConsumer.Dispose();
                    this.eventConsumer.Dispose();
                    this.disposed = true;
                }
            }
        }
    }
}
