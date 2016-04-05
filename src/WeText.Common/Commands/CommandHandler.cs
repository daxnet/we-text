using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Commands
{
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        public Task HandleAsync(object message)
        {
            var command = message as TCommand;
            if (command != null)
            {
                return HandleAsync(command);
            }
            else
            {
                return Task.CompletedTask; // Returns with doing nothing.
            }
        }

        public abstract Task HandleAsync(TCommand message);
    }
}
