using EntityFX.EconomicsArcade.Infrastructure.Repository.Criterion;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User
{
    public class GetUserByNameCriterion  : ICriterion
    {
        public string Name { get; set; }

        public GetUserByNameCriterion(string name)
        {
            Name = name;
        }
    }
}