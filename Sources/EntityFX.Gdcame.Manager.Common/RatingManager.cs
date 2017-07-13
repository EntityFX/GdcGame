using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;
using EntityFX.Gdcame.Manager.Contract.Common.RatingManager;

namespace EntityFX.Gdcame.Manager.Common
{
    public class RatingManager : IRatingManager
    {
        private readonly IRatingDataAccess _ratingManager;

        public RatingManager(IRatingDataAccess ratingManager)
        {
            _ratingManager = ratingManager;
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            return _ratingManager.GetRaiting(top);
        }
       
    }
}