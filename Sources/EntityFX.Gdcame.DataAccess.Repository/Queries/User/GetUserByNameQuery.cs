using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.User;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.User
{
    public class GetUserByNameQuery : QueryBase, IQuery<GetUserByNameCriterion, UserEntity>
    {
        public GetUserByNameQuery(EconomicsArcadeDbContext dbContext)
            : base(dbContext)
        {
        }

        public UserEntity Execute(GetUserByNameCriterion criterion)
        {
            return DbContext.Set<UserEntity>().SingleOrDefault(_ => _.Email == criterion.Name);
        }
    }
}