using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Commands;
using WeText.Domain.Events;

namespace WeText.Services.Social.EventHandlers
{
    public class InvitationApprovedEventHandler : DomainEventHandler<InvitationApprovedEvent>
    {
        private readonly IDomainRepository repository;
        private readonly ICommandSender commandSender;

        public InvitationApprovedEventHandler(IDomainRepository repository, ICommandSender commandSender)
        {
            this.repository = repository;
            this.commandSender = commandSender;
        }

        public override async Task HandleAsync(InvitationApprovedEvent message)
        {
            var invitation = await repository.GetByKeyAsync<Guid, Invitation>(message.CollaborationId);
            invitation.Transit(message);
            await this.repository.SaveAsync<Guid, Invitation>(invitation);

            // Once the application get approved, send the add friend command.
            var command = new AddFriendCommand
            {
                AcceptorId = message.ApproverId,
                OriginatorId = message.OriginatorId
            };

            this.commandSender.Publish(command);
        }
    }
}
