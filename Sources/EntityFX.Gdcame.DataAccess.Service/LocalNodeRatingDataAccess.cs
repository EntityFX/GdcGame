using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract;
using EntityFX.Gdcame.DataAccess.Repository.Contract.Criterions.RatingHistory;

namespace EntityFX.Gdcame.DataAccess.Service
{
    class LocalNodeRatingDataAccess : ILocalNodeRatingDataAccess
    {
        private readonly IRatingStatisticsRepository _ratingStatisticsRepository;
        private readonly IRatingHistoryRepository _ratingHistoryRepository;

        public LocalNodeRatingDataAccess(IRatingStatisticsRepository ratingStatisticsRepository, IRatingHistoryRepository ratingHistoryRepository)
        {
            _ratingStatisticsRepository = ratingStatisticsRepository;
            _ratingHistoryRepository = ratingHistoryRepository;
        }

        public void PersistUsersRatingHistory(RatingHistory[] ratingHistory)
        {
            _ratingHistoryRepository.PersistUsersRatingHistory(ratingHistory);
        }

        public RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period)
        {
            return _ratingHistoryRepository.ReadHistoryWithUsersIds(new GetUsersRatingHistoryCriterion(){ UsersIds =  userslds, Period = period});
        }

        public void CleanOldHistory(TimeSpan period)
        {
            _ratingHistoryRepository.CleanOldHistory(period);
        }

        public void CreateOrUpdateUsersRatingStatistics(RatingStatistics[] ratingStatistics)
        {
            _ratingStatisticsRepository.CreateOrUpdateUsersRatingStatistics(ratingStatistics);
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
           return _ratingStatisticsRepository.GetRaiting(top);
        }

    }

}

