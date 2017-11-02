namespace EntityFX.Gdcame.Utils.ConsoleHostApp.Starter.RatingServer
{
    using System;
    using System.Linq;

    using EntityFX.Gdcame.Application.Api.Common;
    using EntityFX.Gdcame.Application.Api.Common.Mappers;
    using EntityFX.Gdcame.Application.Api.Common.Providers;
    using EntityFX.Gdcame.Application.Api.Controller.RatingServer;
    using EntityFX.Gdcame.Application.Contract.Controller.Common;
    using EntityFX.Gdcame.Common.Application.Model;
    using EntityFX.Gdcame.Contract.Common.Statistics;
    using EntityFX.Gdcame.Contract.Common.UserRating;
    using EntityFX.Gdcame.Engine.Common;
    using EntityFX.Gdcame.Engine.Contract.Common;
    using EntityFX.Gdcame.Engine.Contract.RatingServer;
    using EntityFX.Gdcame.Infrastructure.Common;
    using EntityFX.Gdcame.Manager.Common;
    using EntityFX.Gdcame.Manager.Contract.Common;
    using EntityFX.Gdcame.Manager.Contract.Common.AdminManager;
    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;
    using EntityFX.Gdcame.Utils.Common;
    using EntityFX.Gdcame.Utils.Hashing;
    using EntityFX.Gdcame.Utils.RatingServer;

    using Microsoft.AspNet.Identity;
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


        public IIocContainer Configure(IIocContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ILogger>(() => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger"))));

            var childBootstrappers = this.GetRepositoryProviders(this._appConfiguration.RepositoryProvider).Concat(new IContainerBootstrapper[]
                        {
                new EntityFX.Gdcame.DataAccess.Service.Common.ContainerBootstrapper(),
                new EntityFX.Gdcame.DataAccess.Service.RatingServer.ContainerBootstrapper(),
                new Manager.Common.ContainerBootstrapper()
                        }).ToArray();
            Array.ForEach(childBootstrappers, _ => _.Configure(container));

            container.RegisterType<IMapperFactory, MapperFactory>();

            container.RegisterType<IMapper<StatisticsInfo, ServerStatisticsInfoModel>, StatisticsInfoMapper<StatisticsInfo, ServerStatisticsInfoModel>>();

            container.RegisterType<IPerformanceHelper>(() => new PerformanceHelper(), ContainerScope.Singleton);

            container.RegisterType(() => new SystemInfo()
            {
                CpusCount = Environment.ProcessorCount,
                Os = Environment.OSVersion.ToString(),
                Runtime = RuntimeHelper.GetRuntimeName(),
                MemoryTotal = RuntimeHelper.GetTotalMemoryInMb()
            }, ContainerScope.Singleton);

            container.RegisterType<ITaskTimerFactory, TaskTimerFactory>();
            container.RegisterType<ITaskTimer, GenericTaskTimer>();

            container.RegisterType<IHashHelper, HashHelper>();

            container.RegisterType<IAdminManager<StatisticsInfo>, AdminManagerBase<StatisticsInfo>>();


            container.RegisterType<ISimpleUserManager, SimpleUserManager>();

            container.RegisterType<ISessions>(() => new SessionsProvider(container.Resolve<ILogger>()), ContainerScope.Singleton);

            container.RegisterType<ISessionManager, SessionManager>();
            container.RegisterType<ISessionManagerClientFactory, SessionManagerClientFactory>();
            container.RegisterType<INodeRatingClientFactory, NodeRatingClientFactory>();


            /* if (ConfigurationManager.AppSettings["UseLoggerInterceptor"] == "True")
             {
                 container.Configure<Interception>()
                     .AddPolicy("logging")
                     .AddCallHandler<LoggerCallHandler>(new ContainerControlledLifetimeManager())
                     .AddMatchingRule<NamespaceMatchingRule>(new InjectionConstructor("EntityFX.Gdcame.*", true));
             }*/


          //  container.RegisterType<IRatingController, RatingController>();
            container.RegisterType<IRatingController, RatingController>();
            container.RegisterType<IServerController, ServerController>();
            container.RegisterType<IStatisticsInfo<ServerStatisticsInfoModel>, AdminController>();

            container.RegisterType<IOperationContextHelper, NoWcfOperationContextHelper>();

            container.RegisterType<ApplicationUserManagerFacotory>();
            container.RegisterType<IUserStore<UserIdentity>, GameUserStore>();
            container.RegisterType<UserManager<UserIdentity>, ApplicationUserManager>();
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