using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFX.EconomicsArcade.Contract.Common;
using EntityFX.EconomicsArcade.DataAccess.Model;
using EntityFX.EconomicsArcade.DataAccess.Repository.Criterions.UserRating;
using EntityFX.EconomicsArcade.Infrastructure.Repository.Query;

namespace EntityFX.EconomicsArcade.DataAccess.Repository.Queries.UserRating
{
    public class GetAllUsersRatingsQuery : QueryBase,
        IQuery<GetAllUsersRatingsCriterion, IEnumerable<Contract.Manager.RatingManager.UserRating>>
    {
        public GetAllUsersRatingsQuery(DbContext dbContext)
            : base(dbContext)
        {
        }

        public IEnumerable<Contract.Manager.RatingManager.UserRating> Execute(GetAllUsersRatingsCriterion criterion)
        {
            var userRatings = from user in DbContext.Set<UserEntity>().ToArray()
                              join gameData in DbContext.Set<UserGameCounterEntity>().ToArray() on user.Id equals gameData.UserId
                              select
                                  new Contract.Manager.RatingManager.UserRating()
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