using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common
{
    public abstract class Saga<TKey, TSagaData> : AggregateRoot<TKey>
        where TKey : IEquatable<TKey>
        where TSagaData : class
    {
        public TSagaData Sate { get; private set; }
    }
}
