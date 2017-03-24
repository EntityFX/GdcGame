using EntityFX.Gdcame.Common.Contract.UserRating;
using EntityFX.Gdcame.DataAccess.Contract.Rating;

namespace EntityFX.Gdcame.DataAccess.Service
{
    public class GlobalRatingDataAccess : IGlobalRatingDataAccess
    {
        private readonly IGlobalRatingDataAccess _globalRatingDataAccess;

        public GlobalRatingDataAccess(IGlobalRatingDataAccess globalRatingDataAccess)
        {
            _globalRatingDataAccess = globalRatingDataAccess;
        }

        public TopRatingStatistics GetRaiting(int top = 500)
        {
            return _globalRatingDataAccess.GetRaiting(top);
        }

        public void PeristTopRatingStatisticsFromNode(TopRatingStatistics topRatingStatistics)
        {
            _globalRatingDataAccess.PeristTopRatingStatisticsFromNode(topRatingStatistics);
        }
    }
}