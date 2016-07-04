using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EntityFX.EconomicsArcade.Presentation.WebApplication.Startup))]

namespace EntityFX.EconomicsArcade.Presentation.WebApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
