using System;
using System.Linq;
using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.GameData;
using EntityFX.Gdcame.DataAccess.Contract.User;
using EntityFX.Gdcame.Manager.Contract.RatingManager;
using Microsoft.Practices.ObjectBuilder2;
using EntityFX.Gdcame.DataAccess.Contract.Rating;

namespace EntityFX.Gdcame.Manager
{
    public class RatingManager : IRatingManager
    {
        private readonly IRatingDataAccess _ratingManager;

        public RatingManager(IRatingDataAccess ratingManager)
        {
            _ratingManager = ratingManager;
        }

        public RatingStatisticsUserInfo[] GetRaiting(int top = 500)
        {
            return _ratingManager.GetRaiting(top);
        }
       
    }
}