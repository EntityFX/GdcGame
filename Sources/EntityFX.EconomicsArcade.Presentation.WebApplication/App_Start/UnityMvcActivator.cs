using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Factories;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(EntityFX.EconomicsArcade.Presentation.WebApplication.App_Start.UnityWebActivator), "Start")]


namespace EntityFX.EconomicsArcade.Presentation.WebApplication.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with ASP.NET MVC.</summary>
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            var container = new UnityContainer();
            UnityConfig.RegisterComponents(container);
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(container));
            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler), container.Resolve<InvalidSessionExceptionHandler>());
            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

    }
}