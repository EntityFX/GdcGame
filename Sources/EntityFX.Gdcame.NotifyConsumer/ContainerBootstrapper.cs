using EntityFX.Gdcame.Common.Contract;
using EntityFX.Gdcame.Common.Contract.Counters;
using EntityFX.Gdcame.Common.Contract.Items;
using EntityFX.Gdcame.Common.Presentation.Model;
using EntityFX.Gdcame.Common.Presentation.Model.Mappers;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.NotifyConsumer.Contract;
using EntityFX.Gdcame.NotifyConsumer.Signalr;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.NotifyConsumer
{
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