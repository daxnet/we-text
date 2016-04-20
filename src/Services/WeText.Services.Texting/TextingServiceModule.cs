using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using WeText.Common;
using WeText.Common.Commands;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Common.Repositories;
using WeText.Common.Services;
using WeText.Querying.MySqlClient;

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

            builder
                .Register(x => new MessageRedirectingConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"),
                    x.ResolveNamed<ICommandSender>("LocalMessageQueueCommandSender",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Commands"))))
                .Named<IMessageConsumer>("TextingServiceCommandRedirectingConsumer");
            builder
                .Register(x => new MessageRedirectingConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"),
                    x.ResolveNamed<IEventPublisher>("LocalMessageQueueEventPublisher",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Events"))))
                .Named<IMessageConsumer>("TextingServiceEventRedirectingConsumer");

            // Register command handlers
            builder
                .Register(x => new TextingCommandHandler(x.Resolve<IDomainRepository>()))
                .Named<ICommandHandler>("TextingServiceCommandHandler");

            // Register event handlers
            builder
                .Register(x => new TextingEventHandler(
                    x.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == "TextingServiceTableDataGateway").Value))
                .Named<IDomainEventHandler>("TextingServiceEventHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueCommandSubscriber",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Commands")),
                        x.ResolveNamed<IEnumerable<ICommandHandler>>("TextingServiceCommandHandler")))
                .Named<ICommandConsumer>("TextingServiceCommandConsumer");

            // Register event consumer and assign message subscriber and event handler to the consumer.
            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueEventSubscriber",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Events")),
                    x.ResolveNamed<IEnumerable<IDomainEventHandler>>("TextingServiceEventHandler")))
                .Named<IEventConsumer>("TextingServiceEventConsumer");

            // Register micros service.
            builder.Register(x => new TextingService(
                        x.ResolveNamed<IMessageConsumer>("TextingServiceCommandRedirectingConsumer"),
                        x.ResolveNamed<IMessageConsumer>("TextingServiceEventRedirectingConsumer"),
                        x.ResolveNamed<ICommandConsumer>("TextingServiceCommandConsumer"),
                        x.ResolveNamed<IEventConsumer>("TextingServiceEventConsumer")))
                .As<IService>()
                .SingleInstance(); // We can only have one Texting Service within the same application domain.
        }
    }
}
