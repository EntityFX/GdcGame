using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserFundsDriver;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.UserFundsDriver
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