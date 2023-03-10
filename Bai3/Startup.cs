using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Bai3.Startup))]
namespace Bai3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
