using System.Collections.Generic;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.User;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.User
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