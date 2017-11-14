namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.FundsDriver
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.FundsDriver;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;
    using Microsoft.EntityFrameworkCore;

    public class GetAllFundsDriversQuery : QueryBase,
        IQuery<GetAllFundsDriversCriterion, IEnumerable<FundsDriverEntity>>
    {
        public GetAllFundsDriversQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<FundsDriverEntity> Execute(GetAllFundsDriversCriterion criterion)
        {
            return this.DbContext.Set<FundsDriverEntity>()
                .Include(e => e.Incrementors)
                .Include(e => e.CustomRule)
                .ToArray();
        }
    }
}