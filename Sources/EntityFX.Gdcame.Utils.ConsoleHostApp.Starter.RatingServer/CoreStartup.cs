using System;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    public class CoreStartup : CoreStartupBase
    {

        public override void Configure(IApplicationBuilder app)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            return base.ConfigureServices(services);
        }
    }
}
