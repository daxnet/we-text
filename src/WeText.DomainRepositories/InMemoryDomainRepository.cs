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
        public InMemoryDomainRepository(IMessagePublisher bus)
            : base(bus)
        { }

        private readonly List<object> aggregates = new List<object>();

        protected override Task<TAggregateRoot> GetAggregateAsync<TKey, TAggregateRoot>(TKey aggregateRootKey)
        {
            var query = from obj in this.aggregates
                        let ar = obj as TAggregateRoot
                        where ar != null &&
                        ar.Id.Equals(aggregateRootKey)
                        select obj;
            return Task.FromResult(query.FirstOrDefault() as TAggregateRoot);
        }

        protected override Task SaveAggregateAsync<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            return Task.Run(()=>this.aggregates.Add(aggregateRoot));
        }
        
    }
}
