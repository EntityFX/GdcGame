using System;
using EntityFX.EconomicsArcade.Contract.Manager.GameManager;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Presentation.WebApplication.Factories
{
    class GameClientFactory : IGameClientFactory
    {
        private readonly IUnityContainer _unityContainer;

        public GameClientFactory(IUnityContainer unityContainer)
        {
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