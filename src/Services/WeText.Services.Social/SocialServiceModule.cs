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
using WeText.Common.Services;
using WeText.Querying.MySqlClient;
using WeText.Services.Social.EventHandlers;

namespace WeText.Services.Social
{
    public class SocialServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register table data gateway
            builder
                .Register(x => new MySqlTableDataGateway("server=127.0.0.1;uid=root;pwd=P@ssw0rd;database=wetext.social;"))
                .As<ITableDataGateway>()
                .WithMetadata<NamedMetadata>(x => x.For(y => y.Name, "SocialServiceTableDataGateway"));

            // Register event handlers
            builder
                .Register(x => new UserCreatedEventHandler(
                    x.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == "SocialServiceTableDataGateway").Value))
                .Named<IDomainEventHandler>("SocialServiceEventHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"),
                        //x.ResolveNamed<IEnumerable<ICommandHandler>>("SocialServiceCommandHandler")))
                        null))
                .Named<ICommandConsumer>("SocialServiceCommandConsumer");

            // Register event consumer and assign message subscriber and event handler to the consumer.
            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"),
                    x.ResolveNamed<IEnumerable<IDomainEventHandler>>("SocialServiceEventHandler")))
                    //null))
                .Named<IEventConsumer>("SocialServiceEventConsumer");

            // Register micros service.
            builder.Register(x => new SocialService(x.ResolveNamed<ICommandConsumer>("SocialServiceCommandConsumer"),
                        x.ResolveNamed<IEventConsumer>("SocialServiceEventConsumer")))
                .As<IService>()
                .SingleInstance(); // We can only have one Social Service within the same application domain.
        }
    }
}
