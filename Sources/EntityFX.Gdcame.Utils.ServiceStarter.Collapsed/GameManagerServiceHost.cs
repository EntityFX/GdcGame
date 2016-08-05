using System.ServiceModel;
using EntityFX.Gdcame.Infrastructure.Service;
using EntityFX.Gdcame.Infrastructure.Service.NetNamedPipe;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using EntityFX.Gdcame.Utils.Common;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.ServiceStarter.Collapsed
{
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
}