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
        public InvitationCompletedEvent(object aggregateRootKey, Guid fromUserId, Guid toUserId)
            : base(aggregateRootKey)
        {
            this.FromUserId = fromUserId;
            this.ToUserId = toUserId;
        }

        public Guid FromUserId { get; set; }

        public Guid ToUserId { get; set; }
    }
}
