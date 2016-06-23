using System.Data.Entity;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserFundsDriver
{
    public class GetFundDriverByUserIdQuery : QueryBase, IQuery<GetFundsDriverByUserIdCriterion, UserFundsDriverEntity>
    {
        public GetFundDriverByUserIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserFundsDriverEntity Execute(GetFundsDriverByUserIdCriterion criterion)
        {
            return DbContext.Set<UserFundsDriverEntity>().SingleOrDefault(_ => _.UserId == criterion.UserId);
        }
    }
}