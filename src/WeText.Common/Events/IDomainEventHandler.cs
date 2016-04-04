using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Repositories;

namespace WeText.Common.Events
{
    public interface IDomainEventHandler<TEvent> : IHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
    }
}
