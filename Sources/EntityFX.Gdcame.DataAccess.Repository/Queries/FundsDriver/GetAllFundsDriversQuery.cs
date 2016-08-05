using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.FundsDriver
{
    public class GetAllFundsDriversQuery : QueryBase,
        IQuery<GetAllFundsDriversCriterion, IEnumerable<FundsDriverEntity>>
    {
        public GetAllFundsDriversQuery(EconomicsArcadeDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<FundsDriverEntity> Execute(GetAllFundsDriversCriterion criterion)
        {
            return DbContext.Set<FundsDriverEntity>()
                .Include("Incrementors")
                .Include("CustomRule")
                .ToArray();
        }
    }
}