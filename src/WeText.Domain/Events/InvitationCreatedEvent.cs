using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class InvitationCreatedEvent : DomainEvent
    {
        public InvitationCreatedEvent(object aggregateRootKey) : base(aggregateRootKey) { }
    }
}
