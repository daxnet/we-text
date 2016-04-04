using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Repositories
{
    /// <summary>
    /// Represents the domain repository in a CQRS architecture.
    /// </summary>
    public interface IDomainRepository
    {
        Task SaveAsync<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot, bool purge = true)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new();

        Task<TAggregateRoot> GetByKeyAsync<TKey, TAggregateRoot>(TKey key)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>, new();
    }
}
