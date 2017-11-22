using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;

namespace EntityFX.Gdcame.NotifyConsumer
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IIocContainer Configure(IIocContainer container)
        {
            //container.RegisterType<IMapper<Cash, CashModel>, FundsCounterModelMapper>();
            //container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            //container.RegisterType<IMapper<Item, ItemModel>, FundsDriverModelMapper>();
            //container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();

            //container.RegisterType<GameDataHub>();
            //container.RegisterType<IHubContextAccessor, HubContextAccessor>();
            container.RegisterType<IConnections>((c) => new Connections(), ContainerScope.Singleton);

            return container;
        }
    }
}