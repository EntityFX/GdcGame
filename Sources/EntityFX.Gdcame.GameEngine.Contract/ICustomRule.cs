using EntityFX.Gdcame.GameEngine.Contract.Items;

namespace EntityFX.Gdcame.GameEngine.Contract
{
    public interface ICustomRule
    {
        void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo);

        int Id { get;  set; }
    }
}