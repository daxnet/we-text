using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;
using WeText.Services.Accounts.Querying;

namespace WeText.Services.Accounts.EventHandlers
{
    internal sealed class UserCreatedEventHandler : DomainEventHandler<UserCreatedEvent>
    {
        private readonly ITableDataGateway gateway;

        public UserCreatedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override async Task HandleAsync(UserCreatedEvent message)
        {
            var userTableObject = new UserTableObject
            {
                Id = message.AggregateRootKey.ToString(),
                Name = message.Name,
                Email = message.Email
            };
            await this.gateway.InsertAsync<UserTableObject>(new[] { userTableObject });
        }
    }
}
