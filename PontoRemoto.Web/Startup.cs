using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PontoRemoto.Web.Startup))]
namespace PontoRemoto.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
