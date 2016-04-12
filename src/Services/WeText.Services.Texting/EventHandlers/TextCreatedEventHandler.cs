using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;
using WeText.Services.Texting.Querying;

namespace WeText.Services.Texting.EventHandlers
{
    public class TextCreatedEventHandler : DomainEventHandler<TextCreatedEvent>
    {
        private readonly ITableDataGateway gateway;

        public TextCreatedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override async Task HandleAsync(TextCreatedEvent message)
        {
            var textObject = new TextTableObject
            {
                Id = message.AggregateRootKey.ToString(),
                Title = message.Title,
                Content = message.Content,
                UserId = message.UserId.ToString(),
                DateCreated = message.Timestamp
            };

            await this.gateway.InsertAsync<TextTableObject>(new[] { textObject });
        }
    }
}
