namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    using EntityFX.Gdcame.Utils.Common;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using Owin;

    public class CoreStartup : CoreStartupBase
    {


        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseAppBuilder(appBuilder =>
            {
                appBuilder.SetDataProtectionProvider(app);
                new WebApiStartup(Container, AppConfiguration).Configuration(appBuilder);
                appBuilder.UseNancy(new Nancy.Owin.NancyOptions() { Bootstrapper = new NancyWebAppBootstrapper() });
            });
        }
    }
}
