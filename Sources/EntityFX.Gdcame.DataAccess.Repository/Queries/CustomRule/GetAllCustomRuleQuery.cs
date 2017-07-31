namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.CustomRule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.CustomRule;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetAllCustomRuleQuery : QueryBase,
        IQuery<GetAllCustomRulesCriterion, IEnumerable<CustomRuleEntity>>
    {
        public GetAllCustomRuleQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<CustomRuleEntity> Execute(GetAllCustomRulesCriterion criterion)
        {
            return this.DbContext.Set<CustomRuleEntity>()
                .ToArray();
        }
    }
}