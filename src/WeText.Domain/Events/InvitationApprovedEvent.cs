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
        public Guid InvitationId { get; set; }

        public Guid OriginatorId { get; set; }

        public Guid ApproverId { get; set; }

        protected InvitationApprovedEvent() : base() { }

        public InvitationApprovedEvent(object aggregateRootKey) : base(aggregateRootKey)
        { }

        public InvitationApprovedEvent(object aggregateRootKey, Guid invitationId, Guid originatorId, Guid approverId)
            : base(aggregateRootKey)
        {
            this.InvitationId = invitationId;
            this.OriginatorId = originatorId;
            this.ApproverId = approverId;
        }
    }
}
