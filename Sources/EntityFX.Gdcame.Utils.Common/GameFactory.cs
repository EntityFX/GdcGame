using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Manager;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.Common
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