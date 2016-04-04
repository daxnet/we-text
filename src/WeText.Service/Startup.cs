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

namespace WeText.Service
{
    public class Startup
    {
        const string SearchPath = "services";

        static void LoadServices(ContainerBuilder builder)
        {
            var searchFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), SearchPath);
            foreach (var file in Directory.EnumerateFiles(searchFolder, "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    var exportedTypes = assembly.GetExportedTypes();
                    if (exportedTypes.Any(t => typeof(IService).IsAssignableFrom(t)))
                    {

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
            builder.Register(x => new RabbitMqCommandBus("localhost", "wetext_command_exchange")).As<ICommandBus>();
            builder.Register(x => new RabbitMqEventBus("localhost", "wetext_event_exchange")).As<IEventBus>();
            builder.Register(x => new MongoDomainRepository(x.Resolve<IEventBus>())).As<IDomainRepository>();

            // Loads the microservices.
            LoadServices(builder);

            var container = builder.Build();
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
    }
}
