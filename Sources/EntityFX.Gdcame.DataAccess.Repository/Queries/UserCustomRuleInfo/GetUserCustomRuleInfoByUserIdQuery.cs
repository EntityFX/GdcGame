using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCustomRuleInfo;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.UserCustomRuleInfo
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