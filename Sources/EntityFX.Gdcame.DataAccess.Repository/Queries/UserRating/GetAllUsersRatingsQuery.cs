using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model.Ef;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.UserRating;
using EntityFX.Gdcame.Infrastructure.Repository.EF;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Ef.Queries.UserRating
{
    public class GetAllUsersRatingsQuery : QueryBase,
        IQuery<GetAllUsersRatingsCriterion, IEnumerable<Common.Contract.UserRating.UserRating>>
    {
        public GetAllUsersRatingsQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<Common.Contract.UserRating.UserRating> Execute(GetAllUsersRatingsCriterion criterion)
        {
            var userRatings =
                DbContext.Set<UserEntity>().AsEnumerable().Select(u => new Common.Contract.UserRating.UserRating
                {
                    UserName = u.Email
                });

            return userRatings;
        }
    }
}