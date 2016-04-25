// ---------------------------------------------------------------------------------------------------------------                                                                                       
//                                                                                                    
//     XNSPZ.    qXFXZ:   LPPN@:             N0kXSk5kSPkPqM:                                 .        
//     @B@B@.   B@B@B@7   B@B@B             r@@@B@B@B@B@B@B                              UM@@@        
//     B@B@B   G@B@B@B:  B@B@@              uBMEqB@B@@E8MMS                              B@@@J        
//     BB@B@  L@B@O@@@: :@@@B    r8@B@B@Mi       @B@B5       :P@B@B@BY  L@B@BB   @B@B@u@B@B@B@B@      
//     G@B@B  @@@iL@@B, @B@B.  :@B@B2j@B@@i     EB@B@.      @B@BN7@@@BX  @@B@B  @B@BO O@@B@B@B@E      
//     0B@BN @B@M J@B@ rB@B7  ,@B@B. :B@@@:     B@B@B      @@@B7  B@B@S   B@B@M@B@B,   :B@B@r         
//     F@B@2E@@B  UB@BvB@Bq   @B@B@B@B@BO:     r@B@@L     XB@B@B@B@BB7    vB@B@B@B     F@B@B          
//     5@@B@B@B.  v@B@B@B@    B@B@O            @B@B@      M@B@B:         Z@@B@B@B@i    @B@BM          
//     2@B@B@B;   vB@B@B@     2B@@@J:iPBZ     .B@@@@      :B@@@q:i2B@  7@B@BB @@@B@.  .B@B@B@B        
//     uB@B@@X    r@B@B@,      rB@B@@@B@.     X@B@Br       ,M@B@B@B@; M@B@BO  i@B@B@   XB@B@BM   
//
// WeText - A simple application that demonstrates the DDD, CQRS, Event Sourcing and Microservices architecture.
//
// Copyright (C) by daxnet 2016
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ---------------------------------------------------------------------------------------------------------------

using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Represents that the derived types are microservice registers.
    /// </summary>
    /// <typeparam name="TService">The type of the microservice.</typeparam>
    /// <seealso cref="Autofac.Module" />
    public abstract class MicroserviceRegister<TService> : Module
        where TService : Microservice
    {
        private readonly WeTextConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicroserviceRegister{TService}"/> class.
        /// </summary>
        /// <param name="configuration">The instance of WeText configuration.</param>
        protected MicroserviceRegister(WeTextConfiguration configuration)
        {
            this.configuration = configuration;
        }


        /// <summary>
        /// Gets the WeText configuration specific for the current service.
        /// </summary>
        /// <value>
        /// The configuration instance.
        /// </value>
        protected ServiceElement ThisConfiguration => this.configuration.Services.GetItemByKey(typeof(TService).FullName);

        /// <summary>
        /// Resolves the instance of <see cref="IDomainRepository"/> that has been registered globally from the Autofac container.
        /// </summary>
        /// <param name="context">The <see cref="IComponentContext"/> instance that handles the Autofac registration.</param>
        /// <returns></returns>
        protected IDomainRepository ResolveGlobalDomainRepository(IComponentContext context) => context.Resolve<IDomainRepository>();

        protected ICommandSender ResolveGlobalCommandSender(IComponentContext context) => context.Resolve<ICommandSender>();

        protected IEventPublisher ResolveGlobalEventPublisher(IComponentContext context) => context.Resolve<IEventPublisher>();

        protected ITableDataGateway ResolveTableDataGateway(IComponentContext context) => context.Resolve<IEnumerable<Lazy<ITableDataGateway, NamedMetadata>>>().First(p => p.Metadata.Name == $"{ThisConfiguration.Type}.TableDataGateway").Value;

        protected virtual Func<IComponentContext, ITableDataGateway> TableDataGatewayInitializer => null;

        protected virtual IEnumerable<Func<IComponentContext, ICommandHandler>> CommandHandlersInitializer => null;

        protected virtual IEnumerable<Func<IComponentContext, IDomainEventHandler>> EventHandlersInitializer => null;

        protected abstract Func<ICommandConsumer, IEventConsumer, TService> ServiceInitializer { get; }

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            this.RegisterTableDataGateway(builder, this.TableDataGatewayInitializer);
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
            var commandQueueExchangeName = ThisConfiguration?.LocalCommandQueue?.ExchangeName;
            var commandQueueName = ThisConfiguration?.LocalCommandQueue?.QueueName;

            if (string.IsNullOrEmpty(commandQueueName) ||
                string.IsNullOrEmpty(commandQueueExchangeName) ||
                string.IsNullOrEmpty(commandQueueName))
            {
                throw new ServiceRegistrationException("Either of the settings for Command Queue is empty (HostName, ExchangeName or QueueName).");
            }

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
                .Register(x => new CommandConsumer(x.ResolveNamed<IMessageSubscriber>("CommandSubscriber",
                                new NamedParameter("hostName", commandQueueHostName), 
                                new NamedParameter("exchangeName", commandQueueExchangeName), 
                                new NamedParameter("queueName", commandQueueName)
                            ),
                        commandHandlersResolver(x)))
                .Named<ICommandConsumer>($"{ThisConfiguration.Type}.LocalCommandConsumer");
        }

        private void RegisterLocalEventConsumer(ContainerBuilder builder)
        {
            var eventQueueHostName = ThisConfiguration?.LocalEventQueue?.HostName;
            var eventQueueExchangeName = ThisConfiguration?.LocalEventQueue?.ExchangeName;
            var eventQueueName = ThisConfiguration?.LocalEventQueue?.QueueName;

            if (string.IsNullOrEmpty(eventQueueHostName) ||
                string.IsNullOrEmpty(eventQueueExchangeName) ||
                string.IsNullOrEmpty(eventQueueName))
            {
                throw new ServiceRegistrationException("Either of the settings for Command Queue is empty (HostName, ExchangeName or QueueName).");
            }

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
                .Register(x => new EventConsumer(x.ResolveNamed<IMessageSubscriber>("EventSubscriber",
                                new NamedParameter("hostName", eventQueueHostName),
                                new NamedParameter("exchangeName", eventQueueExchangeName),
                                new NamedParameter("queueName", eventQueueName)
                            ),
                            eventHandlersResolver(x)))
                .Named<IEventConsumer>($"{ThisConfiguration.Type}.LocalEventConsumer");
        }

        private void RegisterService(ContainerBuilder builder,
            Func<ICommandConsumer, IEventConsumer, TService> serviceInitializer)
        {
            Func<IComponentContext, ICommandConsumer> localCommandConsumerResolver = context =>
                context.ResolveNamed<ICommandConsumer>($"{ThisConfiguration.Type}.LocalCommandConsumer");
            Func<IComponentContext, IEventConsumer> localEventConsumerResolver = context =>
                context.ResolveNamed<IEventConsumer>($"{ThisConfiguration.Type}.LocalEventConsumer");

            builder.Register(x => serviceInitializer(localCommandConsumerResolver(x), localEventConsumerResolver(x)))
                .As<IService>()
                .SingleInstance();
        }
    }
}
