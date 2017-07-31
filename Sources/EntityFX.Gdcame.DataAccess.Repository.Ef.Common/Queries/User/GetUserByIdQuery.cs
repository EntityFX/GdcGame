namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Queries.User
{
    using System.Linq;

    using EntityFX.Gdcame.DataAccess.Repository.Contract.Common.Criterions.User;
    using EntityFX.Gdcame.DataAccess.Repository.Ef.Common.Entities;
    using EntityFX.Gdcame.Infrastructure.Repository.EF;
    using EntityFX.Gdcame.Infrastructure.Repository.Query;

    public class GetUserByIdQuery : QueryBase, IQuery<GetUserByIdCriterion, UserEntity>
    {
        public GetUserByIdQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public UserEntity Execute(GetUserByIdCriterion criterion)
        {
            return this.DbContext.Set<UserEntity>().SingleOrDefault(_ => _.Id == criterion.Id);
        }
    }
}