using EntityFX.EconomicsArcade.Contract.Common;

namespace EntityFX.EconomicArcade.Engine.GameEngine.UssrSimulator
{
    public interface INotifyGameDataChanged
    {
        void GameDataChanged(GameData gameData);
    }
}