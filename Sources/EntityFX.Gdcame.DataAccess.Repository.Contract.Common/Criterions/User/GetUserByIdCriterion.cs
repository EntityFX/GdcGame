namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User
{
    using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

    public class GetUserByIdCriterion : ICriterion
    {
        public GetUserByIdCriterion(string id)
        {
            this.Id = id;
        }

        public string Id { get; set; }
    }
}