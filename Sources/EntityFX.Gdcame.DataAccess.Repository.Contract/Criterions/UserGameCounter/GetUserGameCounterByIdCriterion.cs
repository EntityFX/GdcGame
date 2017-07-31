namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameCounter
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserGameCounterByIdCriterion : ICriterion
    {
        public GetUserGameCounterByIdCriterion(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; private set; }
    }
}