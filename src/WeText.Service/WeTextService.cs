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

namespace WeText.Service
{
    internal sealed class WeTextService : Common.Services.Service
    {
        private const string SearchPath = "services";
        private static List<IService> microServices = new List<IService>();

        static void DiscoverServices(ContainerBuilder builder)
        {
            var searchFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), SearchPath);
            foreach (var file in Directory.EnumerateFiles(searchFolder, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    var exportedTypes = assembly.GetExportedTypes();
                    if (exportedTypes.Any(t=>t.IsSubclassOf(typeof(Autofac.Module))))
                    {
                        builder.RegisterAssemblyModules(assembly);
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
            builder.Register(x => new RabbitMqCommandSender("localhost", "WeTextCommandExchange")).As<ICommandSender>();
            builder.Register(x => new RabbitMqEventPublisher("localhost", "WeTextEventExchange")).As<IEventPublisher>();
            builder.Register(x => new RabbitMqMessageSubscriber("localhost", "WeTextCommandExchange")).Named<IMessageSubscriber>("CommandSubscriber");
            builder.Register(x => new RabbitMqMessageSubscriber("localhost", "WeTextEventExchange")).Named<IMessageSubscriber>("EventSubscriber");
            builder.Register(x => new MongoDomainRepository(x.Resolve<IEventPublisher>())).As<IDomainRepository>();

            // Discovers the services.
            DiscoverServices(builder);

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
            var url = "http://+:9023/";
            using (WebApp.Start<WeTextService>(url: url))
            {
                microServices.ForEach(ms => ms.Start(args));
                Console.WriteLine("Service started.");
                Console.ReadLine();
            }
        }
    }
}
