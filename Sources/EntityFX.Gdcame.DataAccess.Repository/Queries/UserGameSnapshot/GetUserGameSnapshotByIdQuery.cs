using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameCounter;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.UserGameCounter
{
    public class GetUserGameSnapshotByIdQuery : QueryBase, IQuery<GetUserGameSnapshotByIdCriterion, UserGameDataSnapshotEntity>
    {
        public GetUserGameSnapshotByIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserGameDataSnapshotEntity Execute(GetUserGameSnapshotByIdCriterion criterion)
        {
            return DbContext.Set<UserGameDataSnapshotEntity>().SingleOrDefault(_ => _.UserId == criterion.UserId);
        }
    }
}