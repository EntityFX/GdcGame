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
            _logger.Trace("EntityFX.EconomicsArcade.Presentation.WebApplication.GameClientFactory.BuildGameClient():");
            _logger.Info("sessionGuid is {0}", sessionGuid.ToString());
            try
            {
                var game = _unityContainer.Resolve<IGameManager>(
                    new ParameterOverride("sesionGuid", sessionGuid));
                _logger.Trace("Success");

                return game;
            }
            catch (Exception exp)
            {
                _logger.Error(exp);
                throw;
            }
        }
    }
}