namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserCounter
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserCountersByUserIdCriterion : ICriterion
    {
        public GetUserCountersByUserIdCriterion(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; private set; }
    }
}