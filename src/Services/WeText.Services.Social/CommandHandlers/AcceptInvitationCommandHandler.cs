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
    public class AcceptInvitationCommandHandler : CommandHandler<AcceptInvitationCommand>
    {
        private readonly IDomainRepository domainRepository;

        public AcceptInvitationCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(AcceptInvitationCommand message)
        {
            var invitation = await this.domainRepository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            var user = await this.domainRepository.GetByKeyAsync<Guid, User>(message.UserId);
            user.ApproveInvitation(invitation);
            await this.domainRepository.SaveAsync<Guid, User>(user);
        }
    }
}
