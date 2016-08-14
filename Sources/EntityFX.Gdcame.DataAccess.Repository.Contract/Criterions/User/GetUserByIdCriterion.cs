using EntityFX.Gdcame.Infrastructure.Repository.Criterion;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User
{
    public class GetUserByIdCriterion : GetByIdCriterion, ICriterion
    {
        public GetUserByIdCriterion(int id)
            : base(id)
        {
        }
    }
}