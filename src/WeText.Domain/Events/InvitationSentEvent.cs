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
        public Guid OriginatorId { get; set; }

        public Guid TargetUserId { get; set; }

        public string OriginatorName { get; set; }

        public string TargetUserName { get; set; }

        public string InvitationLetter { get; set; }

        protected InvitationSentEvent() : base() { }

        public InvitationSentEvent(object aggregateRootKey, Guid originatorId, Guid targetUserId, string originatorName, string targetUserName, string invitationLetter)
            :base(aggregateRootKey)
        {
            this.OriginatorId = originatorId;
            this.TargetUserId = targetUserId;
            this.OriginatorName = originatorName;
            this.TargetUserName = targetUserName;
            this.InvitationLetter = invitationLetter;
        }
    }
}
