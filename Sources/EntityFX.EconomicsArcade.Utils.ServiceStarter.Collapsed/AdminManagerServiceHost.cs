using System.ServiceModel;
using EntityFX.EconomicsArcade.Contract.Manager.AdminManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using EntityFX.EconomicsArcade.Infrastructure.Service.NetNamedPipe;
using EntityFX.EconomicsArcade.Manager;
using EntityFX.EconomicsArcade.Utils.Common;
using Microsoft.Practices.Unity;

internal class AdminManagerServiceHost : NetNamedPipeServiceHost<IAdminManager>
{
    public AdminManagerServiceHost(IUnityContainer container)
        : base(container)
    {
    }

    protected override void BeforeServiceOpen(ServiceHost serviceHost)
    {
        var serviceEndpointCollection = serviceHost.Description.Endpoints;
        foreach (var serviceEndpoint in serviceEndpointCollection)
        {
            foreach (var operation in serviceEndpoint.Contract.Operations)
            {
                operation.Behaviors.Add(new CheckRolePermissionsOperationBehavior(Container.Resolve<IOperationContextHelper>(), Container.Resolve<GameSessions>()));
            }
        }
    }
}