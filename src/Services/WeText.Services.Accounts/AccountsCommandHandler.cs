using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Commands;

namespace WeText.Services.Accounts
{
    internal sealed class AccountsCommandHandler :
        ICommandHandler<CreateUserCommand>,
        ICommandHandler<UpdateUserCommand>
    {
        private readonly IDomainRepository domainRepository;

        public AccountsCommandHandler(IDomainRepository domainRepository)
        {
            this.domainRepository = domainRepository;
        }

        public async Task HandleAsync(UpdateUserCommand message)
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

        public async Task HandleAsync(CreateUserCommand message)
        {
            var user = new User(message.UserId, message.Name, message.Password, message.Email, message.DisplayName);
            await this.domainRepository.SaveAsync<Guid, User>(user);
        }
    }
}
