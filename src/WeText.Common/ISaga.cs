using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Common
{
    public interface ISaga<TKey, TMessage> : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
        where TMessage : IDomainEvent
    {
        void Transit(TMessage message);
    }
}
