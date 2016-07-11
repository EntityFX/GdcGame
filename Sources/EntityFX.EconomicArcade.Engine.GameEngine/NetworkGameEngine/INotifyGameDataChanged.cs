using EntityFX.EconomicsArcade.Contract.Game;

namespace EntityFX.EconomicArcade.Engine.GameEngine.NetworkGameEngine
{
    public interface INotifyGameDataChanged
    {
        void GameDataChanged(IGame game);
        void AutomaticRefreshed(IGame game);
        void FundsDriverBought(IGame game, EconomicsArcade.Contract.Game.Funds.FundsDriver fundsDriver);
    }
}