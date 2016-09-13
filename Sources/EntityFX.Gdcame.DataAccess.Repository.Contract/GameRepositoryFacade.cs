namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public class GameRepositoryFacade
    {
        public IItemRepository FundsDriverRepository { get; set; }
        public ICountersRepository CountersRepository { get; set; }
        public ICustomRuleRepository CustomRuleRepository { get; set; }
    }
}