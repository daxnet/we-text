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

namespace WeText.Services.Texting
{
    [RoutePrefix("api")]
    public class TextingController : MicroserviceApiController<TextingService>
    {
        public TextingController(WeTextConfiguration configuration,
            ICommandSender commandSender,
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
            :base(configuration, commandSender, tableGatewayRegistration)
        {

        }

        [HttpGet]
        [Route("texts/system/info")]
        public IHttpActionResult GetSystemInformation()
        {
            return Ok(new
            {
                Service = "Texting",
                Environment.MachineName,
                Environment.Is64BitOperatingSystem,
                Environment.Is64BitProcess,
                Environment.OSVersion.VersionString
            });
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
            CommandSender.Publish(createTextCommand);
            return Ok(textId);
        }

        [HttpGet]
        [Route("texts/user/{userId}")]
        public async Task<IHttpActionResult> GetAllTextsForUser(string userId)
        {
            Expression<Func<TextTableObject, bool>> specification = x => x.UserId == userId;
            return Ok((await this.TableDataGateway.SelectAsync<TextTableObject>(specification)).OrderByDescending(v=>v.DateCreated));
        }

        [HttpGet]
        [Route("texts/{id}")]
        public async Task<IHttpActionResult> GetText(string id)
        {
            Expression<Func<TextTableObject, bool>> specification = x => x.Id == id;
            return Ok(await this.TableDataGateway.SelectAsync<TextTableObject>(specification));
        }

        [HttpPost]
        [Route("texts/update/{id}")]
        public IHttpActionResult UpdateText(string id, [FromBody] dynamic model)
        {
            var textId = new Guid(id);
            var changeTextCommand = new ChangeTextCommand
            {
                Id = Guid.NewGuid(),
                Content = model.Content,
                Title = model.Title,
                TextId = textId
            };
            CommandSender.Publish(changeTextCommand);
            return Ok();
        }
    }
}
