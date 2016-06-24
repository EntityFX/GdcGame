using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.Contract.Common.Funds;
using EntityFX.EconomicsArcade.Contract.Game;

namespace EntityFX.EconomicArcade.Engine.GameEngine.UssrSimulator
{
    public interface INotifyGameDataChanged
    {
        void GameDataChanged(IGame game);

        void FundsDriverBought(IGame game, EconomicsArcade.Contract.Game.Funds.FundsDriver fundsDriver);
    }
}