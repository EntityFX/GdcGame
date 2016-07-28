using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(EntityFX.EconomicsArcade.Presentation.GameApi.Startup))]

namespace EntityFX.EconomicsArcade.Presentation.GameApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
