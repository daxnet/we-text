using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WeText.Common;
using WeText.Common.Config;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Domain.Commands;
using WeText.Services.Common;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social
{
    [RoutePrefix("api")]
    public class SocialController : MicroserviceApiController<SocialService>
    {
        public SocialController(WeTextConfiguration configuration,
            ICommandSender commandSender,
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
            : base(configuration, commandSender, tableGatewayRegistration)
        { }

        [HttpPost]
        [Route("social/invitation/send")]
        public IHttpActionResult SendInvitation([FromBody] dynamic model)
        {
            var originatorId = (string)model.OriginatorId;
            var targetUserId = (string)model.TargetUserId;
            var invitationLetter = (string)model.InvitationLetter;

            var command = new SendInvitationCommand
            {
                OriginatorId = Guid.Parse(originatorId),
                TargetUserId = Guid.Parse(targetUserId),
                InvitationLetter = invitationLetter
            };
            this.CommandSender.Publish(command);
            return Ok();
        }

        [HttpPost]
        [Route("social/invitation/accept")]
        public IHttpActionResult AcceptInvitation([FromBody] dynamic model)
        {
            var currentUserId = (string)model.CurrentUserId;
            var invitationId = (string)model.InvitationId;
            var command = new AcceptInvitationCommand
            {
                InvitationId = Guid.Parse(invitationId),
                UserId = Guid.Parse(currentUserId)
            };
            this.CommandSender.Publish(command);
            return Ok();
        }

        [HttpPost]
        [Route("social/invitation/reject")]
        public IHttpActionResult RejectInvitation([FromBody] dynamic model)
        {
            var currentUserId = (string)model.CurrentUserId;
            var invitationId = (string)model.InvitationId;
            var command = new RejectInvitationCommand
            {
                InvitationId = Guid.Parse(invitationId),
                UserId = Guid.Parse(currentUserId)
            };
            this.CommandSender.Publish(command);
            return Ok();
        }

        [HttpGet]
        [Route("social/invitation/{userId}/sent")]
        public async Task<IHttpActionResult> MySentInvitations(string userId)
        {
            Expression<Func<NetworkTableObject, bool>> specification = x => x.OriginatorId == userId;
            return Ok(await this.TableDataGateway.SelectAsync<NetworkTableObject>(specification));
        }

        [HttpGet]
        [Route("social/invitation/{userId}/received")]
        public async Task<IHttpActionResult> MyReceivedInvitations(string userId)
        {
            Expression<Func<NetworkTableObject, bool>> specification = x => x.TargetId == userId;
            return Ok(await this.TableDataGateway.SelectAsync<NetworkTableObject>(specification));
        }

        [HttpGet]
        [Route("social/others/{thisUserId}")]
        public async Task<IHttpActionResult> GetOtherUsers(string thisUserId)
        {
            Expression<Func<UserNameTableObject, bool>> specification = x => x.UserId != thisUserId;

            return Ok(await this.TableDataGateway.SelectAsync<UserNameTableObject>(specification));
        }
    }
}
