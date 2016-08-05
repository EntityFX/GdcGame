using EntityFX.Gdcame.GameEngine.Contract;

namespace EntityFX.Gdcame.Manager
{
    public interface IGameFactory
    {
        IGame BuildGame(int userId,string userName);
    }
}