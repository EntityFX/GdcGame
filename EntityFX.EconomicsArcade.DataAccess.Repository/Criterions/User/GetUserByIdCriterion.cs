using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User
{
    public class GetUserByIdCriterion : GetByIdCriterion, ICriterion
    {
        public GetUserByIdCriterion(int id)
            : base(id)
        {
        }
    }
}