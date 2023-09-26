using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Senao_EE.Startup))]
namespace Senao_EE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
