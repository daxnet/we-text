using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Commands;

namespace WeText.Services.Social.CommandHandlers
{
    public class SendInvitationCommandHandler : CommandHandler<SendInvitationCommand>
    {
        private readonly IDomainRepository domainRepository;

        public SendInvitationCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(SendInvitationCommand message)
        {
            var originator = await this.domainRepository.GetByKeyAsync<Guid, User>(message.OriginatorId);
            var targetUser = await this.domainRepository.GetByKeyAsync<Guid, User>(message.TargetUserId);

            originator.SendInvitation(targetUser, message.InvitationLetter);
            await this.domainRepository.SaveAsync<Guid, User>(originator);
        }
    }
}
