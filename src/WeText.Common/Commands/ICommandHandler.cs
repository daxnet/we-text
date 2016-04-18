using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Repositories;

namespace WeText.Common.Commands
{
    public interface ICommandHandler
    {

    }

    public interface ICommandHandler<TCommand> : IHandler<TCommand>, ICommandHandler
        where TCommand : class, ICommand
    {
    }
}
