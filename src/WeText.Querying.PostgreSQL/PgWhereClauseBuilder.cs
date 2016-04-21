using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Querying;

namespace WeText.Querying.PostgreSQL
{
    internal sealed class PgWhereClauseBuilder<TTableObject> : WhereClauseBuilder<TTableObject>
        where TTableObject : class, new()
    {
        protected override char ParameterChar => '@';
    }
}
