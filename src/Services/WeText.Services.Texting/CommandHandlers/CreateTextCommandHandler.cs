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
    public class CreateTextCommandHandler : CommandHandler<CreateTextCommand>
    {
        private IDomainRepository domainRepository;

        public CreateTextCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(CreateTextCommand message)
        {
            var text = new Text(message.TextId, message.Title, message.Content, message.UserId);
            await this.domainRepository.SaveAsync<Guid, Text>(text);
        }
    }
}
