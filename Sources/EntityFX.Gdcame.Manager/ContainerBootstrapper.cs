using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Counters;
using EntityFX.Gdcame.GameEngine.Contract.Incrementors;
using EntityFX.Gdcame.GameEngine.Contract.Items;
using EntityFX.Gdcame.GameEngine.Mappers;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;

using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.UserManager;
using EntityFX.Gdcame.Manager.MainServer.Mappers;
using EntityFX.Gdcame.Manager.MainServer.Mappers.Store;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using CounterBase = EntityFX.Gdcame.GameEngine.Contract.Counters.CounterBase;
using ManualStepResult = EntityFX.Gdcame.GameEngine.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager.MainServer
{
    using EntityFX.Gdcame.DataAccess.Contract.MainServer.GameData.Store;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IMapper<IncrementorBase, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, EntityFX.Gdcame.Common.Contract.Counters.CounterBase>, CounterContractMapper>();
            container
                .RegisterType
                <IMapper<GameCash, Cash>, FundsCountersContractMapper>();
            container.RegisterType<IMapper<Item, Gdcame.Common.Contract.Items.Item>, FundsDriverContractMapper>();
            container
                .RegisterType
                <IMapper<CustomRuleInfo, Gdcame.Common.Contract.Items.CustomRuleInfo>, CustomRuleInfoContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataMapper>("GameDataMapper");
            container
                .RegisterType
                <IMapper<ManualStepResult, Contract.MainServer.GameManager.ManualStepResult>, ManualStepContractMapper>();

            container.RegisterType<IGame, NetworkGame>();

            //Store
            container.RegisterType<IMapper<IncrementorBase, StoredIncrementor>, StoreIncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, StoredCounterBase>, StoreCounterContractMapper>();
            container
                .RegisterType
                <IMapper<GameCash, StoredCash>, StoreFundsCountersContractMapper>();
            container.RegisterType<IMapper<Item, StoredItem>, StoreFundsDriverContractMapper>();
            container.RegisterType<IMapper<CustomRuleInfo, StoredCustomRuleInfo>, StoreCustomRuleInfoContractMapper>();
            container.RegisterType<IMapper<IGame, StoredGameData>, StoreGameDataMapper>("StoreGameDataMapper");
            /////

            container.RegisterType<IMapperFactory, MapperFactory>();


            container.RegisterType<ISessionManager, SessionManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>(),
                new Interceptor<InterfaceInterceptor>());



            container.RegisterType<IGameManager, GameManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());
            container.RegisterType<ISimpleUserManager, SimpleUserManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());
            container.RegisterType<IAdminManager, AdminManager>(
                new InterceptionBehavior<PolicyInjectionBehavior>()
                , new Interceptor<InterfaceInterceptor>());

            return container;
        }
    }
}