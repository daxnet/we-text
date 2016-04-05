using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeText.Common.Messaging
{
    public interface IMessagePublisher : IDisposable
    {
        void Publish<TMessage>(TMessage message);
    }
}
