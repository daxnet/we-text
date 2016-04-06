using System;
using System.Web.Http;
using WeText.Common.Messaging;
using WeText.Domain.Commands;

namespace WeText.Services.Accounts.ApiControllers
{
    [RoutePrefix("api")]
    public class AccountController : ApiController
    {
        private readonly ICommandSender commandSender;

        public AccountController(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
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
            commandSender.Publish(createUserCommand);
            return Ok();
        }
    }
}
