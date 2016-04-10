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
    public class UserDisplayNameChangedEventHandler : DomainEventHandler<UserDisplayNameChangedEvent>
    {
        private readonly ITableDataGateway gateway;

        public UserDisplayNameChangedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override async Task HandleAsync(UserDisplayNameChangedEvent message)
        {
            await Task.CompletedTask;
        }
    }
}
