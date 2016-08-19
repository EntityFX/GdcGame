using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EntityFX.Gdcame.Presentation.Web.WebApp.GdcameStartup))]

namespace EntityFX.Gdcame.Presentation.Web.WebApp
{
    public class GdcameStartup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
