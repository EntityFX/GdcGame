namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Queries.User
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetAllUsersQuery : QueryBase, IQuery<GetAllUsersCriterion, IEnumerable<UserEntity>>
    {
        public GetAllUsersQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserEntity> Execute(GetAllUsersCriterion criterion)
        {
            return this.DbContext.Set<UserEntity>().ToArray();
        }
    }
}