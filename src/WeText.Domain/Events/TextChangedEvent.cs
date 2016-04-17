using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class TextChangedEvent : DomainEvent
    {
        public string Title { get; set; }

        public string Content { get; set; }

        protected TextChangedEvent() : base() { }

        public TextChangedEvent(object aggregateRootKey)
            : base(aggregateRootKey)
        {

        }
    }
}
