using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserCustomRuleInfo
{
    public class GetUserCustomRuleInfoByUserIdCriterion : ICriterion
    {
        public int UserId { get; private set; }

        public GetUserCustomRuleInfoByUserIdCriterion(int userId)
        {
            UserId = userId;
        }
    }
}