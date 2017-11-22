using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.Extensions.DependencyModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    using EntityFX.Gdcame.Utils.Common;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;


    public class Startup : CoreStartupBase
    {


        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();

            return base.ConfigureServices(services);


        }

        public override void Configure(IApplicationBuilder app)
        {
            /*app.UseAppBuilder(appBuilder =>
            {
                appBuilder.SetDataProtectionProvider(app);
                new WebApiStartup(Container, AppConfiguration).Configuration(appBuilder);
                appBuilder.UseNancy(new Nancy.Owin.NancyOptions() { Bootstrapper = new NancyWebAppBootstrapper() });
            });*/

            var options = new JwtBearerOptions
            {

                TokenValidationParameters = {
                    ValidIssuer = "ExampleIssuer",
                    ValidAudience = "ExampleAudience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Gdcame")),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }
            };

            app.UseJwtBearerAuthentication(options);

            base.Configure(app);
        }
    }
}
