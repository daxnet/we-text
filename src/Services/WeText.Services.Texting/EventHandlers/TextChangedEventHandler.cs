using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;
using WeText.Services.Texting.Querying;

namespace WeText.Services.Texting.EventHandlers
{
    public class TextChangedEventHandler : DomainEventHandler<TextChangedEvent>
    {
        private readonly ITableDataGateway gateway;

        public TextChangedEventHandler(ITableDataGateway gateway)
        {
            this.gateway = gateway;
        }

        public override async Task HandleAsync(TextChangedEvent message)
        {
            var id = message.AggregateRootKey.ToString();
            var updateCriteria = new UpdateCriteria<TextTableObject>();
            if (!string.IsNullOrEmpty(message.Title))
            {
                updateCriteria.Add(x => x.Title, message.Title);
            }
            if (!string.IsNullOrEmpty(message.Content))
            {
                updateCriteria.Add(x => x.Content, message.Content);
            }

            if (updateCriteria.Count == 0)
            {
                return;
            }

            Expression<Func<TextTableObject, bool>> updateSpecification = x => x.Id == id;
            await gateway.UpdateAsync<TextTableObject>(updateCriteria, updateSpecification);
        }
    }
}
