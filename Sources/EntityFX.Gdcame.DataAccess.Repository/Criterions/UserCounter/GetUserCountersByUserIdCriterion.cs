using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter
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