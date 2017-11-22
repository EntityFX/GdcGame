using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EntityFX.Gdcame.Utils.ConsoleHostApp.AllInOne.MainServerNetCore
{
    internal class Startup1
    {
        public virtual void ConfigureServices(IServiceCollection services) { 

            /*.AddControllersAsServices();

            //services.AddTransient(ctx =>
            //    new TstController());*/

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            //});


            //return PrepareContainer?.Invoke(IocContainer, services);
            //return services.BuildServiceProvider();
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
               // app.UseDeveloperExceptionPage();
               // app.UseBrowserLink();
            }

            //app.UseSwagger();

            //// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //});
            app.UseMvc();
            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });

            //app.UseExceptionHandler(new ExceptionHandlerOptions()
            //{
            //    ExceptionHandler = context =>
            //    {
            //        return Task.FromResult(0);
            //    }
            //});
        }
    }
}