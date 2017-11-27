using System;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

namespace EntityFX.Gdcame.Application.Api.Common.Providers
{
    public class SessionManagerClientFactory : ISessionManagerClientFactory
    {
        private readonly IResolver _unityContainer;

        public SessionManagerClientFactory(IResolver unityContainer)
        {
            this._unityContainer = unityContainer;
        }

        public ISessionManager BuildSessionManagerClient(Guid sessionGuid)
        {
            var game = this._unityContainer.Resolve<ISessionManager>(null, new Tuple<string, object>("sessionGuid", sessionGuid));
            return game;
        }
    }
}