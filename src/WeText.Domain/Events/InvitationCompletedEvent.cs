using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class InvitationCompletedEvent : DomainEvent
    {
        public InvitationCompletedEvent(object aggregateRootKey, Guid originatorId, Guid targetUserId, bool accepted = false)
            : base(aggregateRootKey)
        {
            this.Accepted = accepted;
            this.OriginatorId = originatorId;
            this.TargetUserId = targetUserId;
        }

        public bool Accepted { get; set; }

        public Guid OriginatorId { get; set; }

        public Guid TargetUserId { get; set; }
    }
}
