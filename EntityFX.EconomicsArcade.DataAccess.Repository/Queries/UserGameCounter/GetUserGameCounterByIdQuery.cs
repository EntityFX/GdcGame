using System.Data.Entity;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserGameCounter
{
    public class GetUserGameCounterByIdQuery : QueryBase, IQuery<GetUserGameCounterByIdCriterion, UserGameCounterEntity>
    {
        public GetUserGameCounterByIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserGameCounterEntity Execute(GetUserGameCounterByIdCriterion criterion)
        {
            return DbContext.Set<UserGameCounterEntity>().SingleOrDefault(_ => _.UserId == criterion.UserId);
        }
    }
}