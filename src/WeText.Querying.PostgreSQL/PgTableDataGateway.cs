using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;

namespace WeText.Querying.PostgreSQL
{
    public sealed class PgTableDataGateway : TableDataGateway
    {
        private readonly string connectionString;

        public PgTableDataGateway(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override DbCommand CreateCommand(string sql, DbConnection connection) => new NpgsqlCommand(sql, (NpgsqlConnection)connection);

        protected override DbConnection CreateDatabaseConnection() => new NpgsqlConnection(this.connectionString);

        protected override DbParameter CreateParameter() => new NpgsqlParameter();

        protected override WhereClauseBuilder<TTableObject> CreateWhereClauseBuilder<TTableObject>() => new PgWhereClauseBuilder<TTableObject>();
    }
}
