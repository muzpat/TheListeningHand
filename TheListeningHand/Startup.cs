using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheListeningHand.Startup))]
namespace TheListeningHand
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
