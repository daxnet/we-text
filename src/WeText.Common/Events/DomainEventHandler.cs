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
        public abstract Task HandleAsync(TEvent message);
    }
}
