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
    public class User : AggregateRoot<Guid>
    {
        public User()
        {
            ApplyEvent(new UserCreatedEvent(Guid.Empty));
        }

        public User(Guid id)
        {
            ApplyEvent(new UserCreatedEvent(id));
        }

        public User(Guid id, string name, string email)
        {
            ApplyEvent(new UserCreatedEvent(id, name, email));
        }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public string Email { get; private set; }

        public void ChangeDisplayName(string displayName)
        {
            ApplyEvent(new DisplayNameChangedEvent(this.Id, displayName));
        }

        [InlineEventHandler]
        private void HandleUserCreatedEvent(UserCreatedEvent evnt)
        {
            this.Id = (Guid)evnt.AggregateRootKey;
            this.Name = evnt.Name;
            this.Email = evnt.Email;
        }

        [InlineEventHandler]
        private void HandleChangeDisplayNameEvent(DisplayNameChangedEvent evnt)
        {
            this.DisplayName = evnt.DisplayName;
        }
    }
}
