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
    public class RejectInvitationCommandHandler : CommandHandler<RejectInvitationCommand>
    {
        private readonly IDomainRepository domainRepository;

        public RejectInvitationCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(RejectInvitationCommand message)
        {
            var user = await this.domainRepository.GetByKeyAsync<Guid, User>(message.UserId);
            var invitation = await this.domainRepository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            user.RejectInvitation(invitation);
            await this.domainRepository.SaveAsync<Guid, User>(user);
        }
    }
}
