﻿using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Manager
{
    public class LocalRatingManager
    {
        private readonly ILocalRatingDataAccess _ratingManager;

        public LocalRatingManager(ILocalRatingDataAccess ratingManager)
        {
            _ratingManager = ratingManager;
        }

        public RatingStatistics[] GetRaiting(int top = 500)
        {
            return _ratingManager.GetRaiting(top);
        }
    }
}
