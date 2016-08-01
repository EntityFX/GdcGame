using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter
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