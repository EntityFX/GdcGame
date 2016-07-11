using EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine;
using EntityFX.EconomicsArcade.Contract.Game;
using EntityFX.EconomicsArcade.Manager;
using Microsoft.Practices.Unity;

namespace EntityFX.EconomicsArcade.Utils.ServiceStarter.Manager
{
    public class GameFactory : IGameFactory
    {
        private readonly IUnityContainer _unityContainer;

        public GameFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IGame BuildGame(int userId, string userName)
        {
            var game = _unityContainer.Resolve<IGame>(
                new ParameterOverride("notifyGameDataChanged", _unityContainer.Resolve<INotifyGameDataChanged>(
                    new ParameterOverride("userId", userId), new ParameterOverride("userName", userName))), new ParameterOverride("userId", userId));
            return game;
        }
    }
}