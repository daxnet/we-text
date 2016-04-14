using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Events;

namespace WeText.Services.Social.EventHandlers
{
    public class InvitationSentEventHandler : DomainEventHandler<InvitationSentEvent>
    {
        private readonly IDomainRepository repository;

        public InvitationSentEventHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public override async Task HandleAsync(InvitationSentEvent message)
        {
            var invitation = new Invitation();
            invitation.Transit(message);
            await this.repository.SaveAsync<Guid, Invitation>(invitation);
        }
    }
}
