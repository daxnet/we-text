using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Repositories;

namespace WeText.DomainRepositories
{
    public class InMemoryDomainRepository : DomainRepository
    {
        public InMemoryDomainRepository(IBus bus)
            : base(bus)
        { }

        private readonly List<IDomainEvent> events = new List<IDomainEvent>();

        protected override Task<IEnumerable<IDomainEvent>> GetDomainEventsAsync<TKey, TAggregateRoot>(TKey aggregateRootKey)
        {
            return Task.FromResult(events.Where(x => x.AggregateRootKey.Equals(aggregateRootKey) && x.AggregateRootType == typeof(TAggregateRoot).FullName));
        }

        protected override Task SaveDomainEventsAsync(IEnumerable<IDomainEvent> events)
        {
            return Task.Run(() => this.events.AddRange(events));
        }

        public IList<IDomainEvent> Events => events;
    }
}
