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
using WeText.Services.Common;
using WeText.Common.Config;

namespace WeText.Services.Accounts
{
    #region Obsolete
    ///// <summary>
    ///// Represents the module that will register the types within Account Service to the Autofac container builder.
    ///// </summary>
    //public class AccountServiceModule : Module
    //{
    //    protected override void Load(ContainerBuilder builder)
    //    {
    //        // Register table data gateway
    //        builder
    //            .Register(x => new MySqlTableDataGateway("server=127.0.0.1;uid=root;pwd=P@ssw0rd;database=wetext.accounts;"))
    //            .As<ITableDataGateway>()
    //            .WithMetadata<NamedMetadata>(x => x.For(y => y.Name, "AccountServiceTableDataGateway"));

    //        builder
    //            .Register(x => new MessageRedirectingConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"),
    //                x.ResolveNamed<ICommandSender>("LocalMessageQueueCommandSender",
    //                    new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Commands"))))
    //            .Named<IMessageConsumer>("AccountServiceCommandRedirectingConsumer");
    //        builder
    //            .Register(x => new MessageRedirectingConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"), 
    //                x.ResolveNamed<IEventPublisher>("LocalMessageQueueEventPublisher",
    //                    new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Events"))))
    //            .Named<IMessageConsumer>("AccountServiceEventRedirectingConsumer");


    //        // Register command handlers
    //        builder
    //            .Register(x => new AccountsCommandHandler(x.Resolve<IDomainRepository>()))
    //            .Named<ICommandHandler>("AccountServiceCommandHandler");

    //        // Register event handlers
    //        builder
    //            .Register(x => new AccountsEventHandler(
    //                x.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == "AccountServiceTableDataGateway").Value))
    //            .Named<IDomainEventHandler>("AccountServiceEventHandler");

    //        // Register command consumer and assign message subscriber and command handler to the consumer.
    //        builder
    //            .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueCommandSubscriber",
    //                    new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Commands")), 
    //                    x.ResolveNamed<IEnumerable<ICommandHandler>>("AccountServiceCommandHandler")))
    //            .Named<ICommandConsumer>("AccountsServiceCommandConsumer");

    //        // Register event consumer and assign message subscriber and event handler to the consumer.
    //        builder
    //            .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueEventSubscriber",
    //                    new NamedParameter("hostName", "localhost"), new NamedParameter("queueName", this.GetType().Name + ".Events")),
    //                x.ResolveNamed<IEnumerable<IDomainEventHandler>>("AccountServiceEventHandler")))
    //            .Named<IEventConsumer>("AccountsServiceEventConsumer");

    //        // Register micros service.
    //        builder.Register(x => new AccountService(
    //                    x.ResolveNamed<IMessageConsumer>("AccountServiceCommandRedirectingConsumer"),
    //                    x.ResolveNamed<IMessageConsumer>("AccountServiceEventRedirectingConsumer"),
    //                    x.ResolveNamed<ICommandConsumer>("AccountsServiceCommandConsumer"), 
    //                    x.ResolveNamed<IEventConsumer>("AccountsServiceEventConsumer")))
    //            .As<IService>()
    //            .SingleInstance(); // We can only have one Account Service within the same application domain.

    //    }
    //}
    #endregion

    public sealed class AccountServiceRegister : MicroserviceRegister<AccountService>
    {
        private readonly string tableDataGatewayConnectionString;

        public AccountServiceRegister(WeTextConfiguration configuration) : base(configuration)
        {
            this.tableDataGatewayConnectionString = ThisConfiguration?.Settings?.GetItemByKey("TableDataGatewayConnectionString").Value;
            if (string.IsNullOrEmpty(this.tableDataGatewayConnectionString))
            {
                throw new ServiceRegistrationException("Connection String for TableDataGateway has not been specified.");
            }
        }

        protected override Func<IComponentContext, ITableDataGateway> TableDataGatewayInitializer => 
            x => new MySqlTableDataGateway(this.tableDataGatewayConnectionString);

        protected override IEnumerable<Func<IComponentContext, ICommandHandler>> CommandHandlersInitializer
        {
            get
            {
                yield return x => new AccountsCommandHandler(x.Resolve<IDomainRepository>());
            }
        }

        protected override IEnumerable<Func<IComponentContext, IDomainEventHandler>> EventHandlersInitializer
        {
            get
            {
                yield return x => new AccountsEventHandler(this.ResolveTableDataGateway(x));
            }
        }

        protected override Func<ICommandConsumer, IEventConsumer, AccountService> ServiceInitializer => 
            (cc, ec) => new AccountService(cc, ec);

    }
}
