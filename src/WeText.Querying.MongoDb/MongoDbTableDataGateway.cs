using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;
using WeText.Common.Specifications;

namespace WeText.Querying.MongoDb
{
    public class MongoDbTableDataGateway : ITableDataGateway
    {
        private readonly string connectionString;
        private readonly string databaseName;
        private readonly string collectionName;

        public MongoDbTableDataGateway(string connectionString, string databaseName, string collectionName)
        {
            this.connectionString = connectionString;
            this.databaseName = databaseName;
            this.collectionName = collectionName;
        }

        public Task InsertAsync<TTableObject>(IEnumerable<TTableObject> tableObjects) where TTableObject : class, new()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TTableObject>> SelectAsync<TTableObject>(Specification<TTableObject> specification) where TTableObject : class, new()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync<TTableObject>(UpdateCriteria<TTableObject> updateCriteria, Specification<TTableObject> specification) where TTableObject : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
