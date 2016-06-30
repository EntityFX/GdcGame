using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserFundsDriver
{
    public class GetUserFundDriverByUserIdQuery : QueryBase, IQuery<GetUserFundsDriverByUserIdCriterion, IEnumerable<UserFundsDriverEntity>>
    {
        public GetUserFundDriverByUserIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserFundsDriverEntity> Execute(GetUserFundsDriverByUserIdCriterion criterion)
        {
            return DbContext.Set<UserFundsDriverEntity>().Where(_ => _.UserId == criterion.UserId);
        }
    }
}