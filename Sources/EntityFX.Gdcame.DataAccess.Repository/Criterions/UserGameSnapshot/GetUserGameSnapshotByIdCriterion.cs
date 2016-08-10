using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot
{
    public class GetUserGameSnapshotByIdCriterion : ICriterion
    {
        public int UserId { get; private set; }

        public GetUserGameSnapshotByIdCriterion(int userId)
        {
            UserId = userId;
        }
    }
}