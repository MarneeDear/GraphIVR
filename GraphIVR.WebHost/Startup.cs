using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraphIVR.Startup))]
namespace GraphIVR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
