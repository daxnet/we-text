using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common.Messaging;
using WeText.Common.Services;

namespace WeText.Services.Accounts
{
    public class AccountServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Assign command and event handlers to each of the following registration.

            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("command_subscriber"), null))
                .As<ICommandConsumer>()
                .Named<ICommandConsumer>("accountsServiceCommandConsumer");

            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("event_subscriber"), null))
                .As<IEventConsumer>()
                .Named<IEventConsumer>("accountsServiceEventConsumer");

            builder.Register(x => new AccountService(x.ResolveNamed<ICommandConsumer>(""), x.ResolveNamed<IEventConsumer>("")))
                .As<IService>()
                .Named<IService>("AccountService")
                .SingleInstance();


            base.Load(builder);
        }
    }
}
