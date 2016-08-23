using EntityFX.Gdcame.GameEngine.Contract.Items;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public interface ICustomRule
    {
        int Id { get; set; }
        void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo);
    }
}