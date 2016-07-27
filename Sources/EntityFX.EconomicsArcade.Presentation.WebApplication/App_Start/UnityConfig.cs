using System;
using System.Configuration;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Presentation.Models;
using EntityFX.EconomicsArcade.Presentation.Models.Mappers;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Contract.Manager.RatingManager;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Model.Common.Mappers;
using EntityFX.EconomicsArcade.Model.Common.Model;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Controllers;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Factories;
using EntityFX.EconomicsArcade.Presentation.WebApplication.Providers;
using EntityFX.EconomicsArcade.Utils.ClientProxy.Manager;
using Microsoft.Practices.Unity.InterceptionExtension;
using PortableLog.NLog;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication
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

            container.RegisterType<IGameApiController, GameApiController>();
            container.RegisterType<IRatingApiController, RatingApiController>();

            container.RegisterType<IGameManager, GameManagerClient<NetTcpProxy<IGameManager>>>(
                new InjectionConstructor(
                    new ResolvedParameter<ILogger>(),
                    ConfigurationManager.AppSettings["ManagerEndpointAddress_GameManager"], typeof(Guid))
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
                ConfigurationManager.AppSettings["ManagerEndpointAddress_SessionManager"], new ResolvedParameter<Guid>())
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>()
                    );
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