using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.CustomRule;
using EntityFX.Gdcame.Infrastructure.Repository.EF;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.CustomRule
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