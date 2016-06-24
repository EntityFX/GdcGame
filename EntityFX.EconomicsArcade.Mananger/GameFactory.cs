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

        public IGame BuildGame(int userId)
        {
            var game = _unityContainer.Resolve<IGame>(
                new ParameterOverride("notifyGameDataChanged", _unityContainer.Resolve<INotifyGameDataChanged>(
                    new ParameterOverride("userId", userId))), new ParameterOverride("userId", userId));
            return game;
        }
    }
}