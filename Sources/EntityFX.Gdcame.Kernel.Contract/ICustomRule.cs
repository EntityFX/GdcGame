namespace EntityFX.Gdcame.Kernel.Contract
{
    using EntityFX.Gdcame.Kernel.Contract.Items;

    public interface ICustomRule
    {
        int Id { get; set; }
        void PerformRuleWhenBuyFundDriver(IGame game, CustomRuleInfo customRuleInfo);
    }
}