using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Events
{
    /// <summary>
    /// Represents the event raised in the CQRS domain.
    /// </summary>
    public interface IDomainEvent
    {
        Guid Id { get; }

        object AggregateRootKey { get; set; }

        string AggregateRootType { get; set; }

        DateTime Timestamp { get; }
    }
}
