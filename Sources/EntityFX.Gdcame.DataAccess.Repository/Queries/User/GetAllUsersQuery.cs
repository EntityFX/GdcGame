using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;
using EntityFX.Gdcame.Infrastructure.Repository.EF;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.User
{
    public class GetAllUsersQuery : QueryBase, IQuery<GetAllUsersCriterion, IEnumerable<UserEntity>>
    {
        public GetAllUsersQuery(EconomicsArcadeDbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserEntity> Execute(GetAllUsersCriterion criterion)
        {
            return DbContext.Set<UserEntity>().ToArray();
        }
    }
}