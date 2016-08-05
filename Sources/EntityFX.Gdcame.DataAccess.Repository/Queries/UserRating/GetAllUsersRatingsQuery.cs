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
            var userRatings = from user in DbContext.Set<UserEntity>().ToArray()
                              join gameData in DbContext.Set<UserGameCounterEntity>().ToArray() on user.Id equals gameData.UserId
                              select
                                  new Gdcame.Common.Contract.UserRating.UserRating()
                                  {
                                      UserName = user.Email,
                                      GdcPoints = user.UserCounters.ToArray()[0].Value,
                                      TotalFunds = gameData.TotalFunds,
                                      ManualStepsCount = gameData.ManualStepsCount
                                  };
            return userRatings;
        }
    }
}