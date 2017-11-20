using System;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.MainServer.GameManager;
using EntityFX.Gdcame.Application.Providers.MainServer;

namespace EntityFX.Gdcame.Utils.MainServer
{
    public class GameClientFactory : IGameClientFactory
    {
        private readonly IIocContainer _unityContainer;
        private ILogger _logger;

        public GameClientFactory(ILogger logger, IIocContainer unityContainer)
        {
            _logger = logger;
            _unityContainer = unityContainer;
        }

        public IGameManager BuildGameClient(Guid sessionGuid)
        {
            var game = _unityContainer.Resolve<IGameManager>(null,
                new Tuple<string, object>("sesionGuid", sessionGuid));

            return game;
        }
    }

    public class NoWcfGameManagerFactory : IGameClientFactory
    {
        private readonly IIocContainer _unityContainer;
        private ILogger _logger;
        private readonly IOperationContextHelper _operationContextHelper;

        public NoWcfGameManagerFactory(ILogger logger, IOperationContextHelper operationContextHelper, IIocContainer unityContainer)
        {
            _logger = logger;
            _operationContextHelper = operationContextHelper;
            _unityContainer = unityContainer;
        }

        public IGameManager BuildGameClient(Guid sessionGuid)
        {
            _operationContextHelper.Instance.SessionId = sessionGuid;
            var game = _unityContainer.Resolve<IGameManager>(null,
                new Tuple<string, object>("sesionGuid", sessionGuid));

            return game;
        }
    }
}