using System;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.Common
{
    public class GameClientFactory : IGameClientFactory
    {
        private readonly IUnityContainer _unityContainer;
        private ILogger _logger;

        public GameClientFactory(ILogger logger, IUnityContainer unityContainer)
        {
            _logger = logger;
            _unityContainer = unityContainer;
        }

        public IGameManager BuildGameClient(Guid sessionGuid)
        {
            var game = _unityContainer.Resolve<IGameManager>(
                new ParameterOverride("sesionGuid", sessionGuid));

            return game;
        }
    }

    public class NoWcfGameManagerFactory : IGameClientFactory
    {
        private readonly IUnityContainer _unityContainer;
        private ILogger _logger;
        private readonly IOperationContextHelper _operationContextHelper;

        public NoWcfGameManagerFactory(ILogger logger, IOperationContextHelper operationContextHelper, IUnityContainer unityContainer)
        {
            _logger = logger;
            _operationContextHelper = operationContextHelper;
            _unityContainer = unityContainer;
        }

        public IGameManager BuildGameClient(Guid sessionGuid)
        {
            _operationContextHelper.Instance.SessionId = sessionGuid;
            var game = _unityContainer.Resolve<IGameManager>(
                new ParameterOverride("sesionGuid", sessionGuid));

            return game;
        }
    }
}