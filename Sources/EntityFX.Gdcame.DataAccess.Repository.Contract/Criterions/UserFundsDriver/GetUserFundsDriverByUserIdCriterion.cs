namespace EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserFundsDriver
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserFundsDriverByUserIdCriterion : ICriterion
    {
        public GetUserFundsDriverByUserIdCriterion(int userId)
        {
            this.UserId = userId;
        }

        public int UserId { get; private set; }
    }
}