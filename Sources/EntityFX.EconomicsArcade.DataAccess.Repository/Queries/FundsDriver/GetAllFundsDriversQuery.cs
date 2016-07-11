using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.FundsDriver
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
                .ToArray();
        }
    }
}