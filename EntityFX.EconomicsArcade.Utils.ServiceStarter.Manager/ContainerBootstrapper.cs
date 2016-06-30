using System;
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
using EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        //public 
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<ILogger>(new InjectionFactory(
                _=> new Logger(new NLoggerAdapter((new NLogLogExFactory()).GetLogger("logger")))));

            container.RegisterType<ISessionManager, SessionManager>();

            container.RegisterType<INotifyGameDataChanged, NotifyGameDataChanged>();

            //container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient>();
            container.RegisterType<IGameDataRetrieveDataAccessService, GameDataRetrieveDataAccessClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_GameDataRetrieveDataAccessService"]
                    ));
            container.RegisterType<IUserDataAccessService, UserDataAccessClient>();
            container.RegisterType<IGameDataStoreDataAccessService, GameDataStoreDataAccessClient>(
                new InjectionConstructor(
                    ConfigurationManager.AppSettings["ClentProxyAddress_GameDataStoreDataAccessService"]
                    ));

            container.RegisterType<IMapper<IncrementorBase, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, Contract.Common.Counters.CounterBase>, CounterContractMapper>();
            container.RegisterType<IMapper<FundsCounters, Contract.Common.Counters.FundsCounters>, FundsCountersContractMapper>();
            container.RegisterType<IMapper<FundsDriver, Contract.Common.Funds.FundsDriver>, FundsDriverContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataContractMapper>();
            container.RegisterType<IGame, NetworkGame>();
            container.RegisterType<IGameManager, GameManager>();
            container.RegisterType<ISimpleUserManager, SimpleUserManager>();

            return container;
        }
    }
}
