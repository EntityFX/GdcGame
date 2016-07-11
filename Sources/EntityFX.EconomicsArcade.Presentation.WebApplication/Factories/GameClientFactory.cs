using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using EntityFX.EconomicsArcade.Infrastructure.Common;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Factories
{
    class GameClientFactory : IGameClientFactory
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