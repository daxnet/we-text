using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Repositories;

namespace WeText.DomainRepositories
{
    public class MongoDomainRepository : DomainRepository
    {
        private readonly WeTextMongoSetting setting = new WeTextMongoSetting();
        private readonly Lazy<MongoClient> client;
        private readonly Lazy<IMongoDatabase> database;

        public MongoDomainRepository(IBus bus) : base(bus)
        {
            this.client = new Lazy<MongoClient>(() => new MongoClient(setting.ConnectionString));
            this.database = new Lazy<IMongoDatabase>(() => client.Value.GetDatabase(setting.DatabaseName));
        }

        protected override async Task<IEnumerable<IDomainEvent>> GetDomainEventsAsync<TKey, TAggregateRoot>(TKey aggregateRootKey)
        {
            var collection = database.Value.GetCollection<IDomainEvent>(setting.CollectionName);
            var aggregateRootTypeName = typeof(TAggregateRoot).FullName;
            var builder = Builders<IDomainEvent>.Filter;
            var filter = builder.Eq(x => x.AggregateRootKey, aggregateRootKey) & 
                builder.Eq(x => x.AggregateRootType, aggregateRootTypeName);
            return await collection.Find(filter).ToListAsync();
        }

        protected override async Task SaveDomainEventsAsync(IEnumerable<IDomainEvent> events)
        {
            var collection = database.Value.GetCollection<IDomainEvent>(setting.CollectionName);
            await collection.InsertManyAsync(events);
        }
    }
}
