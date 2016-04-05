using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Events
{
    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        public Task HandleAsync(object message)
        {
            var domainEvent = message as TEvent;
            if (domainEvent != null)
            {
                return HandleAsync(domainEvent);
            }
            else
            {
                return Task.CompletedTask; // Returns with doing nothing.
            }
        }

        public abstract Task HandleAsync(TEvent message);
    }
}
