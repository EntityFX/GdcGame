using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCustomRuleInfo
{
    public class GetUserCustomRuleInfoByUserIdQuery : QueryBase, IQuery<GetUserCustomRuleInfoByUserIdCriterion, IEnumerable<UserCustomRuleEntity>>
    {
        public GetUserCustomRuleInfoByUserIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserCustomRuleEntity> Execute(GetUserCustomRuleInfoByUserIdCriterion criterion)
        {
            return DbContext.Set<UserCustomRuleEntity>().Where(_ => _.UserId == criterion.UserId);
        }
    }
}