using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.FundsDriver;
using EntityFX.Gdcame.Infrastructure.Repository.EF;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.FundsDriver
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