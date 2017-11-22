
namespace EntityFX.Gdcame.Utils.MainServer
{
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine;
    using EntityFX.Gdcame.Infrastructure.Common;

    public class GameDataPersisterFactory : IGameDataPersisterFactory
    {
        private readonly IResolver _unityContainer;

        public GameDataPersisterFactory(IResolver unityContainer)
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