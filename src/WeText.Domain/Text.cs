using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
using WeText.Common.Events;
using WeText.Domain.Events;

namespace WeText.Domain
{
    public class Text : AggregateRoot<Guid>
    {
        public string Title { get; private set; }

        public string Content { get; private set; }

        public DateTime DateCreated { get; private set; }

        public Guid UserId { get; private set; }

        public Text()
        {
            ApplyEvent(new TextCreatedEvent(Guid.Empty));
        }

        public Text(Guid id)
        {
            ApplyEvent(new TextCreatedEvent(id));
        }

        public Text(Guid id, string title, string content, Guid userId)
        {
            ApplyEvent(new TextCreatedEvent(id, title, content, userId));
        }

        public void ChangeTitle(string title)
        {
            ApplyEvent(new TextChangedEvent(this.Id) { Title = title });
        }

        public void ChangeContent(string content)
        {
            ApplyEvent(new TextChangedEvent(this.Id) { Content = content });
        }

        [InlineEventHandler]
        private void HandleTextCreatedEvent(TextCreatedEvent evnt)
        {
            this.Id = (Guid)evnt.AggregateRootKey;
            this.UserId = evnt.UserId;
            this.Title = evnt.Title;
            this.Content = evnt.Content;
            this.DateCreated = evnt.Timestamp;
        }

        [InlineEventHandler]
        private void HandleTextChangedEvent(TextChangedEvent evnt)
        {
            if (!string.IsNullOrEmpty(evnt.Title))
            {
                this.Title = evnt.Title;
            }
            if (!string.IsNullOrEmpty(evnt.Content))
            {
                this.Content = evnt.Content;
            }
        }
    }
}
