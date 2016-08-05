using System;
using System.Configuration;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.Gdcame.Utils.ServiceStarter.DataAccess
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {

        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            var childBootstrappers = new IContainerBootstrapper[]
            {
                new Gdcame.DataAccess.Repository.ContainerBootstrapper(),
                new Gdcame.DataAccess.Service.ContainerBootstrapper()
            };

            childBootstrappers.ForEach(_ => _.Configure(container));
            container.AddNewExtension<Interception>();

            container.RegisterType<ILogger>(new InjectionFactory(
               _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
            {
                container.Configure<Interception>()
                .AddPolicy("logging")
                .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.Gdcame.*", true));
            }

            if (!Environment.UserInteractive)
            {
                container.RegisterType<IServiceInfoHelper, ServiceInfoHelperLogger>();
            }
            else
            {
                container.RegisterType<IServiceInfoHelper, ServiceInfoHelperConsole>();
            }

            return container;
        }
    }
}
