using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Service;
using EntityFX.EconomicsArcade.Infrastructure.Service.NetNamedPipe;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;

internal class GameManagerServiceHost : NetNamedPipeServiceHost<IGameManager>
{
    public GameManagerServiceHost(IUnityContainer container)
        : base(container)
    {
    }

    protected override void BeforeServiceOpen(ServiceHost serviceHost)
    {
        serviceHost.Description.Behaviors.Add(new ErrorHandlerBehavior(new InvalidSessionFaultHandler()));
    }
}