using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot
{
    public class GetUserGameSnapshotByIdCriterion : ICriterion
    {
        public GetUserGameSnapshotByIdCriterion(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; private set; }
    }
}