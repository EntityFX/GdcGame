using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using EntityFX.Gdcame.Infrastructure.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

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


        public static IIocContainer IocContainer { get; set; }

        public static event Func<IIocContainer, IServiceCollection, IServiceProvider> PrepareContainer;

        public static Assembly[] LoadedAssemblies
        {
            get;
            set;
        }

        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {

            var builder = services.AddMvc().AddJsonOptions((options =>
            {
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
            }));

            foreach (var assembly in LoadedAssemblies)
            {
                builder.AddApplicationPart(assembly);
            }

            return PrepareContainer?.Invoke(IocContainer, services);
        }

        public virtual void Configure(IApplicationBuilder app)
        {

            app.UseMvc();
            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });
        }

        public class ServiceBasedControllerActivator : IControllerActivator
        {
            public object Create(ControllerContext actionContext)
            {
                var controllerType = actionContext.ActionDescriptor.ControllerTypeInfo.AsType();

                return actionContext.HttpContext.RequestServices.GetRequiredService(controllerType);
            }

            public virtual void Release(ControllerContext context, object controller)
            {
            }
        }

        public class StatusCodeExceptionHandler
        {
            private readonly RequestDelegate request;

            public StatusCodeExceptionHandler(RequestDelegate pipeline)
            {
                this.request = pipeline;
            }

            public Task Invoke(HttpContext context) => this.InvokeAsync(context); // Stops VS from nagging about async method without ...Async suffix.

            async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await this.request(context);
                }
                catch (Exception exception)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Headers.Clear();
                }
            }
        }




    }

    public static class CustomControllersActivator
    {
        public static IMvcBuilder AddControllersAsServices(this IMvcBuilder builder)
        {
            var feature = new ControllerFeature();
            builder.PartManager.PopulateFeature(feature);

            foreach (var controller in feature.Controllers.Select(c => c.AsType()))
            {
                builder.Services.TryAddTransient(controller, controller);
            }

            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            return builder;
        }
    }
}