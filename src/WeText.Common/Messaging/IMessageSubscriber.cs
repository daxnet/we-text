using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Messaging
{
    /// <summary>
    /// Represents that the implemented classes can subscribe to a message bus and trigger the event
    /// where there is any incoming message.
    /// </summary>
    /// <remarks>This class will not be responsible for classifying the message by its topic.</remarks>
    public interface IMessageSubscriber : IDisposable
    {
        /// <summary>
        /// Subscribe to the message bus.
        /// </summary>
        void Subscribe();

        /// <summary>
        /// Represents the event that occurs when there is any incoming message.
        /// </summary>
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}
