using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WeText.Common;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Services.Social.Querying;

namespace WeText.Services.Social.ApiControllers
{
    [RoutePrefix("api")]
    public class SocialController : ApiController
    {
        private readonly ICommandSender commandSender;
        private readonly ITableDataGateway tableDataGateway;

        public SocialController(ICommandSender commandSender,
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
        {
            this.commandSender = commandSender;
            this.tableDataGateway = tableGatewayRegistration.First(x => x.Metadata.Name == "SocialServiceTableDataGateway").Value;
        }

        [HttpGet]
        [Route("social/others/{thisUserId}")]
        public async Task<IHttpActionResult> GetOtherUsers(string thisUserId)
        {
            Expression<Func<UserNameTableObject, bool>> specification = x => x.UserId != thisUserId;

            return Ok(await this.tableDataGateway.SelectAsync<UserNameTableObject>(specification));
        }
    }
}
