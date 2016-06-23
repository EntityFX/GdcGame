using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver
{
    public class GetFundsDriverByUserIdCriterion : ICriterion
    {
        public int UserId { get; private set; }

        public GetFundsDriverByUserIdCriterion(int userId)
        {
            UserId = userId;
        }
    }
}