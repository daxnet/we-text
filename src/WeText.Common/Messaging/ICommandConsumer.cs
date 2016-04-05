using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;

namespace WeText.Common.Messaging
{
    public interface ICommandConsumer : IMessageConsumer
    {
        IEnumerable<ICommandHandler> CommandHandlers { get; }
    }
}
