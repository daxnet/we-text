using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Commands;
using WeText.Common.Messaging;
using WeText.Common.Repositories;
using WeText.Common.Services;
using WeText.Services.Accounts.CommandHandlers;

namespace WeText.Services.Accounts
{
    /// <summary>
    /// Represents the module that will register the types within Account Service to the Autofac container builder.
    /// </summary>
    public class AccountServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register command handlers
            builder
                .Register(x => new CreateUserCommandHandler(x.Resolve<IDomainRepository>()))
                .Named<ICommandHandler>("AccountServiceCommandHandler");

            // Register command consumer and assign message subscriber and command handler to the consumer.
            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber"), 
                        x.ResolveNamed<IEnumerable<ICommandHandler>>("AccountServiceCommandHandler")))
                .Named<ICommandConsumer>("AccountsServiceCommandConsumer");

            // Register event consumer and assign message subscriber and event handler to the consumer.
            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber"), null))
                .Named<IEventConsumer>("AccountsServiceEventConsumer");

            // Register micros service.
            builder.Register(x => new AccountService(x.ResolveNamed<ICommandConsumer>("AccountsServiceCommandConsumer"), 
                        x.ResolveNamed<IEventConsumer>("AccountsServiceEventConsumer")))
                .As<IService>()
                //.Named<IService>("AccountService")
                .SingleInstance(); // We can only have one Account Service within the same application domain.

        }
    }
}
