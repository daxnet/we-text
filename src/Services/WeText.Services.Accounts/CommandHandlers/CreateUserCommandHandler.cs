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
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
    {
        private readonly IDomainRepository repository;

        public CreateUserCommandHandler(IDomainRepository repository)
        {
            this.repository = repository;
        }

        public override async Task HandleAsync(CreateUserCommand message)
        {
            var user = new User(Guid.NewGuid(), message.Name, message.Email);
            user.ChangeDisplayName(message.DisplayName);
            await this.repository.SaveAsync<Guid, User>(user);
        }
    }
}
