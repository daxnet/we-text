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

        public Guid FromUserId { get; private set; }

        public Guid ToUserId { get; private set; }

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
            ApplyEvent(new InvitationSentTransitionEvent(message.AggregateRootKey, message.ToUserId, message.InvitationLetter));
        }

        public void MarkCompletedIfNeeded()
        {
            if (this.Sent && (this.Approved || this.Rejected))
            {
                ApplyEvent(new InvitationCompletedEvent(this.Id, this.FromUserId, this.ToUserId));
            }
        }

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
        private void Handle(InvitationApprovedEvent evnt)
        {
            this.Approved = true;
            MarkCompletedIfNeeded();
        }

        [InlineEventHandler]
        private void Handle(InvitationSentTransitionEvent evnt)
        {
            this.Sent = true;
            this.FromUserId = (Guid)evnt.AggregateRootKey;
            this.ToUserId = evnt.ToUserId;
            this.InvitationLetter = evnt.InvitationLetter;

            MarkCompletedIfNeeded();
        }

        [InlineEventHandler]
        private void Handle(InvitationCompletedEvent evnt)
        {
            this.IsCompleted = true;
        }
    }

    public class InvitationSentTransitionEvent : DomainEvent
    {
        public Guid ToUserId { get; set; }
        public string InvitationLetter { get; set; }

        public InvitationSentTransitionEvent(object aggregateRootKey, Guid toUserId, string invitationLetter)
            :base(aggregateRootKey)
        {
            this.ToUserId = toUserId;
            this.InvitationLetter = invitationLetter;
        }
    }

    public class InvitationApprovedTransitionEvent : DomainEvent
    {
        
    }

    public class InvitationRejectedTransitionEvent : DomainEvent
    {
    }
}
