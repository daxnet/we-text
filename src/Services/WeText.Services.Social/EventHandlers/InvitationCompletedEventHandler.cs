using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Domain.Commands;
using WeText.Domain.Events;

namespace WeText.Services.Social.EventHandlers
{
    public class InvitationCompletedEventHandler : DomainEventHandler<InvitationCompletedEvent>
    {
        private readonly ICommandSender commandSender;

        public InvitationCompletedEventHandler(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        public override Task HandleAsync(InvitationCompletedEvent message)
        {
            var addFriendCommand = new AddFriendCommand
            {
                UserId = message.FromUserId,
                FriendId = message.ToUserId,
                Id = Guid.NewGuid()
            };
            commandSender.Publish(addFriendCommand);
            return Task.CompletedTask;
        }
    }
}
