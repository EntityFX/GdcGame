namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Queries.User
{
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetUserByNameQuery : QueryBase, IQuery<GetUserByNameCriterion, UserEntity>
    {
        public GetUserByNameQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserEntity Execute(GetUserByNameCriterion criterion)
        {
            return this.DbContext.Set<UserEntity>().SingleOrDefault(_ => _.Email == criterion.Name);
        }
    }
}