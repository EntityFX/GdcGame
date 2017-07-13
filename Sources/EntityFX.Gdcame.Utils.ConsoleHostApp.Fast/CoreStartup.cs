using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Fast.MainServer
{
    public class CoreStartup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAppBuilder(appBuilder =>
            {
                appBuilder.SetDataProtectionProvider(app);
                new WebApiStartup().Configuration(appBuilder);
            });
        }
    }
}
