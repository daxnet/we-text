using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;

namespace WeText.Common.Messaging
{
    public sealed class MessageRedirectingConsumer : DisposableObject, IMessageConsumer
    {
        private readonly IMessageSubscriber subscriber;
        private readonly IMessagePublisher publisher;
        private bool disposed;


        public MessageRedirectingConsumer(IMessageSubscriber subscriber, IMessagePublisher publisher)
        {
            this.subscriber = subscriber;
            this.publisher = publisher;
            this.subscriber.MessageReceived += (sender, e) =>
              {
                  publisher.Publish(e.Message);
              };
        }

        public IMessageSubscriber Subscriber => subscriber;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.subscriber.Dispose();
                    this.publisher.Dispose();
                    disposed = true;
                }
            }
        }
    }
}
