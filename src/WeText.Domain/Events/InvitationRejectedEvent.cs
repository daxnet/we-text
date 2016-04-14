using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class InvitationRejectedEvent : DomainEvent
    {
        public Guid CollaborationId { get; set; }

        public InvitationRejectedEvent(object aggregateRootKey) : base(aggregateRootKey)
        { }

        public InvitationRejectedEvent(object aggregateRootKey, Guid collaborationId)
            : base(aggregateRootKey)
        {
            this.CollaborationId = collaborationId;
        }
    }
}
