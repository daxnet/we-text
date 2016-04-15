using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
using WeText.Common.Events;
using WeText.Domain.Events;

namespace WeText.Domain
{
    public class Invitation : AggregateRoot<Guid>,
        ISaga<Guid, InvitationSentEvent>,
        ISaga<Guid, InvitationApprovedEvent>,
        ISaga<Guid, InvitationRejectedEvent>
    {
        public Invitation()
        {
            ApplyEvent(new InvitationCreatedEvent(Guid.NewGuid()));
        }

        public Invitation(Guid id)
        {
            ApplyEvent(new InvitationCreatedEvent(id));
        }

        public bool IsCompleted { get; private set; }

        public bool Sent { get; private set; }

        public bool Approved { get; private set; }

        public bool Rejected { get; private set; }

        public Guid OriginatorId { get; private set; }

        public Guid TargetUserId { get; private set; }

        public string InvitationLetter { get; private set; }

        public void Transit(InvitationRejectedEvent message)
        {
            ApplyEvent(new InvitationRejectedTransitionEvent());
        }

        public void Transit(InvitationApprovedEvent message)
        {
            ApplyEvent(new InvitationApprovedTransitionEvent());
        }

        public void Transit(InvitationSentEvent message)
        {
            ApplyEvent(new InvitationSentTransitionEvent(this.Id, message.OriginatorId, message.TargetUserId, message.InvitationLetter));
        }

        public void MarkCompletedIfNeeded()
        {
            if (this.Sent && (this.Approved || this.Rejected))
            {
                ApplyEvent(new InvitationCompletedEvent(this.Id, this.OriginatorId, this.TargetUserId, this.Approved));
            }
        }

        [InlineEventHandler]
        private void Handle(InvitationCreatedEvent evnt)
        {
            this.Id = (Guid)evnt.AggregateRootKey;
        }

        [InlineEventHandler]
        private void Handle(InvitationRejectedTransitionEvent evnt)
        {
            this.Rejected = true;
            MarkCompletedIfNeeded();
        }

        [InlineEventHandler]
        private void Handle(InvitationApprovedTransitionEvent evnt)
        {
            this.Approved = true;
            MarkCompletedIfNeeded();
        }

        [InlineEventHandler]
        private void Handle(InvitationSentTransitionEvent evnt)
        {
            this.Sent = true;
            this.OriginatorId = evnt.OriginatorId;
            this.TargetUserId = evnt.TargetUserId;
            this.InvitationLetter = evnt.InvitationLetter;

            MarkCompletedIfNeeded();
        }

        [InlineEventHandler]
        private void Handle(InvitationCompletedEvent evnt)
        {
            this.IsCompleted = true;
        }
    }

    #region Transition Events
    public class InvitationSentTransitionEvent : DomainEvent
    {
        public Guid OriginatorId { get; set; }

        public Guid TargetUserId { get; set; }

        public string InvitationLetter { get; set; }

        public InvitationSentTransitionEvent(object aggregateRootKey, Guid originatorId, Guid targetUserId, string invitationLetter)
            :base(aggregateRootKey)
        {
            this.OriginatorId = originatorId;
            this.TargetUserId = targetUserId;
            this.InvitationLetter = invitationLetter;
        }
    }

    public class InvitationApprovedTransitionEvent : DomainEvent
    {
        
    }

    public class InvitationRejectedTransitionEvent : DomainEvent
    {
    }
    #endregion
}
