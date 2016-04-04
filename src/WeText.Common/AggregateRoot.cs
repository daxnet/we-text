using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Common
{
    public abstract class AggregateRoot<TKey> : IAggregateRoot<TKey>, IPurgeable
        where TKey : IEquatable<TKey>
    {
        private readonly Queue<IDomainEvent> uncommittedEvents = new Queue<IDomainEvent>();

        public TKey Id { get; set; }

        public IEnumerable<IDomainEvent> UncommittedEvents => uncommittedEvents;

        public void Replay(IEnumerable<IDomainEvent> events)
        {
            ((IPurgeable)this).Purge();
            foreach(var evnt in events)
            {
                this.ApplyEvent(evnt);
            }
        }

        protected void ApplyEvent<TEvent>(TEvent evnt) where TEvent : IDomainEvent
        {
            var eventHandlerMethods = from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                      let parameters = m.GetParameters()
                                      where m.IsDefined(typeof(InlineEventHandlerAttribute)) &&
                                      m.ReturnType == typeof(void) &&
                                      parameters.Length == 1 &&
                                      parameters[0].ParameterType == evnt.GetType()
                                      select m;

            evnt.AggregateRootType = this.GetType().FullName;

            foreach(var eventHandlerMethod in eventHandlerMethods)
            {
                eventHandlerMethod.Invoke(this, new object[] { evnt });
            }

            this.uncommittedEvents.Enqueue(evnt);
        }

        void IPurgeable.Purge()
        {
            if (this.uncommittedEvents.Count > 0)
            {
                this.uncommittedEvents.Clear();
            }
        }
    }
}
