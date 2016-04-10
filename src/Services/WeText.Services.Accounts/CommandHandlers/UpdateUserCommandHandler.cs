using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Commands;

namespace WeText.Services.Accounts.CommandHandlers
{
    public class UpdateUserCommandHandler : CommandHandler<UpdateUserCommand>
    {
        private readonly IDomainRepository domainRepository;

        public UpdateUserCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public override async Task HandleAsync(UpdateUserCommand message)
        {
            var user = await domainRepository.GetByKeyAsync<Guid, User>(message.UserId);
            bool updated = false;
            if (!string.IsNullOrEmpty(message.DisplayName) && user.DisplayName != message.DisplayName)
            {
                user.ChangeDisplayName(message.DisplayName);
                updated = true;
            }
            if (!string.IsNullOrEmpty(message.Email) && user.Email != message.Email)
            {
                user.ChangeEmail(message.Email);
                updated = true;
            }
            if (updated)
            {
                await domainRepository.SaveAsync<Guid, User>(user);
            }
        }
    }
}
