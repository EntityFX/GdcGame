using EntityFX.EconomicsArcade.Application.NotifySignalrService;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Counters;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Model.Common.Mappers;
using EntityFX.EconomicsArcade.Model.Common.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Application.NotifyConsumerService
{
    public class ContainerBootstrapper : IContainerBootstrapper
    {
        public IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterType<IMapper<FundsCounters, FundsCounterModel>, FundsCounterModelMapper>();
            container.RegisterType<IMapper<CounterBase, CounterModelBase>, CounterModelMapper>();
            container.RegisterType<IMapper<FundsDriver, FundsDriverModel>, FundsDriverModelMapper>();
            container.RegisterType<IMapper<GameData, GameDataModel>, GameDataModelMapper>();

            container.RegisterType<GameDataHub>();

            container.RegisterType<IHubContext>(
                new InjectionFactory(_ => GlobalHost.ConnectionManager.GetHubContext<GameDataHub>()));

            return container;

        }
    }
}