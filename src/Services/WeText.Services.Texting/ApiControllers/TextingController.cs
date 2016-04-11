using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WeText.Common;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Domain.Commands;

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
    }
}
