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
        public Guid InvitationId { get; set; }

        public Guid OriginatorId { get; set; }

        public Guid RejectorId { get; set; }

        protected InvitationRejectedEvent() : base() { }

        public InvitationRejectedEvent(object aggregateRootKey) : base(aggregateRootKey)
        { }

        public InvitationRejectedEvent(object aggregateRootKey, Guid invitationId, Guid originatorId, Guid rejectorId)
            : base(aggregateRootKey)
        {
            this.InvitationId = invitationId;
            this.OriginatorId = originatorId;
            this.RejectorId = rejectorId;
        }
    }
}
