using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Domain.Events;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social.EventHandlers
{
    public class InvitationCompletedEventHandler : DomainEventHandler<InvitationCompletedEvent>
    {
        private readonly ITableDataGateway tableGateway;

        public InvitationCompletedEventHandler(ITableDataGateway tableGateway)
        {
            this.tableGateway = tableGateway;
        }

        public override async Task HandleAsync(InvitationCompletedEvent message)
        {
            InvitationEndReason reason = message.Accepted ? InvitationEndReason.Accepted : InvitationEndReason.Rejected;
            var updateCriteria = new UpdateCriteria<NetworkTableObject>
                {
                    { x => x.InvitationEndDate, message.Timestamp },
                    { x => x.InvitationEndReason, reason }
                };
            var originatorId = message.OriginatorId.ToString();
            var targetUserId = message.TargetUserId.ToString();
            Expression<Func<NetworkTableObject, bool>> specification = x => x.OriginatorId == originatorId && x.TargetId == targetUserId;
            await this.tableGateway.UpdateAsync<NetworkTableObject>(updateCriteria, specification);
        }
    }
}
