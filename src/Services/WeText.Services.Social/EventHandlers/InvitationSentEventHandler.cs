using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Common.Repositories;
using WeText.Domain;
using WeText.Domain.Events;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social.EventHandlers
{
    public class InvitationSentEventHandler : DomainEventHandler<InvitationSentEvent>
    {
        private readonly IDomainRepository repository;
        private readonly ITableDataGateway tableGateway;

        public InvitationSentEventHandler(IDomainRepository repository, ITableDataGateway tableGateway)
        {
            this.repository = repository;
            this.tableGateway = tableGateway;
        }

        public override async Task HandleAsync(InvitationSentEvent message)
        {
            var invitation = new Invitation();
            invitation.Transit(message);
            await this.repository.SaveAsync<Guid, Invitation>(invitation);

            var network = new NetworkTableObject
            {
                InvitationId = invitation.Id.ToString(),
                OriginatorId = message.AggregateRootKey.ToString(),
                TargetId = message.TargetUserId.ToString(),
                OriginatorName = message.OriginatorName,
                TargetUserName = message.TargetUserName,
                InvitationStartDate = message.Timestamp,
                InvitationEndReason = InvitationEndReason.None
            };

            await this.tableGateway.InsertAsync<NetworkTableObject>(new[] { network });
        }
    }
}
