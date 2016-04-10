using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeText.Web.Startup))]
namespace WeText.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
