using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
using WeText.Common.Commands;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Common.Repositories;
using WeText.Common.Services;
using WeText.Querying.MySqlClient;
using WeText.Services.Texting.CommandHandlers;
using WeText.Services.Texting.EventHandlers;

namespace WeText.Services.Texting
{
    public class TextingServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register table data gateway
            builder
                .Register(x => new MySqlTableDataGateway("server=127.0.0.1;uid=root;pwd=P@ssw0rd;database=wetext.texting;"))
                .As<ITableDataGateway>()
                .WithMetadata<NamedMetadata>(x => x.For(y => y.Name, "TextingServiceTableDataGateway"));

            // Register command handlers
            builder
                .Register(x => new CreateTextCommandHandler(x.Resolve<IDomainRepository>()))
                .Named<ICommandHandler>("TextingServiceCommandHandler");

            // Register event handlers
            builder
                .Register(x => new TextCreatedEventHandler(
                    x.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == "TextingServiceTableDataGateway").Value))
                .Named<IDomainEventHandler>("TextingServiceEventHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"),
                        x.ResolveNamed<IEnumerable<ICommandHandler>>("TextingServiceCommandHandler")))
                .Named<ICommandConsumer>("TextingServiceCommandConsumer");

            // Register event consumer and assign message subscriber and event handler to the consumer.
            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"),
                    x.ResolveNamed<IEnumerable<IDomainEventHandler>>("TextingServiceEventHandler")))
                .Named<IEventConsumer>("TextingServiceEventConsumer");

            // Register micros service.
            builder.Register(x => new TextingService(x.ResolveNamed<ICommandConsumer>("TextingServiceCommandConsumer"),
                        x.ResolveNamed<IEventConsumer>("TextingServiceEventConsumer")))
                .As<IService>()
                .SingleInstance(); // We can only have one Texting Service within the same application domain.
        }
    }
}
