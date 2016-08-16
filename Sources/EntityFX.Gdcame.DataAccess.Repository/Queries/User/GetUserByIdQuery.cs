using System.Linq;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.User;
using EntityFX.Gdcame.Infrastructure.Repository.EF;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.User
{
    public class GetUserByIdQuery : QueryBase, IQuery<GetUserByIdCriterion, UserEntity>
    {
        public GetUserByIdQuery(EconomicsArcadeDbContext dbContext)
            : base(dbContext)
        {
        }

        public UserEntity Execute(GetUserByIdCriterion criterion)
        {
            return DbContext.Set<UserEntity>().SingleOrDefault(_ => _.Id == criterion.Id);
        }
    }
}