using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User
{
    public class GetUserByIdCriterion : ICriterion
    {
        public GetUserByIdCriterion(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}