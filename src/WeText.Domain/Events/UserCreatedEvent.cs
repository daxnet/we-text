using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }

        protected UserCreatedEvent() : base() { }

        public UserCreatedEvent(object aggregateRootKey) : base(aggregateRootKey) { }

        public UserCreatedEvent(object aggregateRootKey, string name, string password, string email, string displayName) : base(aggregateRootKey)
        {
            this.Name = name;
            this.Password = password;
            this.DisplayName = displayName;
            this.Email = email;
        }
    }
}
