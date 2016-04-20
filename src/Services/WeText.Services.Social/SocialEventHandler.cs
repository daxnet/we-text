using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Common.Repositories;
using WeText.Common.Specifications;
using WeText.Domain;
using WeText.Domain.Commands;
using WeText.Domain.Events;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social
{
    internal sealed class SocialEventHandler :
        IDomainEventHandler<InvitationApprovedEvent>,
        IDomainEventHandler<InvitationCompletedEvent>,
        IDomainEventHandler<InvitationRejectedEvent>,
        IDomainEventHandler<InvitationSentEvent>,
        IDomainEventHandler<UserCreatedEvent>,
        IDomainEventHandler<UserDisplayNameChangedEvent>
    {
        private readonly IDomainRepository domainRepository;
        private readonly ITableDataGateway tableGateway;
        private readonly ICommandSender commandSender;

        public SocialEventHandler(IDomainRepository domainRepository, ITableDataGateway tableGateway, ICommandSender commandSender)
        {
            this.domainRepository = domainRepository;
            this.tableGateway = tableGateway;
            this.commandSender = commandSender;
        }

        public async Task HandleAsync(InvitationRejectedEvent message)
        {
            var invitation = await domainRepository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            invitation.Transit(message);
            await this.domainRepository.SaveAsync<Guid, Invitation>(invitation);
        }

        public async Task HandleAsync(UserCreatedEvent message)
        {
            await this.tableGateway.InsertAsync<UserNameTableObject>(new[] { new UserNameTableObject { UserId = message.AggregateRootKey.ToString(), DisplayName = message.DisplayName } });
        }

        public async Task HandleAsync(UserDisplayNameChangedEvent message)
        {
            var userId = message.AggregateRootKey.ToString();
            var networkTableUpdateBatch = new List<Tuple<UpdateCriteria<NetworkTableObject>, Specification<NetworkTableObject>>>();
            Expression<Func<NetworkTableObject, bool>> originatorEqualsSpecification = x => x.OriginatorId == userId;
            Expression<Func<NetworkTableObject, bool>> targetUserEqualsSpecification = x => x.TargetId == userId;
            networkTableUpdateBatch.Add(new Tuple<UpdateCriteria<NetworkTableObject>, Specification<NetworkTableObject>>(
                new UpdateCriteria<NetworkTableObject> { { x => x.OriginatorName, message.DisplayName } }, originatorEqualsSpecification));
            networkTableUpdateBatch.Add(new Tuple<UpdateCriteria<NetworkTableObject>, Specification<NetworkTableObject>>(
                new UpdateCriteria<NetworkTableObject> { { x => x.TargetUserName, message.DisplayName } }, targetUserEqualsSpecification));
            await this.tableGateway.UpdateAsync<NetworkTableObject>(networkTableUpdateBatch);

            var userNamesTableUpdateCriteria = new UpdateCriteria<UserNameTableObject> { { x => x.DisplayName, message.DisplayName } };
            Expression<Func<UserNameTableObject, bool>> userIdSpecification = x => x.UserId == userId;
            await this.tableGateway.UpdateAsync<UserNameTableObject>(userNamesTableUpdateCriteria, userIdSpecification);
        }

        public async Task HandleAsync(InvitationSentEvent message)
        {
            var invitation = new Invitation();
            invitation.Transit(message);
            await this.domainRepository.SaveAsync<Guid, Invitation>(invitation);

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

        public async Task HandleAsync(InvitationCompletedEvent message)
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

        public async Task HandleAsync(InvitationApprovedEvent message)
        {
            var invitation = await domainRepository.GetByKeyAsync<Guid, Invitation>(message.InvitationId);
            invitation.Transit(message);
            await this.domainRepository.SaveAsync<Guid, Invitation>(invitation);

            // Once the application get approved, send the add friend command.
            var command = new AddFriendCommand
            {
                AcceptorId = message.ApproverId,
                OriginatorId = message.OriginatorId
            };

            this.commandSender.Publish(command);
        }
    }
}
