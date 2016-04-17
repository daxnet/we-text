using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Querying;
using WeText.Common.Specifications;
using WeText.Domain.Events;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social.EventHandlers
{
    public class UserDisplayNameChangedEventHandler : DomainEventHandler<UserDisplayNameChangedEvent>
    {
        private readonly ITableDataGateway tableDataGateway;

        public UserDisplayNameChangedEventHandler(ITableDataGateway tableDataGateway)
        {
            this.tableDataGateway = tableDataGateway;
        }

        public override async Task HandleAsync(UserDisplayNameChangedEvent message)
        {
            var userId = message.AggregateRootKey.ToString();
            var networkTableUpdateBatch = new List<Tuple<UpdateCriteria<NetworkTableObject>, Specification<NetworkTableObject>>>();
            Expression<Func<NetworkTableObject, bool>> originatorEqualsSpecification = x => x.OriginatorId == userId;
            Expression<Func<NetworkTableObject, bool>> targetUserEqualsSpecification = x => x.TargetId == userId;
            networkTableUpdateBatch.Add(new Tuple<UpdateCriteria<NetworkTableObject>, Specification<NetworkTableObject>>(
                new UpdateCriteria<NetworkTableObject> { { x => x.OriginatorName, message.DisplayName } }, originatorEqualsSpecification));
            networkTableUpdateBatch.Add(new Tuple<UpdateCriteria<NetworkTableObject>, Specification<NetworkTableObject>>(
                new UpdateCriteria<NetworkTableObject> { { x => x.TargetUserName, message.DisplayName } }, targetUserEqualsSpecification));
            await this.tableDataGateway.UpdateAsync<NetworkTableObject>(networkTableUpdateBatch);

            var userNamesTableUpdateCriteria = new UpdateCriteria<UserNameTableObject> { { x => x.DisplayName, message.DisplayName } };
            Expression<Func<UserNameTableObject, bool>> userIdSpecification = x => x.UserId == userId;
            await this.tableDataGateway.UpdateAsync<UserNameTableObject>(userNamesTableUpdateCriteria, userIdSpecification);
        }
    }
}
