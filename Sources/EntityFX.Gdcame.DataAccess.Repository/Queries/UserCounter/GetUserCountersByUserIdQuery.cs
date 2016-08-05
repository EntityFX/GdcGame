using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.UserCounter
{
    public class GetUserCountersByUserIdQuery : QueryBase, IQuery<GetUserCountersByUserIdCriterion, IEnumerable<UserCounterEntity>>
    {
        public GetUserCountersByUserIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserCounterEntity> Execute(GetUserCountersByUserIdCriterion criterion)
        {
            return DbContext.Set<UserCounterEntity>().Include("Counter").Where(_ => _.UserId == criterion.UserId);
        }
    }
}