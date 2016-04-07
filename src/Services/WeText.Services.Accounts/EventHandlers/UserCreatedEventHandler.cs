using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;

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
            await Task.CompletedTask;
        }
    }
}
