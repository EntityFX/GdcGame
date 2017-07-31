namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserGameSnapshotByIdCriterion : ICriterion
    {
        public GetUserGameSnapshotByIdCriterion(string userId)
        {
            this.UserId = userId;
        }

        public string UserId { get; private set; }
    }
}