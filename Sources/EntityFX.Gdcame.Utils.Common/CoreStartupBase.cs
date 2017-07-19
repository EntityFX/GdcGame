namespace EntityFX.Gdcame.Utils.Common
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// The core startup base.
    /// </summary>
    public abstract class CoreStartupBase
    {
        /// <summary>
        /// Gets or sets the app configuration.
        /// </summary>
        public static AppConfiguration AppConfiguration { get; set; }

        public static IUnityContainer Container { get; set; }

        public abstract void ConfigureServices(IServiceCollection services);
        public abstract void Configure(IApplicationBuilder app);
    }
}