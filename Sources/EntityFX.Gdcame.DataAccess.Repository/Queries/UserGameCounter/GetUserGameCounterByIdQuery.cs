using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.UserGameCounter
{
    public class GetUserGameCounterByIdQuery : QueryBase, IQuery<GetUserGameCounterByIdCriterion, UserGameCounterEntity>
    {
        public GetUserGameCounterByIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserGameCounterEntity Execute(GetUserGameCounterByIdCriterion criterion)
        {
            return DbContext.Set<UserGameCounterEntity>().SingleOrDefault(_ => _.UserId == criterion.UserId);
        }
    }
}