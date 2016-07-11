using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EntityFX.EconomicsArcade.Contract.Manager.SessionManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Manager;
using Microsoft.Practices.Unity;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Manager.Mappers;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Incrementors;
using EntityFX.EconomicsArcade.Contract.DataAccess.GameData;
using EntityFX.EconomicsArcade.Contract.DataAccess.User;
using EntityFX.EconomicsArcade.Contract.Game.Counters;
using EntityFX.EconomicsArcade.Contract.Game.Funds;
using EntityFX.EconomicsArcade.Contract.Game.Incrementors;
using EntityFX.EconomicsArcade.Contract.Manager.UserManager;
using EntityFX.EconomicsArcade.Utils.ClientProxy.DataAccess;
using PortableLog.NLog;
using System.Configuration;
using System.Linq;


using EntityFX.EconomicArcade.Engine.GameEngine.Mappers;
using EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine;
using EntityFX.EconomicsArcade.Contract.NotifyConsumerService;
using EntityFX.EconomicsArcade.Infrastructure.Service.Logger;
using EntityFX.EconomicsArcade.Utils.ClientProxy.NotifyConsumer;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();


            container.RegisterType<ILogger>(new InjectionFactory(
                _ => new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            //container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient>();
            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_GameDataRetrieveDataAccessService"]
                    )
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>()
                    );
            container.RegisterType<IUserDataAccessService, UserDataAccessClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_UserDataAccessService"]
                    )
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>()
                    );
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_GameDataStoreDataAccessService"]
                    )
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>()
                );


            container.RegisterType<IMapper<IncrementorBase, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, Contract.Common.Counters.CounterBase>, CounterContractMapper>();
            container.RegisterType<IMapper<FundsCounters, Contract.Common.Counters.FundsCounters>, FundsCountersContractMapper>();
            container.RegisterType<IMapper<FundsDriver, Contract.Common.Funds.FundsDriver>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataMapper>("GameDataMapper");
            container.RegisterType<IMapper<Contract.Game.ManualStepResult, Contract.Manager.GameManager.ManualStepResult>, ManualStepContractMapper>();
            container.RegisterType<IMapper<Contract.Game.ManualStepResult, Contract.Manager.GameManager.ManualStepResult>, ManualStepContractMapper>();

            container.RegisterType<IGameFactory, GameFactory>();

            container.RegisterType<ISessionManager, SessionManager>(
                new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>());

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>(
                new InjectionConstructor(
                    new ResolvedParameter<int>(),
                    new ResolvedParameter<string>(),
                    new ResolvedParameter<IGameDataStoreDataAccessService>(),
                    new ResolvedParameter<IMapper<IGame, GameData>>("GameDataMapper"),
                    new ResolvedParameter<IMapper<FundsDriver, Contract.Common.Funds.FundsDriver>>(),
                    new ResolvedParameter<INotifyConsumerService>()
                )
                , new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>("GameDataChanged")
                , new InterceptionBehavior<LoggerInterceptor>("FundsDriverBought")
            );

            container.RegisterType<IGame, NetworkGame>(
                new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>("PostPerformAutoStep")
                , new InterceptionBehavior<LoggerInterceptor>("PostBuyFundDriver")
                );
            container.RegisterType<IGameManager, GameManager>(
                new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>());
            container.RegisterType<ISimpleUserManager, SimpleUserManager>(
                new Interceptor<InterfaceInterceptor>()
                , new InterceptionBehavior<LoggerInterceptor>("Create"));
            //container.RegisterType<INotifyConsumerService, NotifyConsumerService>();
            container.RegisterType<INotifyConsumerService, NotifyConsumerServiceClient>(
             new InjectionConstructor(
                 ConfigurationManager.AppSettings["ClientProxyAddress_NotifyConsumerService"]
                 )
            , new Interceptor<InterfaceInterceptor>()
            , new InterceptionBehavior<LoggerInterceptor>("ejahwkfcxkf"));



            return container;
        }
    }
}
