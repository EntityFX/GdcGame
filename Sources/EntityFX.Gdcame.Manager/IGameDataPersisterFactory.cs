using EntityFX.Gdcame.GameEngine.NetworkGameEngine;

namespace EntityFX.Gdcame.Manager.MainServer
{
    public interface IGameDataPersisterFactory
    {
        IGameDataPersister BuildGameDataPersister();
    }
}