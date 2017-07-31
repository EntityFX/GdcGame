using EntityFX.Gdcame.Manager.MainServer;
using Microsoft.Practices.Unity;

namespace EntityFX.Gdcame.Utils.MainServer
{
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;

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