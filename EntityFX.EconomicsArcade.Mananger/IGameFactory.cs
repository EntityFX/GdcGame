using EntityFX.EconomicsArcade.Contract.Game;

namespace EntityFX.EconomicsArcade.Manager
{
    public interface IGameFactory
    {
        IGame BuildGame(int userId);
    }
}