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
    public class InvitationApprovedEventHandler : DomainEventHandler<InvitationApprovedEvent>
    {
        private readonly IDomainRepository repository;

        public InvitationApprovedEventHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public override async Task HandleAsync(InvitationApprovedEvent message)
        {
            var invitation = await repository.GetByKeyAsync<Guid, Invitation>(message.CollaborationId);
            invitation.Transit(message);
            await this.repository.SaveAsync<Guid, Invitation>(invitation);
        }
    }
}
