using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SOWA.Startup))]
namespace SOWA
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
