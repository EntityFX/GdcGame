namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Queries.User
{
    using System.Collections.Generic;
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetUsersBySearchStringQuery : QueryBase, IQuery<GetUsersBySearchStringCriterion, IEnumerable<UserEntity>>
    {
        public GetUsersBySearchStringQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserEntity> Execute(GetUsersBySearchStringCriterion criterion)
        {
            IQueryable<UserEntity> usersQuery = this.DbContext.Set<UserEntity>();

            if (!string.IsNullOrWhiteSpace(criterion.SearchString))
            {
                usersQuery = usersQuery.Where(u => u.Email.StartsWith(criterion.SearchString));
            }
            return usersQuery.ToArray();
        }
    }

    public class GetUsersByOffsetQuery : QueryBase, IQuery<GetUsersByOffsetCriterion, IEnumerable<UserEntity>>
    {
        public GetUsersByOffsetQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<UserEntity> Execute(GetUsersByOffsetCriterion criterion)
        {
            IQueryable<UserEntity> usersQuery = this.DbContext.Set<UserEntity>();
            usersQuery.Skip(criterion.Offset).Take(criterion.Size);

            return usersQuery.ToArray();
        }
    }
}