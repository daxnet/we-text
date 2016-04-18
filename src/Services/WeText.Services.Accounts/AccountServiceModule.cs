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

namespace WeText.Services.Accounts
{
    /// <summary>
    /// Represents the module that will register the types within Account Service to the Autofac container builder.
    /// </summary>
    public class AccountServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register table data gateway
            builder
                .Register(x => new MySqlTableDataGateway("server=127.0.0.1;uid=root;pwd=P@ssw0rd;database=wetext.accounts;"))
                .As<ITableDataGateway>()
                .WithMetadata<NamedMetadata>(x => x.For(y => y.Name, "AccountServiceTableDataGateway"));

            // Register command handlers
            builder
                .Register(x => new AccountsCommandHandler(x.Resolve<IDomainRepository>()))
                .Named<ICommandHandler>("AccountServiceCommandHandler");

            // Register event handlers
            builder
                .Register(x => new AccountsEventHandler(
                    x.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == "AccountServiceTableDataGateway").Value))
                .Named<IDomainEventHandler>("AccountServiceEventHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"), 
                        x.ResolveNamed<IEnumerable<ICommandHandler>>("AccountServiceCommandHandler")))
                .Named<ICommandConsumer>("AccountsServiceCommandConsumer");

            // Register event consumer and assign message subscriber and event handler to the consumer.
            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"),
                    x.ResolveNamed<IEnumerable<IDomainEventHandler>>("AccountServiceEventHandler")))
                .Named<IEventConsumer>("AccountsServiceEventConsumer");

            // Register micros service.
            builder.Register(x => new AccountService(x.ResolveNamed<ICommandConsumer>("AccountsServiceCommandConsumer"), 
                        x.ResolveNamed<IEventConsumer>("AccountsServiceEventConsumer")))
                .As<IService>()
                .SingleInstance(); // We can only have one Account Service within the same application domain.

        }
    }
}
