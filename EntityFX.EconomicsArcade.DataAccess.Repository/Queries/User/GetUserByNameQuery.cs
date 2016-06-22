using System.Linq;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.User;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.User
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