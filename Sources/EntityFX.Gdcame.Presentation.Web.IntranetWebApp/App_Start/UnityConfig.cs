using System;
using System.Configuration;
using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Funds;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Common.Presentation.Model.Mappers;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.Infrastructure.Service.NetTcp;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using EntityFX.Gdcame.Manager.Contract.SessionManager;
using EntityFX.Gdcame.Manager.Contract.UserManager;
using EntityFX.Gdcame.Presentation.Web.Controller;
using EntityFX.Gdcame.Presentation.Web.Controllers;
using EntityFX.Gdcame.Presentation.Web.Model;
using EntityFX.Gdcame.Presentation.Web.Model.Mappers;
using EntityFX.Gdcame.Presentation.Web.Providers.Providers;
using EntityFX.Gdcame.Utils.ClientProxy.Manager;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.Gdcame.Presentation.Web.IntranetWebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents(IUnityContainer container)
        {
            //container.AddNewExtension<Interception>();

            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            container.RegisterType<IOperationContextHelper, WcfOperationContextHelper>();

            container.RegisterType<IGameApiController, GameApiController>();
            container.RegisterType<IRatingApiController, RatingApiController>();
            var isCollapsed = true;
            if (!isCollapsed)
            {
                container.RegisterType<IGameManager, GameManagerClient<NetTcpProxy<IGameManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<ILogger>(),
                        new ResolvedParameter<IOperationContextHelper>(),
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], typeof (Guid))
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISimpleUserManager, SimpleUserManagerClient<NetTcpProxy<ISimpleUserManager>>>(
                    new InjectionConstructor(
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_UserManager"])
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container.RegisterType<IRatingManager, RatingManagerClient<NetTcpProxy<IRatingManager>>>(
                    new InjectionConstructor(
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_RatingManager"]),
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISessionManager, SessionManagerClient<NetTcpProxy<ISessionManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<IOperationContextHelper>(),
                        ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"],
                        new ResolvedParameter<Guid>())
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
            }
            else
            {
                container.RegisterType<IGameManager, GameManagerClient<NetNamedPipeProxy<IGameManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<ILogger>(),
                        new ResolvedParameter<IOperationContextHelper>(),
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.GameManager.IGameManager", typeof(Guid))
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISimpleUserManager, SimpleUserManagerClient<NetNamedPipeProxy<ISimpleUserManager>>>(
                    new InjectionConstructor(
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.UserManager.ISimpleUserManager")
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );

                container.RegisterType<IRatingManager, RatingManagerClient<NetNamedPipeProxy<IRatingManager>>>(
                    new InjectionConstructor(
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.RatingManager.IRatingManager"),
                    new Interceptor<InterfaceInterceptor>(),
                    new InterceptionBehavior<LoggerInterceptor>()
                    );
                container.RegisterType<ISessionManager, SessionManagerClient<NetNamedPipeProxy<ISessionManager>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<IOperationContextHelper>(),
                        "net.pipe://localhost/EntityFX.Gdcame.Manager.Contract.SessionManager.ISessionManager",
                        new ResolvedParameter<Guid>())
                    , new Interceptor<InterfaceInterceptor>()
                    , new InterceptionBehavior<LoggerInterceptor>()
                    );
            }
            container.RegisterType<IMapper<FundsCounters, FundsCounterModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<FundsDriver, FundsDriverModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();
            container.RegisterType<IMapper<BuyFundDriverResult, BuyDriverModel>, FundsDriverBuyInfoMapper>();
            container.RegisterType<IGameClientFactory, GameClientFactory>();
            container.RegisterType<ISessionManagerClientFactory, SessionManagerClientFactory>();
            container.RegisterType<IGameDataProvider, GameDataProvider>();
        }
    }
}