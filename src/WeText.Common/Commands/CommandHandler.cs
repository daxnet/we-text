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
        public abstract Task HandleAsync(TCommand message);
    }
}
