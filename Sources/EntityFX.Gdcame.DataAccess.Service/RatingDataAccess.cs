using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class RatingDataAccess : IRatingDataAccess
    {
        private readonly IRatingStatisticsRepository _ratingStatisticsRepository;

        public RatingDataAccess(IRatingStatisticsRepository ratingRepository)
        {
            _ratingStatisticsRepository = ratingRepository;
        }
        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            _ratingStatisticsRepository.CreateOrUpdateUsersRatingStatistics(ratingStatistics);
        }

        public RatingStatistics[] GetRaiting(int top = 500)
        {
            return _ratingStatisticsRepository.GetRaiting(top);
        }
    }
}
