namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.Counetrs
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.Counters;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetAllCountersQuery : QueryBase,
        IQuery<GetAllCountersCriterion, IEnumerable<CounterEntity>>
    {
        public GetAllCountersQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<CounterEntity> Execute(GetAllCountersCriterion criterion)
        {
            return this.DbContext.Set<CounterEntity>()
                .ToArray();
        }
    }
}