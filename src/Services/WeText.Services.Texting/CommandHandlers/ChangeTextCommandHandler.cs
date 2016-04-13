using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Commands;

namespace WeText.Services.Texting.CommandHandlers
{
    public class ChangeTextCommandHandler : CommandHandler<ChangeTextCommand>
    {
        private IDomainRepository domainRepository;

        public ChangeTextCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(ChangeTextCommand message)
        {
            var text = await this.domainRepository.GetByKeyAsync<Guid, Text>(message.TextId);
            bool updated = false; 
            if (!string.IsNullOrEmpty(message.Title) && text.Title != message.Title)
            {
                text.ChangeTitle(message.Title);
                updated = true;
            }
            if (!string.IsNullOrEmpty(message.Content) && text.Content != message.Content)
            {
                text.ChangeContent(message.Content);
                updated = true;
            }
            // In Event Sourcing context, if nothing has changed, then no event will 
            // be raised, at that point, there is no need to use a flag like 'updated'
            // to check if the aggregate root has been changed.
            if (updated)
            {
                await this.domainRepository.SaveAsync<Guid, Text>(text);
            }
        }
    }
}
