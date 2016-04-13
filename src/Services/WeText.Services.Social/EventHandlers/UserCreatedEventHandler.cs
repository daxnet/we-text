using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social.EventHandlers
{
    public class UserCreatedEventHandler : DomainEventHandler<UserCreatedEvent>
    {
        private readonly ITableDataGateway gateway;

        public UserCreatedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override async Task HandleAsync(UserCreatedEvent message)
        {
            await this.gateway.InsertAsync<UserNameTableObject>(new[] { new UserNameTableObject { UserId = message.AggregateRootKey.ToString(), DisplayName = message.DisplayName } });
        }
    }
}
