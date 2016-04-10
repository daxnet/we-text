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
        private readonly IMessagePublisher messagePublisher;

        protected DomainRepository(IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
        }

        public async Task SaveAsync<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot, bool purge = true)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new()
        {
            // When doing a CQRS architecture with Event Sourcing (ES), this step should be going
            // to save the events occurred within the aggregate. In this example, we simply save
            // the entire aggregate root to avoid handling the snapshots.

            await this.SaveAggregateAsync<TKey, TAggregateRoot>(aggregateRoot);
            foreach(var evnt in aggregateRoot.UncommittedEvents)
            {
                messagePublisher.Publish(evnt);
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
            var result = await this.GetAggregateAsync<TKey, TAggregateRoot>(key);
            ((IPurgeable)result).Purge();
            return result;
        }

        protected abstract Task SaveAggregateAsync<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new();

        protected abstract Task<TAggregateRoot> GetAggregateAsync<TKey, TAggregateRoot>(TKey aggregateRootKey)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new();
    }
}
