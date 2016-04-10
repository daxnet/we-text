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
    public class UserEmailChangedEventHandler : DomainEventHandler<UserEmailChangedEvent>
    {
        private readonly ITableDataGateway gateway;

        public UserEmailChangedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override Task HandleAsync(UserEmailChangedEvent message)
        {
            return Task.CompletedTask;
        }
    }
}
