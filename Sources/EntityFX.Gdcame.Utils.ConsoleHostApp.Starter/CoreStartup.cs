using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using EntityFX.Gdcame.Application.Api.Common;
using EntityFX.Gdcame.Application.Api.Common.Providers;
using Microsoft.Extensions.DependencyModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer
{
    using EntityFX.Gdcame.Utils.Common;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;


    public class Startup : CoreStartupBase
    {


        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<UserIdentity, UserIdentityRole>()
                .UseGameUserStoreAdaptor()
                .AddDefaultTokenProviders();

            services.AddAuthentication();
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

            var issuuerOptions = new JwtIssuerOptions();

            var options = new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = {
                    ValidIssuer = issuuerOptions.Issuer,
                    ValidAudience = issuuerOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuuerOptions.SecretKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme
            };
            app.UseIdentity();
            app.UseJwtBearerAuthentication(options);
            base.Configure(app);
        }
    }
}
