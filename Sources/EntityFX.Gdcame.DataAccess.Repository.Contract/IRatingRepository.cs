using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using EntityFX.Gdcame.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.DataAccess.Repository.Contract
{
    public interface IRatingRepository
    {
         void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics);
         RatingStatistics[] GetRaiting(int top = 500);
    }
}
