﻿using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Repository.Contract;

namespace EntityFX.Gdcame.DataAccess.Service
{
    class LocalRatingDataAccess : ILocalRatingDataAccess
    {
        private readonly IRatingStatisticsRepository _ratingStatisticsRepository;
        private readonly IRatingHistoryRepository _ratingHistoryRepository;

        public void PersistRatingHistory(RatingHistory ratingHistory)
        {
            _ratingHistoryRepository.PersistRatingHistory(ratingHistory);
        }

        public RatingHistory[] ReadHistoryWithUsersIds(string[] userslds, TimeSpan period)
        {
            return _ratingHistoryRepository.ReadHistoryWithUsersIds(userslds, period);
        }

        public void CleanOldHistory(TimeSpan period)
        {
            _ratingHistoryRepository.CleanOldHistory(period);
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

