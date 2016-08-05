using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver
{
    public class GetUserFundsDriverByUserIdCriterion : ICriterion
    {
        public int UserId { get; private set; }

        public GetUserFundsDriverByUserIdCriterion(int userId)
        {
            UserId = userId;
        }
    }
}