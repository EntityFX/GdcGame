using EntityFX.Gdcame.Contract.MainServer.Store;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Common;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;
using EntityFX.Gdcame.Manager.Contract.Common.ServerManager;
using EntityFX.Gdcame.Manager.Contract.Common.WorkerManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.AdminManager;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;
using EntityFX.Gdcame.Manager.MainServer.Mappers;
using EntityFX.Gdcame.Manager.MainServer.Mappers.Store;

using CounterBase = EntityFX.Gdcame.Kernel.Contract.Counters.CounterBase;
using ManualStepResult = EntityFX.Gdcame.Kernel.Contract.ManualStepResult;

namespace EntityFX.Gdcame.Manager.MainServer
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Incrementors;
    using EntityFX.Gdcame.Engine.GameEngine.Mappers;
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;
    using EntityFX.Gdcame.Kernel.Contract.Counters;
    using EntityFX.Gdcame.Kernel.Contract.Incrementors;
    using EntityFX.Gdcame.Kernel.Contract.Items;
    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;
    using EntityFX.Gdcame.Manager.Contract.Common.UserManager;

    using CounterBase = EntityFX.Gdcame.Kernel.Contract.Counters.CounterBase;
    using IGame = EntityFX.Gdcame.Kernel.Contract.IGame;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            container.RegisterType<IMapper<IncrementorBase, Incrementor>, IncrementorContractMapper>();
            container.RegisterType<IMapper<CounterBase, Gdcame.Contract.MainServer.Counters.CounterBase>, CounterContractMapper>();
            container
                .RegisterType
                <IMapper<GameCash, Cash>, FundsCountersContractMapper>();
            container.RegisterType<IMapper<Item, Gdcame.Contract.MainServer.Items.Item>, FundsDriverContractMapper>();
            container
                .RegisterType
                <IMapper<CustomRuleInfo, Gdcame.Contract.MainServer.Items.CustomRuleInfo>, CustomRuleInfoContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataContractMapper>();
            container.RegisterType<IMapper<IGame, GameData>, GameDataMapper>(ContainerScope.Instance, "GameDataMapper");
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
            container.RegisterType<IMapper<IGame, StoredGameData>, StoreGameDataMapper>(ContainerScope.Instance, "StoreGameDataMapper");
            /////

            container.RegisterType<IMapperFactory, MapperFactory>();


            container.RegisterType<ISessionManager, SessionManager>();



            container.RegisterType<IGameManager, GameManager>();
            container.RegisterType<ISimpleUserManager, SimpleUserManager>();
            container.RegisterType<IAdminManager, AdminManager>();

            return container;
        }
    }
}