using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
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

        public MongoDomainRepository(IMessagePublisher bus) : base(bus)
        {
            this.client = new Lazy<MongoClient>(() => new MongoClient(setting.ConnectionString));
            this.database = new Lazy<IMongoDatabase>(() => client.Value.GetDatabase(setting.DatabaseName));
        }

        private async Task<TAggregateRoot> GetAggregateAsync<TKey, TAggregateRoot>(FilterDefinition<TAggregateRoot> filter)
            where TKey : IEquatable<TKey>
            where TAggregateRoot : IAggregateRoot<TKey>
        {
            var collection = database.Value.GetCollection<TAggregateRoot>(setting.AggregatesCollectionName);
            return await collection.Find<TAggregateRoot>(filter).FirstOrDefaultAsync();
        }

        protected override async Task<TAggregateRoot> GetAggregateAsync<TKey, TAggregateRoot>(TKey aggregateRootKey)
        {
            var builder = Builders<TAggregateRoot>.Filter;
            var filter = builder.Eq(x => x.Id, aggregateRootKey);
            return await GetAggregateAsync<TKey, TAggregateRoot>(filter);
        }

        protected override async Task SaveAggregateAsync<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot)
        {
            var collection = database.Value.GetCollection<TAggregateRoot>(setting.AggregatesCollectionName);
            var builder = Builders<TAggregateRoot>.Filter;
            var filter = builder.Eq(x => x.Id, aggregateRoot.Id);
            var saved = await GetAggregateAsync<TKey, TAggregateRoot>(filter);
            if (saved!=null)
            {
                await collection.ReplaceOneAsync(filter, aggregateRoot);
            }
            else
            {
                await collection.InsertOneAsync(aggregateRoot);
            }
        }
    }
}
