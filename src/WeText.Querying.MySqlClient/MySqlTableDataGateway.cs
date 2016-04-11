using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;

namespace WeText.Querying.MySqlClient
{
    public sealed class MySqlTableDataGateway : SqlTableDataGateway
    {
        private readonly string connectionString;

        public MySqlTableDataGateway(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override DbCommand CreateCommand(string sql, DbConnection connection)
        {
            return new MySqlCommand(sql, (MySqlConnection)connection);
        }

        protected override DbConnection CreateDatabaseConnection()
        {
            return new MySqlConnection(this.connectionString);
        }

        protected override DbParameter CreateParameter()
        {
            return new MySqlParameter();
        }

        protected override WhereClauseBuilder<TTableObject> CreateWhereClauseBuilder<TTableObject>()
        {
            return new MySqlWhereClauseBuilder<TTableObject>();
        }
    }
}
