namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserCustomRuleInfo
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserCustomRuleInfoByUserIdCriterion : ICriterion
    {
        public GetUserCustomRuleInfoByUserIdCriterion(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; private set; }
    }
}