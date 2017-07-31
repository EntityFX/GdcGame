namespace EntityFX.Gdcame.Kernel.Contract
{
    public interface IGameFactory
    {
        IGame BuildGame(string userId, string userName);
    }
}