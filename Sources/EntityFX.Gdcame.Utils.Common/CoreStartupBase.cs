using EntityFX.Gdcame.Infrastructure.Common;

namespace EntityFX.Gdcame.Utils.Common
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// The core startup base.
    /// </summary>
    public abstract class CoreStartupBase
    {
        /// <summary>
        /// Gets or sets the app configuration.
        /// </summary>
        public static AppConfiguration AppConfiguration { get; set; }

        public static IIocContainer Container { get; set; }

        public abstract void ConfigureServices(IServiceCollection services);
        public abstract void Configure(IApplicationBuilder app);
    }
}