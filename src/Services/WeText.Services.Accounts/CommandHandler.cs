using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Commands;
using WeText.Common.Commands;
using WeText.Common.Repositories;
using WeText.Domain;

namespace WeText.Services.Accounts
{
    public class CommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IDomainRepository repository;

        public CommandHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public async Task HandleAsync(CreateUserCommand message)
        {
            var user = new User(Guid.NewGuid(), message.Name, message.Email);
            user.ChangeDisplayName(message.DisplayName);
            await this.repository.SaveAsync<Guid, User>(user);
        }
    }
}
