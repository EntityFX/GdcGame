using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserGameSnapshot;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.UserGameSnapshot
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