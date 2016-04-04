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
        public string DisplayName { get; private set; }

        public DisplayNameChangedEvent(string displayName)
        {
            this.DisplayName = displayName;
        }
    }
}
