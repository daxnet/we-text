using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Web.Http;
using WeText.Common.Messaging;
using WeText.Common.Repositories;
using WeText.DomainRepositories;
using WeText.Messaging.RabbitMq;
using WeText.Common.Services;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using WeText.Common;
using WeText.Common.Config;
using WeText.Services.Common;

namespace WeText.Service
{
    internal sealed class WeTextService : Common.Services.Service
    {
        private readonly WeTextConfiguration configuration = WeTextConfiguration.Instance;

        private const string SearchPath = "services";
        private static List<IService> microServices = new List<IService>();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        private void DiscoverServices(ContainerBuilder builder)
        {
            var searchFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), SearchPath);
            foreach (var file in Directory.EnumerateFiles(searchFolder, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    var exportedTypes = assembly.GetExportedTypes();
                    var microserviceRegisterType = exportedTypes.FirstOrDefault(x => x.IsSubclassOf(typeof(Autofac.Module)) &&
                        x.BaseType.GetGenericTypeDefinition() == typeof(MicroserviceRegister<>));
                    if (microserviceRegisterType != null)
                    {
                        var registeringService = microserviceRegisterType.BaseType.GetGenericArguments().First();
                        if (configuration.Services.All(x=>x.Type != registeringService.FullName))
                        {
                            continue;
                        }
                        var mod = (Autofac.Module)Activator.CreateInstance(microserviceRegisterType, configuration);
                        builder.RegisterModule(mod);
                    }

                    if (exportedTypes.Any(t => t.IsSubclassOf(typeof(ApiController))))
                    {
                        builder.RegisterApiControllers(assembly).InstancePerRequest();
                    }
                }
                catch { }
            }
        }

        public void Configuration(IAppBuilder app)
        {
            
            var builder = new ContainerBuilder();

            builder.RegisterInstance<WeTextConfiguration>(this.configuration).SingleInstance();

            builder.Register(x => new RabbitMqCommandSender(configuration.CommandQueue.ConnectionUri, configuration.CommandQueue.ExchangeName))
                .As<ICommandSender>();

            builder.Register(x => new RabbitMqEventPublisher(configuration.EventQueue.ConnectionUri, configuration.EventQueue.ExchangeName))
                .As<IEventPublisher>();

            builder.RegisterType<RabbitMqMessageSubscriber>()
                .Named<IMessageSubscriber>("CommandSubscriber");

            builder.RegisterType<RabbitMqMessageSubscriber>()
                .Named<IMessageSubscriber>("EventSubscriber");

            var mongoSetting = new MongoSetting
            {
                ConnectionString = configuration.MongoEventStore.ConnectionString,
                CollectionName = configuration.MongoEventStore.CollectionName,
                DatabaseName = configuration.MongoEventStore.Database
            };
            builder.Register(x => new MongoDomainRepository(mongoSetting, x.Resolve<IEventPublisher>())).As<IDomainRepository>();

            // Discovers the services.
            DiscoverServices(builder);

            // Register the API controllers within the current assembly.
            builder.RegisterApiControllers(this.GetType().Assembly);

            // Create the container by builder.
            var container = builder.Build();

            // Register the services.
            microServices.AddRange(container.Resolve<IEnumerable<IService>>());

            app.UseAutofacMiddleware(container);

            HttpConfiguration config = new HttpConfiguration();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }

        public override void Start(object[] args)
        {
            // Validate the applicatoin configuration
            if (string.IsNullOrEmpty(configuration?.ApplicationSetting?.Url))
            {
                throw new WeTextConfigurationException("Url is not specified in the configuraiton.");
            }

            if (string.IsNullOrEmpty(configuration?.CommandQueue?.ConnectionUri))
            {
                throw new WeTextConfigurationException("ConnectionUri of the Command Queue is not specified in the configuraiton.");
            }

            if (string.IsNullOrEmpty(configuration?.CommandQueue?.ExchangeName))
            {
                throw new WeTextConfigurationException("ExchangeName of the Command Queue is not specified in the configuraiton.");
            }

            if (string.IsNullOrEmpty(configuration?.EventQueue?.ConnectionUri))
            {
                throw new WeTextConfigurationException("ConnectionUri of the Event Queue is not specified in the configuraiton.");
            }

            if (string.IsNullOrEmpty(configuration?.EventQueue?.ExchangeName))
            {
                throw new WeTextConfigurationException("ExchangeName of the Event Queue is not specified in the configuraiton.");
            }

            var url = configuration.ApplicationSetting.Url;
            log.Info("Starting WeText Service...");
            using (WebApp.Start<WeTextService>(url: url))
            {
                microServices.ForEach(ms =>
                {
                    log.Info($"Starting microservice '{ms.GetType().FullName}'...");
                    ms.Start(args);
                });
                log.Info("WeText Service started successfully.");
                Console.ReadLine();
            }
        }
    }
}
