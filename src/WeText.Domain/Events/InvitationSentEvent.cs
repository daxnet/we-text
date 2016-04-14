using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class InvitationSentEvent : DomainEvent
    {
        public Guid ToUserId { get; set; }
        public string InvitationLetter { get; set; }

        public InvitationSentEvent(object aggregateRootKey, Guid toUserId, string invitationLetter)
            :base(aggregateRootKey)
        {
            this.ToUserId = toUserId;
            this.InvitationLetter = invitationLetter;
        }
    }
}
