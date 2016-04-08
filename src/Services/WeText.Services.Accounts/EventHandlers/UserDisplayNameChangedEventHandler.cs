using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Domain.Events;

namespace WeText.Services.Accounts.EventHandlers
{
    public class UserDisplayNameChangedEventHandler : DomainEventHandler<DisplayNameChangedEvent>
    {
        public override async Task HandleAsync(DisplayNameChangedEvent message)
        {
            await Task.CompletedTask;
        }
    }
}
