using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class InvitationApprovedEvent : DomainEvent
    {
        public Guid CollaborationId { get; set; }

        public Guid OriginatorId { get; set; }

        public Guid ApproverId { get; set; }

        public InvitationApprovedEvent(object aggregateRootKey) : base(aggregateRootKey)
        { }

        public InvitationApprovedEvent(object aggregateRootKey, Guid collaborationId)
            : base(aggregateRootKey)
        {
            this.CollaborationId = collaborationId;
        }
    }
}
