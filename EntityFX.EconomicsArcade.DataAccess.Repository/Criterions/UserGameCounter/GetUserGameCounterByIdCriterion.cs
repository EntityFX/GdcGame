using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter
{
    public class GetUserGameCounterByIdCriterion : GetByIdCriterion, ICriterion
    {
        public GetUserGameCounterByIdCriterion(int id)
            : base(id)
        {
        }
    }
}