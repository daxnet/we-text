using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;
using WeText.Services.Accounts.Querying;

namespace WeText.Services.Accounts.EventHandlers
{
    public class UserEmailChangedEventHandler : DomainEventHandler<UserEmailChangedEvent>
    {
        private readonly ITableDataGateway gateway;

        public UserEmailChangedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override async Task HandleAsync(UserEmailChangedEvent message)
        {
            var accountId = message.AggregateRootKey.ToString();
            var updateCriteria = new UpdateCriteria<AccountTableObject> { { x => x.Email, message.Email } };
            Expression<Func<AccountTableObject, bool>> updateSpecification = x => x.Id == accountId;
            await gateway.UpdateAsync<AccountTableObject>(updateCriteria, updateSpecification);
        }
    }
}
