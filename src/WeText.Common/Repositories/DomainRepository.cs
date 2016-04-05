using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Messaging;

namespace WeText.Common.Repositories
{
    public abstract class DomainRepository : IDomainRepository
    {
        private readonly IMessagePublisher bus;

        protected DomainRepository(IMessagePublisher bus)
        {
            this.bus = bus;
        }

        public async Task SaveAsync<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot, bool purge = true)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new()
        {
            await this.SaveDomainEventsAsync(aggregateRoot.UncommittedEvents);
            foreach(var evnt in aggregateRoot.UncommittedEvents)
            {
                bus.Publish(evnt);
            }

            if (purge)
            {
                ((IPurgeable)aggregateRoot).Purge();
            }
        }

        public async Task<TAggregateRoot> GetByKeyAsync<TKey, TAggregateRoot>(TKey key)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new()
        {
            var evnts = await this.GetDomainEventsAsync<TKey, TAggregateRoot>(key);
            var aggregateRoot = new TAggregateRoot();
            aggregateRoot.Id = key;
            aggregateRoot.Replay(evnts);
            return aggregateRoot;
        }

        protected abstract Task SaveDomainEventsAsync(IEnumerable<IDomainEvent> events);

        protected abstract Task<IEnumerable<IDomainEvent>> GetDomainEventsAsync<TKey, TAggregateRoot>(TKey aggregateRootKey)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new();
    }
}
