using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Domain;
using WeText.Common.Repositories;
using WeText.Domain.Commands;

namespace WeText.Services.Social.CommandHandlers
{
    public class AddFriendCommandHandler : CommandHandler<AddFriendCommand>
    {
        private readonly IDomainRepository domainRepository;

        public AddFriendCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(AddFriendCommand message)
        {
            var originator = await this.domainRepository.GetByKeyAsync<Guid, User>(message.OriginatorId);
            originator.AddFriend(message.AcceptorId);
            await this.domainRepository.SaveAsync<Guid, User>(originator);

            var acceptor = await this.domainRepository.GetByKeyAsync<Guid, User>(message.AcceptorId);
            acceptor.AddFriend(message.OriginatorId);
            await this.domainRepository.SaveAsync<Guid, User>(acceptor);
        }
    }
}
