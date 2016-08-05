using EntityFX.Gdcame.GameEngine.Contract;
using EntityFX.Gdcame.GameEngine.Contract.Funds;

namespace EntityFX.Gdcame.GameEngine.NetworkGameEngine
{
    public interface INotifyGameDataChanged
    {
        void GameDataChanged(IGame game);
        void AutomaticRefreshed(IGame game);
        void FundsDriverBought(IGame game, FundsDriver fundsDriver);
    }
}