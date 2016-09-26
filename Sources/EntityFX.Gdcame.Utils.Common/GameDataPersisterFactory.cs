using EntityFX.Gdcame.GameEngine.NetworkGameEngine;
using EntityFX.Gdcame.Manager;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.Common
{
    public class GameDataPersisterFactory : IGameDataPersisterFactory
    {
        private readonly IUnityContainer _unityContainer;

        public GameDataPersisterFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IGameDataPersister BuildGameDataPersister()
        {
            var gameDataPersister = _unityContainer.Resolve<IGameDataPersister>();
            return gameDataPersister;
        }
    }
}