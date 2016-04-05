using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Common.Messaging
{
    public class CommandConsumer : ICommandConsumer
    {
        private readonly IEnumerable<ICommandHandler> commandHandlers;
        private readonly IMessageSubscriber subscriber;

        ~CommandConsumer()
        {
            Dispose(false);
        }

        public CommandConsumer(IMessageSubscriber subscriber, IEnumerable<ICommandHandler> commandHandlers)
        {
            this.subscriber = subscriber;
            this.commandHandlers = commandHandlers;
            subscriber.MessageReceived += async (sender, e) =>
              {
                  if (this.commandHandlers != null)
                  {
                      foreach (var handler in this.commandHandlers)
                      {
                          await handler.HandleAsync(e.Message);
                      }
                  }
              };
        }

        public IEnumerable<ICommandHandler> CommandHandlers => commandHandlers;

        public IMessageSubscriber Subscriber => subscriber;

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.subscriber.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
