using EntityFX.Gdcame.Kernel.Contract;
using EntityFX.Gdcame.Kernel.Contract.Items;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public interface INotifyGameDataChanged
    {
        void GameDataChanged(IGame game);
        void AutomaticRefreshed(IGame game);
        void FundsDriverBought(IGame game, Item item);
    }
}