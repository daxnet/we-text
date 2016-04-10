using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class UserEmailChangedEvent : DomainEvent
    {
        public string Email { get; set; }

        public UserEmailChangedEvent(object aggregateRootId, string email)
            :base(aggregateRootId)
        {
            this.Email = email;
        }

    }
}
