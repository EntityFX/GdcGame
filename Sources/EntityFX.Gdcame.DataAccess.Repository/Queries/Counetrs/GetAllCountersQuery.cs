using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.Counters;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.Counetrs
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