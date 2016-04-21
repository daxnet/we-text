using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Specifications;

namespace WeText.Common.Querying
{
    /// <summary>
    /// Represents that the implemented classes are Table Data Gateways that can access a relational database
    /// based on the CLR table object.
    /// </summary>
    /// <remarks>
    /// For more information about the Table Data Gateway, please refer to: http://martinfowler.com/eaaCatalog/tableDataGateway.html.
    /// In this example project, a Table Data Gateway only wraps the relational database operations based on a particular
    /// CLR table object, which means no table relations will be considered and operations across two or more tables will be in 
    /// different database transactions.
    /// </remarks>
    public interface ITableDataGateway
    {
        Task<IEnumerable<TTableObject>> SelectAsync<TTableObject>(Specification<TTableObject> specification)
            where TTableObject : class, new();

        Task InsertAsync<TTableObject>(IEnumerable<TTableObject> tableObjects)
            where TTableObject : class, new();

        Task UpdateAsync<TTableObject>(UpdateCriteria<TTableObject> updateCriteria, Specification<TTableObject> specification)
            where TTableObject : class, new();

        Task UpdateAsync<TTableObject>(IEnumerable<Tuple<UpdateCriteria<TTableObject>, Specification<TTableObject>>> batch)
            where TTableObject : class, new();
    }
}
