using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Commands;

namespace WeText.Services.Social
{
    internal sealed class SocialCommandHandler :
        ICommandHandler<SendInvitationCommand>, 
        ICommandHandler<AcceptInvitationCommand>,
        ICommandHandler<RejectInvitationCommand>,
        ICommandHandler<AddFriendCommand>
    {

        private readonly IDomainRepository domainRepository;

        public SocialCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }


        public async Task HandleAsync(RejectInvitationCommand message)
        {
            var user = await this.domainRepository.GetByKeyAsync<Guid, User>(message.UserId);
            var invitation = await this.domainRepository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            user.RejectInvitation(invitation);
            await this.domainRepository.SaveAsync<Guid, User>(user);
        }

        public async Task HandleAsync(AddFriendCommand message)
        {
            var originator = await this.domainRepository.GetByKeyAsync<Guid, User>(message.OriginatorId);
            originator.AddFriend(message.AcceptorId);
            await this.domainRepository.SaveAsync<Guid, User>(originator);

            var acceptor = await this.domainRepository.GetByKeyAsync<Guid, User>(message.AcceptorId);
            acceptor.AddFriend(message.OriginatorId);
            await this.domainRepository.SaveAsync<Guid, User>(acceptor);
        }

        public async Task HandleAsync(AcceptInvitationCommand message)
        {
            var invitation = await this.domainRepository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            var user = await this.domainRepository.GetByKeyAsync<Guid, User>(message.UserId);
            user.ApproveInvitation(invitation);
            await this.domainRepository.SaveAsync<Guid, User>(user);
        }

        public async Task HandleAsync(SendInvitationCommand message)
        {
            var originator = await this.domainRepository.GetByKeyAsync<Guid, User>(message.OriginatorId);
            var targetUser = await this.domainRepository.GetByKeyAsync<Guid, User>(message.TargetUserId);

            originator.SendInvitation(targetUser, message.InvitationLetter);
            await this.domainRepository.SaveAsync<Guid, User>(originator);
        }
    }
}
