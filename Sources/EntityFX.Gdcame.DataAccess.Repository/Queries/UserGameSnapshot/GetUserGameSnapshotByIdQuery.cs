namespace EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Queries.UserGameSnapshot
{
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.MainServer.Criterions.UserGameSnapshot;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.MainServer.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetUserGameSnapshotByIdQuery : QueryBase,
        IQuery<GetUserGameSnapshotByIdCriterion, UserGameDataSnapshotEntity>
    {
        public GetUserGameSnapshotByIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserGameDataSnapshotEntity Execute(GetUserGameSnapshotByIdCriterion criterion)
        {
            return this.DbContext.Set<UserGameDataSnapshotEntity>().SingleOrDefault(_ => _.UserId == criterion.UserId);
        }
    }
}