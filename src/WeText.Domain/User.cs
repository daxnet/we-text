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

        public User(Guid id, string name, string password, string email, string displayName)
        {
            ApplyEvent(new UserCreatedEvent(id, name, password, email, displayName));
        }

        public string Name { get; private set; }

        public string Password { get; private set; }

        public string DisplayName { get; private set; }

        public string Email { get; private set; }

        public void ChangeDisplayName(string displayName)
        {
            ApplyEvent(new UserDisplayNameChangedEvent(this.Id, displayName));
        }

        public void ChangeEmail(string email)
        {
            ApplyEvent(new UserEmailChangedEvent(this.Id, email));
        }

        [InlineEventHandler]
        private void HandleUserCreatedEvent(UserCreatedEvent evnt)
        {
            this.Id = (Guid)evnt.AggregateRootKey;
            this.Password = evnt.Password;
            this.DisplayName = evnt.DisplayName;
            this.Name = evnt.Name;
            this.Email = evnt.Email;
        }

        [InlineEventHandler]
        private void HandleChangeDisplayNameEvent(UserDisplayNameChangedEvent evnt)
        {
            this.DisplayName = evnt.DisplayName;
        }

        [InlineEventHandler]
        private void HandleChangeEmailEvent(UserEmailChangedEvent evnt)
        {
            this.Email = evnt.Email;
        }
    }
}
