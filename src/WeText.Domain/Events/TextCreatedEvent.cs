using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;

namespace WeText.Domain.Events
{
    public class TextCreatedEvent : DomainEvent
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        protected TextCreatedEvent() { }

        public TextCreatedEvent(object aggregateRootKey) : base(aggregateRootKey) { }

        public TextCreatedEvent(object aggregateRootKey, string title, string content, Guid userId)
            :base(aggregateRootKey)
        {
            this.Title = title;
            this.Content = content;
            this.UserId = userId;
        }
    }
}
