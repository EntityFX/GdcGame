using EntityFX.EconomicArcade.Engine.GameEngine.UssrSimulator;
using EntityFX.EconomicsArcade.Contract.Game;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Manager
{
    public class GameFactory
    {
        private readonly IUnityContainer _unityContainer;

        public GameFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IGame BuildGame()
        {
            var game = _unityContainer.Resolve<IGame>();
            return game;
        }
    }
}