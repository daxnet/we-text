using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common
{
    /// <summary>
    /// Represents that the implemented classes are message handlers.
    /// </summary>
    public interface IHandler
    {
        Task HandleAsync(object message);
    }

    public interface IHandler<in T> : IHandler
    {
        Task HandleAsync(T message);
    }
}
