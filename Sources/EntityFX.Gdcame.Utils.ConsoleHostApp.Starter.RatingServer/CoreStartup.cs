using System;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    public class CoreStartup : CoreStartupBase
    {

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            return base.ConfigureServices(services);
        }
    }
}
