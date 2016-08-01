using EntityFX.EconomicsArcade.Contract.Game.Funds;

namespace EntityFX.EconomicsArcade.Contract.Game
{
    public interface ICustomRule
    {
        void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo);

        int Id { get;  set; }
    }
}