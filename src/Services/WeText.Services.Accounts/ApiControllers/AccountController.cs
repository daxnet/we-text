using Autofac.Extras.AttributeMetadata;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Domain.Commands;
using WeText.Common;
using System.Threading.Tasks;
using WeText.Services.Accounts.Querying;
using WeText.Common.Specifications;
using System.Linq.Expressions;

namespace WeText.Services.Accounts.ApiControllers
{
    [RoutePrefix("api")]
    public class AccountController : ApiController
    {
        private readonly ICommandSender commandSender;
        private readonly ITableDataGateway tableDataGateway;

        public AccountController(ICommandSender commandSender, 
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
        {
            this.commandSender = commandSender;
            this.tableDataGateway = tableGatewayRegistration.First(x => x.Metadata.Name == "AccountServiceTableDataGateway").Value;
        }

        [Route("accounts/create")]
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody] dynamic model)
        {
            var userId = Guid.NewGuid();
            var createUserCommand = new CreateUserCommand
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = model.Name,
                Password = model.Password,
                Email = model.Email,
                DisplayName = model.DisplayName
            };
            commandSender.Publish(createUserCommand);
            return Ok(userId);
        }

        [Route("accounts/update/{id}")]
        [HttpPost]
        public IHttpActionResult UpdateUser(Guid id, [FromBody] dynamic model)
        {
            var email = (string)model.Email;
            var displayName = (string)model.DisplayName;
            var updateUserCommand = new UpdateUserCommand
            {
                UserId = id,
                Email = email,
                DisplayName = displayName
            };
            commandSender.Publish(updateUserCommand);
            return Ok();
        }

        [Route("accounts/name/{name}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserByName(string name)
        {
            Expression<Func<AccountTableObject, bool>> specification = a => a.Name == name;
            return Ok(await this.tableDataGateway.SelectAsync<AccountTableObject>(specification));
        }

        [Route("accounts/id/{id}")]
        public async Task<IHttpActionResult> GetUserById(string id)
        {
            Expression<Func<AccountTableObject, bool>> specification = a => a.Id == id;
            return Ok(await this.tableDataGateway.SelectAsync<AccountTableObject>(specification));
        }
    }
}
