using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Common.Messaging
{
    public sealed class EventConsumer : DisposableObject, IEventConsumer
    {
        private readonly IEnumerable<IDomainEventHandler> eventHandlers;
        private readonly IMessageSubscriber subscriber;
        private bool disposed;

        public EventConsumer(IMessageSubscriber subscriber, IEnumerable<IDomainEventHandler> eventHandlers)
        {
            this.subscriber = subscriber;
            this.eventHandlers = eventHandlers;
            subscriber.MessageReceived += async (sender, e) =>
            {
                if (this.eventHandlers != null)
                {
                    foreach (var handler in this.eventHandlers)
                    {
                        //await handler.HandleAsync(e.Message);
                        var handlerType = handler.GetType();
                        var messageType = e.Message.GetType();
                        var methodInfoQuery = from method in handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                              let parameters = method.GetParameters()
                                              where method.Name == "HandleAsync" &&
                                              method.ReturnType == typeof(Task) &&
                                              parameters.Length == 1 &&
                                              parameters[0].ParameterType == messageType
                                              select method;
                        var methodInfo = methodInfoQuery.FirstOrDefault();
                        if (methodInfo != null)
                        {
                            await (Task)methodInfo.Invoke(handler, new[] { e.Message });
                        }
                    }
                }
            };
        }

        public IEnumerable<IDomainEventHandler> EventHandlers => eventHandlers;

        public IMessageSubscriber Subscriber => subscriber;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    this.subscriber.Dispose();
                    disposed = true;
                }
            }
        }
    }
}
