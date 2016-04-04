using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common
{
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>

    {
        TKey Id { get; set; }
    }
}
