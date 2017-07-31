using EntityFX.Gdcame.Utils.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    public class CoreStartup : CoreStartupBase
    {

        public override void Configure(IApplicationBuilder app)
        {
            app.UseAppBuilder(appBuilder =>
            {
                appBuilder.SetDataProtectionProvider(app);
                new WebApiStartup(Container, AppConfiguration).Configuration(appBuilder);
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
        }
    }
}
