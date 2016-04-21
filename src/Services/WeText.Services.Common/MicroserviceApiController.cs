using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WeText.Common;
using WeText.Common.Config;
using WeText.Common.Messaging;
using WeText.Common.Querying;

namespace WeText.Services.Common
{
    public abstract class MicroserviceApiController<TService> : ApiController
        where TService : Microservice
    {
        private readonly WeTextConfiguration configuration;
        private readonly ICommandSender commandSender;
        private readonly ITableDataGateway tableDataGateway;

        protected MicroserviceApiController(WeTextConfiguration configuration,
            ICommandSender commandSender,
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
        {
            this.configuration = configuration;
            this.commandSender = commandSender;
            this.tableDataGateway = tableGatewayRegistration.First(x => x.Metadata.Name == $"{typeof(TService).FullName}.TableDataGateway").Value;
        }

        protected WeTextConfiguration WeTextConfiguration => configuration;

        protected ICommandSender CommandSender => commandSender;

        protected ITableDataGateway TableDataGateway => tableDataGateway;
    }
}
