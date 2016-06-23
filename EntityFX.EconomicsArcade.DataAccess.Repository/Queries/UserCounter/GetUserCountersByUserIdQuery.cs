using System.Data.Entity;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserCounter;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserCounter
{
    public class GetUserCountersByUserIdQuery : QueryBase, IQuery<GetUserCountersByUserIdCriterion, UserCounterEntity>
    {
        public GetUserCountersByUserIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserCounterEntity Execute(GetUserCountersByUserIdCriterion criterion)
        {
            return DbContext.Set<UserCounterEntity>().SingleOrDefault(_ => _.UserId == criterion.UserId);
        }
    }
}