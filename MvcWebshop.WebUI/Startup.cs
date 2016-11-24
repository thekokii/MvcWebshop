using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcWebshop.WebUI.Startup))]
namespace MvcWebshop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
