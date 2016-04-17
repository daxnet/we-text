using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class FriendAddedEvent : DomainEvent
    {
        public Guid FriendUserId { get; set; }

        protected FriendAddedEvent() : base() { }

        public FriendAddedEvent(object aggregateRootId, Guid friendUserId)
            :base(aggregateRootId)
        {
            this.FriendUserId = friendUserId;
        }
    }
}
