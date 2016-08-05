using System;
using EntityFX.Gdcame.Infrastructure.Common;
using EntityFX.Gdcame.Manager.Contract.GameManager;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.Common
{
    public class GameClientFactory : IGameClientFactory
    {
        private ILogger _logger;
        private readonly IUnityContainer _unityContainer;

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
}