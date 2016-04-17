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
    public class InvitationRejectedEventHandler : DomainEventHandler<InvitationRejectedEvent>
    {
        private readonly IDomainRepository repository;

        public InvitationRejectedEventHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public override async Task HandleAsync(InvitationRejectedEvent message)
        {
            var invitation = await repository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            invitation.Transit(message);
            await this.repository.SaveAsync<Guid, Invitation>(invitation);
        }
    }
}
