namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    using System;
    using System.Linq;

    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Application.Api.Common.Mappers;
    using EntityFX.Gdcame.Application.Api.Controller.RatingServer;
    using EntityFX.Gdcame.Application.Contract.Controller.Common;
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Common.Contract.UserRating;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Contract.Common;
    using EntityFX.Gdcame.Utils.Common;
    using EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.MainServer;
    using EntityFX.Gdcame.Utils.Hashing;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    using PortableLog.NLog;

    public class ContainerBootstrapper : IContainerBootstrapper
    {

        private readonly AppConfiguration _appConfiguration;

        public ContainerBootstrapper(AppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
        }


        public IUnityContainer Configure(IUnityContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            var childBootstrappers = this.GetRepositoryProviders(this._appConfiguration.RepositoryProvider).Concat(new IContainerBootstrapper[]
                        {
 
                new EntityFX.Gdcame.DataAccess.Service.RatingServer.ContainerBootstrapper(),
                new Manager.Common.ContainerBootstrapper()
                        }).ToArray();
            Array.ForEach(childBootstrappers, _ => _.Configure(container));
            container.AddNewExtension<Interception>();

            container.RegisterType<IMapperFactory, MapperFactory>();

            /////

            container.RegisterType<ITaskTimerFactory, TaskTimerFactory>();
            container.RegisterType<ITaskTimer, GenericTaskTimer>();

            container.RegisterType<IHashHelper, HashHelper>();
            container.RegisterInstance<IPerformanceHelper>(new PerformanceHelper());




            /* if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
             {
                 container.Configure<Interception>()
                     .AddPolicy("logging")
                     .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                     .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.Gdcame.*", true));
             }*/


            container.RegisterType<IMapper<TopRatingStatistics, TopRatingStatisticsModel>, TopRatingStatisticsModelMapper>();

          //  container.RegisterType<IRatingController, RatingController>();
            container.RegisterType<IRatingController, RatingController>();
            container.RegisterType<IServerController, ServerController>();

            container.RegisterType<IOperationContextHelper, NoWcfOperationContextHelper>();


            
            return container;
        }

        private IContainerBootstrapper[] GetRepositoryProviders(string providerName)
        {
            return new[]
                       {  new CommonDatabasesProvider(providerName)
                               .GetRepositoryProvider(_appConfiguration.MongoConnectionString),
                          new RatingServerDatabasesProvider(providerName)
                               .GetRepositoryProvider(_appConfiguration.MongoConnectionString)
                       };
        }
    }
}