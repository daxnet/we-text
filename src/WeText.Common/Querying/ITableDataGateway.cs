using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Specifications;

namespace WeText.Common.Querying
{
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
