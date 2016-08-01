using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.CustomRule;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.CustomRule
{
    public class GetAllCustomRuleQuery : QueryBase,
        IQuery<GetAllCustomRulesCriterion, IEnumerable<CustomRuleEntity>>
    {
        public GetAllCustomRuleQuery(EconomicsArcadeDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<CustomRuleEntity> Execute(GetAllCustomRulesCriterion criterion)
        {
            return DbContext.Set<CustomRuleEntity>()
                .ToArray();
        }
    }
}