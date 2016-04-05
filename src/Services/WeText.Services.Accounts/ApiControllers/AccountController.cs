using System;
using System.Web.Http;
using WeText.Commands;
using WeText.Common.Messaging;

namespace WeText.Services.Accounts.ApiControllers
{
    [RoutePrefix("api")]
    public class AccountController : ApiController
    {
        private readonly ICommandSender commandBus;

        public AccountController(ICommandSender commandBus)
        {
            this.commandBus = commandBus;
        }

        [Route("users/create")]
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody] dynamic model)
        {
            var createUserCommand = new CreateUserCommand
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                DisplayName = model.DisplayName
            };
            commandBus.Publish(createUserCommand);
            return Ok();
        }
    }
}
