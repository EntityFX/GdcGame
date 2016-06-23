using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter
{
    public class GetUserCountersByUserIdCriterion : ICriterion
    {
        public int UserId { get; private set; }

        public GetUserCountersByUserIdCriterion(int userId)
        {
            UserId = userId;
        }
    }
}