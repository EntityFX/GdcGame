using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.Gdcame.DataAccess.Model;
using EntityFX.Gdcame.DataAccess.Repository.Criterions.UserRating;
using EntityFX.Gdcame.Infrastructure.Repository.Query;

namespace EntityFX.Gdcame.DataAccess.Repository.Queries.UserRating
{
    public class GetAllUsersRatingsQuery : QueryBase,
        IQuery<GetAllUsersRatingsCriterion, IEnumerable<Gdcame.Common.Contract.UserRating.UserRating>>
    {
        public GetAllUsersRatingsQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<Gdcame.Common.Contract.UserRating.UserRating> Execute(GetAllUsersRatingsCriterion criterion)
        {
            var userRatings =
                    DbContext.Set<UserEntity>().AsEnumerable().Select(u => new Common.Contract.UserRating.UserRating()
                    {
                        UserName = u.Email
                    });

            return userRatings;
        }
    }
}