using EntityFX.Gdcame.GameEngine.NetworkGameEngine;

namespace EntityFX.Gdcame.Manager
{
    public interface IGameDataPersisterFactory
    {
        IGameDataPersister BuildGameDataPersister();
    }
}