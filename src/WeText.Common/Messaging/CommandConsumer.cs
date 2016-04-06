using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Common.Messaging
{
    /// <summary>
    /// Represents the default command consumer which will simply iterate
    /// each registered command handler and handle the command within it.
    /// </summary>
    public sealed class CommandConsumer : ICommandConsumer
    {
        private readonly IEnumerable<ICommandHandler> commandHandlers;
        private readonly IMessageSubscriber subscriber;
        private bool disposed;

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
                if (!disposed)
                {
                    this.subscriber.Dispose();
                    disposed = true;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
