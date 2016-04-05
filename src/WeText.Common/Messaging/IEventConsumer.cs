using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Common.Messaging
{
    public interface IEventConsumer : IMessageConsumer
    {
        IEnumerable<IDomainEventHandler> EventHandlers { get; }
    }
}
