using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class DisplayNameChangedEvent : DomainEvent
    {
        public string DisplayName { get; set; }

        public DisplayNameChangedEvent(object aggregateRootId, string displayName)
            : base(aggregateRootId)
        {
            this.DisplayName = displayName;
        }
    }
}
