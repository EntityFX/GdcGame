﻿namespace EntityFX.Gdcame.Utils.Common
{
    using System;

    using EntityFX.Gdcame.Manager.Contract.Common.SessionManager;

    using Microsoft.Practices.Unity;
    using Unity;
    using Unity.Resolution;

    public class SessionManagerClientFactory : ISessionManagerClientFactory
    {
        private readonly IUnityContainer _unityContainer;

        public SessionManagerClientFactory(IUnityContainer unityContainer)
        {
            this._unityContainer = unityContainer;
        }

        public ISessionManager BuildSessionManagerClient(Guid sessionGuid)
        {
            var game = this._unityContainer.Resolve<ISessionManager>(new ParameterOverride("sessionGuid", sessionGuid));
            return game;
        }
    }
}