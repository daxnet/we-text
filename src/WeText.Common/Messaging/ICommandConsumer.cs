using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Common.Messaging
{
    /// <summary>
    /// Represents the consumer that will subscribe to the message bus and
    /// consume the messages (commands) by its registered command handlers.
    /// </summary>
    public interface ICommandConsumer : IMessageConsumer
    {
        IEnumerable<ICommandHandler> CommandHandlers { get; }
    }
}
