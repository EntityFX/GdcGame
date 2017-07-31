namespace EntityFX.Gdcame.Engine.GameEngine.NetworkGameEngine
{
    using EntityFX.Gdcame.Engine.Contract.GameEngine;
    using EntityFX.Gdcame.Kernel.Contract;

    public interface IGameDataChangesNotifier
    {
        ///////void GameDataChanged(IGame game);
        void AutomaticRefreshed(IGame game);
        ///////void FundsDriverBought(IGame game, Item item);
    }
}