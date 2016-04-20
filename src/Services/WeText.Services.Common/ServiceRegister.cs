using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeText.Common;
using WeText.Common.Commands;
using WeText.Common.Config;
using WeText.Common.Events;
using WeText.Common.Messaging;
using WeText.Common.Querying;
using WeText.Common.Repositories;
using WeText.Common.Services;

namespace WeText.Services.Common
{
    public abstract class MicroserviceRegister<TService> : Module
        where TService : IService
    {
        private readonly WeTextConfiguration configuration;

        protected MicroserviceRegister(WeTextConfiguration configuration)
        {
            this.configuration = configuration;
        }

        
        private void RegisterMessageRedirecting(ContainerBuilder builder)
        {
            var commandQueueHostName = ThisConfiguration?.LocalCommandQueue?.HostName;
            var commandQueueName = ThisConfiguration?.LocalCommandQueue?.ResourceType == MessageQueueResourceType.MessageQueue ? 
                ThisConfiguration?.LocalCommandQueue?.ResourceName : 
                null;

            if (!string.IsNullOrEmpty(commandQueueHostName) &&
                !string.IsNullOrEmpty(commandQueueName))
            {
                builder
                    .Register(x => new MessageRedirectingConsumer(this.ResolveGlobalCommandSubscriber(x),
                        x.ResolveNamed<ICommandSender>("LocalMessageQueueCommandSender",
                            new NamedParameter("hostName", commandQueueHostName), new NamedParameter("queueName", commandQueueName))))
                    .Named<IMessageConsumer>($"{ThisConfiguration.Type}.CommandRedirectingConsumer");
            }

            var eventQueueHostName = ThisConfiguration?.LocalEventQueue?.HostName;
            var eventQueueName = ThisConfiguration?.LocalEventQueue?.ResourceType == MessageQueueResourceType.MessageQueue ? 
                ThisConfiguration?.LocalEventQueue?.ResourceName : 
                null;

            if (!string.IsNullOrEmpty(eventQueueHostName) &&
                !string.IsNullOrEmpty(eventQueueName))
            {
                builder
                    .Register(x => new MessageRedirectingConsumer(this.ResolveGlobalEventSubscriber(x),
                        x.ResolveNamed<IEventPublisher>("LocalMessageQueueEventPublisher",
                            new NamedParameter("hostName", eventQueueHostName), new NamedParameter("queueName", eventQueueName))))
                    .Named<IMessageConsumer>($"{ThisConfiguration.Type}.EventRedirectingConsumer");
            }
        }

        protected ServiceElement ThisConfiguration => this.configuration.Services.GetItemByKey(typeof(TService).FullName);

        protected override void Load(ContainerBuilder builder)
        {
            this.RegisterTableDataGateway(builder, this.TableDataGatewayInitializer);
            this.RegisterMessageRedirecting(builder);
            this.RegisterCommandHandlers(builder, this.CommandHandlersInitializer);
            this.RegisterEventHandlers(builder, this.EventHandlersInitializer);
            this.RegisterLocalCommandConsumer(builder);
            this.RegisterLocalEventConsumer(builder);
            this.RegisterService(builder, this.ServiceInitializer);

            base.Load(builder);
        }


        private void RegisterTableDataGateway(ContainerBuilder builder,
            Func<IComponentContext, ITableDataGateway> initializer)
        {
            if (initializer != null)
            {
                builder
                    .Register(x => initializer(x))
                    .As<ITableDataGateway>()
                    .WithMetadata<NamedMetadata>(x => x.For(y => y.Name, $"{ThisConfiguration.Type}.TableDataGateway"));
            }
        }

        private void RegisterCommandHandlers(ContainerBuilder builder, 
            IEnumerable<Func<IComponentContext, ICommandHandler>> initializer)
        {
            if (initializer != null)
            {
                foreach (var value in initializer)
                {
                    builder.Register(x => value(x))
                        .Named<ICommandHandler>($"{ThisConfiguration.Type}.CommandHandlers");
                }
            }
        }

        private void RegisterEventHandlers(ContainerBuilder builder,
            IEnumerable<Func<IComponentContext, IDomainEventHandler>> initializer)
        {
            if (initializer != null)
            {
                foreach (var value in initializer)
                {
                    builder.Register(x => value(x))
                        .Named<IDomainEventHandler>($"{ThisConfiguration.Type}.EventHandlers");
                }
            }
        }

        private void RegisterLocalCommandConsumer(ContainerBuilder builder)
        {
            var commandQueueHostName = ThisConfiguration?.LocalCommandQueue?.HostName;
            var commandQueueName = ThisConfiguration?.LocalCommandQueue?.ResourceType == MessageQueueResourceType.MessageQueue ?
                ThisConfiguration?.LocalCommandQueue?.ResourceName :
                null;

            Func<IComponentContext, IEnumerable<ICommandHandler>> commandHandlersResolver = (context) =>
            {
                object result;
                if (context.TryResolveNamed($"{ThisConfiguration.Type}.CommandHandlers", typeof(IEnumerable<ICommandHandler>), out result))
                {
                    return (IEnumerable<ICommandHandler>)result;
                }
                return null;
            };

            builder
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueCommandSubscriber",
                        new NamedParameter("hostName", commandQueueHostName), new NamedParameter("queueName", commandQueueName)),
                        commandHandlersResolver(x)))
                .Named<ICommandConsumer>($"{ThisConfiguration.Type}.LocalCommandConsumer");
        }

        private void RegisterLocalEventConsumer(ContainerBuilder builder)
        {
            var eventQueueHostName = ThisConfiguration?.LocalEventQueue?.HostName;
            var eventQueueName = ThisConfiguration?.LocalEventQueue?.ResourceType == MessageQueueResourceType.MessageQueue ?
                ThisConfiguration?.LocalEventQueue?.ResourceName :
                null;

            Func<IComponentContext, IEnumerable<IDomainEventHandler>> eventHandlersResolver = (context) =>
            {
                object result;
                if (context.TryResolveNamed($"{ThisConfiguration.Type}.EventHandlers", typeof(IEnumerable<IDomainEventHandler>), out result))
                {
                    return (IEnumerable<IDomainEventHandler>)result;
                }
                return null;
            };

            builder
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("LocalMessageQueueEventSubscriber",
                        new NamedParameter("hostName", eventQueueHostName), new NamedParameter("queueName", eventQueueName)),
                            eventHandlersResolver(x)))
                .Named<IEventConsumer>($"{ThisConfiguration.Type}.LocalEventConsumer");
        }

        private void RegisterService(ContainerBuilder builder,
            Func<IMessageConsumer, IMessageConsumer, ICommandConsumer, IEventConsumer, TService> serviceInitializer)
        {
            Func<IComponentContext, IMessageConsumer> commandRedirectingConsumerResolver = context => 
                context.ResolveNamed<IMessageConsumer>($"{ThisConfiguration.Type}.CommandRedirectingConsumer");
            Func<IComponentContext, IMessageConsumer> eventRedirectingConsumerResolver = context =>
                context.ResolveNamed<IMessageConsumer>($"{ThisConfiguration.Type}.EventRedirectingConsumer");
            Func<IComponentContext, ICommandConsumer> localCommandConsumerResolver = context =>
                context.ResolveNamed<ICommandConsumer>($"{ThisConfiguration.Type}.LocalCommandConsumer");
            Func<IComponentContext, IEventConsumer> localEventConsumerResolver = context =>
                context.ResolveNamed<IEventConsumer>($"{ThisConfiguration.Type}.LocalEventConsumer");

            builder.Register(x => serviceInitializer(commandRedirectingConsumerResolver(x), 
                    eventRedirectingConsumerResolver(x), 
                    localCommandConsumerResolver(x), 
                    localEventConsumerResolver(x)))
                .As<IService>()
                .SingleInstance();
        }

        protected IDomainRepository ResolveGlobalDomainRepository(IComponentContext context) => context.Resolve<IDomainRepository>();

        protected ICommandSender ResolveGlobalCommandSender(IComponentContext context) => context.Resolve<IEnumerable<Lazy<ICommandSender, NamedMetadata>>>().First(p => p.Metadata.Name == "CommandSender").Value;

        protected IEventPublisher ResolveGlobalEventPublisher(IComponentContext context) => context.Resolve<IEnumerable<Lazy<IEventPublisher, NamedMetadata>>>().First(p => p.Metadata.Name == "EventPublisher").Value;

        protected IMessageSubscriber ResolveGlobalCommandSubscriber(IComponentContext context) => context.ResolveNamed<IMessageSubscriber>("CommandSubscriber");

        protected IMessageSubscriber ResolveGlobalEventSubscriber(IComponentContext context) => context.ResolveNamed<IMessageSubscriber>("EventSubscriber");

        protected ITableDataGateway ResolveTableDataGateway(IComponentContext context) => context.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == $"{ThisConfiguration.Type}.TableDataGateway").Value;

        protected virtual Func<IComponentContext, ITableDataGateway> TableDataGatewayInitializer => null;

        protected virtual IEnumerable<Func<IComponentContext, ICommandHandler>> CommandHandlersInitializer => null;

        protected virtual IEnumerable<Func<IComponentContext, IDomainEventHandler>> EventHandlersInitializer => null;

        protected abstract Func<IMessageConsumer, IMessageConsumer, ICommandConsumer, IEventConsumer, TService> ServiceInitializer { get; }

    }
}
