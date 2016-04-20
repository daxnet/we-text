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

namespace WeText.Services.Social
{
    public class SocialServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Func<IComponentContext, ITableDataGateway> tableDataGatewayResolver = x => 
                x.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == "SocialServiceTableDataGateway").Value;

            // Register table data gateway
            builder
                .Register(x => new MySqlTableDataGateway("server=127.0.0.1;uid=root;pwd=P@ssw0rd;database=wetext.social;"))
                .As<ITableDataGateway>()
                .WithMetadata<NamedMetadata>(x => x.For(y => y.Name, "SocialServiceTableDataGateway"));

            builder
                .Register(x => new MessageRedirectingConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"),
                    x.ResolveNamed<ICommandSender>("LocalMessageQueueCommandSender",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Commands"))))
                .Named<IMessageConsumer>("SocialServiceCommandRedirectingConsumer");
            builder
                .Register(x => new MessageRedirectingConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"),
                    x.ResolveNamed<IEventPublisher>("LocalMessageQueueEventPublisher",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Events"))))
                .Named<IMessageConsumer>("SocialServiceEventRedirectingConsumer");

            // Register event handlers
            builder
                .Register(x => new SocialEventHandler(x.Resolve<IDomainRepository>(), tableDataGatewayResolver(x), x.Resolve<IEnumerable<Lazy<ICommandSender, NamedMetadata>>>().First(p => p.Metadata.Name == "CommandSender").Value))
                .Named<IDomainEventHandler>("SocialServiceEventHandler");


            // Register command handlers
            builder
                .Register(x => new SocialCommandHandler(x.Resolve<IDomainRepository>()))
                .Named<ICommandHandler>("SocialServiceCommandHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueCommandSubscriber",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Commands")),
                        x.ResolveNamed<IEnumerable<ICommandHandler>>("SocialServiceCommandHandler")))
                        //null))
                .Named<ICommandConsumer>("SocialServiceCommandConsumer");

            // Register event consumer and assign message subscriber and event handler to the consumer.
            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueEventSubscriber",
                        new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Events")),
                    x.ResolveNamed<IEnumerable<IDomainEventHandler>>("SocialServiceEventHandler")))
                    //null))
                .Named<IEventConsumer>("SocialServiceEventConsumer");

            // Register micros service.
            builder.Register(x => new SocialService(
                        x.ResolveNamed<IMessageConsumer>("SocialServiceCommandRedirectingConsumer"),
                        x.ResolveNamed<IMessageConsumer>("SocialServiceEventRedirectingConsumer"),
                        x.ResolveNamed<ICommandConsumer>("SocialServiceCommandConsumer"),
                        x.ResolveNamed<IEventConsumer>("SocialServiceEventConsumer")))
                .As<IService>()
                .SingleInstance(); // We can only have one Social Service within the same application domain.
        }
    }
}
