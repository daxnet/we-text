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
using WeText.Domain.Commands;
using WeText.Services.Texting.Querying;

namespace WeText.Services.Texting.ApiControllers
{
    [RoutePrefix("api")]
    public class TextingController : ApiController
    {
        private readonly ICommandSender commandSender;
        private readonly ITableDataGateway tableDataGateway;

        public TextingController(ICommandSender commandSender,
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
        {
            this.commandSender = commandSender;
            this.tableDataGateway = tableGatewayRegistration.First(x => x.Metadata.Name == "TextingServiceTableDataGateway").Value;
        }

        [HttpPost]
        [Route("texts/create")]
        public IHttpActionResult CreateText([FromBody] dynamic model)
        {
            var textId = Guid.NewGuid();
            var createTextCommand = new CreateTextCommand
            {
                Id = Guid.NewGuid(),
                Content = model.Content,
                Title = model.Title,
                UserId = model.UserId,
                TextId = textId
            };
            commandSender.Publish(createTextCommand);
            return Ok(textId);
        }

        [HttpGet]
        [Route("texts/user/{userId}")]
        public async Task<IHttpActionResult> GetAllTextsForUser(string userId)
        {
            Expression<Func<TextTableObject, bool>> specification = x => x.UserId == userId;
            return Ok((await this.tableDataGateway.SelectAsync<TextTableObject>(specification)).OrderByDescending(v=>v.DateCreated));
        }

        [HttpGet]
        [Route("texts/{id}")]
        public async Task<IHttpActionResult> GetText(string id)
        {
            Expression<Func<TextTableObject, bool>> specification = x => x.Id == id;
            return Ok(await this.tableDataGateway.SelectAsync<TextTableObject>(specification));
        }
    }
}
