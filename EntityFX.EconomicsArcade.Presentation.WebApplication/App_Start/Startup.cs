using System.Threading.Tasks;
using EntityFX.EconomicsArcade.Presentation.WebApplication;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EntityFX.EconomicsArcade.Presentation.WebApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration()
            {
                EnableDetailedErrors = true,
                EnableJSONP = true
            });
        }
    }


}
