using System;
using EntityFX.Gdcame.Manager.Contract.MainServer.SessionManager;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.MainServer
{
    public class SessionManagerClientFactory : ISessionManagerClientFactory
    {
        private readonly IUnityContainer _unityContainer;

        public SessionManagerClientFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public ISessionManager BuildSessionManagerClient(Guid sessionGuid)
        {
            var game = _unityContainer.Resolve<ISessionManager>(new ParameterOverride("sessionGuid", sessionGuid));
            return game;
        }
    }
}