using System.Collections.Generic;
using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.Counters;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.Counetrs
{
    public class GetAllCountersQuery : QueryBase,
        IQuery<GetAllCountersCriterion, IEnumerable<CounterEntity>>
    {
        public GetAllCountersQuery(EconomicsArcadeDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<CounterEntity> Execute(GetAllCountersCriterion criterion)
        {
            return DbContext.Set<CounterEntity>()
                .ToArray();
        }
    }
}