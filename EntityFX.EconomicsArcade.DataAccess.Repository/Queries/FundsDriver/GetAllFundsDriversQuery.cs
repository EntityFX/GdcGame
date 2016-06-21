using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.FundsDriver;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.FundsDriver
{
    public class GetAllFundsDriversQuery : QueryBase, IQuery<GetAllFundsDriversCriterion, IEnumerable<FundsDriverEntity>>
    {
        public GetAllFundsDriversQuery(EconomicsArcadeDbContext dbContext)
            :base(dbContext)
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
