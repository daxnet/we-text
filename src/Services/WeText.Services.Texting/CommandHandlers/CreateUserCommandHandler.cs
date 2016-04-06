using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Domain.Commands;

namespace WeText.Services.Texting.CommandHandlers
{
    public class CreateUserCommandHandler : CommandHandler<CreateUserCommand>
    {
        public override Task HandleAsync(CreateUserCommand message)
        {
            return Task.CompletedTask;
        }
    }
}
