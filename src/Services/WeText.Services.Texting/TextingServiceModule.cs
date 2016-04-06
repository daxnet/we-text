using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Messaging;
using WeText.Common.Services;
using WeText.Services.Texting.CommandHandlers;

namespace WeText.Services.Texting
{
    public class TextingServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register command handlers
            builder
                .Register(x => new CreateUserCommandHandler())
                .Named<ICommandHandler>("TextingServiceCommandHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"),
                        x.ResolveNamed<IEnumerable<ICommandHandler>>("TextingServiceCommandHandler")))
                .Named<ICommandConsumer>("TextingServiceCommandConsumer");

            // Register micros service.
            builder.Register(x => new TextingService(x.ResolveNamed<ICommandConsumer>("TextingServiceCommandConsumer")))
                .As<IService>()
                .SingleInstance(); // We can only have one Texting Service within the same application domain.
        }
    }
}
