using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TPS2.Startup))]
namespace TPS2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
