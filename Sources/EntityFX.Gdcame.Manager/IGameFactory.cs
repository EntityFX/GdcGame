using EntityFX.Gdcame.GameEngine.Contract;

namespace EntityFX.Gdcame.Manager
{
    public interface IGameFactory
    {
        IGame BuildGame(string userId, string userName);
    }
}