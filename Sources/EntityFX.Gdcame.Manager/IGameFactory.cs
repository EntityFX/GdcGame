using EntityFX.Gdcame.GameEngine.Contract;

namespace EntityFX.Gdcame.Manager.MainServer
{
    public interface IGameFactory
    {
        IGame BuildGame(string userId, string userName);
    }
}