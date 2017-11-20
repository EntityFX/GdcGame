using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace EntityFX.Gdcame.Utils.Common
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    using DataProtectionProviderDelegate = Func<string[], Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>>;
    using DataProtectionTuple = Tuple<Func<byte[], byte[]>, Func<byte[], byte[]>>;
    /*
    public static class CoreApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAppBuilder(this IApplicationBuilder app, Action<IAppBuilder> configure)
        {
            app.UseOwin(addToPipeline =>
            {
                addToPipeline(next =>
                {
                    var appBuilder = new AppBuilder();
                    appBuilder.Properties["builder.DefaultApp"] = next;

                    configure(appBuilder);

                    return appBuilder.Build<AppFunc>();
                });
            });
            return app;
        }

        public static IAppBuilder SetDataProtectionProvider(this IAppBuilder appBuilder, IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.DataProtection.IDataProtectionProvider>();
            appBuilder.Properties["security.DataProtectionProvider"] = new DataProtectionProviderDelegate(purposes =>
            {
                var dataProtection = provider.CreateProtector(string.Join(",", purposes));
                return new DataProtectionTuple(dataProtection.Protect, dataProtection.Unprotect);
            });
            return appBuilder;
        }
    }*/
}
