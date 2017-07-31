using EntityFX.Gdcame.Application.Api.MainServer.Mappers;
using EntityFX.Gdcame.Common.Application.Model;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.NotifyConsumer.Signalr;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.NotifyConsumer
{
    using EntityFX.Gdcame.Contract.MainServer;
    using EntityFX.Gdcame.Contract.MainServer.Counters;
    using EntityFX.Gdcame.Contract.MainServer.Items;

    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IMapper<Cash, CashModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<Item, ItemModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();

            container.RegisterType<GameDataHub>();
            container.RegisterType<IHubContextAccessor, HubContextAccessor>();
            container.RegisterInstance<IConnections>(new Connections());

            return container;
        }
    }
}